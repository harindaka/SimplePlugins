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
using System.Windows.Forms;

namespace SimplePlugins.Loaders
{
    [Serializable]
    public abstract class PluginLoaderBase : MarshalByRefObject
    {
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
            public PluginLoadingEventArgs(string pluginFileName, AppDomainSetup domainSetup)
                : base()
            {
                this.PluginFileName = pluginFileName;
                this.DomainSetup = domainSetup;
                this.Cancel = false;
            }

            public string PluginFileName
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

        public class PluginExecutingEventArgs : EventArgs
        {
            public PluginExecutingEventArgs(PluginInfo info)
                : base()
            {
                this.Info = info;
            }

            public PluginInfo Info
            {
                get;
                private set;
            }

            public bool Cancel { get; set; }

            public object CancelReason { get; set; }
        }

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

        public class PluginUnloadedEventArgs : EventArgs
        {
            public PluginUnloadedEventArgs(PluginInfo info, PluginParameters results, bool execCancelled, object execCancelledReason)
                : base()
            {
                this.Info = info;
                if (results == null)
                    this.Results = new PluginParameters();
                else
                    this.Results = results;

                this.ExecutionCancelled = execCancelled;
                if (execCancelled)
                    this.ExecutionCancelledReason = execCancelledReason;
                else
                    this.PluginException = results.PluginException;
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

            public bool ExecutionCancelled { get; private set; }

            public object ExecutionCancelledReason { get; private set; }

            public PluginException PluginException { get; private set; }
        }

        public delegate void ProbeCompleteEventHandler(object sender, ProbeCompleteEventArgs e);
        public delegate void PluginLoadingEventHandler(object sender, PluginLoadingEventArgs e);
        public delegate void PluginLoadedEventHandler(object sender, PluginLoadedEventArgs e);
        public delegate void PluginExecutingEventHandler(object sender, PluginExecutingEventArgs e);
        public delegate void PluginUnloadedEventHandler(object sender, PluginUnloadedEventArgs e);

        public event ProbeCompleteEventHandler ProbeComplete;
        public event PluginLoadingEventHandler PluginLoading;
        public event PluginLoadedEventHandler PluginLoaded;
        public event PluginExecutingEventHandler PluginExecuting;
        public event PluginUnloadedEventHandler PluginUnloaded;

        private PluginBase _currentPlugin;
        private PluginInfoList _currentInfoList;

        protected abstract PluginInfoList OnProbe(string pluginFolderPath, SearchOption scanDeapth);
        protected abstract PluginInfo OnGetInfo(string pluginFileName);
        protected abstract PluginInfo OnLoad(string pluginFileName);
        protected abstract PluginParameters OnExecute(PluginParameters args);
        protected abstract void OnAbort();

        protected PluginLoaderBase()
        {
            this.IsPluginLoaded = false;
            this.EventsEnabled = true;

            _currentInfoList = new PluginInfoList();
            this.AssemblyResolutionPaths = new List<string>();

            this.ShadowCopyEnabled = false;
            this.UseDefaultExecutionValidation = true;
        }

        protected PluginBase LoadedPlugin
        {
            get { return _currentPlugin; }
            set
            {
                this.IsPluginLoaded = (value != null);
                _currentPlugin = value;
            }
        }

        public List<string> AssemblyResolutionPaths { get; internal set; }

        private bool IsPluginLoaded { get; set; }

        public bool EventsEnabled { get; set; }

        public bool ShadowCopyEnabled { get; set; }

        public bool UseDefaultExecutionValidation { get; set; }

        public ReadOnlyCollection<PluginInfo> LoadedPlugins
        {
            get { return new ReadOnlyCollection<PluginInfo>(this._currentInfoList); }
        }

        protected virtual void OnPluginLoading(PluginLoadingEventArgs e)
        {
            if (this.EventsEnabled)
            {
                var evnt = this.PluginLoading;
                if (evnt != null)
                    evnt(this, e);
            }
        }

        protected virtual void OnPluginLoaded(PluginLoadedEventArgs e)
        {
            if (this.EventsEnabled)
            {
                var evnt = this.PluginLoaded;
                if (evnt != null)
                    evnt(this, e);
            }
        }

        protected virtual void OnPluginExecuting(PluginExecutingEventArgs e)
        {
            if (this.EventsEnabled)
            {
                var evnt = this.PluginExecuting;
                if (evnt != null)
                    evnt(this, e);
            }
        }

        protected virtual void OnPluginUnloaded(PluginUnloadedEventArgs e)
        {
            if (this.EventsEnabled)
            {
                var evnt = this.PluginUnloaded;
                if (evnt != null)
                    evnt(this, e);
            }
        }

