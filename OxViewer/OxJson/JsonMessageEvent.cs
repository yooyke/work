namespace OxJson
{
    public struct JsonState
    {
        public enum Type : int
        {
            None,
            Initialize = 1,
            Login,
            Logout,
            Cleanup,
        }

        public int state;

        public JsonState(int state)
        {
            this.state = state;
        }
    }

    public struct JsonAgentInfo
    {
        public string id;
        public string simID;
        public string first;
        public string last;

        public JsonAgentInfo(string id, string simID, string first, string last)
        {
            this.id = id;
            this.simID = simID;
            this.first = first;
            this.last = last;
        }
    }

    public struct JsonAgentAnimationList
    {
        public string id;
        public string[] list;

        public JsonAgentAnimationList(string id, string[] list)
        {
            this.id = id;
            this.list = list;
        }
    }

    public struct JsonChatReceived
    {
        public string message;
        public int type;
        public string name;
        public string id;
        public string ownerID;
        public float x;
        public float y;
        public float z;

        public JsonChatReceived(string message, int type, string name, string id, string ownerID, float x, float y, float z)
        {
            this.message = message;
            this.type = type;
            this.name = name;
            this.id = id;
            this.ownerID = ownerID;
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public struct JsonIMReceived
    {
        public string fromID;
        public string fromName;
        /// <summary>
        /// groupIM is 1 if im is group instant message
        /// </summary>
        public int groupIM;
        public string imSessionID;
        public string message;        
        public float x;
        public float y;
        public float z;
        public string regionID;
        public string timestamp;
        public string toID;

        public JsonIMReceived(string fromID, string fromName, int groupIM, string imSessionID, string message, float x, float y, float z, string regionID, string timestamp, string toID)
        {
            this.fromID = fromID;
            this.fromName = fromName;
            this.groupIM = groupIM;
            this.imSessionID = imSessionID;
            this.message = message;
            this.x = x;
            this.y = y;
            this.z = z;
            this.regionID = regionID;
            this.timestamp = timestamp;
            this.toID = toID;
        }
    }

    public struct JsonClicked
    {
        public string id;

        public JsonClicked(string id)
        {
            this.id = id;
        }
    }

    public struct JsonObjectInfo
    {
        /// <summary>
        /// Request ID
        /// </summary>
        public int requestID;
        /// <summary>
        /// Object UUID
        /// </summary>
        public string id;
        /// <summary>
        /// From PointData.ObjectType
        /// </summary>
        public int type;
        /// <summary>
        /// From ClickActionType
        /// </summary>
        public int click;

        public string name;

        public float[] position;
        public float[] rotation;
        public float[] scale;

        public JsonObjectInfo(int requestID, string id, int type, int click, string name, float[] position, float[] rotation, float[] scale)
        {
            this.requestID = requestID;
            this.id = id;
            this.type = type;
            this.click = click;
            this.name = name;

            if (position != null && position.Length == 3)
                this.position = position;
            else
                this.position = new float[3] { 0, 0, 0 };

            if (rotation != null && rotation.Length == 3)
                this.rotation = rotation;
            else
                this.rotation = new float[3] { 0, 0, 0 };

            if (scale != null && scale.Length == 3)
                this.scale = scale;
            else
                this.scale = new float[3] { 0, 0, 0 };
        }
    }

    public struct JsonPathInfo
    {
        public int count;
        public string[] keys;
        public string[] values;

        public JsonPathInfo(string[] keys, string[] values)
        {
            if ((keys == null) || (values == null) || keys.Length != values.Length)
            {
                count = 0;
                this.keys = null;
                this.values = null;
            }
            else
            {
                count = keys.Length;
                this.keys = keys;
                this.values = values;
            }
        }
    }
}
