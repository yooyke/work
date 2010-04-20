using System;
using System.Collections.Generic;
using System.IO;
using IrrlichtNETCP;
using OxLoader;
using OxCore;
using OxCore.Data;
using OxJson;

namespace OxRender.Plugin.Default
{
    public class AvatarName : OxRenderComponentPlugin
    {
        private Dictionary<string, SceneNode> nameDic = new Dictionary<string, SceneNode>();
        private Queue<JsonObjectUpdated> pipline = new Queue<JsonObjectUpdated>();

        public AvatarName(Ox ox, Render render)
            : base(ox, render)
        {
            Ox.OnEvent += new OxEventHandler(Ox_OnEvent);
        }

        public override void Update(ApplicationTime time)
        {
            while (pipline.Count > 0)
            {
                JsonObjectUpdated j;
                lock (pipline)
                    j = pipline.Dequeue();

                if (Ox.DataStore.World.Status.Status < StatusData.Type.LoginEnd || StatusData.Type.Logout <= Ox.DataStore.World.Status.Status)
                    continue;

                if (string.IsNullOrEmpty(j.id))
                    continue;

                IOxRenderPluginAvatar avatar = (IOxRenderPluginAvatar)Ox.Service.Get(typeof(IOxRenderPluginAvatar));
                if (avatar != null)
                {
                    ObjectData[] datas = Ox.DataStore.World.SimCollection.GetAvatarAll();
                    if (datas != null)
                    {
                        foreach (ObjectData data in datas)
                        {
                            if (!(data is AvatarData))
                                continue;

                            AvatarData avatarData = data as AvatarData;

                            SceneNode node = avatar.GetAvatarScneNode(avatarData.ID);
                            if (node == null)
                                return;

                            switch (j.type)
                            {
                                case (int)JsonObjectUpdated.Type.Add:
                                    Create(avatarData.ID, node, avatarData.First, avatarData.Last);
                                    break;
                                case (int)JsonObjectUpdated.Type.Delete:
                                    Delete(avatarData.ID);
                                    break;
                            }
                        }
                    }
                }
            }

            base.Update(time);
        }

        public override void Unload()
        {
            pipline.Clear();
            nameDic.Clear();

            base.Unload();
        }

        void Ox_OnEvent(string message)
        {
            JsonMessage parse_msg;
            JsonType type = JsonUtil.DeserializeMessage(message, out parse_msg);
            if (type != JsonType.ObjectUpdated)
                return;

            JsonObjectUpdated j = (JsonObjectUpdated)JsonUtil.Deserialize<JsonObjectUpdated>(parse_msg.value);
            if (j.prim == (int)JsonObjectUpdated.PrimType.Prim || (j.type != (int)JsonObjectUpdated.Type.Add && j.type != (int)JsonObjectUpdated.Type.Delete))
                return;

            lock (pipline)
                pipline.Enqueue(j);
        }

        private void Create(string id, SceneNode node, string first, string last)
        {
            if (nameDic.ContainsKey(id))
                nameDic.Remove(id);

            TextSceneNode tsn;
            tsn = Render.Scene.AddTextSceneNode(Render.GUI.BuiltInFont, string.Format("{0} {1}", first, last), Color.Black, node);
            tsn.Position = new Vector3D(0, 192, 0);

            nameDic.Add(id, tsn);
        }

        private void Delete(string id)
        {
            nameDic.Remove(id);
        }
    }
}
