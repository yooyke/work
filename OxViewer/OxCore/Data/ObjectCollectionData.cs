using System.Collections.Generic;

namespace OxCore.Data
{
    public class ObjectCollectionData
    {
        protected Dictionary<string, ObjectData> list = new Dictionary<string, ObjectData>();

        public int Count { get { return list.Count; } }

        public bool Update(string id)
        {
            if (!Contains(id))
                return false;

            ObjectData d = list[id];
            return true;
        }

        public bool Delete(string id)
        {
            if (!Contains(id))
                return false;

            list.Remove(id);
            return true;
        }

        public ObjectData[] GetAll()
        {
            if (list.Count == 0)
                return null;

            List<ObjectData> tmp = new List<ObjectData>(list.Count);
            foreach (string key in list.Keys)
                tmp.Add(list[key]);

            return tmp.ToArray();
        }

        public bool TryGet(string id, out ObjectData data)
        {
            data = null;
            if (!Contains(id))
                return false;

            data = list[id];
            return true;
        }

        public bool Contains(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            return list.ContainsKey(id);
        }

        internal void Reset()
        {
            list.Clear();
        }
    }
}
