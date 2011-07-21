using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SimplePlugins.Exceptions
{
    public class PluginThreadException:Exception
    {
        public PluginThreadException(ReadOnlyCollection<Exception> threadExceptions)
            : base()
        {
            List<PluginException> serializableExList = new List<PluginException>();
            foreach (Exception ex in threadExceptions)
                serializableExList.Add(new PluginException(ex));

            this.ThreadExceptions = new ReadOnlyCollection<PluginException>(serializableExList);
        }

        public ReadOnlyCollection<PluginException> ThreadExceptions
        {
            get;
            private set;
        }
    }    
}
