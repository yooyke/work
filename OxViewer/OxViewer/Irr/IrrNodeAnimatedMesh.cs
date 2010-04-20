using System.Collections.Generic;

namespace OxViewer.Irr
{
    public class IrrNodeAnimatedMesh : IrrNode
    {
        public CullingType AutomaticCulling;
        public int DebugDataVisible;
        public bool IsDebugObject;
        public string Mesh;
        public bool Looping;
        public bool ReadOnlyMaterials;
        public float FramesPerSecond;

        public override string[] GetAssets()
        {
            List<string> list = new List<string>();
            string[] a = base.GetAssets();
            if (a != null && a.Length != 0)
                list.AddRange(a);

            if (!string.IsNullOrEmpty(Mesh))
                list.Add(Mesh);

            if (list.Count == 0)
                return null;

            return list.ToArray();
        }


        public static new IrrNodeAnimatedMesh Get(IrrData data)
        {
            IrrNodeAnimatedMesh node = new IrrNodeAnimatedMesh();
            Copy(data, node);

            return node;
        }

        public static void Copy(IrrData data, IrrNodeAnimatedMesh node)
        {
            foreach (IrrData child in data.Children)
            {
                if (string.IsNullOrEmpty(child.Attr.Name))
                    continue;

                switch (child.Attr.Name.ToLower())
                {
                    case "automaticculling":
                        node.AutomaticCulling = CullingType.Box;
                        break;
                    case "debugdatavisible":
                        node.DebugDataVisible = (int)child.Attr.Value;
                        break;
                    case "isdebugobject":
                        node.IsDebugObject = (bool)child.Attr.Value;
                        break;
                    case "mesh":
                        node.Mesh = child.Attr.Value as string;
                        break;
                    case "looping":
                        node.Looping = (bool)child.Attr.Value;
                        break;
                    case "readonlymaterials":
                        node.ReadOnlyMaterials = (bool)child.Attr.Value;
                        break;
                    case "framespersecond":
                        node.FramesPerSecond = (float)child.Attr.Value;
                        break;
                }
            }

            IrrNode.Copy(data, node);
        }
    }
}
