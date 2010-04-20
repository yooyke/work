using System;
using System.Collections.Generic;
using OxCore;
using OxCore.Data;
using OxJson;

namespace OxViewer
{
    class Status : OxComponent
    {
        private Queue<StatusData.Type> pipline = new Queue<StatusData.Type>();

        public Status(Ox ox)
            : base(ox)
        {
            Priority = (int)PriorityBase.ControllerStatus;

            Ox.OnEvent += new OxLoader.OxEventHandler(Ox_OnEvent);
        }

        public override void Update(ApplicationTime time)
        {
            while (pipline.Count > 0)
            {
                StatusData.Type type = StatusData.Type.None;
                lock (pipline)
                    type = pipline.Dequeue();

                if (Ox.DataStore.World.Status.Status != type)
                {
                    JsonState.Type t = JsonState.Type.None;
                    switch (type)
                    {
                        case StatusData.Type.Initialize:
                            t = JsonState.Type.Initialize;
                            break;
                        case StatusData.Type.InitializeEnd:
                            type = StatusData.Type.Waiting;
                            break;
                        case StatusData.Type.Running:
                            t = JsonState.Type.Login;
                            break;
                        case StatusData.Type.Waiting:
                            t = JsonState.Type.Logout;
                            break;
                        case StatusData.Type.CleanupEnd:
                            t = JsonState.Type.Cleanup;
                            break;
                    }

                    Ox.DataStore.World.Status.Status = type;

                    // send outside (to js)
                    if (t != JsonState.Type.None)
                        Ox.EventFire(JsonUtil.SerializeMessage(JsonType.State, new JsonState((int)t)), false);
                }
            }

            base.Update(time);
        }

        void Ox_OnEvent(string message)
        {
            JsonMessage parse_msg;
            JsonType type = JsonUtil.DeserializeMessage(message, out parse_msg);

            switch (type)
            {
                case JsonType.StateInside:
                    MessageState(parse_msg.value);
                    break;
                case JsonType.Login:
                    SetEnqueue(StatusData.Type.Login);
                    break;
                case JsonType.Logout:
                    SetEnqueue(StatusData.Type.Logout);
                    break;
            }
        }

        private void MessageState(string message)
        {
            JsonStateInside j = (JsonStateInside)JsonUtil.Deserialize<JsonStateInside>(message);
            SetEnqueue((StatusData.Type)j.state);
        }

        private void SetEnqueue(StatusData.Type type)
        {
            lock (pipline)
                pipline.Enqueue(type);
        }
    }
}
