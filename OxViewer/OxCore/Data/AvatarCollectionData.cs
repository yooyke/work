using System.Collections.Generic;

namespace OxCore.Data
{
    public class AvatarCollectionData : ObjectCollectionData
    {
        public void Add(string id, string first, string last, string parentID, float[] position, float[] rotation, float[] scale, float[] velocity, bool myself)
        {
            if (Contains(id))
                Delete(id);

            list.Add(id, new AvatarData(id, first, last, parentID, position, rotation, scale, velocity, myself));
        }
    }
}
