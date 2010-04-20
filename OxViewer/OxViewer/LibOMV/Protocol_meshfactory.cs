using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using OpenMetaverse;
using PrimMesher;
using OxCore;
using OxJson;
using OxCore.Data;

namespace OxViewer.LibOMV
{
    public partial class Protocol
    {
        public enum DetailLevel
        {
            Low = 0,
            Medium = 1,
            High = 2,
            Highest = 3
        }

        private void MeshFactory()
        {
            while (true)
            {
                JsonObjectUpdated update = MeshFactoryDequeue();

                Primitive prim = GetObjectFromID(update.id);
                if (prim == null)
                    continue;

                ObjectData objectData;
                if (!Ox.DataStore.World.SimCollection.TryGetObject(update.simID, update.id, out objectData))
                    continue;

                if (!(objectData is PrimData))
                    continue;

                PrimData primData = objectData as PrimData;

                primData.Loaded = false;
                if (string.IsNullOrEmpty(primData.SceneName))
                {
                    if (!GetPrimMeshData(primData, prim, DetailLevel.Highest))
                        continue;
                }
                else
                {
                    if (!GetIrrScene(primData.SceneName))
                        continue;
                }
                primData.Loaded = true;

                string msg = JsonUtil.SerializeMessage(JsonType.ObjectUpdated, new JsonObjectUpdated(
                    update.simID,
                    update.id,
                    (int)JsonObjectUpdated.PrimType.Prim,
                    (int)JsonObjectUpdated.Type.UpdateFull
                    ));
                Ox.EventFire(msg, false);
            }
        }

        private void MeshFactoryEnqueue(JsonObjectUpdated prim)
        {
            Monitor.Enter(queueMeshFactory);
            try
            {
                queueMeshFactory.Enqueue(prim);
                Monitor.Pulse(queueMeshFactory);
            }
            finally
            {
                Monitor.Exit(queueMeshFactory);
            }
        }

        private JsonObjectUpdated MeshFactoryDequeue()
        {
            Monitor.Enter(queueMeshFactory);
            try
            {
                while (queueMeshFactory.Count == 0)
                {
                    Monitor.Wait(queueMeshFactory);
                }
                return queueMeshFactory.Dequeue();
            }
            finally
            {
                Monitor.Exit(queueMeshFactory);
            }
        }

        private bool GetTextureFromFilename(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return false;

            if (!string.IsNullOrEmpty(Ox.IO.Load(filename)))
                return false;

            if (string.IsNullOrEmpty(Ox.DataStore.World.Agent.AssetServerUri))
            {
                string name = System.IO.Path.GetFileNameWithoutExtension(filename);
                Ox.Function(JsonUtil.SerializeMessage(JsonType.RequestImage, new JsonRequestImage(name)));
                return true;
            }
            else
            {
                LibOMV.AssetBase ab = Network.Asset.Get(Ox.DataStore.World.Agent.AssetServerUri, Ox.DataStore.World.Agent.AuthToken, filename);
                return SaveTexutreFromAssetBase(ab);
            }
        }

        private bool SaveTexutreFromAssetBase(LibOMV.AssetBase ab)
        {
            LibOMV.Asset.ImageDecompress(ab);

            if (ab == null)
                return false;

            Ox.IO.Save(ab.Name, ab.Data);

            return true;
        }

        private bool GetIrrScene(string sceneName)
        {
            if (Ox.IO.Contains(sceneName))
            {
                LibOMV.AssetIrrFile irr = LibOMV.Asset.LoadIrrScene(sceneName);
                Ox.IO.Load(irr.Filename);
                string[] materials = irr.GetMaterials();
                foreach (string material in materials)
                    GetTextureFromFilename(material);
            }
            else
            {
                LibOMV.AssetIrrMemory irr = Network.Asset.GetIrrScene(Ox.DataStore.World.Agent.AssetServerUri, Ox.DataStore.World.Agent.AuthToken, sceneName);
                Ox.IO.Save(irr.IrrFile.Name, irr.IrrFile.Data);
                LibOMV.AssetBase[] abs = irr.GetMaterials();
                foreach (LibOMV.AssetBase ab in abs)
                    SaveTexutreFromAssetBase(ab);
            }

            return true;
        }

