using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SimplePlugins.Exceptions;

namespace SimplePlugins
{
    [Serializable]
    public sealed class PluginParameters:SerializableParameters
    {
        public PluginParameters():base()
        {
            
        }
                
        internal PluginException PluginException { get; set; }               
    }
}
