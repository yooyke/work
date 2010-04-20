using System;
using System.Collections.Generic;
using System.IO;
using IrrlichtNETCP;
using OxCore;
using OxCore.Data;
using OxJson;

namespace OxRender.Plugin.Default
{
    public class Fade : OxRenderComponentPlugin
    {
        private const float FADE_SPEED = 14;
        private const int WAIT_MILLISECOND = 400;

        private Queue<int> pipline = new Queue<int>();
        private OxUtil.Fade f;
        private Position2D position = new Position2D();
        private Rect rect;
        private Color color = new Color(0, 255, 255, 255);

        public Fade(Ox ox, Render render)
            : base(ox, render)
        {
            Priority = (int)PriorityBase.RenderFade;

            Ox.OnEvent += new OxLoader.OxEventHandler(Ox_OnEvent);
        }

        public override void Initialize()
        {
            base.Initialize();

            rect = new Rect(0, 0, Ox.DataStore.Width, Ox.DataStore.Height);
        }

        public override void Update(ApplicationTime time)
        {
            while (pipline.Count > 0)
            {
                int state;
                lock (pipline)
                    state = pipline.Dequeue();

                switch (state)
                {
                    case (int)StatusData.Type.RunningFade:
                        f = new OxUtil.Fade(FADE_SPEED, WAIT_MILLISECOND, OxUtil.Alpha.FpsType.Fps30);
                        f.OnEnd += new EventHandler(f_OnEnd);
                        break;
                    case (int)StatusData.Type.WaitingFade:
                        f = new OxUtil.Fade(FADE_SPEED, WAIT_MILLISECOND, OxUtil.Alpha.FpsType.Fps30);
                        f.OnEnd += new EventHandler(f_OnEnd);
                        break;
                }
            }

            base.Update(time);
        }

        public override void Draw()
        {
            if (f != null)
            {
                color.A = f.Value255;
                Render.Video.Draw2DImage(Render.RenderData.BlankTexture, ref position, ref rect, ref color, true);
            }

            base.Draw();
        }

        void f_OnEnd(object sender, EventArgs e)
        {
            f = null;
        }

        void Ox_OnEvent(string message)
        {
            JsonMessage parse_msg;
            JsonType type = JsonUtil.DeserializeMessage(message, out parse_msg);
            if (type != JsonType.StateInside)
                return;

            JsonStateInside j = (JsonStateInside)JsonUtil.Deserialize<JsonStateInside>(parse_msg.value);
            lock (pipline)
                pipline.Enqueue(j.state);
        }
    }
}
