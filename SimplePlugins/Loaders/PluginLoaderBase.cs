using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using SimplePlugins.Exceptions;
using System.Collections.ObjectModel;

namespace SimplePlugins.Loaders
{
    [Serializable]
    public abstract class PluginLoaderBase : MarshalByRefObject
    {
        [Serializable]
        public class PluginLoadedEventArgs : EventArgs
        {
            public PluginLoadedEventArgs(PluginInfo info)
                : base()
            {
                this.Info = info;
            }

            public PluginInfo Info
            {
                get;
                private set;
            }
        }

        [Serializable]
        public class PluginUnloadedEventArgs : EventArgs
        {
            public PluginUnloadedEventArgs(PluginInfo info, PluginParameters results)
                : base()
            {
                this.Info = info;
                if (results == null)
                    this.Results = new PluginParameters();
                else
                    this.Results = results;
            }

            public PluginInfo Info
            {
                get;
                private set;
            }

            public PluginParameters Results
            {
                get;
                private set;
            }
        }

        [Serializable]
        public class ProbeCompleteEventArgs : EventArgs
        {
            public ProbeCompleteEventArgs(PluginInfoList infoList)
                : base()
            {
                this.InfoList = infoList;
            }

            public PluginInfoList InfoList
            {
                get;
                private set;
            }
        }

        [Serializable]
        public class PluginLoadingEventArgs : EventArgs
        {
            public PluginLoadingEventArgs(string assemblyFileName, AppDomainSetup domainSetup)
                : base()
            {
                this.AssemblyFileName = assemblyFileName;
                this.DomainSetup = domainSetup;
                this.Cancel = false;
            }

            public string AssemblyFileName
            {
                get;
                private set;
            }

            public AppDomainSetup DomainSetup
            {
                get;
                private set;
            }

            public bool Cancel { get; set; }
        }
        
        public delegate void PluginLoadingEventHandler(object sender, PluginLoadingEventArgs e);
        public delegate void PluginLoadedEventHandler(object sender, PluginLoadedEventArgs e);
        public delegate void PluginUnloadedEventHandler(object sender, PluginUnloadedEventArgs e);
        public delegate void ProbeCompleteEventHandler(object sender, ProbeCompleteEventArgs e);
        
        public event PluginLoadingEventHandler PluginLoading;
        public event PluginLoadedEventHandler PluginLoaded;
        public event PluginUnloadedEventHandler PluginUnloaded;
        public event ProbeCompleteEventHandler ProbeComplete;
        
        private PluginBase _loadedPlugin;
        private PluginInfoList _loadedInfoList;

        protected abstract PluginInfoList OnProbe(string pluginFolderPath, SearchOption scanDeapth);
        protected abstract PluginInfo OnGetInfo(string assemblyFileName);
        protected abstract PluginInfo OnLoad(string assemblyFileName);
        protected abstract PluginParameters OnExecute(PluginParameters args);
        protected abstract void OnUnloadNotification();

        protected PluginLoaderBase()
        {
            this.IsPluginLoaded = false;
            this.EventsEnabled = true;

            _loadedInfoList = new PluginInfoList();
            this.AssemblyResolutionPaths = new List<string>();

            this.ShadowCopyEnabled = false;
        }

        protected PluginBase LoadedPlugin
        {
            get { return _loadedPlugin; }
            set
            {
                this.IsPluginLoaded = (value != null);
                _loadedPlugin = value;
            }
        }

        public List<string> AssemblyResolutionPaths { get; internal set; }

        private bool IsPluginLoaded { get; set; }

        public bool EventsEnabled { get; set; }

        public bool ShadowCopyEnabled { get; set; }

        public ReadOnlyCollection<PluginInfo> LoadedPlugins
        {
            get { return new ReadOnlyCollection<PluginInfo>(this._loadedInfoList); }
        }

        protected virtual void OnPluginLoading(PluginLoadingEventArgs e)
        {
            if (this.EventsEnabled)
            {
                if (this.PluginLoading != null)
                    this.PluginLoading(this, e);
            }
        }

        protected virtual void OnPluginLoaded(PluginLoadedEventArgs e)
        {
            if (this.EventsEnabled)
            {
                if (this.PluginLoaded != null)
                    this.PluginLoaded(this, e);
            }
        }

        protected virtual void OnPluginUnloaded(PluginUnloadedEventArgs e)
        {
            if (this.EventsEnabled)
            {
                if (this.PluginUnloaded != null)
                    this.PluginUnloaded(this, e);
            }
        }

