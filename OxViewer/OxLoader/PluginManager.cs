using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace OxLoader
{
    public class PluginManager<T>
    {
        private string directory;
        private T[] plugins;

        public T[] Plugins { get { return plugins; } }

        public PluginManager(string directory)
        {
            this.directory = directory;
        }

        public static PluginInfo[] Find(string directory, Type interfaceType)
        {
            if (!Directory.Exists(directory))
                return null;

            string[] dlls = Directory.GetFiles(directory, "*.dll");

            List<PluginInfo> load_list = new List<PluginInfo>();
            foreach (string dll in dlls)
            {
                Assembly assembly;
                try { assembly = Assembly.LoadFrom(dll); }
                catch { continue; }

                Type[] ts = null;
                try { ts = assembly.GetTypes(); }
                catch { continue; }

                if (ts == null) continue;

                foreach (Type t in ts)
                {
                    if (!t.IsAbstract && t.IsClass && t.IsPublic && (t.GetInterface(interfaceType.FullName) != null))
                        load_list.Add(new PluginInfo(dll, t.FullName));
                }
            }

            return load_list.ToArray();
        }

        public void Load(object[] args)
        {
            PluginInfo[] dlls = Find(directory, typeof(T));

            if (dlls == null || dlls.Length == 0)
                return;

            if (plugins != null)
                Unload();

            plugins = new T[dlls.Length];
            int count = 0;
            foreach (PluginInfo dll in dlls)
            {
                Assembly assembly = Assembly.LoadFrom(dll.Path);
                try
                {
                    plugins[count++] = (T)assembly.CreateInstance(dll.ClassName, true, BindingFlags.CreateInstance, null, args, null, null);
                }
                catch { }
            }
        }

        public void Unload()
        {
            if (plugins != null)
            {
                for (int i = 0; i < plugins.Length; i++)
                    plugins[i] = default(T);
                plugins = null;
            }
        }
    }
}
