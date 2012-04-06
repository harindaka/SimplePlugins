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
        public PythonPlugin(string fileName)
            : base(fileName)
        {
            try
            {                
                this.Python = IronPython.Hosting.Python.CreateRuntime(AppDomain.CurrentDomain);
                
                ScriptScope scope = this.Python.UseFile(fileName);
                scope.SetVariable("plugin", this);

                this.Script = scope;
            }
            catch(Exception)
            {
                if (this.Python != null)
                    this.Python.Shutdown();

                throw;
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
                catch (Exception)
                {
                    this.Python.Shutdown();
                    throw;
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
                catch (Exception)
                {
                    this.Python.Shutdown();
                    throw;
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
            catch (Exception)
            {
                this.Python.Shutdown();
                throw;
            }
        }
    }
}
