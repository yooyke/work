using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace OxCore
{
    public class ApplicationPath
    {
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern uint SHGetKnownFolderPath(ref Guid rfid, uint dwFlags, IntPtr hToken, out StringBuilder path);

        private string application;
        private string user;
        private string cache;
        public string Application { get { return application; } }
        public string User { get { return user; } }
        public string Cache { get { return cache; } }

        public ApplicationPath()
        {
            application = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            user = GetUserDirectory();
            cache = Path.Combine(user, "cache");

            Check(user);
            Check(cache);
        }

        public string Check(string directory)
        {
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);

            return directory;
        }

        public string GetUserDirectory()
        {
            string location = @"\OxViewer";

            if (Environment.OSVersion.Version.Major >= 6)
            {
                Guid FOLDERID_LocalAppDataLow = new Guid("A520A1A4-1780-4FF6-BD18-167343C5AF16");
                StringBuilder path = new StringBuilder(260);
                uint retval = SHGetKnownFolderPath(ref FOLDERID_LocalAppDataLow, 0, IntPtr.Zero, out path);
                if (retval == 0)
                    return System.IO.Path.Combine(path.ToString(), location);
            }

            return (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + location);
        }
    }
}