        protected virtual void OnProbeComplete(ProbeCompleteEventArgs e)
        {
            if (this.EventsEnabled)
            {
                if (this.ProbeComplete != null)
                    this.ProbeComplete(this, e);
            }
        }
                
        public virtual PluginInfoList Probe(string pluginFolderPath, SearchOption scanDeapth)
        {
            AppDomain domain = null;
            PluginInfoList infoList = new PluginInfoList();

            domain = AppDomain.CreateDomain("PluginLoaderBase.Probe");
            Type t = this.GetType();
            PluginLoaderBase loader = (PluginLoaderBase)domain.CreateInstanceFromAndUnwrap(t.Assembly.Location, t.FullName);
            infoList = loader.OnProbeWrapper(pluginFolderPath, scanDeapth);

            AppDomain.Unload(domain);

            ProbeCompleteEventArgs probeCompleteArgs = new ProbeCompleteEventArgs(infoList);
            this.OnProbeComplete(probeCompleteArgs);

            return infoList;
        }
        protected PluginInfoList OnProbeWrapper(string pluginFolderPath, SearchOption scanDeapth)
        {
            try
            {
                return this.OnProbe(pluginFolderPath, scanDeapth);
            }
            catch (ThreadAbortException ex) { throw ex; }
            catch (Exception ex)
            {
                throw new PluginException(ex);
            }
        }

        public virtual PluginInfo GetInfo(string assemblyFileName)
        {
            AppDomain domain = null;

            domain = AppDomain.CreateDomain("PluginLoaderBase.Probe");
            Type t = this.GetType();
            PluginLoaderBase loader = (PluginLoaderBase)domain.CreateInstanceFromAndUnwrap(t.Assembly.Location, t.FullName);
            PluginInfo info = loader.OnGetInfoWrapper(assemblyFileName);

            AppDomain.Unload(domain);

            return info;
        }
        protected PluginInfo OnGetInfoWrapper(string assemblyFileName)
        {
            try
            {
                return this.OnGetInfo(assemblyFileName);
            }
            catch (ThreadAbortException ex) { throw ex; }
            catch (Exception ex)
            {
                throw new PluginException(ex);
            }
        }

        public virtual void Load(string assemblyFileName, PluginParameters args)
        {
            if (args == null)
                args = new PluginParameters();

            AppDomainSetup domainSetup = new AppDomainSetup();
            Assembly entryAsm = Assembly.GetEntryAssembly();
            AssemblyProductAttribute[] atts = ((AssemblyProductAttribute[])entryAsm.GetCustomAttributes(typeof(AssemblyProductAttribute), false));
            string productName = null;
            if (atts != null)
            {
                if (atts.Length > 0)
                    productName = atts[0].Product;
            }
            
            if (String.IsNullOrEmpty(productName))
                domainSetup.ApplicationName = Path.GetFileNameWithoutExtension(entryAsm.Location);
            else
                domainSetup.ApplicationName = productName;

            domainSetup.ConfigurationFile = Path.GetFileName(assemblyFileName) + ".config";
            domainSetup.ApplicationBase = Path.GetDirectoryName(assemblyFileName);
            domainSetup.PrivateBinPath = "bin";

            if (this.ShadowCopyEnabled)
            {
                domainSetup.ShadowCopyFiles = "true";
                domainSetup.ShadowCopyDirectories = Path.GetDirectoryName(assemblyFileName) + ";" + Path.Combine(Path.GetDirectoryName(assemblyFileName), "bin");                
            }

            PluginLoadingEventArgs loadingArgs = new PluginLoadingEventArgs(assemblyFileName, domainSetup);
            this.OnPluginLoading(loadingArgs);

            if (!loadingArgs.Cancel)
            {
                AppDomain domain = null;

                domain = AppDomain.CreateDomain(loadingArgs.DomainSetup.ApplicationName, null, loadingArgs.DomainSetup);
                Type t = this.GetType();
                PluginLoaderBase loader = (PluginLoaderBase)domain.CreateInstanceFromAndUnwrap(t.Assembly.Location, t.FullName);

                loader.AssemblyResolutionPaths = this.AssemblyResolutionPaths;
                PluginInfo info = loader.OnLoadWrapper(assemblyFileName);

                if (info == null)
                    AppDomain.Unload(domain);
                else
                {
                    if (loader.IsPluginLoaded)
                    {
                        info.Domain = domain;
                        this._loadedInfoList.Add(info);

                        PluginLoadedEventArgs pluginLoadedArgs = new PluginLoadedEventArgs(info);
                        this.OnPluginLoaded(pluginLoadedArgs);

                        PluginBase.ExecutionModes execMode = info.ExecutionMode;
                        bool canExecute = false;
                        if (execMode == PluginBase.ExecutionModes.AsynchronousMultiInstance || execMode == PluginBase.ExecutionModes.SynchronousMultiInstance)
                            canExecute = true;
                        else if (execMode == PluginBase.ExecutionModes.AsynchronousExclusive || execMode == PluginBase.ExecutionModes.SynchronousExclusive)
                        {
                            if (this._loadedInfoList.Count == 1)
                                canExecute = true;
                        }
                        else if (execMode == PluginBase.ExecutionModes.AsynchronousSingleton || execMode == PluginBase.ExecutionModes.SynchronousSingleton)
                        {
                            if (this.GetLoadedInstances(info.FileName).Count == 1)
                                canExecute = true;
                        }

                        if (canExecute)
                        {
                            info.Loader = loader;
                            info.Parameters = args;

                            if (execMode == PluginBase.ExecutionModes.SynchronousSingleton || execMode == PluginBase.ExecutionModes.SynchronousExclusive || execMode == PluginBase.ExecutionModes.SynchronousMultiInstance)
                                this.SynchronousExecute(info);
                            else
                                this.AsynchronousExecute(info);
                        }
                        else
                        {
                            args.ExecutionDenied = true;
                            this.Unload(info, args);
                        }
                    }
                    else
                        AppDomain.Unload(domain);
                }
            }
        }
        protected PluginInfo OnLoadWrapper(string assemblyFileName)
        {
            try
            {
                return this.OnLoad(assemblyFileName);
            }
            catch (ThreadAbortException ex) { throw ex; }
            catch (Exception ex)
            {
                throw new PluginException(ex);
            }
        }
                
