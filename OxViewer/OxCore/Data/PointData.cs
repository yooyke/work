using System.Collections.Generic;

namespace OxCore.Data
{
    public class PointData
    {
        private List<HitData> list = new List<HitData>();

        public enum ObjectType : int
        {
            None,
            Avatar,
            AvatarSelf,
            Prim,
            Ground,
        }

        public struct HitData
        {
            private float[] point;
            private ObjectType type;
            private ClickActionType click;
            private float length;
            private string id;

            public float[] Point { get { return point; } }
            public float Length { get { return length; } }
            public ObjectType Type { get { return type; } }
            public ClickActionType Click { get { return click; } }
            public string ID { get { return id; } }

            public HitData(float x, float y, float z, ObjectType type, ClickActionType click, float length, string id)
            {
                this.point = new float[3] { x, y, z };
                this.type = type;
                this.click = click;
                this.length = length;
                this.id = id;
            }
        }

        public ulong RegionHandle;
        public ObjectType Type = ObjectType.None;
        public ClickActionType Click = ClickActionType.None;
        public float[] Position = new float[3];
        public string ID;

        public void Add(ref HitData data)
        {
            list.Add(data);
        }

        public void Clear()
        {
            list.Clear();
        }

        public HitData[] GetAll()
        {
            if (list.Count == 0)
                return null;

            return list.ToArray();
        }
    }
}
