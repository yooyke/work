using System;
using System.Collections.Generic;
using OpenMetaverse;
using OxCore;
using OxJson;
using OxCore.Data;
using OpenMetaverse.Packets;

namespace OxViewer.LibOMV
{
    partial class Protocol
    {
        private void ProcessLogin(OpenMetaverse.LoginStatus login, string message)
        {
            string beforeStatus = Ox.DataStore.World.Agent.LoginStatus;
            Ox.DataStore.World.Agent.LoginStatus = login.ToString();
            Ox.DataStore.World.Agent.LoginMessage = message;

            Ox.EventFire(JsonUtil.SerializeMessage(JsonType.StateInside, new JsonState((int)StatusData.Type.LoginEnd)), true);

            switch (login)
            {
                case LoginStatus.ConnectingToLogin:
                    break;

                case LoginStatus.ConnectingToSim:
                    break;

                case LoginStatus.Failed:
                    if (Ox.DataStore.World.Agent.LoginFailedCount > 0)
                    {
                        Ox.Function(JsonUtil.SerializeMessage(JsonType.Logout, new JsonLogout()));
                        System.Windows.Forms.MessageBox.Show("Login failed. Please wait and retry.");
                        break;
                    }

                    Ox.Function(JsonUtil.SerializeMessage(JsonType.Login, new JsonLogin(
                        loginCurrentParam.FirstName,
                        loginCurrentParam.LastName,
                        loginCurrentParam.Password,
                        loginCurrentParam.URI,
                        loginCurrentParam.Start
                        )));
                    Ox.DataStore.World.Agent.LoginFailedCount++;
                    break;

                case LoginStatus.None:
                    break;

                case LoginStatus.ReadingResponse:
                    break;

                case LoginStatus.Redirecting:
                    break;

                case LoginStatus.Success:
                    Ox.DataStore.World.Agent.LoginFailedCount = 0;
                    Ox.DataStore.World.Agent.AssetServerUri = client.Network.AssetServerUri;
                    Ox.DataStore.World.Agent.AuthToken = client.Network.SecureSessionID.ToString();
                    Ox.DataStore.World.Agent.SimID = client.Network.CurrentSim.ID.ToString();
                    Ox.DataStore.World.Agent.ID = client.Self.AgentID.ToString();
                    Ox.DataStore.World.Agent.First = client.Self.FirstName;
                    Ox.DataStore.World.Agent.Last = client.Self.LastName;
                    Ox.EventFire(JsonUtil.SerializeMessage(JsonType.AgentInfo, new JsonAgentInfo(
                        Ox.DataStore.World.Agent.ID,
                        Ox.DataStore.World.Agent.SimID,
                        Ox.DataStore.World.Agent.First,
                        Ox.DataStore.World.Agent.Last
                        )), true);
                    break;
            }
        }

        private void ProcessSimConnected(OpenMetaverse.Simulator simulator)
        {
            if (simulator.ID == UUID.Zero)
                return;

            uint x, y;
            OpenMetaverse.Utils.LongToUInts(simulator.Handle, out x, out y);
            Ox.DataStore.World.SimCollection.Add(simulator.ID.ToString(), simulator.Handle, x, y);
        }

        private void ProcessCurrentSimChanged(OpenMetaverse.Simulator PreviousSimulator)
        {
            Ox.DataStore.World.Agent.SimID = client.Network.CurrentSim.ID.ToString();
            Ox.DataStore.World.Agent.SimName = client.Network.CurrentSim.Name;
        }

        private void ProcessSimDisconnected(OpenMetaverse.Simulator simulator, OpenMetaverse.NetworkManager.DisconnectType reason)
        {
            Ox.DataStore.World.SimCollection.Delete(simulator.ID.ToString());

            switch (reason)
            {
                case NetworkManager.DisconnectType.ClientInitiated:
                    Console.WriteLine("ProcessSimDisconnected : " + simulator.ID.ToString() + " ClientInitiated");
                    break;

                case NetworkManager.DisconnectType.NetworkTimeout:
                    Console.WriteLine("ProcessSimDisconnected : " + simulator.ID.ToString() + " NetworkTimeout");
                    break;

                case NetworkManager.DisconnectType.ServerInitiated:
                    Console.WriteLine("ProcessSimDisconnected : " + simulator.ID.ToString() + " ServerInitiated");
                    break;

                case NetworkManager.DisconnectType.SimShutdown:
                    Console.WriteLine("ProcessSimDisconnected : " + simulator.ID.ToString() + " SimShutdown");
                    break;
            }
        }

        private void ProcessAssetReceived(AssetDownload transfer, OpenMetaverse.Asset asset)
        {
            Console.WriteLine("AssetID : {0} AssetType : {1} Channel : {2} ID : {3} SimID : {4} Size : {5} Source : {6 Status : {7} Success : {8} Target : {9} AssetID : {10} Type : {11}",
                transfer.AssetID.ToString(),
                transfer.AssetType.ToString(),
                transfer.Channel.ToString(),
                transfer.ID.ToString(),
                transfer.Simulator.ID.ToString(),
                transfer.Size.ToString(),
                transfer.Source.ToString(),
                transfer.Status.ToString(),
                transfer.Success.ToString(),
                transfer.Target.ToString(),
                asset.AssetID.ToString(),
                asset.AssetType.ToString()
                );
        }

