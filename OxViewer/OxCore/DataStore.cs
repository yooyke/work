using System;
using OxCore.Data;

namespace OxCore
{
    public class DataStore
    {
        private OxData core = new OxData();
        private InputData input = new InputData();
        private CameraData camera = new CameraData();
        private WorldData world = new WorldData();

        public OxData Core { get { return core; } }
        public InputData Input { get { return input; } }
        public CameraData Camera { get { return camera; } }
        public WorldData World { get { return world; } }

        public int Width { get; set; }
        public int Height { get; set; }
    }
}