        private bool GetPrimMeshData(PrimData primData, Primitive prim, DetailLevel lod)
        {
            MeshData[] meshes = GetMeshsDataFromPrim(prim, DetailLevel.Highest);
            if (meshes == null)
                return false;

            foreach (MeshData data in meshes)
            {
                GetTextureFromFilename(data.Texture1);
                data.Texture1DownLoaded = true;
            }

            primData.MeshUpdated(meshes);

            return true;
        }

        private MeshData[] GetMeshsDataFromPrim(Primitive prim, DetailLevel lod)
        {
            PrimitiveBaseShape primShape = new PrimitiveBaseShape(prim);

            float profileBegin = (float)primShape.ProfileBegin * 2.0e-5f;
            float profileEnd = 1.0f - (float)primShape.ProfileEnd * 2.0e-5f;
            float profileHollow = (float)primShape.ProfileHollow * 2.0e-5f;

            int sides = 4;
            if ((prim.PrimData.profileCurve & 0x07) == (byte)ProfileCurve.EqualTriangle)
                sides = 3;
            else if ((prim.PrimData.profileCurve & 0x07) == (byte)ProfileCurve.Circle)
                sides = 24;
            else if ((prim.PrimData.profileCurve & 0x07) == (byte)ProfileCurve.HalfCircle)
            {
                // half circle, prim is a sphere
                sides = 24;

                profileBegin = 0.5f * profileBegin + 0.5f;
                profileEnd = 0.5f * profileEnd + 0.5f;
            }

            int hollowSides = sides;
            if (prim.PrimData.ProfileHole == HoleType.Circle)
                hollowSides = 24;
            else if (prim.PrimData.ProfileHole == HoleType.Square)
                hollowSides = 4;
            else if (prim.PrimData.ProfileHole == HoleType.Triangle)
                hollowSides = 3;

            PrimMesh primMesh = new PrimMesh(sides, profileBegin, profileEnd, profileHollow, hollowSides);
            primMesh.viewerMode = true;
            primMesh.calcVertexNormals = true;
            primMesh.topShearX = primShape.PathShearX < 128 ? (float)primShape.PathShearX * 0.01f : (float)(primShape.PathShearX - 256) * 0.01f;
            primMesh.topShearY = primShape.PathShearY < 128 ? (float)primShape.PathShearY * 0.01f : (float)(primShape.PathShearY - 256) * 0.01f;
            primMesh.pathCutBegin = (float)primShape.PathBegin * 2.0e-5f;
            primMesh.pathCutEnd = 1.0f - (float)primShape.PathEnd * 2.0e-5f;
            switch (lod)
            {
                case DetailLevel.Highest:
                case DetailLevel.High:
                    primMesh.stepsPerRevolution = 24;
                    break;

                case DetailLevel.Medium:
                    primMesh.stepsPerRevolution = 12;
                    break;

                case DetailLevel.Low:
                    primMesh.stepsPerRevolution = 6;
                    break;
            }

            float pathScaleX = (float)(primShape.PathScaleX - 100) * 0.01f;
            float pathScaleY = (float)(primShape.PathScaleY - 100) * 0.01f;
            if (prim.PrimData.PathCurve == OpenMetaverse.PathCurve.Line)
            {
                primMesh.twistBegin = primShape.PathTwistBegin * 18 / 10;
                primMesh.twistEnd = primShape.PathTwist * 18 / 10;
                primMesh.taperX = pathScaleX;
                primMesh.taperY = pathScaleY;

                try
                {
                    primMesh.ExtrudeLinear();
                }
                catch { return null; }
            }
            else
            {
                primMesh.holeSizeX = (200 - primShape.PathScaleX) * 0.01f;
                primMesh.holeSizeY = (200 - primShape.PathScaleY) * 0.01f;
                primMesh.radius = 0.01f * primShape.PathRadiusOffset;
                primMesh.revolutions = 1.0f + 0.015f * primShape.PathRevolutions;
                primMesh.skew = 0.01f * primShape.PathSkew;
                primMesh.twistBegin = primShape.PathTwistBegin * 36 / 10;
                primMesh.twistEnd = primShape.PathTwist * 36 / 10;
                primMesh.taperX = primShape.PathTaperX * 0.01f;
                primMesh.taperY = primShape.PathTaperY * 0.01f;

                try
                {
                    primMesh.ExtrudeCircular();
                }
                catch { return null; }
            }

            return GetMeshsDataFromPrimFaces(prim, primMesh.viewerFaces.ToArray());
        }

