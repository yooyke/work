using System;
using System.Collections.Generic;
using System.Text;

namespace OxViewer.LibOMV
{
    public class AssetIrrFile
    {
        private string filename;
        private List<string> materials = new List<string>();

        public string Filename { get { return filename; } }

        public AssetIrrFile(string filename)
        {
            this.filename = filename;
        }

        public void AddMaterial(string material)
        {
            materials.Add(material);
        }

        public string[] GetMaterials()
        {
            return materials.ToArray();
        }
    }

    public class AssetIrrMemory
    {
        private AssetBase irrFile;
        private List<AssetBase> materials = new List<AssetBase>();

        public AssetBase IrrFile { get { return irrFile; } }

        public AssetIrrMemory(AssetBase assetBase)
        {
            irrFile = assetBase;
        }

        public void AddMaterial(AssetBase material)
        {
            materials.Add(material);
        }

        public AssetBase[] GetMaterials()
        {
            return materials.ToArray();
        }
    }
}
