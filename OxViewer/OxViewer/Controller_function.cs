using System;
using System.Collections.Generic;
using OxLoader;
using OxCore;
using OxCore.Data;
using OxJson;
using OxViewer.LibOMV;

namespace OxViewer
{
    partial class Controller
    {
        void Ox_OnEvent(string message)
        {
            JsonMessage param;
            JsonType type = JsonUtil.DeserializeMessage(message, out param);
            switch (type)
            {
                case JsonType.Login:
                    Action = FunctionLogin;
                    break;
                case JsonType.Logout:
                    Action = FunctionLogout;
                    break;
                case JsonType.RequestImage:
                    Action = FunctionRequestImage;
                    break;

                default: return;
            }

            Action(param.value);
        }

        void Ox_OnFunction(string message)
        {
            JsonMessage j;
            JsonType type = JsonUtil.DeserializeMessage(message, out j);
            switch (type)
            {
                // async event.
                case JsonType.Login:
                    Ox.EventFire(JsonUtil.SerializeMessage(JsonType.StateInside, new JsonState((int)StatusData.Type.Login)), false);
                    Ox.EventFire(message, true);
                    return;
                case JsonType.Logout:
                    Ox.EventFire(JsonUtil.SerializeMessage(JsonType.StateInside, new JsonState((int)StatusData.Type.Logout)), false);
                    Ox.EventFire(message, true);
                    return;
                case JsonType.RequestImage:
                    Ox.EventFire(message, true);
                    return;

                case JsonType.Setting: Action = FunctionSetting; break;
                case JsonType.Title: Action = FunctionTitle; break;
                case JsonType.Chat: Action = FunctionChat; break;
                case JsonType.IM: Action = FunctionIM; break;
                case JsonType.Sit: Action = FunctionSit; break;
                case JsonType.Standup: Action = FunctionStandup; break;
                case JsonType.Touch: Action = FunctionTouch; break;
                case JsonType.TeleportFromLandmark: Action = FunctionTeleportFromLandmark; break;
                case JsonType.TeleportFromSimName: Action = FunctionTeleportFromSimName; break;
                case JsonType.TeleportFromSimName2: Action = FunctionTeleportFromSimName2; break;
                case JsonType.Movement: Action = FunctionMovement; break;
                case JsonType.AgentHead: Action = FunctionAgentHead; break;
                case JsonType.RequestObjectInfo: Action = FuntionRequestObjectInfo; break;
                case JsonType.RequestPathInfo: Action = FuntionRequestPathInfo; break;
                default: return;
            }

            Action(j.value);
        }

        private void FunctionSetting(string message)
        {
            JsonSetting p = (JsonSetting)JsonUtil.Deserialize<JsonSetting>(message);
            if (p.economy == 1)
                Ox.DataStore.Core.FpsDeactiveTarget = Default.CORE_FPS_DEACTIVE_TARGET;
            else
                Ox.DataStore.Core.FpsDeactiveTarget = Default.CORE_FPS_ACTIVE_TARGET;

            if (p.login_wait_second < 1)
                Ox.DataStore.World.Status.ProgressForceWaitSecond = 1;
            else
                Ox.DataStore.World.Status.ProgressForceWaitSecond = p.login_wait_second;
        }

        private void FunctionTitle(string message)
        {
            JsonTitle p = (JsonTitle)JsonUtil.Deserialize<JsonTitle>(message);
            Ox.EventFire(JsonUtil.SerializeMessage(JsonType.Title, p), true);
        }

        private void FunctionLogin(string message)
        {
            JsonLogin p = (JsonLogin)JsonUtil.Deserialize<JsonLogin>(message);
            protocol.Login(p.first, p.last, p.password, p.uri);
        }

        private void FunctionLogout(string message)
        {
            JsonLogout p = (JsonLogout)JsonUtil.Deserialize<JsonLogout>(message);
            protocol.Logout();
        }

        private void FunctionRequestImage(string message)
        {
            JsonRequestImage p = (JsonRequestImage)JsonUtil.Deserialize<JsonRequestImage>(message);
            protocol.RequestImage(p.imageID);
        }

        private void FunctionChat(string message)
        {
            JsonChat p = (JsonChat)JsonUtil.Deserialize<JsonChat>(message);
            protocol.Chat(p.message, p.channel, p.type);
        }

        private void FunctionIM(string message)
        {
            JsonIM p = (JsonIM)JsonUtil.Deserialize<JsonIM>(message);
            protocol.IM(p.targetUUID, p.message);
        }

