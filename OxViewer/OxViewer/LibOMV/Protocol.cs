using System;
using System.Collections.Generic;
using System.Threading;
using OpenMetaverse;
using OxLoader;
using OxCore;
using OxCore.Data;
using OxJson;
using OxUtil;

namespace OxViewer.LibOMV
{
    partial class Protocol : OxComponent, IProtocol
    {
        private enum ControlType
        {
            None,
            Load,
            Unload,
        }

        private GridClient client;
        private LoginParams loginCurrentParam;
        private Dictionary<uint, OpenMetaverse.Primitive> objectDictByLocalID = new Dictionary<uint, Primitive>();
        private Queue<ControlType> queueControl = new Queue<ControlType>();
        private Queue<QueueAsset> queueAssetFactory = new Queue<QueueAsset>();
        private Queue<JsonObjectUpdated> queueMeshFactory = new Queue<JsonObjectUpdated>();
        private Thread threadAssetFactory;
        private Thread threadMeshFactory;

        public Protocol(Ox ox)
            : base(ox)
        {
            Priority = (int)PriorityBase.ControllerProtocol;

            Ox.OnEvent += new OxEventHandler(Ox_OnEvent);

            threadAssetFactory = new Thread(AssetFactory);
            threadAssetFactory.Start();

            threadMeshFactory = new Thread(MeshFactory);
            threadMeshFactory.Start();
        }

        public override void Dispose()
        {
            if (threadMeshFactory != null)
            {
                threadMeshFactory.Abort();
                threadMeshFactory = null;
            }

            if (threadAssetFactory != null)
            {
                threadAssetFactory.Abort();
                threadAssetFactory = null;
            }

            base.Dispose();
        }

        public override void Update(ApplicationTime time)
        {
            while (queueControl.Count > 0)
            {
                ControlType type = ControlType.None;
                lock (queueControl)
                    type = queueControl.Dequeue();

                switch (type)
                {
                    case ControlType.Load:
                        LoadProcess();
                        break;
                    case ControlType.Unload:
                        UnloadProcess();
                        break;
                }
            }

            while (processQueue.Count > 0)
            {
                QueueBase q;
                lock (processQueue)
                    q = processQueue.Dequeue();

                if (q.Method == null)
                    return;

                if (q.Args == null)
                    q.Method.DynamicInvoke();
                else
                    q.Method.DynamicInvoke(q.Args);
            }

            base.Update(time);
        }

        public override void Cleanup()
        {
            Logout();

            base.Cleanup();
        }

        void Ox_OnEvent(string message)
        {
            JsonMessage parse_msg;
            JsonType type = JsonUtil.DeserializeMessage(message, out parse_msg);

            switch (type)
            {
                case JsonType.StateInside:
                    StateInside(parse_msg.value);
                    break;
                case JsonType.ObjectUpdated:
                    ObjectUpdate(parse_msg.value);
                    break;
            }
        }

        private void LoadProcess()
        {
        }

        private void UnloadProcess()
        {
            Ox.DataStore.World.Reset();

            lock (processQueue)
                processQueue.Clear();

            lock (objectDictByLocalID)
                objectDictByLocalID.Clear();

            lock (queueMeshFactory)
                queueMeshFactory.Clear();
        }

        private void StateInside(string message)
        {
            JsonStateInside j = (JsonStateInside)JsonUtil.Deserialize<JsonStateInside>(message);
            switch (j.state)
            {
                case (int)StatusData.Type.LoginEnd:
                    lock (queueControl) queueControl.Enqueue(ControlType.Load);
                    break;
                case (int)StatusData.Type.LogoutEnd:
                    lock (queueControl) queueControl.Enqueue(ControlType.Unload);
                    break;
            }
        }

        private void ObjectUpdate(string message)
        {
            JsonObjectUpdated j = (JsonObjectUpdated)JsonUtil.Deserialize<JsonObjectUpdated>(message);
            if (j.prim == (int)JsonObjectUpdated.PrimType.Avatar || j.type != (int)JsonObjectUpdated.Type.Add)
                return;

            MeshFactoryEnqueue(j);
        }

