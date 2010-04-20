namespace OxCore.Data
{
    public class SimData
    {
        private string id = string.Empty;
        private ulong handle = 0;
        private AvatarCollectionData avatarCollection = new AvatarCollectionData();
        private PrimCollectionData primCollection = new PrimCollectionData();
        private TerrainData terrain;

        public string ID { get { return id; } }
        public ulong Handle { get { return handle; } }
        public AvatarCollectionData AvatarCollection { get { return avatarCollection; } }
        public PrimCollectionData PrimCollection { get { return primCollection; } }
        public TerrainData Terrain { get { return terrain; } }

        public SimData(string id, ulong handle, uint gridX, uint gridY)
        {
            this.id = id;
            this.handle = handle;
            terrain = new TerrainData(gridX, gridY);
        }

        public bool TryGet(string avatar_prim_uuid, out ObjectData data)
        {
            if (avatarCollection.TryGet(avatar_prim_uuid, out data))
                return true;

            if (primCollection.TryGet(avatar_prim_uuid, out data))
                return true;

            return false;
        }

        public bool Contaion(string avatar_prim_uuid)
        {
            if (avatarCollection.Contains(avatar_prim_uuid))
                return true;

            if (primCollection.Contains(avatar_prim_uuid))
                return true;

            return false;
        }

        internal void Reset()
        {
            id = string.Empty;
            handle = 0;
            avatarCollection.Reset();
            primCollection.Reset();
            terrain.Reset();
        }
    }
}
