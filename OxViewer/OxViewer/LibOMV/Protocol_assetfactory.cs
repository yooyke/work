using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using OpenMetaverse;
using PrimMesher;
using OxCore;
using OxJson;
using OxCore.Data;

namespace OxViewer.LibOMV
{
    public partial class Protocol
    {
        private void AssetFactory()
        {
            while (true)
            {
                QueueAsset q = AssetFactoryDequeue();
                switch (q.asset.AssetType)
                {
                    case AssetType.ImageJPEG:
                    case AssetType.ImageTGA:
                    case AssetType.Texture:
                    case AssetType.TextureTGA:
                        if ((q.transfer is ImageDownload) && (q.asset is AssetTexture))
                            AssetImageFactory(q.transfer as ImageDownload, q.asset as AssetTexture);
                        break;
                }
            }
        }

        private void AssetFactoryEnqueue(QueueAsset asset)
        {
            Monitor.Enter(queueAssetFactory);
            try
            {
                queueAssetFactory.Enqueue(asset);
                Monitor.Pulse(queueAssetFactory);
            }
            finally
            {
                Monitor.Exit(queueAssetFactory);
            }
        }

        private QueueAsset AssetFactoryDequeue()
        {
            Monitor.Enter(queueAssetFactory);
            try
            {
                while (queueAssetFactory.Count == 0)
                {
                    Monitor.Wait(queueAssetFactory);
                }
                return queueAssetFactory.Dequeue();
            }
            finally
            {
                Monitor.Exit(queueAssetFactory);
            }
        }

        private void AssetImageFactory(ImageDownload image, AssetTexture asset)
        {
            string extension = "tga";
            byte[] data = asset.AssetData;
            switch (asset.AssetType)
            {
                case AssetType.ImageJPEG:
                    extension = "jpg";
                    break;
                case AssetType.Texture:
                    if (asset.Decode())
                        data = asset.Image.ExportTGA();
                    else
                        data = null;
                    break;
            }

            if (data == null)
                return;

            Ox.IO.Save(string.Format("{0}.{1}", asset.AssetID.ToString(), extension), data);
        }
    }
}
