using System;
using System.Windows.Forms;
using OxCore;

namespace OxViewer.Plugin.Default
{
    public class SettingWindow : OxViewerPlugin
    {
        public SettingWindow(Ox ox)
            : base(ox)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Text = "About";
            item.Click += new EventHandler(item_Click);

            Ox.Menu.Add(item);
        }

        void item_Click(object sender, EventArgs e)
        {
            MessageBox.Show("sample", "Setting window");
        }
    }
}
