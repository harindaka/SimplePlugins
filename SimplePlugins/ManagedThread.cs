using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SimplePlugins
{
    public class ManagedThread
    {
        internal ManagedThread(Thread t)
        {
            this.Thread = t;
        }

        public Thread Thread { get; private set; }

        public Exception UnhandledException { get; internal set; }
    }
}
