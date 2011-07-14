using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimplePlugins.Exceptions
{
    public class InvalidPluginParameterException:Exception
    {
        public InvalidPluginParameterException(string parameterName)
            : base("The specified plugin parameter '" + parameterName + "' is not marked as serializeable.")
        {
            this.ParameterName = parameterName;
        }

        public string ParameterName { get; private set; }
    }
}
