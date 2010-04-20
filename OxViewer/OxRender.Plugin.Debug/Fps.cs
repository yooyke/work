using System.Diagnostics;
using OxCore;
using IrrlichtNETCP;

namespace OxRender.Plugin.Debug
{
    public class Fps : OxRenderComponentPlugin
    {
        private GUIStaticText elem;
        private Stopwatch sw = new Stopwatch();
        private double fps = 0;
        private double updateIntervalSecond = 1.0f;
        private double framecount = 0;

        public Fps(Ox ox, Render render)
            : base(ox, render)
        {
        }

        public override void Initialize()
        {
            IOxRenderDebugController debug = (IOxRenderDebugController)Ox.Service.Get(typeof(IOxRenderDebugController));
            debug.CheckedChanged += new System.EventHandler(debug_CheckedChanged);

            elem = Render.GUI.AddStaticTextW(string.Empty, new Rect(new Position2D(), new Dimension2D(320, 240)), false, true, Render.GUI.RootElement, 0, false);
            elem.OverrideColor = Color.Red;
            sw.Start();

            base.Initialize();
        }

        public override void Update(ApplicationTime time)
        {
            framecount++;
            if (sw.ElapsedMilliseconds >= (updateIntervalSecond * 1000))
            {
                fps = framecount / updateIntervalSecond;
                framecount = 0;

                sw.Reset();
                sw.Start();
            }

            elem.Text = fps.ToString();

            base.Update(time);
        }

        void debug_CheckedChanged(object sender, System.EventArgs e)
        {
            IOxRenderDebugController debug = (IOxRenderDebugController)Ox.Service.Get(typeof(IOxRenderDebugController));
            Root.Visible = debug.GetVisible(OxRender.Plugin.Debug.Controller.Type.Fps);
            elem.Visible = debug.GetVisible(OxRender.Plugin.Debug.Controller.Type.Fps);
        }
    }
}
