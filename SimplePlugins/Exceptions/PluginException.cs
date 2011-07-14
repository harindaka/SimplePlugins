using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SimplePlugins.Exceptions
{
    [Serializable]
    public class PluginException : Exception, ISerializable
    {
        string _exString;
        string _exStackTrace;

        protected PluginException(SerializationInfo info, StreamingContext context)
            : base(info, context) 
        {
            this._exString = info.GetString("_exString");
            this._exStackTrace = info.GetString("_exStackTrace");
            this.ExceptionType = info.GetString("ExceptionType");
            this.InnerException = (PluginException)info.GetValue("NewInnerException", this.GetType());
        }

        internal PluginException(Exception ex)
            : base(ex.Message)
        {
            this._exString = ex.ToString();
            this._exStackTrace = ex.StackTrace;
            this.ExceptionType = ex.GetType().ToString();           
            this.Source = ex.Source;

            this.InnerException = null;
            PluginException pex = this;
            Exception exp = ex;
            while (exp.InnerException != null)
            {
                pex.InnerException = new PluginException(ex.InnerException);
                pex = pex.InnerException;
                exp = exp.InnerException;
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("_exString", _exString);
            info.AddValue("_exStackTrace", _exStackTrace);
            info.AddValue("ExceptionType", this.ExceptionType);
            info.AddValue("NewInnerException", this.InnerException);
        }
               
        public string ExceptionType { get; private set; }

        public new PluginException InnerException { get; private set; }

        public override string StackTrace
        {
            get { return this._exStackTrace; }
        }

        public override System.Collections.IDictionary Data
        {
            get { return null; }
        }

        public override string ToString()
        {
            return this._exString;
        }

    }
}
