namespace OxCore.Data
{
    public class ObjectData
    {
        private string id;
        private string parentID;
        private bool myself;

        public float[] Position = new float[3];
        public float[] Rotation = new float[3];
        public float[] Scale = new float[3];
        public float[] Velocity = new float[3];
        public float[] OPosition = new float[3];
        public float[] OQuaternion = new float[4];
        public string ID { get { return id; } }
        public string ParentID { get { return parentID; } }
        public bool Myself { get { return myself; } }

        public ObjectData(string id, string parentID, float[] position, float[] quaternion, float[] scale, float[] velocity, bool myself)
        {
            this.id = id;
            this.parentID = parentID;
            this.myself = myself;
            this.OPosition = position;
            this.OQuaternion = quaternion;
            this.Scale = scale;
            this.Velocity = velocity;
        }
    }
}
