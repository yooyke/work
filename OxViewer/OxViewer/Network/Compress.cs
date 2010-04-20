using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace OxViewer.Util
{
    public class DecompressionRequest
    {
        public string sourceFilename;
        public string targetFilename;
        public DecompressionRequest(string sourceFilename, string targetFilename)
        {
            this.sourceFilename = sourceFilename;
            this.targetFilename = targetFilename;
        }
    }

    public static class Compress
    {
        private static Queue<DecompressionRequest> DecompressionRequests = new Queue<DecompressionRequest>();

        public static void AddDecompressionRequest(DecompressionRequest req)
        {
            lock (DecompressionRequests)
            {
                DecompressionRequests.Enqueue(req);
            }
        }

        public static void DecompressWaitingRequests()
        {
            lock (DecompressionRequests)
            {
                while (DecompressionRequests.Count > 0)
                {
                    DecompressionRequest req = DecompressionRequests.Dequeue();
                    Decompress(req);
                }
            }
        }

        public static byte[] Decompress(byte[] compressedBinary)
        {
            byte[] buffer = new byte[4096];
            List<byte> list = new List<byte>();

            MemoryStream ms = null;
            GZipStream gzip = null;
            try
            {
                ms = new MemoryStream(compressedBinary);
                gzip = new GZipStream(ms, CompressionMode.Decompress, true);
                while (true)
                {
                    int count = gzip.Read(buffer, 0, buffer.Length);
                    if (count == 0)
                        break;

                    for (int i = 0; i < count; i++)
                        list.Add(buffer[i]);

                    // have reached the end
                    if (count != buffer.Length)
                        break;
                }
            }
            finally
            {
                if (gzip != null)
                {
                    gzip.Close();
                    gzip = null;
                }

                if (ms != null)
                {
                    ms.Close();
                    ms = null;
                }
            }

            return list.ToArray();
        }

        private static int Decompress(DecompressionRequest req)
        {
            string zipName = req.sourceFilename;
            string fileName = req.targetFilename;
            string dstFile = "";
            FileStream fsIn = null;
            FileStream fsOut = null;
            GZipStream gzip = null;
            const int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];
            int count = 0;
            int totalCount = 0;
            try
            {
                dstFile = fileName;
                fsIn = new FileStream(zipName, FileMode.Open, FileAccess.Read, FileShare.Read);
                fsOut = new FileStream(dstFile, FileMode.Create, FileAccess.Write, FileShare.None);
                gzip = new GZipStream(fsIn, CompressionMode.Decompress, true);
                while (true)
                {
                    count = gzip.Read(buffer, 0, bufferSize);
                    if (count != 0)
                    {
                        fsOut.Write(buffer, 0, count);
                        totalCount += count;
                    }
                    if (count != bufferSize)
                    {
                        // have reached the end
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                // handle or display the error
                System.Diagnostics.Debug.Assert(false, ex.ToString());
            }

            finally
            {
                if (gzip != null)
                {
                    gzip.Close();
                    gzip = null;
                }
                if (fsOut != null)
                {
                    fsOut.Close();
                    fsOut = null;
                }
                if (fsIn != null)
                {
                    fsIn.Close();
                    fsIn = null;
                }
            }

            return totalCount;
        }
    }
}
