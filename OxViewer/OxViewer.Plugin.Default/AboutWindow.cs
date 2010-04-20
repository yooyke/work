using System;
using System.Windows.Forms;
using OxCore;

namespace OxViewer.Plugin.Default
{
    public class AboutWindow : OxViewerPlugin
    {
        public AboutWindow(Ox ox)
            : base(ox)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Text = "Setting";
            item.Click += new EventHandler(item_Click);

            Ox.Menu.Add(item);
        }

        void item_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is a virtual world viewer for opensim.", "About OxViewer");
        }
    }
}