        protected virtual void OnProbeComplete(ProbeCompleteEventArgs e)
        {
            if (this.EventsEnabled)
            {
                var evnt = this.ProbeComplete;
                if (evnt != null)
                    evnt(this, e);
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

        public virtual PluginInfo GetInfo(string pluginFileName)
        {
            AppDomain domain = null;

            domain = AppDomain.CreateDomain("PluginLoaderBase.Probe");
            Type t = this.GetType();
            PluginLoaderBase loader = (PluginLoaderBase)domain.CreateInstanceFromAndUnwrap(t.Assembly.Location, t.FullName);
            PluginInfo info = loader.OnGetInfoWrapper(pluginFileName);

            AppDomain.Unload(domain);

            return info;
        }
        protected PluginInfo OnGetInfoWrapper(string pluginFileName)
        {
            try
            {
                return this.OnGetInfo(pluginFileName);
            }
            catch (ThreadAbortException ex) { throw ex; }
            catch (Exception ex)
            {
                throw new PluginException(ex);
            }
        }

        public virtual void Load(string pluginFileName, PluginParameters args)
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

            domainSetup.ConfigurationFile = Path.GetFileName(pluginFileName) + ".config";
            domainSetup.ApplicationBase = Path.GetDirectoryName(pluginFileName);
            domainSetup.PrivateBinPath = "bin";

            if (this.ShadowCopyEnabled)
            {
                domainSetup.ShadowCopyFiles = "true";
                domainSetup.ShadowCopyDirectories = domainSetup.ApplicationBase + ";" + Path.Combine(domainSetup.ApplicationBase, "bin");
            }

            PluginLoadingEventArgs loadingArgs = new PluginLoadingEventArgs(pluginFileName, domainSetup);
            this.OnPluginLoading(loadingArgs);

            if (!loadingArgs.Cancel)
            {
                AppDomain domain = null;

                domain = AppDomain.CreateDomain(loadingArgs.DomainSetup.ApplicationName, null, loadingArgs.DomainSetup);
                Type t = this.GetType();
                PluginLoaderBase loader = (PluginLoaderBase)domain.CreateInstanceFromAndUnwrap(t.Assembly.Location, t.FullName);

                loader.AssemblyResolutionPaths = this.AssemblyResolutionPaths;
                PluginInfo info = loader.OnLoadWrapper(pluginFileName);

                if (info == null)
                    AppDomain.Unload(domain);
                else
                {
                    if (loader.IsPluginLoaded)
                    {
                        info.Domain = domain;
                        this._currentInfoList.Add(info);

                        PluginLoadedEventArgs pluginLoadedArgs = new PluginLoadedEventArgs(info);
                        this.OnPluginLoaded(pluginLoadedArgs);

                        PluginBase.ExecutionModes execMode = info.ExecutionMode;
                        bool canExecute = false;

                        if (!this.UseDefaultExecutionValidation)
                            canExecute = true;
                        else if (execMode == PluginBase.ExecutionModes.MultiInstance)
                            canExecute = true;
                        else if (execMode == PluginBase.ExecutionModes.Exclusive)
                        {
                            if (this._currentInfoList.Count == 1)
                                canExecute = true;
                        }
                        else if (execMode == PluginBase.ExecutionModes.Singleton)
                        {
                            if (this.GetLoadedInstances(info.PluginFileName).Count == 1)
                                canExecute = true;
                        }
                        else if (execMode == PluginBase.ExecutionModes.Custom)
                            canExecute = true;

                        if (canExecute)
                        {
                            info.Parameters = args;
                            PluginExecutingEventArgs exeArgs = new PluginExecutingEventArgs(info);
                            this.OnPluginExecuting(exeArgs);

                            if (exeArgs.Cancel)
                                this.Unload(info, args, true, exeArgs.CancelReason);
                            else
                            {
                                info.Loader = loader;

                                Thread pluginThread = new Thread(this.Execute);
                                pluginThread.IsBackground = true;
                                pluginThread.Start(info);
                            }
                        }
                        else
                            this.Unload(info, args, true, null);
                    }
                    else
                        AppDomain.Unload(domain);
                }
            }
        }
        protected PluginInfo OnLoadWrapper(string pluginFileName)
        {
            try
            {
                return this.OnLoad(pluginFileName);
            }
            catch (ThreadAbortException ex) { throw ex; }
            catch (Exception ex)
            {
                throw new PluginException(ex);
            }
        }

        private void Execute(object infoObj)
        {
            PluginInfo info = (PluginInfo)infoObj;
            PluginParameters results = info.Loader.OnExecuteWrapper(info);
            this.Unload(info, results, false, null);
        }
        private PluginParameters OnExecuteWrapper(PluginInfo info)
        {
            try
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

                PluginParameters results = this.OnExecute(info.Parameters);

                if (_currentPlugin.UnhandledException != null)
                    throw _currentPlugin.UnhandledException;

                return results;
            }
            catch (ThreadAbortException ex) { throw ex; }
            catch (PluginException ex)
            {
                info.Parameters.PluginException = ex;
                return info.Parameters;
            }
            catch (Exception ex)
            {
                info.Parameters.PluginException = new PluginException(ex);
                return info.Parameters;
            }
        }

        void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            _currentPlugin.UnhandledException = new PluginException(e.Exception);
            Application.ExitThread();
        }

        private void Unload(PluginInfo info, PluginParameters results, bool execCancelled, object execCancelledReason)
        {
            if (info != null)
            {
                try
                {
                    AppDomain.Unload(info.Domain);
                }
                catch (AppDomainUnloadedException) { }

                this._currentInfoList.Remove(info);

                PluginUnloadedEventArgs pluginUnloadedArgs = new PluginUnloadedEventArgs(info, results, execCancelled, execCancelledReason);
                this.OnPluginUnloaded(pluginUnloadedArgs);
            }
        }

        public virtual void RequestAbort(PluginInfo info)
        {
            info.Loader.OnAbortWrapper();
        }

        protected void OnAbortWrapper()
        {
            try
            {
                this.OnAbort();
            }            
            catch (Exception) { }
        }

        public virtual void RequestAbortAll()
        {
            foreach (PluginInfo info in _currentInfoList)
            {
                Thread notifyThread = new Thread(new ThreadStart(info.Loader.OnAbortWrapper));
                notifyThread.IsBackground = true;
                notifyThread.Start();
            }
        }

        public virtual PluginInfoList GetLoadedInstances(string pluginFileName)
        {
            PluginInfoList list = new PluginInfoList();

            for (int i = 0; i < this._currentInfoList.Count; i++)
            {
                if (pluginFileName.Equals(this._currentInfoList[i].PluginFileName))
                    list.Add(this._currentInfoList[i]);
            }

            return list;
        }
    }
}
