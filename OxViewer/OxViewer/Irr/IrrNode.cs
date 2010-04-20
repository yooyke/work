using System.Collections.Generic;

namespace OxViewer.Irr
{
    public class IrrNode
    {
        public string Name;
        public int ID;
        public float[] Position;
        public float[] Rotation;
        public float[] Scale;
        public bool Visible;
        public List<IrrMaterial> materials = new List<IrrMaterial>();

        public virtual string[] GetAssets()
        {
            List<string> list = new List<string>();
            foreach (IrrMaterial material in materials)
	        {
                string[] a = material.GetAssets();
                if (a == null || a.Length == 0)
                    continue;

                foreach (string m in a)
                {
                    if (list.Contains(m))
                        continue;

                    list.Add(m);
                }
	        }

            if (list.Count == 0)
                return null;

            return list.ToArray();
        }

        public static IrrNode Get(IrrData data)
        {
            IrrNode node = new IrrNode();
            Copy(data, node);

            return node;
        }

        public static void Copy(IrrData data, IrrNode node)
        {
            foreach (IrrData child in data.Children)
            {
                if (string.IsNullOrEmpty(child.Attr.Name))
                    continue;

                switch (child.Attr.Name.ToLower())
                {
                    case "name":
                        node.Name = child.Attr.Value as string;
                        break;
                    case "id":
                        node.ID = (int)child.Attr.Value;
                        break;
                    case "position":
                        node.Position = child.Attr.Value as float[];
                        break;
                    case "rotation":
                        node.Rotation = child.Attr.Value as float[];
                        break;
                    case "scale":
                        node.Scale = child.Attr.Value as float[];
                        break;
                    case "visible":
                        node.Visible = (bool)child.Attr.Value;
                        break;
                }
            }
        }
    }
}
