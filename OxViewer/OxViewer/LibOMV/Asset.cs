using System.Collections.Generic;
using OpenMetaverse;

namespace OxViewer.LibOMV
{
    public static class Asset
    {
        public static AssetIrrFile LoadIrrScene(string path)
        {
            AssetIrrFile irrs = new AssetIrrFile(path);
            Irr.Parser parser = new OxViewer.Irr.Parser();
            parser.Run(path);

            if (parser.Scene == null)
                return irrs;

            Irr.IrrNode[] nodes = parser.Scene.GetNodes();
            if (nodes.Length == 0)
                return irrs;

            List<string> list = new List<string>();

            foreach (Irr.IrrNode node in nodes)
            {
                string[] a = node.GetAssets();
                if (a == null || a.Length == 0)
                    continue;

                foreach (string m in a)
                {
                    if (list.Contains(m))
                        continue;

                    list.Add(m);
                }
            }

            if (list.Count == 0)
                return irrs;

            foreach (string asset in list)
            {
                irrs.AddMaterial(asset);
            }

            return irrs;
        }

        public static void ImageDecompress(AssetBase assetBase)
        {
            if (assetBase == null)
                return;

            AssetType type = (AssetType)assetBase.Type;

            AssetTexture texture = null;
            switch(type)
            {
                case AssetType.ImageJPEG:
                case AssetType.ImageTGA:
                case AssetType.Texture: // Jpeg2000
                case AssetType.TextureTGA:
                    texture = new AssetTexture(new UUID(assetBase.ID), assetBase.Data);
                    break;
            }

            if (texture == null)
                return;

            if (type == AssetType.Texture)
            {
                try
                {
                    if (texture.Decode())
                        assetBase.Data = texture.Image.ExportTGA();
                }
                catch { }
            }
        }
    }
}
