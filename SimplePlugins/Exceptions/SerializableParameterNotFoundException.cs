using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimplePlugins.Exceptions
{
    public class SerializableParameterNotFoundException:Exception
    {
        public SerializableParameterNotFoundException(string parameterName)
            : base("The specified serializable parameter '" + parameterName + "' was not found.")
        {
            this.ParameterName = parameterName;
        }

        public string ParameterName { get; private set; }
    }
}
