using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Configuration;
using System.Threading;
using SimplePlugins.Exceptions;

namespace SimplePlugins
{
    [Serializable]
    public abstract class PluginBase
    {
        public enum ExecutionModes { AsynchronousSingleton, AsynchronousMultiInstance, AsynchronousExclusive, SynchronousSingleton, SynchronousMultiInstance, SynchronousExclusive };

        public PluginBase()
        {

        }

        public abstract string FriendlyName { get; }

        public abstract ExecutionModes ExecutionMode { get; }

        public abstract PluginParameters Main(PluginParameters args);

        public abstract void OnUnloadNotification();
    }
}
