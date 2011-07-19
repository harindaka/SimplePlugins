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
        Assembly _loadedPluginAssembly;

        public PluginLoader()
            : base()
        {

        }

        protected override PluginInfo OnGetInfo(string assemblyFileName)
        {
            AssemblyName assName = AssemblyName.GetAssemblyName(assemblyFileName);

            _loadedPluginAssembly = AppDomain.CurrentDomain.Load(assName);

            Type[] types = _loadedPluginAssembly.GetTypes();
            Type pluginBaseType = typeof(PluginBase);

            foreach (Type t in types)
            {
                if (t.IsSubclassOf(pluginBaseType))
                {
                    PluginBase pi = (PluginBase)Activator.CreateInstance(t);
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

            List<string> files = new List<string>();
            files.AddRange(exes);
            files.AddRange(dlls);

            foreach (string file in files)
            {
                PluginInfo info = this.OnGetInfo(file);
                if (info != null)
                    list.Add(info);
            }

            return list;
        }

        protected override PluginInfo OnLoad(string assemblyFileName)
        {
            if (File.Exists(assemblyFileName))
            {
                AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

                PluginInfo info = this.OnGetInfo(assemblyFileName);

                if (info == null)
                    throw new InvalidPluginAssemblyException(assemblyFileName);
                else
                {
                    this.LoadedPlugin = (PluginBase)Activator.CreateInstance(_loadedPluginAssembly.GetType(info.PluginTypeName));

                    return info;
                }
            }
            else
                throw new FileNotFoundException("Specified assembly file '" + assemblyFileName + "' does not exist.", assemblyFileName);
        }

        protected override PluginParameters OnExecute(PluginParameters args)
        {
            return this.LoadedPlugin.Main(args);
        }

        protected override void OnAbort()
        {
            if (this.LoadedPlugin != null)
                this.LoadedPlugin.OnAbort(null);
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
