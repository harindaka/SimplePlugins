using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimplePlugins.Exceptions
{
    public class InvalidPluginAssemblyException:Exception
    {        
        public InvalidPluginAssemblyException(string assemblyFileName)
            : base("The specified assembly '" + assemblyFileName + " is not a valid plugin assembly.")
        {
            this.AssemblyFileName = assemblyFileName;
        }

        public string AssemblyFileName { get; private set; }
    }
}
