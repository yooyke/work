using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace OxCore
{
    public class Application : IDisposable
    {
        private ApplicationPath paths = new ApplicationPath();
        public ApplicationPath Paths { get { return paths; } }

        public Application()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}
