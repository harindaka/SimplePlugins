using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel;
using SimplePlugins.Exceptions;
using System.Collections.Generic;
using SimplePlugins;
using SimplePlugins.Loaders;

namespace SimplePlugins.Loaders
{    
    [Serializable]
    public class PluginLoader : PluginLoaderBase
    {
        protected const string  PYTHON_PROXY_PLUGIN_FILE_NAME = "SimplePlugins.IronPython.dll";

        Assembly _loadedPluginAssembly;

        public PluginLoader()
            : base()
        {

        }

        protected override PluginInfo OnGetInfo(string pluginFileName)
        {
            PluginInfo.PluginTypes pluginType = PluginInfo.DeterminePluginType(pluginFileName);

            AssemblyName assName = null;
            if (pluginType == PluginInfo.PluginTypes.PythonScript)
            {
                string pythonProxyPluginPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                pythonProxyPluginPath = Path.Combine(pythonProxyPluginPath, PluginLoader.PYTHON_PROXY_PLUGIN_FILE_NAME);
                assName = AssemblyName.GetAssemblyName(pythonProxyPluginPath);
            }
            else
                assName = AssemblyName.GetAssemblyName(pluginFileName);

            _loadedPluginAssembly = AppDomain.CurrentDomain.Load(assName);

            Type[] types = _loadedPluginAssembly.GetTypes();
            Type pluginBaseType = typeof(PluginBase);

            foreach (Type t in types)
            {
                if (t.IsSubclassOf(pluginBaseType))
                {
                    PluginBase pi = null;
                    pi = (PluginBase)Activator.CreateInstance(t, pluginFileName);
                    
                    PluginInfo info = new PluginInfo(pi);
                    return info;
                }
            }

            return null;
        }        

        protected override PluginInfoList OnProbe(string pluginFolderPath, SearchOption scanDeapth)
        {
            PluginInfoList list = new PluginInfoList();

            string[] exes = Directory.GetFiles(pluginFolderPath, "*.exe", scanDeapth);
            string[] dlls = Directory.GetFiles(pluginFolderPath, "*.dll", scanDeapth);
            string[] pys = Directory.GetFiles(pluginFolderPath, "*.py", scanDeapth);

            List<string> files = new List<string>();
            files.AddRange(exes);
            files.AddRange(dlls);
            files.AddRange(pys);

            foreach (string file in files)
            {
                try
                {
                    PluginInfo info = this.OnGetInfo(file);

                    if (info != null)
                        list.Add(info);
                }
                catch (Exception) { }
            }

            return list;
        }

        protected override PluginInfo OnLoad(string pluginFileName)
        {
            if (File.Exists(pluginFileName))
            {
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

                PluginInfo info = this.OnGetInfo(pluginFileName);

                if (info == null)
                    throw new InvalidPluginFileException(pluginFileName);
                else
                {
                    this.LoadedPlugin = (PluginBase)Activator.CreateInstance(_loadedPluginAssembly.GetType(info.PluginTypeName), pluginFileName);
                    
                    return info;
                }
            }
            else
                throw new FileNotFoundException("Specified assembly file '" + pluginFileName + "' does not exist.", pluginFileName);
        }

        protected override PluginParameters OnExecute(PluginParameters args)
        {
            return this.LoadedPlugin.Main(args);
        }

        protected override void OnAbort()
        {
            if (this.LoadedPlugin != null)
                this.LoadedPlugin.OnAbort();
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in loadedAssemblies)
            {
                if (asm.FullName.Equals(args.Name))
                    return asm;
            }

            AssemblyName asmInfo = new AssemblyName(args.Name);
            for (int i = 0; i < this.AssemblyResolutionPaths.Count; i++)
            {
                string asmFilePath = Path.Combine(this.AssemblyResolutionPaths[i], asmInfo.Name + ".dll");
                if (File.Exists(asmFilePath))
                    return Assembly.LoadFrom(asmFilePath);
                else
                {
                    asmFilePath = Path.Combine(this.AssemblyResolutionPaths[i], asmInfo.Name + ".exe");
                    if (File.Exists(asmFilePath))
                        return Assembly.LoadFrom(asmFilePath);
                }
            }

            return null;
        }
    }
}
