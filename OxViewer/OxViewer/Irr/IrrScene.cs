using System;
using System.Collections.Generic;

namespace OxViewer.Irr
{
    public class IrrScene
    {
        private List<IrrNode> nodes = new List<IrrNode>();

        public string Name;
        public int ID;
        public float[] AmbientLight;

        public void AddNode(IrrNode node)
        {
            nodes.Add(node);
        }

        public IrrNode[] GetNodes()
        {
            if (nodes.Count == 0)
                return null;

            return nodes.ToArray();
        }


        public static IrrScene Get(IrrData data)
        {
            IrrScene scene = new IrrScene();
            Copy(data, scene);

            return scene;
        }

        public static void Copy(IrrData data, IrrScene scene)
        {
            foreach (IrrData child in data.Children)
            {
                if (string.IsNullOrEmpty(child.Attr.Name))
                    continue;

                switch (child.Attr.Name.ToLower())
                {
                    case "name":
                        scene.Name = child.Attr.Value as string;
                        break;
                    case "id":
                        scene.ID = (int)child.Attr.Value;
                        break;
                    case "ambientlight":
                        scene.AmbientLight = child.Attr.Value as float[];
                        break;
                }
            }
        }
    }
}
