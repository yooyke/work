using System;

namespace OxLoader
{
    public delegate void OxEventHandler(string message);

    public interface IPlugin
    {
    }

    public interface IViewerPlugin : IPlugin
    {
        event OxEventHandler OnEventJS;

        void Run(IntPtr parentHandle);
        void Function(string message);
        void Exit();
    }

    public interface IUpdatePlugin : IPlugin
    {
        void Run();
    }
}