        private void ProcessImageReceived(ImageDownload image, AssetTexture asset)
        {
            if (image.Success)
                AssetFactoryEnqueue(new QueueAsset(image, asset));
        }

        private void ProcessNewAvatar(OpenMetaverse.Simulator simulator, OpenMetaverse.Avatar avatar, ulong regionHandle, ushort timeDilation)
        {
            if (avatar == null) return;

            if (!Ox.DataStore.World.SimCollection.Contains(simulator.ID.ToString()))
                ProcessSimConnected(simulator);

            string parentID = GetIDFromLocalID(avatar.ParentID);
            Ox.DataStore.World.SimCollection.AddAvatar(
                simulator.ID.ToString(),
                avatar.ID.ToString(),
                avatar.FirstName,
                avatar.LastName, parentID,
                new float[] { avatar.Position.X, avatar.Position.Y, avatar.Position.Z },
                new float[] { avatar.Rotation.X, avatar.Rotation.Y, avatar.Rotation.Z, avatar.Rotation.W },
                new float[] { avatar.Scale.X, avatar.Scale.Y, avatar.Scale.Z },
                new float[] { avatar.Velocity.X, avatar.Velocity.Y, avatar.Velocity.Z },
                (avatar.ID.ToString() == Ox.DataStore.World.Agent.ID)
                );

            AddObjectDictByLocalID(avatar);

            string msg = JsonUtil.SerializeMessage(JsonType.ObjectUpdated, new JsonObjectUpdated(
                simulator.ID.ToString(),
                avatar.ID.ToString(),
                (int)JsonObjectUpdated.PrimType.Avatar,
                (int)JsonObjectUpdated.Type.Add
                ));
            Ox.EventFire(msg, false);
        }

        private void ProcessNewPrim(OpenMetaverse.Simulator simulator, OpenMetaverse.Primitive prim, ulong regionHandle, ushort timeDilation)
        {
            if (prim == null) return;

            if (!Ox.DataStore.World.SimCollection.Contains(simulator.ID.ToString()))
                ProcessSimConnected(simulator);

            ClickActionType clickType = GetClickType(prim);

            string sceneName = string.Empty;
            List<byte[]> datas = prim.OpaqueExtraData;
            foreach (byte[] data in datas)
	        {
                int blockID = data[0] + 256 * data[1];
                int blockLen = (((data[5]) * 256 + data[4]) * 256 + data[3]) * 256 + data[2];
                if (blockID == 0x200) // 3Di data
                {
                    int iByte = 4 + 2; // skip type and length
                    UUID irrFile = new UUID(data, iByte); iByte += 16;
                    UUID colMesh = new UUID(data, iByte); iByte += 16;
                    sceneName = irrFile.ToString() + ".irr";
                    break;
                }
	        }

            string parentID = GetIDFromLocalID(prim.ParentID);
            Ox.DataStore.World.SimCollection.AddPrim(
                simulator.ID.ToString(),
                prim.ID.ToString(),
                parentID,
                sceneName,
                new float[] { prim.Position.X, prim.Position.Y, prim.Position.Z },
                new float[] { prim.Rotation.X, prim.Rotation.Y, prim.Rotation.Z, prim.Rotation.W },
                new float[] { prim.Scale.X, prim.Scale.Y, prim.Scale.Z },
                new float[] { prim.Velocity.X, prim.Velocity.Y, prim.Velocity.Z },
                false,
                clickType
                );

            AddObjectDictByLocalID(prim);

            string msg = JsonUtil.SerializeMessage(JsonType.ObjectUpdated, new JsonObjectUpdated(
                simulator.ID.ToString(),
                prim.ID.ToString(),
                (int)JsonObjectUpdated.PrimType.Prim,
                (int)JsonObjectUpdated.Type.Add
                ));
            Ox.EventFire(msg, false);
        }

        private void ProcessObjectUpdated(Simulator simulator, ObjectUpdate update, ulong regionHandle, ushort timeDilation)
        {
            OpenMetaverse.Primitive p = GetObjectFromLocalID(update.LocalID);
            if (p == null) return;

            ObjectData objectData;
            if (Ox.DataStore.World.SimCollection.TryGetObject(simulator.ID.ToString(), p.ID.ToString(), out objectData))
            {
                objectData.OPosition = new float[] { p.Position.X, p.Position.Y, p.Position.Z };
                objectData.OQuaternion = new float[] { p.Rotation.X, p.Rotation.Y, p.Rotation.Z, p.Rotation.W };
                objectData.Scale = new float[] { p.Scale.X, p.Scale.Y, p.Scale.Z };
                objectData.Velocity = new float[] { p.Velocity.X, p.Velocity.Y, p.Velocity.Z };

                string msg = JsonUtil.SerializeMessage(JsonType.ObjectUpdated, new JsonObjectUpdated(
                    simulator.ID.ToString(),
                    p.ID.ToString(),
                    (p is Avatar ? (int)JsonObjectUpdated.PrimType.Avatar : (int)JsonObjectUpdated.PrimType.Prim),
                    (int)JsonObjectUpdated.Type.Update
                    ));
                Ox.EventFire(msg, false);
            }
        }

