namespace OxCore.Data
{
    public enum ClickActionType : int
    {
        Touch = 0,
        Sit = 1,
        Buy = 2,
        Pay = 3,
        OpenTask = 4,
        PlayMedia = 5,
        OpenMedia = 6,

        None = 99,
    }

    public class PrimData : ObjectData
    {
        private string sceneName;
        private MeshData[] meshes;
        private int meshUpdateCount; // If mesh updated, increment count.
        private ClickActionType clickActionType = ClickActionType.None;

        public string SceneName { get { return sceneName; } }
        public bool Loaded = false;
        public MeshData[] Meshes { get { return meshes; } }
        public int MeshUpdateCount { get { return meshUpdateCount; } }
        public ClickActionType ClickActionType { get { return clickActionType; } }

        public PrimData(string id, string parentID, string sceneName, float[] position, float[] rotation, float[] scale, float[] velocity, bool myself, ClickActionType clickActionType)
            : base(id, parentID, position, rotation, scale, velocity, myself)
        {
            this.sceneName = sceneName;
            this.clickActionType = clickActionType;
        }

        public void MeshUpdated(MeshData[] meshes)
        {
            if (meshes == null)
                return;

            this.meshes = meshes;
            this.meshUpdateCount++;
        }
    }
}
