using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SimplePlugins.Loaders;

namespace SimplePlugins
{
    [Serializable]
    public class PluginInfo
    {        
        public PluginInfo(PluginBase pluginBase)
        {
            this.FriendlyName = pluginBase.FriendlyName;
            this.ExecutionMode = pluginBase.ExecutionMode;

            Type type = pluginBase.GetType();
            this.FileName = type.Assembly.Location;
            this.AssemblyName = type.Assembly.GetName().Name;
            this.AssemblyFullName = type.Assembly.FullName;
            this.Version = type.Assembly.GetName().Version.ToString();
            this.PluginTypeName = type.FullName;

            try
            {
                if (File.Exists(type.Assembly.Location))
                    this.DateLastModified = new FileInfo(type.Assembly.Location).LastWriteTime;
                else
                    this.DateLastModified = null;
            }
            catch (Exception) { this.DateLastModified = null; }
        }
                
        internal AppDomain Domain { get; set; }

        internal PluginBase Plugin { get; set; }

        internal PluginLoaderBase Loader { get; set; }

        internal PluginParameters Parameters { get; set; }

        public string FriendlyName { get; private set; }

        public PluginBase.ExecutionModes ExecutionMode { get; private set; }

        public string PluginTypeName { get; private set; }

        public string FileName { get; private set; }

        public string AssemblyName { get; private set; }

        public string Version { get; private set; }

        public string AssemblyFullName { get; private set; }

        public DateTime? DateLastModified { get; private set; }

        public override string ToString()
        {
            string lastModDateTime;
            if (this.DateLastModified == null)
                lastModDateTime = "";
            else
                lastModDateTime = ", " + this.DateLastModified.Value.ToString("dd-MMM-yyyy hh:mm:ss tt");

            return this.FriendlyName + " (\"" + this.AssemblyName + "\", v" + this.Version + lastModDateTime + ")";
        }
    }
}