        private void ProcessObjectKilled(OpenMetaverse.Simulator simulator, uint objectID)
        {
            OpenMetaverse.Primitive p = GetObjectFromLocalID(objectID);
            if (p == null) return;

            Ox.DataStore.World.SimCollection.DeleteObject(simulator.ID.ToString(), p.ID.ToString());
            DeleteObjectDictByLocalID(objectID);

            string msg = JsonUtil.SerializeMessage(JsonType.ObjectUpdated, new JsonObjectUpdated(
                simulator.ID.ToString(),
                p.ID.ToString(),
                (p is Avatar ? (int)JsonObjectUpdated.PrimType.Avatar : (int)JsonObjectUpdated.PrimType.Prim),
                (int)JsonObjectUpdated.Type.Delete
                ));
            Ox.EventFire(msg, false);
        }

        private void ProcessAlertMessage(string message)
        {
            Console.WriteLine("ProcessAlertMessage : " + message);
        }

        private void ProcessLandPatch(OpenMetaverse.Simulator simulator, int x, int y, int width, float[] data)
        {
            SimData simData;
            if (Ox.DataStore.World.SimCollection.TryGet(simulator.ID.ToString(), out simData))
            {
                simData.Terrain.SetData(x, y, data);
            }
        }

        private void ProcessChat(string message, ChatAudibleLevel audible, ChatType type, ChatSourceType sourceType, string fromName, OpenMetaverse.UUID id, OpenMetaverse.UUID ownerid, OpenMetaverse.Vector3 position)
        {
            string msg = JsonUtil.SerializeMessage(JsonType.ChatReceived, new JsonChatReceived(
                message,
                (int)type,
                fromName,
                id.ToString(),
                ownerid.ToString(),
                position.X,
                position.Y,
                position.Z
                ));
            Ox.EventFire(msg, true);
        }

        private void ProcessIM(OpenMetaverse.InstantMessage im, OpenMetaverse.Simulator simulator)
        {
            string msg = JsonUtil.SerializeMessage(JsonType.IMReceived, new JsonIMReceived(
                im.FromAgentID.ToString(),
                im.FromAgentName,
                (im.GroupIM ? 1 : 0),
                im.IMSessionID.ToString(),
                im.Message,
                im.Position.X,
                im.Position.Y,
                im.Position.Z,
                im.RegionID.ToString(),
                im.Timestamp.ToString(),
                im.ToAgentID.ToString()
                ));
            Ox.EventFire(msg, true);
        }

        private void ProcessTeleport(string message, AgentManager.TeleportStatus status, AgentManager.TeleportFlags flags)
        {
            switch (status)
            {
                case AgentManager.TeleportStatus.Cancelled:
                    Console.WriteLine("ProcessTeleport Cancelled : {0}", message);
                    break;

                case AgentManager.TeleportStatus.Failed:
                    Console.WriteLine("ProcessTeleport Failed : {0}", message);
                    break;

                case AgentManager.TeleportStatus.Finished:
                    Console.WriteLine("ProcessTeleport Finished : {0}", message);
                    break;

                case AgentManager.TeleportStatus.None:
                    Console.WriteLine("ProcessTeleport None : {0}", message);
                    break;

                case AgentManager.TeleportStatus.Progress:
                    Console.WriteLine("ProcessTeleport Progress : {0}", message);
                    break;

                case AgentManager.TeleportStatus.Start:
                    Console.WriteLine("ProcessTeleport Start : {0}", message);
                    break;
            }
        }

        private void ProcessAnimationsPacket(Packet packet, Simulator simulator)
        {
            if (!(packet is AvatarAnimationPacket))
                return;

            AvatarAnimationPacket animation = (AvatarAnimationPacket)packet;

            SimData simData;
            if (!Ox.DataStore.World.SimCollection.TryGet(simulator.ID.ToString(), out simData))
                return;

            ObjectData objectData;
            if (!simData.TryGet(animation.Sender.ID.ToString(), out objectData))
                return;

            if (!(objectData is AvatarData))
                return;

            AvatarData avatarData = objectData as AvatarData;

            List<AvatarData.Animation> anims = new List<AvatarData.Animation>();
            foreach (AvatarAnimationPacket.AnimationListBlock block in animation.AnimationList)
                anims.Add(new AvatarData.Animation(block.AnimID.ToString(), block.AnimSequenceID));
            avatarData.UpdateAnimation(anims.ToArray());

            string msg = JsonUtil.SerializeMessage(JsonType.ObjectUpdated, new JsonObjectUpdated(
                simulator.ID.ToString(),
                animation.Sender.ID.ToString(),
                (int)JsonObjectUpdated.PrimType.Avatar,
                (int)JsonObjectUpdated.Type.UpdateAnimation
                ));
            Ox.EventFire(msg, true);
        }
    }
}
