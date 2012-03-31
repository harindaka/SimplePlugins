using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SimplePlugins.Exceptions;

namespace SimplePlugins
{    
    public class ManagedThread
    {
        internal ManagedThread(Thread t, string threadID)
        {
            this.Thread = t;
            this.ThreadID = threadID;
        }

        public string ThreadID { get; private set; }
        
        public Thread Thread { get; private set; }

        public Exception UnhandledException { get; internal set; }

        public object Tag { get; set; }
    }
}