        public void Login(string first, string last, string pass, string uri)
        {
            if (client != null)
                Logout();

            Setup();

            loginCurrentParam = client.Network.DefaultLoginParams(first, last, pass, Default.LOGIN_USER_AGENT, Default.LOGIN_USER_VERSION);
            if (string.IsNullOrEmpty(uri))
            {
                client.Network.Login(loginCurrentParam);
            }
            else
            {
                loginCurrentParam.URI = uri;
                client.Network.Login3Di(loginCurrentParam);
            }
        }

        public void Logout()
        {
            if (client != null)
            {
                lock (client)
                {
                    client.Network.Logout();
                    Reset();
                    client = null;
                }
            }

            Ox.EventFire(JsonUtil.SerializeMessage(JsonType.StateInside, new JsonState((int)StatusData.Type.LogoutEnd)), true);
        }

        public void RequestImage(string iamgeID)
        {
            if (client == null)
                return;

            //client.Assets.RequestImage(new UUID(iamgeID), ImageType.Normal);
        }

        public void AlwaysRun(bool run)
        {
            Ox.DataStore.World.Agent.AlwaysRun = run;

            if (client != null)
            {
                if (run != client.Self.Movement.AlwaysRun)
                    client.Self.Movement.AlwaysRun = run;
            }
        }

        public void Rotation(float velocity)
        {
            Ox.DataStore.World.Agent.Head += velocity;

            if (Ox.DataStore.World.Agent.Head > MathHelper.TwoPI)
                Ox.DataStore.World.Agent.Head -= MathHelper.TwoPI;

            if (Ox.DataStore.World.Agent.Head < -MathHelper.TwoPI)
                Ox.DataStore.World.Agent.Head += MathHelper.TwoPI;

            if (client != null)
                client.Self.Movement.UpdateFromHeading(Ox.DataStore.World.Agent.Head, false);
        }

        public void Forward(bool press)
        {
            if (client == null)
                return;

            client.Self.Movement.AtPos = press;
        }

        public void Backward(bool press)
        {
            if (client == null)
                return;

            client.Self.Movement.AtNeg = press;
        }

        /// <summary>
        /// Send chat
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="channel">channel (default is 0)</param>
        /// <param name="type">
        /// Whisper = 0,
        /// Normal = 1,
        /// Shout = 2,
        /// StartTyping = 4,
        /// StopTyping = 5,
        /// Debug = 6,
        /// OwnerSay = 8,
        /// </param>
        public void Chat(string message, int channel, int type)
        {
            if (client == null)
                return;

            client.Self.Chat(message, channel, (ChatType)type);
        }

        public void IM(string targetUUID, string message)
        {
            if (client == null)
                return;

            client.Self.InstantMessage(new UUID(targetUUID), message);
        }

        public void Sit(string targetUUID)
        {
            if (client == null)
                return;

            client.Self.RequestSit(new UUID(targetUUID), new Vector3());
        }

        public void Standup()
        {
            if (client == null)
                return;

            if (client.Self.SittingOn != 0)
                client.Self.Stand();
        }

        public void Touch(string targetUUID)
        {
            if (client == null)
                return;

            client.Self.Touch(GetLocalIDFromID(targetUUID));
        }

        public void Teleport(string landmarkUUID)
        {
            if (client == null)
                return;

            client.Self.Teleport(new UUID(landmarkUUID));
        }

        public void Teleport(string simName, float x, float y, float z)
        {
            Teleport(simName, x, y, z, 0, 0, 0);
        }

        public void Teleport(string simName, float x, float y, float z, float lookX, float lookY, float lookZ)
        {
            if (client == null)
                return;

            client.Self.Teleport(simName, new Vector3(x, y, z), new Vector3(lookX, lookY, lookZ));
        }

        public void AgentHead(float angle)
        {
            Ox.DataStore.World.Agent.Head = angle;
        }
    }
}
