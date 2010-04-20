using System;
using System.Collections.Generic;
using OxCore;
using OpenMetaverse;
using OpenMetaverse.Packets;

namespace OxViewer.LibOMV
{
    partial class Protocol
    {
        private Queue<QueueBase> processQueue = new Queue<QueueBase>();

        private void Setup()
        {
            client = new GridClient();
            client.Network.OnLogin += new NetworkManager.LoginCallback(Network_OnLogin);
            //client.Network.OnLogoutReply += new NetworkManager.LogoutCallback(Network_OnLogoutReply); // I tried this event. But It didn't call.
            client.Network.OnSimConnected += new NetworkManager.SimConnectedCallback(Network_OnSimConnected);
            client.Network.OnCurrentSimChanged += new NetworkManager.CurrentSimChangedCallback(Network_OnCurrentSimChanged);
            client.Network.OnSimDisconnected += new NetworkManager.SimDisconnectedCallback(Network_OnSimDisconnected);
            client.Assets.OnAssetReceived += new AssetManager.AssetReceivedCallback(Assets_OnAssetReceived);
            client.Assets.OnImageReceived += new AssetManager.ImageReceivedCallback(Assets_OnImageReceived);
            client.Objects.OnNewAvatar += new ObjectManager.NewAvatarCallback(Objects_OnNewAvatar);
            client.Objects.OnNewPrim += new ObjectManager.NewPrimCallback(Objects_OnNewPrim);
            client.Objects.OnObjectUpdated += new ObjectManager.ObjectUpdatedCallback(Objects_OnObjectUpdated);
            client.Objects.OnObjectKilled += new ObjectManager.KillObjectCallback(Objects_OnObjectKilled);
            client.Terrain.OnLandPatch += new TerrainManager.LandPatchCallback(Terrain_OnLandPatch);
            client.Self.OnAlertMessage += new AgentManager.AlertMessageCallback(Self_OnAlertMessage);
            client.Self.OnChat += new AgentManager.ChatCallback(Self_OnChat);
            client.Self.OnInstantMessage += new AgentManager.InstantMessageCallback(Self_OnInstantMessage);
            client.Self.OnTeleport += new AgentManager.TeleportCallback(Self_OnTeleport);

            client.Network.RegisterCallback(OpenMetaverse.Packets.PacketType.AvatarAnimation, new NetworkManager.PacketCallback(AvatarAnimationPacketCallback));
        }

        private void Reset()
        {
            client.Network.OnLogin -= new NetworkManager.LoginCallback(Network_OnLogin);
            //client.Network.OnLogoutReply -= new NetworkManager.LogoutCallback(Network_OnLogoutReply);
            client.Network.OnSimConnected -= new NetworkManager.SimConnectedCallback(Network_OnSimConnected);
            client.Network.OnCurrentSimChanged -= new NetworkManager.CurrentSimChangedCallback(Network_OnCurrentSimChanged);
            client.Network.OnSimDisconnected -= new NetworkManager.SimDisconnectedCallback(Network_OnSimDisconnected);
            client.Assets.OnAssetReceived -= new AssetManager.AssetReceivedCallback(Assets_OnAssetReceived);
            client.Assets.OnImageReceived -= new AssetManager.ImageReceivedCallback(Assets_OnImageReceived);
            client.Objects.OnNewAvatar -= new ObjectManager.NewAvatarCallback(Objects_OnNewAvatar);
            client.Objects.OnNewPrim -= new ObjectManager.NewPrimCallback(Objects_OnNewPrim);
            client.Objects.OnObjectUpdated -= new ObjectManager.ObjectUpdatedCallback(Objects_OnObjectUpdated);
            client.Objects.OnObjectKilled -= new ObjectManager.KillObjectCallback(Objects_OnObjectKilled);
            client.Terrain.OnLandPatch -= new TerrainManager.LandPatchCallback(Terrain_OnLandPatch);
            client.Self.OnAlertMessage -= new AgentManager.AlertMessageCallback(Self_OnAlertMessage);
            client.Self.OnChat -= new AgentManager.ChatCallback(Self_OnChat);
            client.Self.OnInstantMessage -= new AgentManager.InstantMessageCallback(Self_OnInstantMessage);
            client.Self.OnTeleport -= new AgentManager.TeleportCallback(Self_OnTeleport);

            client.Network.UnregisterCallback(PacketType.AvatarAnimation, new NetworkManager.PacketCallback(AvatarAnimationPacketCallback));
        }

