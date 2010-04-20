using System;
using System.IO;
using System.Reflection;

namespace OxLoader
{
    public class UpdateCheck : IDisposable
    {
        public UpdateCheck()
        {
        }

        public void Run()
        {
            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            PluginManager<IUpdatePlugin> plugins = new PluginManager<IUpdatePlugin>(dir);
            plugins.Load(new object[] { });

            foreach (IUpdatePlugin plugin in plugins.Plugins)
                plugin.Run();

            plugins.Unload();
        }

        public void Dispose()
        {
        }
    }
}
