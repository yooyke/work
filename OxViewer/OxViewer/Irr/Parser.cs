using System;
using System.IO;
using System.Xml;
using System.Drawing;
using OxUtil;

namespace OxViewer.Irr
{
    public class Parser
    {
        private IrrScene scene;

        public IrrScene Scene { get { return scene; } }

        public Parser()
        {
        }

        public bool Run(string path)
        {
            if (!File.Exists(path))
                return false;

            bool succeed = false;
            using (StreamReader sr = new StreamReader(path))
            {
                succeed = Run(sr.BaseStream);
                sr.Close();
            }

            return succeed;
        }

        public bool Run(Stream stream)
        {
            if (stream == null)
                return false;

            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            IrrData data = new IrrData();
            data.Name = "root";
            foreach (XmlNode node in doc.ChildNodes)
                data.Children.Add(RecursiveNode(doc, node));

            IrrData sceneData = FindFromName(data, "irr_scene");
            if (sceneData == null)
                return false;

            scene = CreateSceneFromData(sceneData);

            return true;
        }

        private IrrScene CreateSceneFromData(IrrData data)
        {
            IrrScene scene = new IrrScene();
            IrrData attributeData = Parser.FindFromName(data, "attributes");
            IrrScene.Copy(attributeData, scene);

            foreach (IrrData child in data.Children)
            {
                if (child.Name == "node")
                    scene.AddNode(GetNode(child));
            }

            return scene;
        }

        private IrrNode GetNode(IrrData data)
        {
            if (data.Attr.Type == null)
                return null;

            IrrNode node = null;
            IrrData attributeData = Parser.FindFromName(data, "attributes");
            switch (data.Attr.Type)
            {
                case "animatedMesh":
                    node = IrrNodeAnimatedMesh.Get(attributeData);
                    break;
            }

            if (node == null)
                return null;

            foreach (IrrData child in data.Children)
            {
                if (string.IsNullOrEmpty(child.Name))
                    continue;

                if (child.Name == "materials")
                {
                    foreach (IrrData material in child.Children)
                    {
                        if (string.IsNullOrEmpty(material.Name))
                            continue;

                        if (material.Name == "attributes")
                            node.materials.Add(IrrMaterial.Get(material));
                    }
                }
            }

            return node;
        }

        private IrrData RecursiveNode(XmlNode parent, XmlNode node)
        {
            IrrData data = new IrrData();

            if (!string.IsNullOrEmpty(node.Name))
            {
                data.Name = node.Name.ToLower();
                data.Attr.Name = XmlHelper.GetAttribute(node, "name");
                string value = XmlHelper.GetAttribute(node, "value");
                string type = XmlHelper.GetAttribute(node, "type");
                switch (node.Name.ToLower())
                {
                    case "xml":
                        break;
                    case "irr_scene":
                        break;
                    case "node":
                        data.Attr.Type = type;
                        break;
                    case "materials":
                        break;
                    case "attributes":
                        break;
                    case "bool":
                        data.Attr.Value = XmlHelper.GetBool(value);
                        break;
                    case "int":
                        data.Attr.Value = XmlHelper.GetInt(value);
                        break;
                    case "float":
                        data.Attr.Value = XmlHelper.GetFloat(value);
                        break;
                    case "string":
                        data.Attr.Value = value;
                        break;
                    case "color":
                        data.Attr.Value = XmlHelper.GetColor(value);
                        break;
                    case "colorf":
                        data.Attr.Value = XmlHelper.GetColorf(value);
                        break;
                    case "vector3d":
                        data.Attr.Value = XmlHelper.GetVector3D(value);
                        break;
                    case "texture":
                        data.Attr.Value = value;
                        break;
                    case "enum":
                        data.Attr.Value = value;
                        break;
                }
            }

            foreach (XmlNode child in node.ChildNodes)
                data.Children.Add(RecursiveNode(node, child));

            return data;
        }


        public static IrrData FindFromName(IrrData data, string name)
        {
            if (data.Name == name)
                return data;

            foreach (IrrData child in data.Children)
            {
                IrrData d = FindFromName(child, name);
                if (d == null)
                    continue;

                if (d.Name == name)
                    return d;
            }

            return null;
        }
    }
}
