using System;
using System.Xml;
using System.Drawing;

namespace OxUtil
{
    public static class XmlHelper
    {
        public static string GetAttribute(XmlNode node, string name)
        {
            if (node.Attributes == null || node.Attributes[name] == null)
                return string.Empty;

            return node.Attributes[name].Value;
        }

        public static bool GetBool(string value)
        {
            bool val;
            bool.TryParse(value, out val);
            return val;
        }

        public static int GetInt(string value)
        {
            int val;
            int.TryParse(value, out val);
            return val;
        }

        public static float GetFloat(string value)
        {
            float val;
            float.TryParse(value, out val);
            return val;
        }

        public static byte[] GetColor(string value)
        {
            Color c = ColorTranslator.FromHtml("0x" + value);
            return new byte[] { c.A, c.R, c.G, c.B };
        }

        public static float[] GetColorf(string value)
        {
            float[] f = GetFloatArrayFromString(value);
            if (f == null || f.Length != 4)
                return null;

            return f;
        }

        public static float[] GetVector3D(string value)
        {
            float[] f = GetFloatArrayFromString(value);
            if (f == null || f.Length != 3)
                return null;

            return f;
        }

        public static float[] GetFloatArrayFromString(string value)
        {
            value = value.Replace(" ", "");
            string[] works = value.Split(new char[] { ',' });

            if (works.Length == 0)
                return null;

            float[] values = new float[works.Length];
            for (int i = 0; i < works.Length; i++)
                float.TryParse(works[i], out values[i]);

            return values;
        }
    }
}
