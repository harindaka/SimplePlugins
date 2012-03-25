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
            this.ManagedThreads = new ReadOnlyDictionary<object, ManagedThread>();
        }

        public Exception UnhandledException { get; internal set; }

        public ReadOnlyDictionary<object, ManagedThread> ManagedThreads { get; private set; }

        public static PluginBase Current { get; private set; }

        public abstract string FriendlyName { get; }

        public abstract ExecutionModes ExecutionMode { get; }

        public abstract PluginParameters Main(PluginParameters args);

        public abstract void OnAbort();

        public ManagedThread CreateManagedThread(object threadID, ThreadStart ts)
        {
            Thread t = new Thread(delegate()
            {
                try
                {
                    ts();
                }
                catch (Exception ex)
                {
                    this.ManagedThreads[threadID].UnhandledException = ex;
                }
            });

            ManagedThread mt = new ManagedThread(t);
            this.ManagedThreads.Items.Add(threadID, mt);

            return mt;
        }
        public ManagedThread CreateManagedThread(object threadID, ParameterizedThreadStart ts)
        {
            Thread t = new Thread(delegate(object arg)
            {
                try
                {
                    ts(arg);
                }
                catch (Exception ex)
                {
                    this.ManagedThreads[threadID].UnhandledException = ex;
                }
            });

            ManagedThread mt = new ManagedThread(t);
            this.ManagedThreads.Items.Add(threadID, mt);

            return mt;
        }
        public ManagedThread CreateManagedThread(object threadID, ThreadStart ts, int maxStackSize)
        {
            Thread t = new Thread(delegate()
            {
                try
                {
                    ts();
                }
                catch (Exception ex)
                {
                    this.ManagedThreads[threadID].UnhandledException = ex;
                }
            }
            , maxStackSize);

            ManagedThread mt = new ManagedThread(t);
            this.ManagedThreads.Items.Add(threadID, mt);

            return mt;
        }
        public ManagedThread CreateManagedThread(object threadID, ParameterizedThreadStart ts, int maxStackSize)
        {
            Thread t = new Thread(delegate(object arg)
            {
                try
                {
                    ts(arg);
                }
                catch (Exception ex)
                {
                    this.ManagedThreads[threadID].UnhandledException = ex;
                }
            }
            , maxStackSize);

            ManagedThread mt = new ManagedThread(t);
            this.ManagedThreads.Items.Add(threadID, mt);

            return mt;
        }
    }
}
