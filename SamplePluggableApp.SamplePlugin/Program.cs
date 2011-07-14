using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SimplePlugins;

namespace SamplePluggableApp.SamplePlugin
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //This way the plugin can be tested without the pluggable app by running it as a standalone exe.
            //However the PluginParameters passed to Main function will have to be emulated as follows.
            //PluginParameters args = new PluginParameters();
            //args.Add("Param1", "The quick brown fox jumped over the lazy dog");
            //args.Add("Param2", 100);
            //args.Add("Param3", 7.321);
            //new Plugin().Main(null);         
        }        
    }
}
