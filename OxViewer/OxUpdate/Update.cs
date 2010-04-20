using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using OxLoader;

namespace OxUpdate
{
    public class Update : IUpdatePlugin
    {
        public Update()
        {
        }

        public void Run()
        {
            Console.WriteLine("Dll Information");

            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            PluginInfo[] p = PluginManager<IPlugin>.Find(dir, typeof(IPlugin));
            Dictionary<string, string> dlllist = new Dictionary<string, string>();
            foreach (PluginInfo info in p)
            {
                FileVersionInfo vi = FileVersionInfo.GetVersionInfo(info.Path);
                if (dlllist.ContainsKey(vi.OriginalFilename))
                    continue;

                dlllist.Add(vi.OriginalFilename, info.Path);
                Console.WriteLine("[{0}] CompanyName : {1} ProductVersion : {2}", vi.OriginalFilename , vi.CompanyName, vi.ProductVersion);
            }

            Console.WriteLine();
        }
    }
}
