namespace OxJson
{
    public struct JsonStateInside
    {
        public int state; // StatusData.cs

        public JsonStateInside(int state)
        {
            this.state = state;
        }
    }

    public struct JsonObjectUpdated
    {
        public enum PrimType : int
        {
            Avatar = 0,
            Prim = 1,
        }

        public enum Type : int
        {
            /// <summary>
            /// 0
            /// </summary>
            Add = 0,
            /// <summary>
            /// 1
            /// </summary>
            Update = 1,
            /// <summary>
            /// 2
            /// </summary>
            Delete = 2,
            /// <summary>
            /// 3
            /// </summary>
            UpdateFull = 3,
            /// <summary>
            /// 4
            /// </summary>
            UpdateAnimation = 4,
        }

        public string simID;
        public string id;
        public int prim; // 0: avatar 1: prim
        public int type; // 0: add 1: update 2: delete 3: update_full 4: animation_update

        public JsonObjectUpdated(string simID, string id, int prim, int type)
        {
            this.simID = simID;
            this.id = id;
            this.prim = prim;
            this.type = type;
        }
    }
}
