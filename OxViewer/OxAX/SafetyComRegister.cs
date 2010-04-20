using System.Text;
using Microsoft.Win32;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OxAX
{
    public partial class OxViewerCtl
    {
        [ComRegisterFunction]
        private static void RegisterClass(string key)
        {
            // Strip off HKEY_CLASSES_ROOT\ from the passed key as I don't need it
            StringBuilder sb = new StringBuilder(key);
            System.Console.WriteLine(key);
            sb.Replace(@"HKEY_CLASSES_ROOT\", "");

            // Open the CLSID\{guid} key for write access
            RegistryKey k = Registry.ClassesRoot.OpenSubKey(sb.ToString(), true);

            // And create the 'Control' key - this allows it to show up in 
            // the ActiveX control container 
            RegistryKey ctrl = k.CreateSubKey("Control");
            ctrl.Close();

            // Next create the CodeBase entry - needed if not string named and GACced.

            RegistryKey inprocServer32 = k.OpenSubKey("InprocServer32", true);
            inprocServer32.SetValue("CodeBase", Assembly.GetExecutingAssembly().CodeBase);
            inprocServer32.Close();

            k.CreateSubKey("Implemented Categories");
            RegistryKey implementedCategories = k.OpenSubKey("Implemented Categories", true);
            implementedCategories.CreateSubKey("{7DD95801-9882-11CF-9FA9-00AA006C42C4}");
            implementedCategories.CreateSubKey("{7DD95802-9882-11CF-9FA9-00AA006C42C4}");
            implementedCategories.Close();

            k.CreateSubKey("InstalledVersion");
            // Finally close the main key
            k.Close();
        }
        [ComUnregisterFunction]
        private static void UnregisterClass(string key)
        {
            StringBuilder sb = new StringBuilder(key);
            sb.Replace(@"HKEY_CLASSES_ROOT\", "");

            // Open HKCR\CLSID\{guid} for write access
            RegistryKey k = Registry.ClassesRoot.OpenSubKey(sb.ToString(), true);

            // Delete the 'Control' key, but don't throw an exception if it does not exist
            k.DeleteSubKey("Control", false);

            // Next open up InprocServer32
            k.OpenSubKey("InprocServer32", true);
            // And delete the CodeBase key, again not throwing if missing 
            k.DeleteSubKey("CodeBase", false);

            k.DeleteSubKey("InstalledVersion", false);

            k.DeleteSubKeyTree("Implemented Categories");
            // Finally close the main key 
            k.Close();
        }
    }
}
