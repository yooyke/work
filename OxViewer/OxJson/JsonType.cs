namespace OxJson
{
    public enum JsonType
    {
        None,

        // Function
        _Function,
        Setting,
        Title,
        Login,
        Logout,
        Chat,
        IM,
        Sit,
        Standup,
        Touch,
        TeleportFromLandmark,
        TeleportFromSimName,
        TeleportFromSimName2,
        Movement,
        AgentHead,
        RequestImage,
        RequestObjectInfo,
        RequestPathInfo,
        _FunctionEnd,

        // Event
        _Event,
        State,
        AgentInfo,
        AgentAnimationList,
        ChatReceived,
        IMReceived,
        Clicked,
        ObjectInfo,
        PathInfo,
        _EventEnd,

        // Inside
        StateInside,
        ObjectUpdated,
    }

    public static partial class JsonUtil
    {
        public static JsonType DeserializeMessage(string message, out JsonMessage parse_msg)
        {
            parse_msg = (JsonMessage)JsonUtil.Deserialize<JsonMessage>(message);

            JsonType type = JsonType.None;
            switch (parse_msg.key.ToLower())
            {
                // Fuction
                case "setting":
                    type = JsonType.Setting;
                    break;
                case "title":
                    type = JsonType.Title;
                    break;
                case "login":
                    type = JsonType.Login;
                    break;
                case "logout":
                    type = JsonType.Logout;
                    break;
                case "chat":
                    type = JsonType.Chat;
                    break;
                case "im":
                    type = JsonType.IM;
                    break;
                case "sit":
                    type = JsonType.Sit;
                    break;
                case "standup":
                    type = JsonType.Standup;
                    break;
                case "touch":
                    type = JsonType.Touch;
                    break;
                case "teleportfromlandmark":
                    type = JsonType.TeleportFromLandmark;
                    break;
                case "teleportfromsimname":
                    type = JsonType.TeleportFromSimName;
                    break;
                case "teleportfromsimname2":
                    type = JsonType.TeleportFromSimName2;
                    break;
                case "movement":
                    type = JsonType.Movement;
                    break;
                case "agenthead":
                    type = JsonType.AgentHead;
                    break;
                case "requestimage":
                    type = JsonType.RequestImage;
                    break;
                case "requestobjectinfo":
                    type = JsonType.RequestObjectInfo;
                    break;
                case "requestpathinfo":
                    type = JsonType.RequestPathInfo;
                    break;

                // Event
                case "state":
                    type = JsonType.State;
                    break;
                case "agentinfo":
                    type = JsonType.AgentInfo;
                    break;
                case "agentanimationlist":
                    type = JsonType.AgentAnimationList;
                    break;
                case "chatreceived":
                    type = JsonType.ChatReceived;
                    break;
                case "imreceived":
                    type = JsonType.IMReceived;
                    break;
                case "clicked":
                    type = JsonType.Clicked;
                    break;
                case "objectinfo":
                    type = JsonType.ObjectInfo;
                    break;
                case "pathinfo":
                    type = JsonType.PathInfo;
                    break;

                // Inside
                case "stateinside":
                    type = JsonType.StateInside;
                    break;
                case "objectupdated":
                    type = JsonType.ObjectUpdated;
                    break;
            }

            return type;
        }
    }
}
