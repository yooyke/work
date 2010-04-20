using System.Collections.Generic;

namespace OxViewer.Irr
{
    public class IrrData
    {
        public struct Attribute
        {
            public string Name;
            public object Value;
            public string Type;
        }

        public string Name;
        public Attribute Attr;
        public List<IrrData> Children = new List<IrrData>();
    }
}
