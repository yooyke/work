using System;
using System.Collections.Generic;

namespace OxCore
{
    public class ServiceManager
    {
        private Dictionary<Type, OxComponent> dict;

        public ServiceManager()
        {
            dict = new Dictionary<Type, OxComponent>();
        }

        public void Add(Type type, OxComponent component)
        {
            dict.Add(type, component);
        }

        public void Remove(Type type)
        {
            if (dict.ContainsKey(type))
                dict.Remove(type);
        }

        public OxComponent Get(Type type)
        {
            if (dict.ContainsKey(type))
                return dict[type];

            return null;
        }
    }
}
