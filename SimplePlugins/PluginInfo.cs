using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SimplePlugins.Loaders;
using SimplePlugins.Exceptions;
using System.Diagnostics;

namespace SimplePlugins
{
    [Serializable]
    public class PluginInfo
    {
        public enum PluginTypes { NetDynamicLinkLibrary, NetExecutable, PythonScript };

        public PluginInfo(PluginBase pluginBase)
        {
            this.FriendlyName = pluginBase.FriendlyName;
            this.ExecutionMode = pluginBase.ExecutionMode;

            this.PluginFileName = pluginBase.PluginFileName;

            Type type = pluginBase.GetType();
            this.AssemblyFileName = type.Assembly.Location;
            this.PluginTypeName = type.FullName;

            this.IsUsingProxyPlugin = this.PluginFileName.ToLower().Equals(this.AssemblyFileName.ToLower());

            this.AssemblyFullName = type.Assembly.FullName;

            if (this.IsUsingProxyPlugin)
            {
                try
                {
                    this.PluginVersion = FileVersionInfo.GetVersionInfo(this.PluginFileName).ProductVersion;
                }
                catch (Exception) { this.PluginVersion = ""; }
            }
            else
                this.PluginVersion = type.Assembly.GetName().Version.ToString();
                        
            try
            {
                this.DateLastModified = new FileInfo(this.PluginFileName).LastWriteTime;                
            }
            catch (Exception) { this.DateLastModified = null; }
        }

        public static PluginTypes DeterminePluginType(string pluginFileName)
        {
            string ext = Path.GetExtension(pluginFileName).ToLower();
            if (ext.Equals(".exe"))
                return PluginTypes.NetExecutable;
            else if (ext.Equals(".dll"))
                return PluginTypes.NetDynamicLinkLibrary;
            else if (ext.Equals(".py"))
                return PluginTypes.PythonScript;
            else
                throw new InvalidPluginFileException(pluginFileName);
        }

        internal AppDomain Domain { get; set; }

        internal PluginBase Plugin { get; set; }

        internal PluginLoaderBase Loader { get; set; }

        internal PluginParameters Parameters { get; set; }

        public string FriendlyName { get; private set; }

        public PluginBase.ExecutionModes ExecutionMode { get; private set; }

        internal PluginTypes PluginType { get; private set; }

        public string PluginTypeName { get; private set; }

        public string AssemblyFileName { get; private set; }

        public string PluginFileName { get; private set; }

        public string PluginVersion { get; private set; }

        public string AssemblyFullName { get; private set; }

        public DateTime? DateLastModified { get; private set; }

        public bool IsUsingProxyPlugin { get; private set; }

        public override string ToString()
        {
            string lastModDateTime;
            if (this.DateLastModified == null)
                lastModDateTime = "";
            else
                lastModDateTime = ", " + this.DateLastModified.Value.ToString("dd-MMM-yyyy hh:mm:ss tt");

            string pluginName = Path.GetFileName(this.PluginFileName);

            return this.FriendlyName + " (\"" + pluginName + "\", v" + this.PluginVersion + lastModDateTime + ")";
        }
    }
}
