using System.Collections.Generic;

namespace OxCore.Data
{
    public class SimCollectionData
    {
        private Dictionary<string, SimData> list = new Dictionary<string, SimData>();

        public int Count { get { return list.Count; } }

        public void Add(string id, ulong handle, uint gridX, uint gridY)
        {
            if (!Contains(id))
                list.Add(id, new SimData(id, handle, gridX, gridY));
        }

        public bool AddAvatar(string id, string avatarID, string first, string last, string parentID, float[] position, float[] rotation, float[] scale, float[] velocity, bool myself)
        {
            if (!Contains(id))
                return false;

            SimData d = list[id];
            d.AvatarCollection.Add(avatarID, first, last, parentID, position, rotation, scale, velocity, myself);
            return true;
        }

        public bool AddPrim(string id, string primID, string parentID, string sceneName, float[] position, float[] rotation, float[] scale, float[] velocity, bool myself, ClickActionType clickActionType)
        {
            if (!Contains(id))
                return false;

            SimData d = list[id];
            d.PrimCollection.Add(primID, parentID, sceneName, position, rotation, scale, velocity, myself, clickActionType);
            return true;
        }

        public bool Delete(string id)
        {
            if (!Contains(id))
                return false;

            list.Remove(id);
            return true;
        }

        public bool DeleteObject(string id, string objectID)
        {
            if (!Contains(id))
                return false;

            SimData d = list[id];
            if (d.AvatarCollection.Contains(objectID))
            {
                d.AvatarCollection.Delete(objectID);
                return true;
            }

            if (d.PrimCollection.Contains(objectID))
            {
                d.PrimCollection.Delete(objectID);
                return true;
            }

            return false;
        }

        public SimData[] GetAll()
        {
            if (list.Count == 0)
                return null;

            List<SimData> tmp = new List<SimData>(list.Count);
            foreach (string key in list.Keys)
                tmp.Add(list[key]);

            return tmp.ToArray();
        }

        public int GetAvatarCount()
        {
            return GetObjectCount(true, false);
        }

        public int GetPrimCount()
        {
            return GetObjectCount(false, true);
        }

        public int GetObjectCount()
        {
            return GetObjectCount(true, true);
        }

        private int GetObjectCount(bool avatar, bool prim)
        {
            SimData[] datas = GetAll();
            if (datas == null)
                return 0;

            int aCount = 0;
            int pCount = 0;
            foreach (SimData data in datas)
            {
                aCount += data.AvatarCollection.Count;
                pCount += data.PrimCollection.Count;
            }

            int count = 0;
            if (avatar)
                count += aCount;
            if (prim)
                count += pCount;

            return count;
        }

        public ObjectData[] GetAvatarAll()
        {
            return GetObjectAll(true, false);
        }

        public ObjectData[] GetPrimAll()
        {
            return GetObjectAll(false, true);
        }

        public ObjectData[] GetObjectAll()
        {
            return GetObjectAll(true, true);
        }

        private ObjectData[] GetObjectAll(bool avatar, bool prim)
        {
            SimData[] datas = GetAll();
            if (datas == null)
                return null;

            List<ObjectData> o = new List<ObjectData>();
            foreach (SimData data in datas)
            {
                ObjectData[] od;
                od = data.AvatarCollection.GetAll();
                if (avatar && od != null && od.Length != 0)
                    o.AddRange(od);

                od = data.PrimCollection.GetAll();
                if (prim && od != null && od.Length != 0)
                    o.AddRange(od);
            }

            return o.ToArray();
        }

        public bool TryGet(string id, out SimData data)
        {
            data = null;
            if (!Contains(id))
                return false;

            data = list[id];
            return true;
        }

        public bool TryGetObject(string primID, out ObjectData data)
        {
            foreach (SimData sim in list.Values)
            {
                if (TryGetObject(sim.ID, primID, out data))
                    return true;
            }

            data = null;
            return false;
        }

        public bool TryGetObject(string id, string primID, out ObjectData data)
        {
            data = null;
            if (!Contains(id))
                return false;

            SimData d = list[id];
            if (d.AvatarCollection.TryGet(primID, out data))
                return true;

            if (d.PrimCollection.TryGet(primID, out data))
                return true;

            return false;
        }

        public bool Contains(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            return list.ContainsKey(id);
        }

        internal void Reset()
        {
            foreach (SimData data in list.Values)
                data.Reset();

            list.Clear();
        }
    }
}
