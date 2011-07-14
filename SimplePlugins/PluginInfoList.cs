using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimplePlugins
{
    [Serializable]
    public class PluginInfoList : List<PluginInfo>
    {
        public PluginInfoList() : base() { }
    }
}
