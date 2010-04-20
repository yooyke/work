using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using OxViewer.LibOMV;

namespace OxViewer.Network
{
    public class Asset
    {
        public static AssetIrrMemory GetIrrScene(string url, string auth, string filename)
        {
            AssetBase irrfile = Get(url, auth, filename);
            AssetIrrMemory irrs = new AssetIrrMemory(irrfile);

            Irr.Parser parser = new OxViewer.Irr.Parser();
            MemoryStream ms = new MemoryStream(irrfile.Data);
            parser.Run(ms);

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
                AssetBase ab = Get(url, auth, asset);
                irrs.AddMaterial(ab);
            }

            return irrs;
        }

        public static AssetBase Get(string url, string auth, string filename)
        {
            string id = Path.GetFileNameWithoutExtension(filename);
            int timeout = 30 * 1000;

            RestClient rest = new RestClient(url);
            rest.RequestMethod = "GET";
            rest.AddResourcePath("assets");
            rest.AddResourcePath(id);
            rest.AddHeader("Authorization", "OpenGrid " + auth);

            Stream stream;
            try
            {
                stream = rest.Request(timeout);
            }
            catch
            {
                return null;
            }
            finally
            {
                Thread.Sleep(1);
            }

            XmlSerializer xs = new XmlSerializer(typeof(AssetBase));
            AssetBase ab = (AssetBase)xs.Deserialize(stream);
            ab.Name = filename;

            bool tryToDecompress = true;
            if (tryToDecompress
                && ab.Data.Length >= 2
                && ab.Data[0] == 0x1f // gzip header == 0x1f8b
                && ab.Data[1] == 0x8b)
            {
                byte[] decompress;
                try
                {
                    decompress = Util.Compress.Decompress(ab.Data);
                    ab.Data = decompress;
                }
                catch { }
            }

            return ab;
        }
    }
}
