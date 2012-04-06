using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimplePlugins.Exceptions
{
    public class InvalidPluginFileException:Exception
    {        
        public InvalidPluginFileException(string pluginFileName)
            : base("The specified file '" + pluginFileName + " is not a valid plugin file.")
        {
            this.PluginFileName = pluginFileName;
        }

        public string PluginFileName { get; private set; }
    }
}
