using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using System.Threading;

namespace OxCore
{
    public class Config
    {
        private const int SAVE_TIMER_INVOKE_SECOND = 3;

        private Dictionary<string, Dictionary<string, object>> dic = new Dictionary<string, Dictionary<string, object>>();
        private string path;
        private Timer save_timer;

        public Config(string file_path)
        {
            this.path = file_path;
        }

        public void Load()
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return;

            XmlDocument xml = new XmlDocument();
            try
            {
                xml.Load(path);
            }
            catch
            {
                // Maybe, someone is opening the document.
                return;
            }

            if (xml.ChildNodes.Count > 1)
            {
                foreach (XmlNode child in xml.ChildNodes)
                {
                    if (string.IsNullOrEmpty(child.Name) || (child.Name.ToLower().CompareTo("xml") == 0))
                        continue;

                    if (child.Name.ToLower().CompareTo("root") == 0)
                    {
                        foreach (XmlNode node in child.ChildNodes)
                        {
                            Dictionary<string, object> data = LoadData(node);
                            if (data == null || data.Count == 0)
                                continue;
                            dic.Add(node.Name, data);
                        }
                    }
                }
            }
        }

        private Dictionary<string, object> LoadData(XmlNode node)
        {
            if (node == null || node.ChildNodes.Count == 0)
                return null;

            Dictionary<string, object> data = new Dictionary<string, object>();
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child == null || string.IsNullOrEmpty(child.Name))
                    continue;

                data.Add(child.Name, child.InnerText);
            }
            return data;
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(path))
                return;

            try
            {
                using (XmlTextWriter w = new XmlTextWriter(path, Encoding.UTF8))
                {
                    w.Formatting = Formatting.Indented;
                    w.WriteStartDocument();
                    w.WriteStartElement("root");
                    foreach (string key in dic.Keys)
                    {
                        w.WriteStartElement(key);
                        foreach (string data_key in dic[key].Keys)
                        {
                            object o = dic[key][data_key];
                            if (o == null)
                                continue;

                            w.WriteStartElement(data_key);
                            w.WriteValue(o);
                            w.WriteEndElement();
                        }
                        w.WriteEndElement();
                    }

                    w.WriteEndElement();
                    w.Close();
                }
            }
            catch
            {
                // Maybe, someone is opening the document. 
            }
        }

        private void SaveTimer(object state)
        {
            Save();

            lock (save_timer)
            {
                save_timer.Dispose();
                save_timer = null;
            }
        }

        public void Add(Type type, string key, object value)
        {
            if (string.IsNullOrEmpty(key) || value == null)
                return;

            string category = type.FullName;
            if (!dic.ContainsKey(category))
                dic.Add(category, new Dictionary<string, object>());

            Dictionary<string, object> data = dic[category];
            if (data.ContainsKey(key))
            {
                if (value is bool)
                {
                    if ((data[key] as string) == value.ToString().ToLower())
                        return;
                }

                if (data[key] == value)
                    return;

                data[key] = value;
            }
            else
            {
                data.Add(key, value);
            }

            if (save_timer == null)
                save_timer = new Timer(SaveTimer, null, 1000 * SAVE_TIMER_INVOKE_SECOND, 0);
        }

        public object Get(Type type, string key, object default_value)
        {
            string category = type.FullName;
            if (!dic.ContainsKey(category))
                Add(type, key, default_value);

            Dictionary<string, object> data = dic[category];
            if (data.ContainsKey(key))
                return data[key];
            else
                Add(type, key, default_value);

            return default_value;
        }

        #region Get functions
        public bool Get(Type type, string key, bool default_value)
        {
            object o = Get(type, key, (object)default_value);
            return ((o is string) ? bool.Parse((o as string)) : (bool)o);
        }

        public int Get(Type type, string key, int default_value)
        {
            object o = Get(type, key, (object)default_value);
            return ((o is string) ? int.Parse((o as string)) : (int)o);
        }

        public long Get(Type type, string key, long default_value)
        {
            object o = Get(type, key, (object)default_value);
            return ((o is string) ? long.Parse((o as string)) : (long)o);
        }

        public float Get(Type type, string key, float default_value)
        {
            object o = Get(type, key, (object)default_value);
            return ((o is string) ? float.Parse((o as string)) : (float)o);
        }

        public double Get(Type type, string key, double default_value)
        {
            object o = Get(type, key, (object)default_value);
            return ((o is string) ? double.Parse((o as string)) : (double)o);
        }

        public string Get(Type type, string key, string default_value)
        {
            object o = Get(type, key, (object)default_value);
            return (o as string);
        }
        #endregion
    }
}
