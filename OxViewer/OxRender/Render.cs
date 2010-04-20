using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using IrrlichtNETCP;
using OxLoader;
using OxCore;
using OxJson;
using OxCore.Data;

namespace OxRender
{
    public partial class Render : OxRenderPlugin
    {
        private enum PluginControlType
        {
            None,
            Load,
            Unload,
        }

        private IrrlichtDevice device;
        private PluginManager<IOxRenderComponentPlugin> plugins;
        private Queue<PluginControlType> queue = new Queue<PluginControlType>();
        private SceneNode root;
        private RenderData renderData = new RenderData();
        private TextureManager textureManager;
        private VideoManager videoManager;

        public SceneManager Scene { get { return device.SceneManager; } }
        public GUIEnvironment GUI { get { return device.GUIEnvironment; } }
        public TextureManager Texture { get { return textureManager; } }
        public VideoManager Video { get { return videoManager; } }
        public SceneNode Root { get { return root; } }
        public RenderData RenderData { get { return renderData; } }

        public Render(Ox ox)
            : base(ox)
        {
            Priority = (int)PriorityBase.Render -1;

            new Collision(Ox);
            plugins = new PluginManager<IOxRenderComponentPlugin>(Ox.Paths.Application);
            plugins.Load(new object[] { Ox, this });

            Ox.OnEvent += new OxEventHandler(Ox_OnEvent);
        }

        public override void Dispose()
        {
            if (device != null)
                device.Dispose();
        }

        public override void Initialize()
        {
            base.Initialize();

            device = new IrrlichtDevice(DriverType.Direct3D9, new Dimension2D(Ox.DataStore.Width, Ox.DataStore.Height), 32, false, false, false, true, Ox.Handle);
            device.OnEvent += new OnEventDelegate(device_OnEvent);
            textureManager = new TextureManager(device.VideoDriver);
            videoManager = new VideoManager(device.VideoDriver);

            RenderData.BlankTexture = textureManager.AddTexture("blank.tga", 1, 1, false);
            RenderData.BlankTexture.SetPixel(0, 0, Color.White);

            System.Type type = this.GetType();
            root = device.SceneManager.AddEmptySceneNode(device.SceneManager.RootSceneNode, 0);
            root.Name = "Root";

            // This node will be changed state change message, when state change running or waiting.
            Root.Visible = false;
        }

        public override void Update(ApplicationTime time)
        {
            while (queue.Count > 0)
            {
                PluginControlType type = PluginControlType.None;
                lock (queue)
                    type = queue.Dequeue();

                switch (type)
                {
                    case PluginControlType.Load:
                        LoadProcess();
                        break;
                    case PluginControlType.Unload:
                        UnloadProcess();
                        break;
                }
            }

            if (!device.WindowActive)
                ResetInput();

            UpdateInput();

            if (device == null || !device.Run())
                return;

            base.Update(time);
        }

        public override void Draw()
        {
            if (device == null)
                return;

            if (device.VideoDriver.BeginScene(true, true, Color.Black))
            {
                device.SceneManager.DrawAll();

                if (plugins.Plugins != null)
                {
                    foreach (IOxRenderComponentPlugin plugin in plugins.Plugins)
                        plugin.Draw();
                }

                device.GUIEnvironment.DrawAll();

                device.VideoDriver.EndScene();
            }

            base.Draw();
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }

        void Ox_OnEvent(string message)
        {
            JsonMessage parse_msg;
            JsonType type = JsonUtil.DeserializeMessage(message, out parse_msg);

            if (type != JsonType.StateInside)
                return;


            JsonStateInside j = (JsonStateInside)JsonUtil.Deserialize<JsonStateInside>(parse_msg.value);
            switch (j.state)
            {
                case (int)StatusData.Type.RunningBef:
                    lock (queue) queue.Enqueue(PluginControlType.Load);
                    break;
                case (int)StatusData.Type.WaitingBef:
                    lock (queue) queue.Enqueue(PluginControlType.Unload);
                    break;
            }
        }

        private void LoadProcess()
        {
            Root.Visible = true;

            if (plugins.Plugins != null)
            {
                foreach (IOxRenderComponentPlugin plugin in plugins.Plugins)
                    plugin.Load();
            }
        }

        private void UnloadProcess()
        {
            if (plugins.Plugins != null)
            {
                foreach (IOxRenderComponentPlugin plugin in plugins.Plugins)
                    plugin.Unload();
            }

            Scene.MeshCache.Clear();

            if (textureManager != null)
                textureManager.Clear();

            Root.Visible = false;
        }
    }
}
