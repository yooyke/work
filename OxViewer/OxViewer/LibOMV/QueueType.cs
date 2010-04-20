using System;
using OpenMetaverse;

namespace OxViewer.LibOMV
{
    struct QueueBase
    {
        public Delegate Method;
        public object[] Args;

        public QueueBase(Delegate method, params object[] args)
        {
            Method = method;
            Args = args;
        }
    }

    struct QueueAsset
    {
        public Transfer transfer;
        public OpenMetaverse.Asset asset;

        public QueueAsset(Transfer transfer, OpenMetaverse.Asset asset)
        {
            this.transfer = transfer;
            this.asset = asset;
        }
    }
}
