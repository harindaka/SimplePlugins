using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using SimplePlugins;
using SimplePlugins.Exceptions;

namespace SamplePluggableApp.SamplePlugin
{
    public class Plugin : PluginBase
    {
        public Plugin()
            : base()
        {
            Plugin.Current = this;
        }

        internal static Plugin Current { get; private set; }

        internal string Param1 { get; private set; }
        internal int Param2 { get; private set; }
        internal double Param3 { get; private set; }

        public override string FriendlyName
        {
            //The friendly name of the plugin. This will be used by default in PluginInfo.ToString()
            get { return "Sample Plugin"; }
        }

        public override ExecutionModes ExecutionMode
        {
            get { return ExecutionModes.AsynchronousMultiInstance; } //Tells Loader to allow loading of multiple instances asynchronously in seperate threads 
            //ExecutionModes.AsynchronousExclusive - PluginLoaderBase object will execute plugin in a seperate thread only if no other plugins are running
            //ExecutionModes.AsynchronousMultiInstance - PluginLoaderBase object will execute plugin in a seperate thread and will allow multiple instances of the plugin to be executed simultaneously
            //ExecutionModes.AsynchronousSingleton - PluginLoaderBase object will execute plugin in a seperate thread and will allow only one instance of the plugin to be running at a time
            //ExecutionModes.SynchronousExclusive - PluginLoaderBase object will execute plugin in the same thread as the pluggable app only if no other plugins are running
            //ExecutionModes.SynchronousMultiInstance - PluginLoaderBase object will execute plugin in the same thread as the pluggable app and will allow multiple instances of the plugin to be executed simultaneously
            //ExecutionModes.SynchronousSingleton - PluginLoaderBase object will execute plugin in the same thread as the pluggable app and will allow only one instance of the plugin to be running at a time
        }

        //This is the main execution entry point for the plugin. 
        public override PluginParameters Main(PluginParameters args)
        {
            //Assigns parameters passed from the pluggable app to public members
            this.Param1 = (string)args.Get("Param1");
            this.Param2 = (int)args.Get("Param2");
            this.Param3 = (double)args.Get("Param3");

            //Creates a window wrapper for the pluggable app's main window.
            //This is only to ensure that the plugin form always stays in front of the pluggable app main window
            IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
            WindowWrapper wr = null;
            if (handle != IntPtr.Zero)
                wr = new WindowWrapper(handle);

            MainView pluginForm = new MainView();
            pluginForm.Show(wr); //show the main plugin window

            Application.Run(pluginForm); //This starts a new message loop for the plugin window. pluginForm.ShowDialog() may also be used instead. 
            //Specifying ExecutionMode to Synchronous* and calling pluginForm.ShowDialog() will show the form in dialog mode and block the Pluggable App.

            args.Add("ReturnValue", "Success"); // Demonstrates how serializeable/primitive types can be passed back to the pluggable app
            //returned values will be available in the pluggable app via PluginLoaderBase.PluginUnloaded event

            return args;
        }

        public override void OnUnloadNotification()
        {
            //This method is called by PluginLoaderBase.NotifyUnload or PluginLoaderBase.NotifyUnloadAll to notify the plugin that it is about to be unloaded.
            //You can write code which will stop plugin process and do cleanup here
            Application.Exit();
        }
    }
}
