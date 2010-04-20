using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Amib.Threading;
using OxLoader;

namespace OxCore
{
    public class Ox : Application
    {
        private const string PATH_FILENAME = "config.xml";

        private DataStore data = new DataStore();
        private IntPtr handle;
        private IntPtr rootHandle;
        private Thread threadMain;
        private ComponentManager component = new ComponentManager();
        private ServiceManager service = new ServiceManager();
        private FileIOManager io;
        private OxMenu menu;
        private Config config;
        private TimeSpan targetElapsedTime;
        private SmartThreadPool eventThreadPool;
        private bool runnning;
        private string config_path;

        public event OxEventHandler OnFunction;
        public event OxEventHandler OnEvent;
        public event OxEventHandler OnEventJS;

        /// <summary>
        /// This handle is RenderForm handle
        /// </summary>
        public IntPtr Handle { get { return handle; } }
        /// <summary>
        /// This handle is Browser or Host handle
        /// </summary>
        public IntPtr RootHandle { get { return rootHandle; } }
        /// <summary>
        /// DataStore is reference for accessing all datas
        /// </summary>
        public DataStore DataStore { get { return data; } }
        /// <summary>
        /// Component is reference for accessing component manager
        /// </summary>
        public ComponentManager Component { get { return component; } }
        /// <summary>
        /// Service is reference for accessing service manager
        /// </summary>
        public ServiceManager Service { get { return service; } }
        /// <summary>
        /// IO is reference for accessing input and output of all datas excluding config
        /// </summary>
        public FileIOManager IO { get { return io; } }
        /// <summary>
        /// Config is reference for accessing config data of this application's 
        /// </summary>
        public Config Config { get { return config; } }
        /// <summary>
        /// Menu is reference for accessing context menu manager
        /// </summary>
        public OxMenu Menu { get { return menu; } }

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        public Ox()
        {
            if (string.IsNullOrEmpty(config_path))
                config_path = System.IO.Path.Combine(Paths.User, PATH_FILENAME);
            config = new Config(config_path);
            config.Load();

            io = new FileIOManager(this);
            menu = new OxMenu(this);
            SetFpsActiveTarget(data.Core.FpsActiveTarget);
        }

        public void SetFpsActiveTarget(float targetFps)
        {
            data.Core.FpsActiveTarget = targetFps;
            targetElapsedTime = new TimeSpan((long)((1000.0f / targetFps) * 1000 * 10));
        }

        public void SetFpsDeactiveTarget(float targetFps)
        {
            data.Core.FpsDeactiveTarget = targetFps;
            targetElapsedTime = new TimeSpan((long)((1000.0f / targetFps) * 1000 * 10));
        }

        public override void Dispose()
        {
            Exit();

            base.Dispose();
        }

        public virtual void Run(IntPtr parentHandle)
        {
            rootHandle = parentHandle;
            handle = parentHandle;
    
            threadMain = new Thread(Start);
            threadMain.SetApartmentState(ApartmentState.STA);
            threadMain.Start();
        }

        public void Exit()
        {
            runnning = false;

            if (threadMain != null)
            {
                while (!threadMain.IsAlive) { }
                threadMain.Join();
            }
        }

        public void Function(string message)
        {
            if (OnFunction != null && IsFunctionMessage(message))
                OnFunction(message);
        }

        public void EventFire(string message, bool async)
        {
            if (OnEvent != null)
            {
                if (async)
                {
                    WorkItemCallback callback = new WorkItemCallback(EventDelegate);
                    eventThreadPool.QueueWorkItem(callback, message);
                }
                else
                {
                    OnEvent(message);
                }
            }

            if (OnEventJS != null && IsOutsideEventMessage(message))
            {
                if (async)
                {
                    WorkItemCallback callback = new WorkItemCallback(EventJSDelegate);
                    eventThreadPool.QueueWorkItem(callback, message);
                }
                else
                {
                    OnEventJS(message as string);
                }
            }
        }

        protected virtual bool IsFunctionMessage(string message)
        {
            return true;
        }

        protected virtual bool IsOutsideEventMessage(string message)
        {
            return true;
        }

        private object EventDelegate(object message)
        {
            if (message == null || !(message is string))
                return null;

            OnEvent(message as string);
            return message;
        }

        private object EventJSDelegate(object message)
        {
            if (message == null || !(message is string))
                return null;

            OnEventJS(message as string);
            return message;
        }

        private void Start()
        {
            using (Form form = new Form())
            {
                form.Width = DataStore.Width;
                form.Height = DataStore.Height;
                form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                form.ImeMode = System.Windows.Forms.ImeMode.Inherit;
                form.Activated += new EventHandler(form_Activated);
                form.Deactivate += new EventHandler(form_Deactivate);

                SetParent(form.Handle, handle);

                form.Location = new Point(0, 0);
                form.Show();

                handle = form.Handle;

                component.Sort();

                Initialize();

                runnning = true;
                OxComponent[] ocs = component.GetAll();
                ApplicationTime time = new ApplicationTime();
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Start();
                while (runnning)
                {
                    if (sw.ElapsedMilliseconds >= targetElapsedTime.TotalMilliseconds)
                    {
                        time.Update(sw.Elapsed);
                        sw.Reset();
                        sw.Start();

                        Update();
                        foreach (OxComponent oc in ocs)
                            oc.Update(time);

                        foreach (OxComponent oc in ocs)
                            if (oc is IDrawable)
                                ((OxDrawableComponent)oc).Draw();
                    }

                    Thread.Sleep(1);
                }

                Cleanup();

                foreach (OxComponent oc in ocs)
                    oc.Dispose();

                form.Close();
            }
        }

        void form_Activated(object sender, EventArgs e)
        {
            SetFpsActiveTarget(data.Core.FpsActiveTarget);
        }

        void form_Deactivate(object sender, EventArgs e)
        {
            SetFpsDeactiveTarget(data.Core.FpsDeactiveTarget);
        }

        public virtual void Initialize()
        {
            eventThreadPool = new SmartThreadPool(120 * 1000, 4);
            eventThreadPool.Start();

            OxComponent[] ocs = component.GetAll();
            foreach (OxComponent oc in ocs)
                oc.Initialize();
        }

        public virtual void Update() { }

        public virtual void Cleanup()
        {
            OxComponent[] ocs = component.GetAll();
            foreach (OxComponent oc in ocs)
                oc.Cleanup();

            if (eventThreadPool != null)
            {
                eventThreadPool.Cancel();
                eventThreadPool.WaitForIdle();
            }

            config.Save();
        }
    }
}