        private void AsynchronousExecute(PluginInfo info)
        {
            Thread t = new Thread(this.SynchronousExecute);
            t.IsBackground = true;
            t.Start(info);
        }        
        private void SynchronousExecute(object infoObj)
        {
            PluginInfo info = (PluginInfo)infoObj;

            bool unloadedAlready = false;
            PluginParameters results = null;
            try
            {
                results = info.Loader.OnExecuteWrapper(info.Parameters);
            }
            catch (ThreadAbortException)
            {
                this.Unload(info, null);
                unloadedAlready = true;
            }

            if (!unloadedAlready)
                this.Unload(info, results);
        }
        private PluginParameters OnExecuteWrapper(PluginParameters args)
        {
            try
            {
                return this.OnExecute(args);
            }
            catch (ThreadAbortException ex) { throw ex; }
            catch (Exception ex)
            {
                args.UnhandledException = new PluginException(ex);
                return args;
            }
        }

        private void Unload(PluginInfo info, PluginParameters results)
        {
            if (info != null)
            {
                try
                {
                    AppDomain.Unload(info.Domain);
                }
                catch (AppDomainUnloadedException) { }

                this._loadedInfoList.Remove(info);

                PluginUnloadedEventArgs pluginUnloadedArgs = new PluginUnloadedEventArgs(info, results);
                this.OnPluginUnloaded(pluginUnloadedArgs);
            }
        }

        public virtual void NotifyUnload(PluginInfo info)
        {
            try
            {
                info.Loader.OnUnloadNotificationWrapper();
            }
            catch (Exception ex)
            {
                throw new PluginException(ex);
            }
        }
        protected void OnUnloadNotificationWrapper()
        {
            try
            {
                this.OnUnloadNotification();
            }
            catch (ThreadAbortException ex) { throw ex; }
            catch (Exception ex)
            {
                throw new PluginException(ex);
            }
        }

        public virtual void NotifyUnloadAll()
        {
            foreach (PluginInfo info in _loadedInfoList)
            {
                try
                {
                    Thread notifyThread = new Thread(new ThreadStart(ref info.Loader.OnUnloadNotification));
                    notifyThread.IsBackground = true;
                    notifyThread.Start();
                }
                catch (PluginException) { }
            }
        }

        public virtual PluginInfoList GetLoadedInstances(string assemblyFileName)
        {
            PluginInfoList list = new PluginInfoList();

            for (int i = 0; i < this._loadedInfoList.Count; i++)
            {
                if (assemblyFileName.Equals(this._loadedInfoList[i].FileName))
                    list.Add(this._loadedInfoList[i]);
            }

            return list;
        }
    }
}
