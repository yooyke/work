using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;

namespace OxCore
{
    public class FileIOManager : OxComponent
    {
        private long length;
        private long max; // Byte
        private long requestMax;
        private WebClient client;
        private Dictionary<string, FileInfo> list = new Dictionary<string, FileInfo>();
        private Dictionary<string, FileInfo> unusedList = new Dictionary<string, FileInfo>();

        public event AsyncCompletedEventHandler DownloadFileCompleted;

        public long Max { get { return max; } }

        public FileIOManager(Ox ox)
            : base(ox)
        {
            Priority = (int)PriorityBase.IO;
        }

        public override void Initialize()
        {
            CreateCacheList();

            base.Initialize();
        }

        public override void Update(ApplicationTime time)
        {
            if (max != requestMax)
                max = requestMax;

            base.Update(time);
        }

        public void SetMax(long max)
        {
            requestMax = max;
        }

        public bool Contains(string filename)
        {
            return list.ContainsKey(filename);
        }

        /// <summary>
        /// Load file from cache, And return file path
        /// </summary>
        /// <param name="filename">File name</param>
        /// <returns>File path</returns>
        public string Load(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return string.Empty;

            Thread.Sleep(1);

            string name = filename.Replace(Ox.Paths.Cache, string.Empty);
            if (unusedList.ContainsKey(name))
                unusedList.Remove(name);
            else
                return string.Empty;

            return Path.Combine(Ox.Paths.Cache, name);
        }

        /// <summary>
        /// Download file from web server. If async is true, call OnDownloadCompleted event
        /// </summary>
        /// <param name="uri">URL</param>
        /// <param name="filename">File name</param>
        /// <param name="async">Ture : async download</param>
        public void Download(string uri, string filename, bool async)
        {
            if (string.IsNullOrEmpty(uri) || string.IsNullOrEmpty(filename))
                return;

            if (Contains(filename))
            {
                if (unusedList.ContainsKey(filename))
                {
                    DeleteCache(filename);
                }
                else
                    return;
            }

            if (client == null)
            {
                client = new WebClient();
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
            }
            else
            {
                if (client.IsBusy)
                {
                }
            }

            string path = Path.Combine(Ox.Paths.Cache, filename);
            if (async)
                client.DownloadFileAsync(new Uri(uri), path, path);
            else
                client.DownloadFile(new Uri(uri), filename);
        }

        void client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.UserState != null && e.UserState is string)
                AddFileInfo((e.UserState as string), true);

            if (DownloadFileCompleted != null)
                DownloadFileCompleted(sender, e);
        }

        public bool Save(string filename, byte[] data)
        {
            string dir = Ox.Paths.Check(Ox.Paths.Cache);

            if ((length + data.Length) > max)
                DeleteCache(length + data.Length - max);

            string path = Path.Combine(dir, filename);

            if (Contains(filename))
            {
                if (unusedList.ContainsKey(filename))
                {
                    DeleteCache(filename);
                }
                else
                    return false;
            }

            File.WriteAllBytes(path, data);
            AddFileInfo(path, true);

            Thread.Sleep(1);
            return true;
        }

        public override void Cleanup()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }

            base.Cleanup();
        }

        private void CreateCacheList()
        {
            length = 0;
            list.Clear();

            string[] paths = Directory.GetFiles(Ox.Paths.Cache);
            foreach (string path in paths)
                AddFileInfo(path, false);
        }

        private void AddFileInfo(string path, bool use)
        {
            FileInfo info = new FileInfo(path);
            length += info.Length;
            list.Add(info.Name, info);

            if (!use)
                unusedList.Add(info.Name, info);
        }

        private void DeleteCache(long length)
        {
            List<string> keys = new List<string>(unusedList.Count);

            long total = 0;
            foreach (FileInfo info in unusedList.Values)
            {
                keys.Add(info.Name);
                total += info.Length;
                if (total > length)
                    break;
            }

            foreach (string key in keys)
                DeleteCache(key);
        }

        private void DeleteCache(string key)
        {
            if (list.ContainsKey(key))
            {
                File.Delete(list[key].FullName);
                list.Remove(key);
            }

            if (unusedList.ContainsKey(key))
                unusedList.Remove(key);
        }
    }
}