        private void FunctionSit(string message)
        {
            JsonSit p = (JsonSit)JsonUtil.Deserialize<JsonSit>(message);
            protocol.Sit(p.targetUUID);
        }

        private void FunctionStandup(string message)
        {
            JsonStandup p = (JsonStandup)JsonUtil.Deserialize<JsonStandup>(message);
            protocol.Standup();
        }

        private void FunctionTouch(string message)
        {
            JsonTouch p = (JsonTouch)JsonUtil.Deserialize<JsonTouch>(message);
            protocol.Touch(p.targetUUID);
        }

        private void FunctionTeleportFromLandmark(string message)
        {
            JsonTeleportFromLandmark p = (JsonTeleportFromLandmark)JsonUtil.Deserialize<JsonTeleportFromLandmark>(message);
            protocol.Teleport(p.landmarkUUID);
        }

        private void FunctionTeleportFromSimName(string message)
        {
            JsonTeleportFromSimName p = (JsonTeleportFromSimName)JsonUtil.Deserialize<JsonTeleportFromSimName>(message);
            protocol.Teleport(p.simName, p.x, p.y, p.z, 0, 0, 0);
        }

        private void FunctionTeleportFromSimName2(string message)
        {
            JsonTeleportFromSimName2 p = (JsonTeleportFromSimName2)JsonUtil.Deserialize<JsonTeleportFromSimName2>(message);
            protocol.Teleport(p.simName, p.x, p.y, p.z, p.lookX, p.lookY, p.lookZ);
        }

        private void FunctionMovement(string message)
        {
            JsonMovement p = (JsonMovement)JsonUtil.Deserialize<JsonMovement>(message);

            if ((p.key & (int)JsonMovement.Type.Left) != 0)
                protocol.Rotation(Default.AGENT_TURN_SPEED);

            if ((p.key & (int)JsonMovement.Type.Right) != 0)
                protocol.Rotation(-Default.AGENT_TURN_SPEED);

            if ((p.key & (int)JsonMovement.Type.RotationUpdate) != 0)
                protocol.Rotation(0);

            protocol.Forward(((p.key & (int)JsonMovement.Type.Forward) != 0));
            protocol.Backward(((p.key & (int)JsonMovement.Type.Backward) != 0));

            if ((p.key & (int)JsonMovement.Type.AlwaysRun) != 0)
                protocol.AlwaysRun(true);

            if ((p.key & (int)JsonMovement.Type.AlwaysWalk) != 0)
                protocol.AlwaysRun(false);
        }

        private void FunctionAgentHead(string message)
        {
            JsonAgentHead p = (JsonAgentHead)JsonUtil.Deserialize<JsonAgentHead>(message);
            protocol.AgentHead(p.angle);
        }

        private void FuntionRequestObjectInfo(string message)
        {
            JsonRequestObjectInfo p = (JsonRequestObjectInfo)JsonUtil.Deserialize<JsonRequestObjectInfo>(message);
            if (string.IsNullOrEmpty(p.id))
                return;

            ObjectData data;
            if (!Ox.DataStore.World.SimCollection.TryGetObject(p.id, out data))
                return;

            int type = (int)PointData.ObjectType.Avatar;
            int click = -1;
            string name = string.Empty;
            if (data is AvatarData)
            {
                if (data.Myself)
                    type = (int)PointData.ObjectType.AvatarSelf;

                AvatarData a = data as AvatarData;
                name = a.Name;
            }
            else if (data is PrimData)
            {
                PrimData primData = data as PrimData;
                type = (int)PointData.ObjectType.Prim;
                click = (int)primData.ClickActionType;
            }
            Ox.EventFire(JsonUtil.SerializeMessage(JsonType.ObjectInfo, new JsonObjectInfo(p.requestID, p.id, type, click, name, data.Position, data.Rotation, data.Scale)), true);
        }

        private void FuntionRequestPathInfo(string message)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("application", Ox.Paths.Application);
            dic.Add("user", Ox.Paths.User);
            dic.Add("cache", Ox.Paths.Cache);

            if (dic == null || dic.Count == 0)
                return;

            string[] keys = new string[dic.Count];
            string[] values = new string[dic.Count];
            int index = 0;
            foreach (string key in dic.Keys)
            {
                keys[index] = key;
                values[index] = dic[key];
                index++;
            }

            JsonPathInfo j = new JsonPathInfo(keys, values);
            Ox.EventFire(JsonUtil.SerializeMessage(JsonType.PathInfo, j), true);
        }
    }
}
