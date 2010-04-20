using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using OxLoader;

namespace OxAX
{
    [ComSourceInterfaces(typeof(IJavaScriptApiEvent))]
    [ComVisible(true)]
    [ProgId("OxAX.OxViewerCtl")]
    [Guid("6D0B8646-DA71-4a4b-AD47-FD6F2A145937")]
    public partial class OxViewerCtl : UserControl, IObjectSafety, IDisposable
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetAncestor(IntPtr hwnd, uint gaFlags);
        [DllImport("user32.dll")]
        private static extern bool InvalidateRect(IntPtr hwnd, IntPtr lpRect, bool bErase);

        private Loader loader;
        private int windowWidth;
        private int windowHeight;
        private int viewerMode;

        [ComVisible(true)]
        public int WindowWidth { get { return windowWidth; } set { windowWidth = value; } }
        [ComVisible(true)]
        public int WindowHeight { get { return windowHeight; } set { windowHeight = value; } }
        [ComVisible(true)]
        public int ViewerMode { get { return viewerMode; } set { viewerMode = value; } }

        public event OxEventHandler OnEvent;

        public OxViewerCtl()
        {
            windowWidth = this.Width;
            windowHeight = this.Height;
            viewerMode = (int)Loader.ModeType.Normal;

            InitializeComponent();
        }

        private void Exit()
        {
            if (loader != null)
                loader.Exit();
        }

        private void OxViewerCtl_Load(object sender, EventArgs e)
        {
            loader = new Loader(windowWidth, windowHeight, viewerMode);
            loader.OnEventJS += new OxEventHandler(loader_OnEventJS);
            windowWidth = loader.Width;
            windowHeight = loader.Height;

            this.Width = windowWidth;
            this.Height = windowHeight;

            Show();

            loader.Run(Handle);
        }

        public void Function(string message)
        {
            if (loader == null)
                return;

            loader.Function(message);
        }

        void loader_OnEventJS(string message)
        {
            if (OnEvent != null)
                OnEvent(message);

            return;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            const int WM_PAINT = 0xf;

            if (m.Msg == WM_PAINT)
            {
                Keys keyCode = (Keys)m.WParam & Keys.KeyCode;
                IntPtr p = GetAncestor(Handle, 1);
                InvalidateRect(p, IntPtr.Zero, true);
            }

            base.WndProc(ref m);
        }
    }
}
