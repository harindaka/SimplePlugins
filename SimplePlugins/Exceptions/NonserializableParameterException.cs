using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimplePlugins.Exceptions
{
    public class NonserializableParameterException:Exception
    {
        public NonserializableParameterException(string parameterName)
            : base("The specified serializable parameter '" + parameterName + "' is not marked as serializable.")
        {
            this.ParameterName = parameterName;
        }

        public string ParameterName { get; private set; }
    }
}
