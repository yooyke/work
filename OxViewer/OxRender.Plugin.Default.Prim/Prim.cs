using System;
using System.Collections.Generic;
using System.IO;
using IrrlichtNETCP;
using OxLoader;
using OxCore;
using OxCore.Data;
using OxJson;

namespace OxRender.Plugin.Prim
{
    public class Prim : OxRenderPrimPlugin
    {
        private Dictionary<string, SceneNode> list = new Dictionary<string, SceneNode>();
        private Queue<JsonObjectUpdated> pipline = new Queue<JsonObjectUpdated>();
        private string dir;

        public Prim(Ox ox, Render render)
            : base(ox, render)
        {
            Priority = (int)PriorityBase.RenderPrim;

            ox.OnEvent += new OxEventHandler(ox_OnEvent);
            dir = Path.Combine(Path.Combine(Ox.Paths.Application, "media"), "test");
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

                switch (j.type)
                {
                    case (int)JsonObjectUpdated.Type.Add:
                    case (int)JsonObjectUpdated.Type.UpdateFull:
                        CreatePrim(ref j);
                        j.type = (int)JsonObjectUpdated.Type.Update;
                        pipline.Enqueue(j);
                        break;
                    case (int)JsonObjectUpdated.Type.Update:
                        UpdatePrim(ref j);
                        break;
                    case (int)JsonObjectUpdated.Type.Delete:
                        DeletePrim(j.id);
                        break;
                }
            }

            base.Update(time);
        }

        public override void Unload()
        {
            pipline.Clear();
            list.Clear();

            if (Root.Children.Length > 0)
                Root.RemoveAll();

            base.Unload();
        }

        public override SceneNode GetPrimScneNode(string key)
        {
            if (list.ContainsKey(key))
                return list[key];

            return null;
        }

        public override void Collision()
        {
            if (Render.Scene.ActiveCamera == null)
                return;

            Position2D mouse = new Position2D((int)Ox.DataStore.Input.Position[0], (int)Ox.DataStore.Input.Position[1]);
            Line3D line = Render.Scene.CollisionManager.GetRayFromScreenCoordinates(mouse, Render.Scene.ActiveCamera);

            foreach (string key in list.Keys)
            {
                SceneNode node = list[key];

                if (node.TriangleSelector == null) // <-- 3dimesh
                    continue;

                Box3D box;
                Util.CreateBox3DFromNode(node, out box);

                // Check inside bounding box
                if (box.IntersectsWithLimitedLine(line))
                {
                    Vector3D intersection = new Vector3D();
                    Triangle3D triangle = new Triangle3D();

                    bool hit = Render.Scene.CollisionManager.GetCollisionPoint(
                                line,
                                node.TriangleSelector,
                                out intersection,
                                out triangle
                                );

                    if (!hit)
                        continue;

                    ObjectData data;
                    if (!Ox.DataStore.World.SimCollection.TryGetObject(key, out data))
                        continue;

                    if (!(data is PrimData))
                        continue;

                    PrimData primData = data as PrimData;
                    float[] point = Util.ToPositionArrayFromIrrlicht(ref intersection);
                    float length = (float)intersection.DistanceFrom(line.Start);
                    PointData.HitData hitData = new PointData.HitData(point[0], point[1], point[2], PointData.ObjectType.Prim, primData.ClickActionType, length, key);
                    Ox.DataStore.World.Point.Add(ref hitData);
                }
            }
        }

        void ox_OnEvent(string message)
        {
            JsonMessage parse_msg;
            JsonType type = JsonUtil.DeserializeMessage(message, out parse_msg);
            if (type != JsonType.ObjectUpdated)
                return;

            JsonObjectUpdated j = (JsonObjectUpdated)JsonUtil.Deserialize<JsonObjectUpdated>(parse_msg.value);
            if (j.prim == (int)JsonObjectUpdated.PrimType.Avatar) // If j.prim is avatar. do return
                return;

            lock (pipline)
                pipline.Enqueue(j);
        }

        private bool CreatePrim(ref JsonObjectUpdated update)
        {
            SimData data;
            if (!Ox.DataStore.World.SimCollection.TryGet(update.simID, out data)) return false;

            ObjectData objectData;
            if (!data.PrimCollection.TryGet(update.id, out objectData)) return false;

            PrimData primData;
            if (!(objectData is PrimData))
                return false;

            primData = objectData as PrimData;

            if (!primData.Loaded)
                return false;

            SceneNode node = null;
            if (list.ContainsKey(update.id))
            {
                node = list[update.id];
                list.Remove(update.id);
            }

            if (string.IsNullOrEmpty(primData.SceneName))
            {
                if (primData.Meshes == null)
                    return false;

                // If node isn't null, Update mesh.
                if (node == null)
                {
                    Mesh mesh = CreateMesh(primData.Meshes);
                    if (mesh == null)
                        return false;

                    node = Render.Scene.AddMeshSceneNode(mesh, Root, -1);
                    node.TriangleSelector = Render.Scene.CreateTriangleSelector(mesh, node);
                    //node.DebugDataVisible = DebugSceneType.BoundingBox;
                }
                else
                {
                    if (node is MeshSceneNode)
                    {
                        MeshSceneNode msn = node as MeshSceneNode;
                        Mesh mesh = msn.GetMesh();
                        msn.SetMesh(UpdateMesh(primData.Meshes, mesh));
                    }
                }

                if (node == null)
                    return false;
            }
            else
            {
                node = Render.Scene.AddEmptySceneNode(Root, -1);
                if (node == null)
                    return false;

                string path = Path.Combine(Ox.Paths.Cache, primData.SceneName);
                if (File.Exists(path))
                {
                    string tmp = Directory.GetCurrentDirectory();
                    Directory.SetCurrentDirectory(Ox.Paths.Cache);
                    Render.Scene.LoadScene(primData.SceneName, node);
                    Directory.SetCurrentDirectory(tmp);
                }
            }

            list.Add(update.id, node);

            return true;
        }

