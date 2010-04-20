using System;
using System.IO;
using System.Reflection;

namespace OxLoader
{
    public class Loader
    {
        private const int WINDOW_WIDTH = 640;
        private const int WINDOW_HEIGHT = 480;

        public enum ModeType
        {
            Normal,
            Silent,

        }

        private PluginManager<IViewerPlugin> plugins;
        private int width;
        private int height;
        private int mode;

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public event OxEventHandler OnEventJS;

        public Loader(int windowWidth, int windowHeight, int mode)
        {
            using (UpdateCheck check = new UpdateCheck())
                check.Run();

            SetWindowSize(windowWidth, windowHeight);

            this.mode = mode;
            if (this.mode < (int)ModeType.Normal || this.mode < (int)ModeType.Silent)
                this.mode = (int)ModeType.Normal;
        }

        public void Run(IntPtr parentHandle)
        {
            InitPlugin();

            IViewerPlugin[] p = plugins.Plugins;
            if (p != null)
            {
                p[0].OnEventJS += new OxEventHandler(Loader_OnEventJS);
                p[0].Run(parentHandle);
            }
        }

        public void Function(string message)
        {
            IViewerPlugin[] p = plugins.Plugins;
            if (p != null)
                p[0].Function(message);
        }

        public void Exit()
        {
            IViewerPlugin[] p = plugins.Plugins;
            if (p != null)
                p[0].Exit();
        }

        private void InitPlugin()
        {
            if (plugins != null)
                CleanupPlugin();

            string dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            plugins = new PluginManager<IViewerPlugin>(dir);
            plugins.Load(new object[] { width, height, mode });
        }

        private void CleanupPlugin()
        {
            if (plugins != null)
            {
                plugins.Unload();
                plugins = null;
            }
        }

        private void SetWindowSize(int windowWidth, int windowHeight)
        {
            if (windowWidth >= 0 && windowHeight >= 0)
            {
                width = windowWidth;
                height = windowHeight;
            }
            else
            {
                width = WINDOW_WIDTH;
                height = WINDOW_HEIGHT;
            }
        }

        void Loader_OnEventJS(string message)
        {
            if (OnEventJS != null)
                OnEventJS(message);
        }
    }
}
