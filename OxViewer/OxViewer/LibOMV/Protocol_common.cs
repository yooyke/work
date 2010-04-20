using System;
using System.Collections.Generic;
using OpenMetaverse;
using OxCore;
using OxCore.Data;

namespace OxViewer.LibOMV
{
    partial class Protocol
    {
        private static ClickActionType GetClickType(Primitive prim)
        {
            ClickActionType clickType = ClickActionType.None;
            switch (prim.ClickAction)
            {
                case ClickAction.Touch:
                    if ((prim.Flags & PrimFlags.Touch) != 0)
                        clickType = ClickActionType.Touch;
                    break;

                case OpenMetaverse.ClickAction.Sit:
                    clickType = ClickActionType.Sit;
                    break;
            }

            return clickType;
        }

        #region UUID and LocalID Util
        private void AddObjectDictByLocalID(Primitive prim)
        {
            DeleteObjectDictByLocalID(prim.LocalID);
            lock (objectDictByLocalID)
                objectDictByLocalID.Add(prim.LocalID, prim);
        }

        private void DeleteObjectDictByLocalID(uint localID)
        {
            if (objectDictByLocalID.ContainsKey(localID))
            {
                lock (objectDictByLocalID)
                    objectDictByLocalID.Remove(localID);
            }
        }

        private uint GetLocalIDFromID(string id)
        {
            Primitive p = GetObjectFromID(id);
            if (p == null)
                return 0;

            return p.LocalID;
        }

        private Primitive GetObjectFromID(string id)
        {
            lock (objectDictByLocalID)
                foreach (Primitive prim in objectDictByLocalID.Values)
                {
                    if (id == prim.ID.ToString())
                        return prim;
                }

            return null;
        }

        private Primitive GetObjectFromLocalID(uint localID)
        {
            if (!objectDictByLocalID.ContainsKey(localID))
                return null;

            return objectDictByLocalID[localID];
        }

        private string GetIDFromLocalID(uint localID)
        {
            Primitive p = GetObjectFromLocalID(localID);
            if (p == null)
                return string.Empty;

            return p.ID.ToString();
        }
        #endregion
    }
}
