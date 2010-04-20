using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OxCore;
using OxCore.Data;
using OxJson;

namespace OxViewer.Plugin.Default
{
    public class ProgressBar : OxViewerPlugin
    {
        private System.Windows.Forms.ProgressBar progress;
        private Queue<bool> queue = new Queue<bool>();

        public ProgressBar(Ox ox)
            : base(ox)
        {
            Control c = Form.FromHandle(Ox.Handle);
            progress = new System.Windows.Forms.ProgressBar();
            progress.Step = 1;
            progress.Maximum = StatusData.PROGRESS_MAX;
            progress.MarqueeAnimationSpeed = 10;
            progress.Size = new Size((int)c.Size.Width, 12);
            progress.Location = new Point((c.Size.Width / 2) - (progress.Size.Width / 2), c.Size.Height - progress.Size.Height);
            progress.Style = ProgressBarStyle.Continuous;
            progress.Visible = false;

            c.Controls.Add(progress);

            Ox.OnEvent += new OxLoader.OxEventHandler(Ox_OnEvent);
        }

        public override void Update(ApplicationTime time)
        {
            while (queue.Count > 0)
            {
                lock (queue)
                    progress.Visible = queue.Dequeue();
            }

            if (progress.Visible)
            {
                int value = Ox.DataStore.World.Status.Progress;
                if (value > StatusData.PROGRESS_MAX)
                    value = StatusData.PROGRESS_MAX;
                progress.Value = value;
            }

            base.Update(time);
        }

        public override void Cleanup()
        {
            Control c = Form.FromHandle(Ox.Handle);
            if (c != null)
                c.Controls.Remove(progress);

            base.Cleanup();
        }

        void Ox_OnEvent(string message)
        {
            JsonMessage parse_msg;
            JsonType type = JsonUtil.DeserializeMessage(message, out parse_msg);
            switch (type)
            {
                case JsonType.StateInside:
                    MessageState(parse_msg.value);
                    break;
            }
        }

        private void MessageState(string message)
        {
            JsonStateInside j = (JsonStateInside)JsonUtil.Deserialize<JsonStateInside>(message);
            switch (j.state)
            {
                case (int)StatusData.Type.Login:
                    lock (queue) queue.Enqueue(true);
                    break;
                case (int)StatusData.Type.Running:
                    lock (queue) queue.Enqueue(false);
                    break;
                case (int)StatusData.Type.Logout:
                    lock (queue) queue.Enqueue(true);
                    break;
                case (int)StatusData.Type.Waiting:
                    lock (queue) queue.Enqueue(false);
                    break;
            }
        }
    }
}
