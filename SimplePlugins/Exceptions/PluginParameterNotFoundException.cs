using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimplePlugins.Exceptions
{
    public class PluginParameterNotFoundException:Exception
    {
        public PluginParameterNotFoundException(string parameterName)
            : base("The specified plugin parameter '" + parameterName + "' was not found.")
        {
            this.ParameterName = parameterName;
        }

        public string ParameterName { get; private set; }
    }
}
