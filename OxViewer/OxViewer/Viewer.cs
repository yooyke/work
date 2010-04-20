using System;
using System.IO;
using OxLoader;
using OxCore;
using OxJson;
using OxRender;
using OxCore.Data;

namespace OxViewer
{
    public class Viewer : Ox, IViewerPlugin
    {
        private PluginManager<IOxRenderPlugin> r_plugins;
        private PluginManager<IOxViewerPlugin> v_plugins;

        public Viewer(int windowWidth, int windowHeight, int mode)
        {
            new Controller(this);
            new Calculator(this);

            if (mode == (int)Loader.ModeType.Normal)
            {
                r_plugins = new PluginManager<IOxRenderPlugin>(this.Paths.Application);
                r_plugins.Load(new object[] { this });
                if (r_plugins.Plugins == null)
                    mode = (int)Loader.ModeType.Silent;
            }

            if (mode == (int)Loader.ModeType.Silent)
                new SilentRender(this);


            DataStore.Width = windowWidth;
            DataStore.Height = windowHeight;
        }

        public override void Initialize()
        {
            DataStore.Core.FpsActiveTarget = Config.Get(this.GetType(), "core_fps_active_target", Default.CORE_FPS_ACTIVE_TARGET);
            DataStore.Core.FpsDeactiveTarget = Config.Get(this.GetType(), "core_fps_deactive_target", Default.CORE_FPS_DEACTIVE_TARGET);
            DataStore.Core.CacheMax = Config.Get(this.GetType(), "core_cache_max", Default.CORE_CACHE_MAX);
            DataStore.Camera.Distance = Config.Get(this.GetType(), "camera_start_distance", Default.CAMERA_START_DISTANCE);
            DataStore.Camera.DistanceMin = Config.Get(this.GetType(), "camera_min_distance", Default.CAMERA_MIN_DISTANCE);
            DataStore.Camera.DistanceMax = Config.Get(this.GetType(), "camera_max_distance", Default.CAMERA_MAX_DISTANCE);
            DataStore.World.Agent.AlwaysRun = Config.Get(this.GetType(), "agent_always_run", Default.AGENT_ALWAYS_RUN);
            DataStore.World.Agent.Head = Config.Get(this.GetType(), "agent_head", Default.AGENT_HEAD);

            EventFire(JsonUtil.SerializeMessage(JsonType.StateInside, new JsonState((int)StatusData.Type.Initialize)), false);
            InitPlugin();

            base.Initialize();

            EventFire(JsonUtil.SerializeMessage(JsonType.StateInside, new JsonState((int)StatusData.Type.InitializeEnd)), false);
        }

        public override void Update()
        {
            IO.SetMax(DataStore.Core.CacheMax);

            base.Update();
        }

        public override void Cleanup()
        {
            CleanupPlugin();
            EventFire(JsonUtil.SerializeMessage(JsonType.StateInside, new JsonState((int)StatusData.Type.Cleanup)), false);

            base.Cleanup();

            EventFire(JsonUtil.SerializeMessage(JsonType.StateInside, new JsonState((int)StatusData.Type.CleanupEnd)), false);
        }

        protected override bool IsFunctionMessage(string message)
        {
            JsonMessage parse_msg;
            JsonType type = JsonUtil.DeserializeMessage(message, out parse_msg);
            if (type <= JsonType._Function || JsonType._FunctionEnd <= type)
                return false;

            return base.IsFunctionMessage(message);
        }

        protected override bool IsOutsideEventMessage(string message)
        {
            JsonMessage parse_msg;
            JsonType type = JsonUtil.DeserializeMessage(message, out parse_msg);
            if (type <= JsonType._Event || JsonType._EventEnd <= type)
                return false;

            return base.IsOutsideEventMessage(message);
        }

        private void InitPlugin()
        {
            if (v_plugins != null)
                CleanupPlugin();

            v_plugins = new PluginManager<IOxViewerPlugin>(this.Paths.Application);
            v_plugins.Load(new object[] { this });
        }

        private void CleanupPlugin()
        {
            if (v_plugins != null)
            {
                v_plugins.Unload();
                v_plugins = null;
            }
        }
    }
}
