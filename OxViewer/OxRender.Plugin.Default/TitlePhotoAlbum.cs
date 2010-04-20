using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using IrrlichtNETCP;
using OxCore;
using OxCore.Data;
using OxJson;

namespace OxRender.Plugin.Default
{
    /// <summary>
    /// This class is for title. So load and unload is opposite to the other plugins.
    /// </summary>
    public class TitlePhotoAlbum : OxRenderComponentPlugin
    {
        private const int DEFAULT_CHANGE_SECOND = 3;

        private Queue<string> paths = new Queue<string>();
        private Queue<string> downloaded = new Queue<string>();
        private bool downloading = false;
        private Position2D position = new Position2D();
        private Rect rect;
        private Color color = new Color(0, 255, 255, 255);
        private List<Texture> list = new List<Texture>();
        private Texture tex;
        private Texture fade_tex;
        private System.Threading.Timer timer;
        private int change_second = DEFAULT_CHANGE_SECOND;
        private int index = 0;
        private OxUtil.Fade fade;

        public TitlePhotoAlbum(Ox ox, Render render)
            : base(ox, render)
        {
            Priority = (int)PriorityBase.Render + 1;

            Ox.OnEvent += new OxLoader.OxEventHandler(Ox_OnEvent);
        }

        public override void Initialize()
        {
            base.Initialize();

            tex = Render.RenderData.BlankTexture;
            fade_tex = tex;
            rect = new Rect(0, 0, Ox.DataStore.Width, Ox.DataStore.Height);
            Ox.IO.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(IO_DownloadFileCompleted);

            CreateTimer();
        }

        public override void Update(ApplicationTime time)
        {
            if ((Ox.DataStore.World.Status.Status < StatusData.Type.RunningFade) || (StatusData.Type.WaitingBef < Ox.DataStore.World.Status.Status))
            {
                while (downloaded.Count > 0)
                {
                    string path = string.Empty;
                    lock (downloaded)
                        path = downloaded.Dequeue();

                    TextureInfo info = Render.Texture.GetTexture(path, false, false);
                    list.Add(info.Texture);
                }

                if (!downloading && paths.Count > 0)
                {
                    string path = string.Empty;
                    lock (paths)
                        path = paths.Dequeue();

                    downloading = true;
                    Ox.IO.Download(path, Path.GetFileName(path), true);
                }
            }

            base.Update(time);
        }

        public override void Draw()
        {
            if ((Ox.DataStore.World.Status.Status < StatusData.Type.RunningFade) || (StatusData.Type.WaitingBef < Ox.DataStore.World.Status.Status))
            {
                if (tex != null)
                {
                    Render.Video.Draw2DImage(tex, ref position, ref rect, ref Color.White, false);
                    if (fade != null)
                    {
                        color.A = fade.Value255;
                        Render.Video.Draw2DImage(fade_tex, ref position, ref rect, ref color, true);
                    }
                }
            }

            base.Draw();
        }

        /// <summary>
        /// [load and unload is opposite to the other plugins!!]
        /// </summary>
        public override void Load()
        {
            DeleteTimer();

            base.Load();
        }

        /// <summary>
        /// [load and unload is opposite to the other plugins!!]
        /// </summary>
        public override void Unload()
        {
            CreateTimer();

            base.Unload();
        }

        void Ox_OnEvent(string message)
        {
            JsonMessage parse_msg;
            JsonType type = JsonUtil.DeserializeMessage(message, out parse_msg);
            if (type != JsonType.Title)
                return;

            JsonTitle j = (JsonTitle)JsonUtil.Deserialize<JsonTitle>(parse_msg.value);
            foreach (string path in j.paths)
            {
                if (string.IsNullOrEmpty(path))
                    continue;

                lock (paths)
                    paths.Enqueue(path);
            }

            if (j.change_second > 0)
            {
                change_second = j.change_second;

                if (timer != null)
                {
                    timer.Dispose();
                    timer = new System.Threading.Timer(new TimerCallback(Change), null, 250 * change_second, 1000 * change_second);
                }
            }
        }

        private void CreateTimer()
        {
            timer = new System.Threading.Timer(new TimerCallback(Change), null, 250 * change_second, 1000 * change_second);
        }

        private void DeleteTimer()
        {
            if (timer == null)
                return;

            timer.Dispose();
            timer = null;
        }

        private void Change(object state)
        {
            if (list.Count == 0)
                return;

            fade_tex = tex;
            fade = new OxUtil.Fade(16);
            fade.OnHalf += new EventHandler(fade_OnHalf);
            fade.OnEnd += new EventHandler(fade_OnEnd);
        }

        void fade_OnHalf(object sender, EventArgs e)
        {
            tex = list[index];
            index = ((index + 1) % list.Count);
        }

        void fade_OnEnd(object sender, EventArgs e)
        {
            fade = null;
        }

        void IO_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            downloading = false;

            if (e.UserState is string)
            {
                lock (downloaded)
                    downloaded.Enqueue(e.UserState as string);
            }
        }
    }
}
