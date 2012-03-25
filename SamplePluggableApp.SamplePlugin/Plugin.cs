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
        internal string Param1 { get; private set; }
        internal DateTime Param2 { get; private set; }
        internal double Param3 { get; private set; }

        public override string FriendlyName
        {
            //The friendly name of the plugin. This will be used by default in PluginInfo.ToString()
            get { return "Sample Plugin"; }
        }

        public override ExecutionModes ExecutionMode
        {
            get { return ExecutionModes.MultiInstance; }
            //If PluginLoaderBase.UseDefaultExecutionValidation is true
            //ExecutionModes.Exclusive - PluginLoaderBase object will execute plugin only if no other plugins are running
            //ExecutionModes.MultiInstance - PluginLoaderBase object will allow multiple instances of the plugin to be executed simultaneously
            //ExecutionModes.Singleton - PluginLoaderBase object will allow only one instance of the plugin to be running at a time            
            //ExecutionModes.Custom - Let PluginLoaderBase object determine whether or not to allow execution programmatically.
        }

        //This is the main execution entry point for the plugin. 
        public override PluginParameters Main(PluginParameters args)
        {
            //Assigns parameters passed from the pluggable app to public members
            this.Param1 = (string)args.Get("Param1");
            this.Param2 = (DateTime)args.Get("Param2");
            this.Param3 = (double)args.Get("Param3");

            //Creates a window wrapper for the pluggable app's main window.
            //This is only to ensure that the plugin form always stays in front of the pluggable app main window
            IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
            WindowWrapper wr = null;
            if (handle != IntPtr.Zero)
                wr = new WindowWrapper(handle);
                        
            Application.Run(new MainView()); //This starts a new message loop for the plugin window. pluginForm.ShowDialog() may also be used instead. 
                 
            if(this.UnhandledException == null)
                args.Add("ReturnValue", "Success"); // Demonstrates how serializeable/primitive types can be passed back to the pluggable app
            else
                args.Add("ReturnValue", "Failed");
            
            //returned values will be available in the pluggable app via PluginLoaderBase.PluginUnloaded event

            return args;
        }
                
        public override void OnAbort()
        {
            //This method is called by PluginLoaderBase.RequestAbort, PluginLoaderBase.RequestAbortAll
            //You should write code which will stop plugin process and do cleanup here

            Application.Exit(); //Exit the message loop.
        }
    }
}
