using System;
using System.Windows.Forms;
using IrrlichtNETCP;
using OxCore;
using OxCore.Data;

namespace OxRender.Plugin.Debug
{
    public interface IOxRenderDebugController : IOxRenderComponentPlugin
    {
        event EventHandler CheckedChanged;

        bool GetVisible(Controller.Type type);
    }

    public class Controller : OxRenderComponentPlugin, IOxRenderDebugController
    {
        private const string ROOT_NAME = "Debug";

        public enum Type
        {
            Fps,
            Information,
            Axis,
            Point,

            COUNT,
        }

        private bool changed = false;
        private bool[] visible = new bool[(int)Type.COUNT];

        public event EventHandler CheckedChanged;

        public Controller(Ox ox, Render render)
            : base(ox, render)
        {
            Ox.Service.Add(typeof(IOxRenderDebugController), this);
            Priority = (int)PriorityBase.Render;
        }

        public override void Initialize()
        {
            base.Initialize();

            for (int i = 0; i < visible.Length; i++)
                visible[i] = Ox.Config.Get(this.GetType(), ((Type)i).ToString(), true);

            // Root - Debug
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Text = ROOT_NAME;

            // Root - Debug - Fps
            //              - Information
            //              - Axis
            //              - Point
            ToolStripMenuItem[] item00 = new ToolStripMenuItem[(int)Type.COUNT];
            for (int i = 0; i < item00.Length; i++)
            {
                item00[i] = new ToolStripMenuItem();
                item00[i].Tag = (Type)i;
                item00[i].Checked = visible[i];
                item00[i].CheckOnClick = true;
                item00[i].CheckedChanged += new EventHandler(Controller_CheckedChanged);
                item00[i].Text = ((Type)i).ToString();

                item.DropDownItems.AddRange(new ToolStripItem[] { item00[i] });
            }

            changed = true;

            Ox.Menu.Add(item);
        }

        public override void Update(ApplicationTime time)
        {
            if (changed)
            {
                if (CheckedChanged != null)
                    CheckedChanged(this, EventArgs.Empty);

                for (int i = 0; i < visible.Length; i++)
                    Ox.Config.Add(this.GetType(), ((Type)i).ToString(), visible[i]);

                changed = false;
            }

            base.Update(time);
        }

        public bool GetVisible(Type type)
        {
            return visible[(int)type];
        }

        void Controller_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem))
                return;

            ToolStripMenuItem item = sender as ToolStripMenuItem;
            int index = (int)item.Tag;
            visible[index] = item.Checked;
            changed = true;
        }
    }
}
