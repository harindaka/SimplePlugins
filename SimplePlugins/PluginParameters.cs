using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SimplePlugins.Exceptions;

namespace SimplePlugins
{
    [Serializable]
    public class PluginParameters
    {
        public PluginParameters()
        {
            this._parameters = new Dictionary<string, object>();
        }

        private Dictionary<string, object> _parameters { get; set; }

        public int Count { get { return _parameters.Count; } }

        public bool ExecutionDenied { get; internal set; }

        public PluginException UnhandledException { get; internal set; }

        public object Get(string parameterName)
        {
            if (this._parameters.ContainsKey(parameterName))
                return this._parameters[parameterName];
            else
                throw new PluginParameterNotFoundException(parameterName);
        }

        public void Set(string parameterName, object serializeableObject)
        {
            Type paramType = serializeableObject.GetType();
            if (paramType.IsPrimitive || paramType.IsSerializable)
            {
                if (_parameters.ContainsKey(parameterName))
                    _parameters[parameterName] = serializeableObject;
                else
                    throw new PluginParameterNotFoundException(parameterName);
            }
            else
                throw new InvalidPluginParameterException(parameterName);
        }
        
        public bool Contains(string parameterName)
        {
            return this._parameters.ContainsKey(parameterName);
        }

        public Dictionary<string, object>.Enumerator GetEnumerator()
        {
            return this._parameters.GetEnumerator();
        }

        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            foreach (string key in dic.Keys)
                dic.Add(key, dic[key]);

            return dic;
        }       

        public void Add(string parameterName, object serializeableObject)
        {
            Type paramType = serializeableObject.GetType();
            if (paramType.IsPrimitive || paramType.IsSerializable)
                this._parameters.Add(parameterName, serializeableObject);
            else
                throw new InvalidPluginParameterException(parameterName);
        }
    }
}
