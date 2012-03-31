using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronPython.Hosting;
using IronPython.Compiler;
using Microsoft.Scripting.Hosting;

namespace SimplePlugins
{
    public class PythonPlugin : PluginBase
    {
        public PythonPlugin()
            : base()
        {
            try
            {
                string fileName = "";

                this.Python = IronPython.Hosting.Python.CreateRuntime(AppDomain.CurrentDomain);
                ScriptScope scope = this.Python.CreateScope();
                scope.SetVariable("this", this);
                scope.SetVariable("Me", this);

                this.Script = this.Python.UseFile(fileName);
            }
            finally
            {
                if (this.Python != null)
                    this.Python.Shutdown();
            }
        }

        private ScriptRuntime Python { get; set; }

        private dynamic Script { get; set; }

        public override string FriendlyName
        {
            get
            {
                try
                {
                    return Convert.ToString(this.Script.FriendlyName());
                }
                finally
                {
                    this.Python.Shutdown();
                }
            }
        }

        public override PluginBase.ExecutionModes ExecutionMode
        {
            get
            {
                try
                {
                    return (PluginBase.ExecutionModes)this.Script.ExecutionMode();
                }
                finally
                {
                    this.Python.Shutdown();
                }
            }
        }

        public override PluginParameters Main(PluginParameters args)
        {
            try
            {
                return (PluginParameters)this.Script.Main(args);
            }
            finally
            {
                this.Python.Shutdown();
            }
        }

        public override void OnAbort()
        {
            try
            {
                this.Script.OnAbort();
            }
            finally
            {
                this.Python.Shutdown();
            }
        }
    }
}
