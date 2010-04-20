using System;
using System.Runtime.InteropServices;

namespace OxAX
{
    [ComImport]
    [Guid("57F183AA-7EC2-4eaa-9784-7FC0616A90CD")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    interface IObjectSafety
    {
        [PreserveSig]
        int GetInterfaceSafetyOptions(ref Guid riid, out int pdwSupportedOptions, out int pdwEnabledOptions);

        [PreserveSig]
        int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions);
    }
}