        private MeshData[] GetMeshsDataFromPrimFaces(Primitive prim, ViewerFace[] faces)
        {
            int dt_index = Primitive.TextureEntry.MAX_FACES;
            Primitive.TextureEntryFace[] entries = new Primitive.TextureEntryFace[Primitive.TextureEntry.MAX_FACES + 1];

            for (int i = 0; i < Primitive.TextureEntry.MAX_FACES; i++)
                entries[i] = prim.Textures.FaceTextures[i];
            entries[dt_index] = prim.Textures.DefaultTexture;

            int faceCount = 0;
            Dictionary<int, int> faceDic = new Dictionary<int, int>();
            List<int> faceList = new List<int>();
            List<VertexData>[] vertexList = new List<VertexData>[entries.Length];
            List<uint>[] indexList = new List<uint>[entries.Length];
            foreach (ViewerFace face in faces)
            {
                int face_number = (entries[face.primFaceNumber] == null) ? dt_index : face.primFaceNumber;
                if (!faceDic.ContainsKey(face_number))
                {
                    vertexList[faceCount] = new List<VertexData>();
                    indexList[faceCount] = new List<uint>();
                    faceList.Add(face_number);
                    faceDic.Add(face_number, faceCount++);
                }
                int index = faceDic[face_number];
                uint i_count = (uint)indexList[index].Count;

                // Vertex 1
                vertexList[index].Add(new VertexData(
                    new float[] { face.v1.X, face.v1.Y, face.v1.Z },
                    new float[] { face.n1.X, face.n1.Y, face.n1.Z },
                    new float[] { face.uv1.U, face.uv1.V },
                    null
                    ));
                indexList[index].Add(i_count++);

                // Vertex 2
                vertexList[index].Add(new VertexData(
                    new float[] { face.v2.X, face.v2.Y, face.v2.Z },
                    new float[] { face.n2.X, face.n2.Y, face.n2.Z },
                    new float[] { face.uv2.U, face.uv2.V },
                    null
                    ));
                indexList[index].Add(i_count++);

                // Vertex 3
                vertexList[index].Add(new VertexData(
                    new float[] { face.v3.X, face.v3.Y, face.v3.Z },
                    new float[] { face.n3.X, face.n3.Y, face.n3.Z },
                    new float[] { face.uv3.U, face.uv3.V },
                    null
                    ));
                indexList[index].Add(i_count++);
            }

            MeshData[] meshes = new MeshData[faceCount];
            for (int i = 0; i < meshes.Length; i++)
            {
                Primitive.TextureEntryFace entry = entries[faceList[i]];

                meshes[i] = new MeshData();
                meshes[i].Vertices = new VertexData[vertexList[i].Count];
                meshes[i].Indices = new uint[indexList[i].Count];
                meshes[i].Texture1 = entry.TextureID.ToString() + ".tga";
                meshes[i].Color = new float[] { entry.RGBA.A, entry.RGBA.R, entry.RGBA.G, entry.RGBA.B };
                vertexList[i].CopyTo(meshes[i].Vertices);
                indexList[i].CopyTo(meshes[i].Indices);
            }

            return meshes;
        }
    }
}
