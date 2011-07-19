using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Configuration;
using System.Threading;
using SimplePlugins.Exceptions;
using System.Collections.ObjectModel;

namespace SimplePlugins
{
    [Serializable]
    public abstract class PluginBase
    {
        public enum ExecutionModes { Singleton, MultiInstance, Exclusive, Custom };

        public PluginBase()
        {
            PluginBase.Current = this;
            this.UnhandledThreadExceptionsList = new List<Exception>();
            this.UnhandledThreadExceptions = new ReadOnlyCollection<Exception>(this.UnhandledThreadExceptionsList); 
        }

        public Exception UnhandledException { get; internal set; }

        public ReadOnlyCollection<Exception> UnhandledThreadExceptions { get; private set; }

        internal List<Exception> UnhandledThreadExceptionsList { get; private set; }

        public static PluginBase Current { get; private set; }

        public abstract string FriendlyName { get; }

        public abstract ExecutionModes ExecutionMode { get; }

        public abstract PluginParameters Main(PluginParameters args);

        public abstract void OnAbort();

        public static void DoEvents()
        {
            PluginBase plugin = PluginBase.Current;
            if (plugin != null)
            {
                if (plugin.UnhandledThreadExceptions.Count > 0)
                    throw new PluginThreadException(plugin.UnhandledThreadExceptions);
            }
        }

        public Thread CreateManagedThread(ThreadStart ts)
        {
            Thread t = new Thread(delegate()
            {
                try
                {
                    ts();
                }
                catch (Exception ex)
                {
                    this.UnhandledThreadExceptionsList.Add(ex);
                }
            }
            );
            
            return t;
        }
        public Thread CreateManagedThread(ParameterizedThreadStart ts)
        {
            Thread t = new Thread(delegate(object arg)
            {
                try
                {
                    ts(arg);
                }
                catch (Exception ex)
                {
                    this.UnhandledThreadExceptionsList.Add(ex);
                }
            }
            );

            return t;
        }
        public Thread CreateManagedThread(ThreadStart ts, int maxStackSize)
        {
            Thread t = new Thread(delegate()
            {
                try
                {
                    ts();
                }
                catch (Exception ex)
                {
                    this.UnhandledThreadExceptionsList.Add(ex);
                }
            }
            , maxStackSize);

            return t;
        }
        public Thread CreateManagedThread(ParameterizedThreadStart ts, int maxStackSize)
        {
            Thread t = new Thread(delegate(object arg)
            {
                try
                {
                    ts(arg);
                }
                catch (Exception ex)
                {
                    this.UnhandledThreadExceptionsList.Add(ex);
                }
            }
            , maxStackSize);

            return t;
        }
    }
}
