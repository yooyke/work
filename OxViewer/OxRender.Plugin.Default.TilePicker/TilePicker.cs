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
    public class TilePicker : OxRenderTilePickerPlugin
    {
        private Queue<JsonObjectUpdated> pipline = new Queue<JsonObjectUpdated>();
        private string dir;
        private MeshSceneNode node;

        public TilePicker(Ox ox, Render render)
            : base(ox, render)
        {
            Priority = (int)PriorityBase.RenderPick;
            Ox.OnEvent += new OxEventHandler(Ox_OnEvent);

            dir = Path.Combine(Path.Combine(Ox.Paths.Application, "media"), "picker");
        }

        public override void Load()
        {
            if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                CreatePicker();

            base.Load();
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

                if (j.type == (int)JsonObjectUpdated.Type.Add || j.type == (int)JsonObjectUpdated.Type.Update)
                    UpdatePosition();
            }

            base.Update(time);
        }

        public override void Unload()
        {
            if (Root.Children.Length > 0)
                Root.RemoveAll();

            node = null;

            base.Unload();
        }

        public override void Collision()
        {
            if (node == null)
                return;

            if (Render.Scene.ActiveCamera == null)
                return;

            Vector3D intersection = new Vector3D();
            Triangle3D triangle = new Triangle3D();

            Position2D mouse = new Position2D((int)Ox.DataStore.Input.Position[0], (int)Ox.DataStore.Input.Position[1]);
            Line3D line = Render.Scene.CollisionManager.GetRayFromScreenCoordinates(mouse, Render.Scene.ActiveCamera);

            bool hit = Render.Scene.CollisionManager.GetCollisionPoint(
                        line,
                        node.TriangleSelector,
                        out intersection,
                        out triangle
                        );

            if (hit)
            {
                float[] point = Util.ToPositionArrayFromIrrlicht(ref intersection);
                float length = (float)intersection.DistanceFrom(line.Start);
                PointData.HitData data = new PointData.HitData(point[0], point[1], point[2], PointData.ObjectType.Ground, ClickActionType.None, length, string.Empty);
                Ox.DataStore.World.Point.Add(ref data);
            }
        }

        void Ox_OnEvent(string message)
        {
            JsonMessage parse_msg;
            JsonType type = JsonUtil.DeserializeMessage(message, out parse_msg);
            if (type != JsonType.ObjectUpdated && type != JsonType.StateInside)
                return;

            JsonObjectUpdated j;
            if (type == JsonType.ObjectUpdated)
            {
                j = (JsonObjectUpdated)JsonUtil.Deserialize<JsonObjectUpdated>(parse_msg.value);
                if (j.prim == (int)JsonObjectUpdated.PrimType.Prim || j.id != Ox.DataStore.World.Agent.ID) // If j.prim is prim. do return
                    return;
            }
            else
            {
                JsonStateInside state = (JsonStateInside)JsonUtil.Deserialize<JsonStateInside>(parse_msg.value);
                if (state.state != (int)StatusData.Type.RunningBef)
                    return;

                j.id = Ox.DataStore.World.Agent.ID;
                j.simID = Ox.DataStore.World.Agent.SimID;
                j.prim = (int)JsonObjectUpdated.PrimType.Avatar;
                j.type = (int)JsonObjectUpdated.Type.Update;
            }

            pipline.Enqueue(j);
        }

        private void CreatePicker()
        {
            string path = Path.Combine(dir, "tile.x");

            if (File.Exists(path))
            {
                AnimatedMesh mesh = Render.Scene.GetMesh(path);
                if (mesh != null)
                    node = Render.Scene.AddMeshSceneNode(mesh.GetMesh(0), Root, -1);

                if (node != null)
                {
                    node.TriangleSelector = Render.Scene.CreateTriangleSelector(mesh.GetMesh(0), node);
                    node.SetMaterialType(MaterialType.TransparentAlphaChannel);
                    node.Rotation = Util.ROTATION_OFFSET;
                }
            }
        }

        private void UpdatePosition()
        {
            if (node == null)
                return;

            IOxRenderPluginAvatar avatar = (IOxRenderPluginAvatar)Ox.Service.Get(typeof(IOxRenderPluginAvatar));
            SceneNode sn =avatar.GetAvatarScneNode(Ox.DataStore.World.Agent.ID);
            if (sn == null)
                return;

            node.Position = sn.Position;
        }
    }
}
