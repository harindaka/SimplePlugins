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
        public class ManagedThreadExceptionEventArgs : EventArgs
        {
            public ManagedThreadExceptionEventArgs(ManagedThread mt)
                : base()
            {
                this.ManagedThread = mt;                
            }

            public ManagedThread ManagedThread { get; private set; }
        }

        public delegate void ManagedThreadExceptionHandler(object sender, ManagedThreadExceptionEventArgs e);

        public event ManagedThreadExceptionHandler ManagedThreadException;

        public enum ManagedThreadDeregistrationModes { Never, UponCompletion, UponException, UponSuccess };

        public enum ExecutionModes { Singleton, MultiInstance, Exclusive, Custom };

        public PluginBase()
        {
            PluginBase.Current = this;
            this.ManagedThreads = new ReadOnlyDictionary<string, ManagedThread>();
        }

        public Exception UnhandledException { get; internal set; }

        public ReadOnlyDictionary<string, ManagedThread> ManagedThreads { get; private set; }

        public static PluginBase Current { get; private set; }

        public abstract string FriendlyName { get; }

        public abstract ExecutionModes ExecutionMode { get; }

        public abstract PluginParameters Main(PluginParameters args);

        public abstract void OnAbort();

        protected virtual void OnManagedThreadException(ManagedThreadExceptionEventArgs e)
        {
            ManagedThreadExceptionHandler evnt = this.ManagedThreadException;
            if (evnt != null)
                this.ManagedThreadException(this, e);
        }

        public ManagedThread CreateManagedThread(ThreadStart ts, ManagedThreadDeregistrationModes deregister = ManagedThreadDeregistrationModes.UponSuccess, string threadID = null, int maxStackSize = 0)
        {
            if (String.IsNullOrEmpty(threadID))
                threadID = Guid.NewGuid().ToString();

            Thread t = new Thread(delegate()
            {
                try
                {
                    ts();

                    if (deregister == ManagedThreadDeregistrationModes.UponSuccess)
                        this.ManagedThreads.Items.Remove(threadID);
                }
                catch (Exception ex)
                {
                    ManagedThread errorThread = this.ManagedThreads[threadID];
                    errorThread.UnhandledException = ex;

                    this.OnManagedThreadException(new ManagedThreadExceptionEventArgs(errorThread));

                    if (deregister == ManagedThreadDeregistrationModes.UponSuccess)
                        this.ManagedThreads.Items.Remove(threadID);
                }
                finally
                {
                    if (deregister == ManagedThreadDeregistrationModes.UponCompletion)
                        this.ManagedThreads.Items.Remove(threadID);
                }
            }, maxStackSize);

            ManagedThread mt = new ManagedThread(t, threadID);

            if (this.ManagedThreads.ContainsKey(threadID))
                throw new Exception("The specified new managed Thread ID '" + threadID + "' already exists.");
            else
                this.ManagedThreads.Items.Add(threadID, mt);

            return mt;
        }
        public ManagedThread CreateManagedThread(ParameterizedThreadStart ts, ManagedThreadDeregistrationModes deregister = ManagedThreadDeregistrationModes.UponSuccess, string threadID = null, int maxStackSize = 0)
        {
            if (String.IsNullOrEmpty(threadID))
                threadID = Guid.NewGuid().ToString();

            Thread t = new Thread(delegate(object args)
            {
                try
                {
                    ts(args);

                    if (deregister == ManagedThreadDeregistrationModes.UponSuccess)
                        this.ManagedThreads.Items.Remove(threadID);
                }
                catch (Exception ex)
                {
                    ManagedThread errorThread = this.ManagedThreads[threadID];
                    errorThread.UnhandledException = ex;

                    this.OnManagedThreadException(new ManagedThreadExceptionEventArgs(errorThread));

                    if (deregister == ManagedThreadDeregistrationModes.UponSuccess)
                        this.ManagedThreads.Items.Remove(threadID);
                }
                finally
                {
                    if (deregister == ManagedThreadDeregistrationModes.UponCompletion)
                        this.ManagedThreads.Items.Remove(threadID);
                }
            }, maxStackSize);

            ManagedThread mt = new ManagedThread(t, threadID);

            if (this.ManagedThreads.ContainsKey(threadID))
                throw new Exception("The specified new managed Thread ID '" + threadID + "' already exists.");
            else
                this.ManagedThreads.Items.Add(threadID, mt);

            return mt;
        }
    }
}
