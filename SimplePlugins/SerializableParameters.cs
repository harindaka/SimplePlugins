using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimplePlugins.Exceptions;

namespace SimplePlugins
{
    [Serializable]
    public class SerializableParameters
    {
        internal SerializableParameters()
        {
            this._parameters = new Dictionary<string, object>();
        }

        private Dictionary<string, object> _parameters { get; set; }

        public int Count { get { return _parameters.Count; } }
                
        public static bool IsSerializeable(object serializeableObject)
        {
            Type objType = serializeableObject.GetType();
            return (objType.IsPrimitive || objType.IsSerializable);
        }

        public object Get(string parameterName)
        {
            if (this._parameters.ContainsKey(parameterName))
                return this._parameters[parameterName];
            else
                throw new SerializableParameterNotFoundException(parameterName);
        }

        public void Set(string parameterName, object serializeableObject)
        {
            Type paramType = serializeableObject.GetType();
            if (SerializableParameters.IsSerializeable(serializeableObject))
            {
                if (_parameters.ContainsKey(parameterName))
                    _parameters[parameterName] = serializeableObject;
                else
                    throw new SerializableParameterNotFoundException(parameterName);
            }
            else
                throw new NonserializableParameterException(parameterName);
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
            if (SerializableParameters.IsSerializeable(serializeableObject))
                this._parameters.Add(parameterName, serializeableObject);
            else
                throw new NonserializableParameterException(parameterName);
        }
    }
}
