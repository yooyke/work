using System.Collections.Generic;

namespace OxCore.Data
{
    public class PrimCollectionData : ObjectCollectionData
    {
        public void Add(string id, string parentID, string sceneName, float[] position, float[] rotation, float[] scale, float[] velocity, bool myself, ClickActionType clickActionType)
        {
            if (Contains(id))
                Delete(id);

            list.Add(id, new PrimData(id, parentID, sceneName, position, rotation, scale, velocity, myself, clickActionType));
        }

    }
}
