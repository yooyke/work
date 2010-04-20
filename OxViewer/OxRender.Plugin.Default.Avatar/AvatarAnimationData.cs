using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using OxUtil;

namespace OxRender.Plugin.Default.Avatar
{
    enum AnimationType
    {
        None,
        Standing,
        Walking,
        Running,
        SitStart,
    }

    class AnimationData
    {
        public string Name;
        public int Start;
        public int End;
        public int Speed;

        public AnimationData()
            : this(string.Empty, 0, 0, 0) { }

        public AnimationData(string name, int start, int end, int speed)
        {
            Name = name;
            Start = start;
            End = end;
            Speed = speed;
        }
    }

    class AvatarAnimationData
    {
        private string current = string.Empty;
        private bool parsed = false;
        private Dictionary<string, AnimationData> list = new Dictionary<string, AnimationData>();
        private Queue<string> request = new Queue<string>();

        public string Current { get { return current; } }

        public bool Contaion(string animationName)
        {
            if (string.IsNullOrEmpty(animationName))
                return false;

            if (list == null)
                return false;

            return list.ContainsKey(animationName);
        }

        public void Request(AnimationType animationType)
        {
            Request(animationType.ToString().ToLower());
        }

        public void Request(string animationName)
        {
            if (!Contaion(animationName))
                return;

            if (!request.Contains(animationName))
            {
                lock(request)
                    request.Enqueue(animationName);
            }
        }

        public AnimationData Play()
        {
            return Play(null);
        }

        public AnimationData Play(string animationName)
        {
            string key = AnimationType.Standing.ToString();

            while (request.Count > 0)
            {
                string req;
                lock (request)
                    req = request.Dequeue();

                //---------------
                // priority check
                //---------------

                key = req;
            }

            if (animationName != null && Contaion(animationName))
                key = animationName;

            AnimationData data;
            if ((current != key) && TryGet(key, out data))
            {
                current = key;
                return data;
            }
            else
            {
                return null;
            }
        }

        public bool IsCurrent(string key)
        {
            if (string.IsNullOrEmpty(key))
                return false;

            return (current == key.ToLower());
        }

        public bool TryGet(AnimationType animationType, out AnimationData animationData)
        {
            return TryGet(animationType.ToString().ToLower(), out animationData);
        }

        public bool TryGet(string animationName, out AnimationData animationData)
        {
            animationData = null;

            if (!Contaion(animationName))
                return false;

            animationData = list[animationName];
            return true;
        }

        public string[] GetAnimationNames()
        {
            if (list.Count == 0)
                return null;

            List<string> names = new List<string>();
            foreach (string key in list.Keys)
                names.Add(key);

            return names.ToArray();
        }

        #region Parse
        public void Parse(string path)
        {
            if (!File.Exists(path))
                return;

            using (StreamReader sr = new StreamReader(path))
            {
                parsed = Run(sr.BaseStream);
                sr.Close();
            }
        }

        private bool Run(Stream stream)
        {
            if (stream == null)
                return false;

            XmlDocument doc = new XmlDocument();
            doc.Load(stream);

            foreach (XmlNode node in doc.ChildNodes)
                RecursiveNode(doc, node);

            return true;
        }

        private void RecursiveNode(XmlNode parent, XmlNode node)
        {
            if (!string.IsNullOrEmpty(node.Name))
            {
                switch (node.Name.ToLower())
                {
                    case "node":
                        break;
                    case "attributes":
                        break;
                    case "animation":
                        AnimationNode(node);
                        break;
                }
            }

            foreach (XmlNode child in node.ChildNodes)
                RecursiveNode(node, child);
        }

        private void AnimationNode(XmlNode node)
        {
            AnimationData ad = new AnimationData();
            foreach (XmlNode child in node.ChildNodes)
            {
                string name = XmlHelper.GetAttribute(child, "name");
                if (!string.IsNullOrEmpty(name))
                {
                    string value = XmlHelper.GetAttribute(child, "value");
                    switch (name.ToLower())
                    {
                        case "name":
                            ad.Name = value;
                            break;
                        case "start":
                            ad.Start = XmlHelper.GetInt(value);
                            break;
                        case "end":
                            ad.End = XmlHelper.GetInt(value);
                            break;
                        case "speed":
                            ad.Speed = XmlHelper.GetInt(value);
                            break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(ad.Name))
                list.Add(ad.Name, ad);
        }
        #endregion
    }
}
