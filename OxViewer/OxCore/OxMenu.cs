using System;
using System.Drawing;
using System.Windows.Forms;
using OxCore;

namespace OxCore
{
    public class OxMenu : OxComponent
    {
        private static readonly Point offset = new Point(0, 0);

        private delegate void MenuDelegate(Control control);
        private ContextMenuStrip menu = new ContextMenuStrip();
        private EventHandler action;

        public bool Visible { get { return menu.Visible; } }

        public OxMenu(Ox ox)
            : base(ox)
        {
        }

        public void Add(ToolStripItem item)
        {
            if (menu.Items.Count > 0)
                menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(item);
        }

        public override void Update(ApplicationTime time)
        {
            if (action != null)
            {
                action(this, EventArgs.Empty);
                action = null;
            }

            base.Update(time);
        }

        public void Show()
        {
            action = ActionShow;
        }

        private void ActionShow(object sender, EventArgs e)
        {
            Control c = Form.FromHandle(Ox.Handle);
            c.Invoke(new MenuDelegate(InvokeShow), c);
        }

        private void InvokeShow(Control sender)
        {
            menu.Show(sender, offset);
        }
    }
}