        void Network_OnLogin(LoginStatus login, string message)
        {
            lock (processQueue) processQueue.Enqueue(new QueueBase(new NetworkManager.LoginCallback(ProcessLogin), login, message));
        }

        void Network_OnSimConnected(Simulator simulator)
        {
            lock (processQueue) processQueue.Enqueue(new QueueBase(new NetworkManager.SimConnectedCallback(ProcessSimConnected), simulator));
        }

        void Network_OnCurrentSimChanged(Simulator PreviousSimulator)
        {
            lock (processQueue) processQueue.Enqueue(new QueueBase(new NetworkManager.CurrentSimChangedCallback(ProcessCurrentSimChanged), PreviousSimulator));
        }

        void Network_OnSimDisconnected(Simulator simulator, NetworkManager.DisconnectType reason)
        {
            lock (processQueue) processQueue.Enqueue(new QueueBase(new NetworkManager.SimDisconnectedCallback(ProcessSimDisconnected), simulator, reason));
        }

        void Assets_OnAssetReceived(AssetDownload transfer, OpenMetaverse.Asset asset)
        {
            lock (processQueue) processQueue.Enqueue(new QueueBase(new AssetManager.AssetReceivedCallback(ProcessAssetReceived), transfer, asset));
        }

        void Assets_OnImageReceived(ImageDownload image, AssetTexture asset)
        {
            lock (processQueue) processQueue.Enqueue(new QueueBase(new AssetManager.ImageReceivedCallback(ProcessImageReceived), image, asset));
        }

        void Objects_OnNewAvatar(Simulator simulator, Avatar avatar, ulong regionHandle, ushort timeDilation)
        {
            lock (processQueue) processQueue.Enqueue(new QueueBase(new ObjectManager.NewAvatarCallback(ProcessNewAvatar), simulator, avatar, regionHandle, timeDilation));
        }

        void Objects_OnNewPrim(Simulator simulator, Primitive prim, ulong regionHandle, ushort timeDilation)
        {
            lock (processQueue) processQueue.Enqueue(new QueueBase(new ObjectManager.NewPrimCallback(ProcessNewPrim), simulator, prim, regionHandle, timeDilation));
        }

        void Objects_OnObjectUpdated(Simulator simulator, ObjectUpdate update, ulong regionHandle, ushort timeDilation)
        {
            lock (processQueue) processQueue.Enqueue(new QueueBase(new ObjectManager.ObjectUpdatedCallback(ProcessObjectUpdated), simulator, update, regionHandle, timeDilation));
        }

        void Objects_OnObjectKilled(Simulator simulator, uint objectID)
        {
            lock (processQueue) processQueue.Enqueue(new QueueBase(new ObjectManager.KillObjectCallback(ProcessObjectKilled), simulator, objectID));
        }

        void Terrain_OnLandPatch(Simulator simulator, int x, int y, int width, float[] data)
        {
            lock (processQueue) processQueue.Enqueue(new QueueBase(new TerrainManager.LandPatchCallback(ProcessLandPatch), simulator, x, y, width, data));
        }

        void Self_OnAlertMessage(string message)
        {
            lock (processQueue) processQueue.Enqueue(new QueueBase(new AgentManager.AlertMessageCallback(ProcessAlertMessage), message));
        }

        void Self_OnChat(string message, ChatAudibleLevel audible, ChatType type, ChatSourceType sourceType, string fromName, UUID id, UUID ownerid, Vector3 position)
        {
            lock (processQueue) processQueue.Enqueue(new QueueBase(new AgentManager.ChatCallback(ProcessChat), message, audible, type, sourceType, fromName, id, ownerid, position));
        }

        void Self_OnInstantMessage(InstantMessage im, Simulator simulator)
        {
            lock (processQueue) processQueue.Enqueue(new QueueBase(new AgentManager.InstantMessageCallback(ProcessIM), im, simulator));
        }

        void Self_OnTeleport(string message, AgentManager.TeleportStatus status, AgentManager.TeleportFlags flags)
        {
            lock (processQueue) processQueue.Enqueue(new QueueBase(new AgentManager.TeleportCallback(ProcessTeleport), message, status, flags));
        }

        void AvatarAnimationPacketCallback(Packet packet, Simulator simulator)
        {
            lock (processQueue) processQueue.Enqueue(new QueueBase(new NetworkManager.PacketCallback(ProcessAnimationsPacket), packet, simulator));
        }
    }
}
