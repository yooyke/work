using System;

namespace OxJson
{
    public struct JsonSetting
    {
        public enum EconomyType : int
        {
            Full = 0,
            Economy  =1,
        }

        public int economy; // 0: fullpower 1: economy
        public int login_wait_second; // 1 ~

        public JsonSetting(int economy, int login_wait_second)
        {
            this.economy = economy;
            this.login_wait_second = login_wait_second;
        }
    }

    public struct JsonTitle
    {
        public string[] paths;
        public int change_second; // 1 ~

        public JsonTitle(string[] paths, int change_second)
        {
            this.paths = paths;
            this.change_second = change_second;
        }
    }

    public struct JsonLogin
    {
        public string first;
        public string last;
        public string password;
        public string uri;
        public string location;

        public JsonLogin(string first, string last, string password, string uri, string location)
        {
            this.first = first;
            this.last = last;
            this.password = password;
            this.uri = uri;
            this.location = location;
        }
    }

    public struct JsonLogout { }

    public struct JsonRequestImage
    {
        public string imageID;

        public JsonRequestImage(string imageID)
        {
            this.imageID = imageID;
        }
    }

    public struct JsonChat
    {
        public string message;
        public int channel;
        public int type;

        public JsonChat(string message, int channel, int type)
        {
            this.message = message;
            this.channel = channel;
            this.type = type;
        }
    }

    public struct JsonIM
    {
        public string targetUUID;
        public string message;

        public JsonIM(string targetUUID, string message)
        {
            this.targetUUID = targetUUID;
            this.message = message;
        }
    }

    public struct JsonSit
    {
        public string targetUUID;

        public JsonSit(string targetUUID)
        {
            this.targetUUID = targetUUID;
        }
    }

    public struct JsonStandup { }

    public struct JsonTouch
    {
        public string targetUUID;

        public JsonTouch(string targetUUID)
        {
            this.targetUUID = targetUUID;
        }
    }

    public struct JsonTeleportFromLandmark
    {
        public string landmarkUUID;

        public JsonTeleportFromLandmark(string landmarkUUID)
        {
            this.landmarkUUID = landmarkUUID;
        }
    }

    public struct JsonTeleportFromSimName
    {
        public string simName;
        public float x;
        public float y;
        public float z;

        public JsonTeleportFromSimName(string simName, float x, float y, float z)
        {
            this.simName = simName;
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public struct JsonTeleportFromSimName2
    {
        public string simName;
        public float x;
        public float y;
        public float z;
        public float lookX;
        public float lookY;
        public float lookZ;

        public JsonTeleportFromSimName2(string simName, float x, float y, float z, float lookX, float lookY, float lookZ)
        {
            this.simName = simName;
            this.x = x;
            this.y = y;
            this.z = z;
            this.lookX = lookX;
            this.lookY = lookY;
            this.lookZ = lookZ;
        }
    }

    public struct JsonMovement
    {
        [Flags]
        public enum Type
        {
            Forward = 1,
            Backward = 2,
            Left = 4,
            Right = 8,
            RotationUpdate = 16,
            AlwaysRun = 32,
            AlwaysWalk = 64,
        }

        public int key;

        public JsonMovement(int key)
        {
            this.key = key;
        }
    }

    public struct JsonAgentHead
    {
        public float angle;

        public JsonAgentHead(float angle)
        {
            this.angle = angle;
        }
    }

    public struct JsonRequestObjectInfo
    {
        public int requestID;
        public string id;

        public JsonRequestObjectInfo(int requestID, string id)
        {
            this.requestID = requestID;
            this.id = id;
        }
    }

    public struct JsonRequestPathInfo { }
}