        private void UpdatePrim(ref JsonObjectUpdated update)
        {
            if (!list.ContainsKey(update.id))
                if (!CreatePrim(ref update))
                    return;

            SceneNode node = list[update.id];

            SimData data;
            if (!Ox.DataStore.World.SimCollection.TryGet(update.simID, out data))
                return;

            ObjectData objectData;
            if (!data.PrimCollection.TryGet(update.id, out objectData))
                return;

            if (!string.IsNullOrEmpty(objectData.ParentID))
            {
                if (!data.Contaion(objectData.ParentID))
                {
                    node.Visible = false;
                    return;
                }
            }

            PrimData primData = null;
            if (!(objectData is PrimData))
                return;

            primData = objectData as PrimData;
            if (string.IsNullOrEmpty(primData.SceneName))
            {
                node.Rotation = Util.ToRotationRH(objectData.Rotation);

                if (node is MeshSceneNode)
                    UpdateBoundingBox((node as MeshSceneNode));

                node.Scale = Util.ToVector3D(objectData.Scale);
            }
            else
            {
                node.Rotation = Util.ToRotationRH(new float[]{
                    objectData.Rotation[0] + Util.ROTATION_OFFSET.X,
                    objectData.Rotation[1] + Util.ROTATION_OFFSET.Y,
                    objectData.Rotation[2] + Util.ROTATION_OFFSET.Z});

                node.Scale = Util.ToScale(objectData.Scale);
            }
            node.Position = Util.ToPositionRH(objectData.Position);
            node.Visible = true;
        }

        private void DeletePrim(string key)
        {
            if (!list.ContainsKey(key)) return;

            SceneNode node = list[key];
            if (node == null) return;

            node.Remove();
            list.Remove(key);
        }

        private Mesh CreateMesh(MeshData[] meshes)
        {
            Mesh mesh = new Mesh();

            foreach (MeshData data in meshes)
            {
                if (data == null)
                    continue;

                bool use_alpha = (data.Color[0] < 0.9999f);

                MeshBuffer mb = new MeshBuffer(VertexType.Standard);
                for (uint i = 0; i < data.Indices.Length; i++)
                {
                    VertexData v = data.Vertices[data.Indices[i]];
                    mb.SetVertex(i, new Vertex3D(
                        new Vector3D(v.Position[0], v.Position[1], v.Position[2]),
                        new Vector3D(v.Normal[0], v.Normal[1], v.Normal[2]),
                        Color.White,
                        new Vector2D(v.UV[0], v.UV[1])
                        ));

                    mb.SetIndex(i, (ushort)data.Indices[i]);
                }
                mb.SetColor(Util.ToColor(data.Color[0], data.Color[1], data.Color[2], data.Color[3]));
                mb.Material.AmbientColor = Util.ToColor(1, data.Color[1], data.Color[2], data.Color[3]);
                TextureInfo info = null;
                if (data.Texture1DownLoaded)
                    info = Render.Texture.GetTexture(Path.Combine(Ox.Paths.Cache, data.Texture1), true, true);
                if (info != null)
                {
                    mb.Material.Texture1 = (info.Texture == null) ? Render.RenderData.BlankTexture : info.Texture;
                    use_alpha |= info.UseAlpha;
                }
                mb.Material.MaterialType = use_alpha ? MaterialType.TransparentAlphaChannel : MaterialType.Solid;

                mesh.AddMeshBuffer(mb);
            }

            return mesh;
        }

        private Mesh UpdateMesh(MeshData[] meshes, Mesh mesh)
        {
            if (mesh != null)
                mesh.Drop();

            return CreateMesh(meshes);
        }

        private void UpdateBoundingBox(MeshSceneNode node)
        {
            if (node == null)
                return;

            Mesh mesh = node.GetMesh();
            if (mesh == null)
                return;

            Box3D box = new Box3D(0, 0, 0, 0, 0, 0);
            for (int i = 0; i < mesh.MeshBufferCount; i++)
            {
                mesh.GetMeshBuffer(i).RecalculateBoundingBox();
                box.AddInternalBox(mesh.GetMeshBuffer(i).BoundingBox);
            }
            mesh.BoundingBox = box;
        }
    }
}
