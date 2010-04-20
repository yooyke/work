using System;

namespace OxAX
{
    public partial class OxViewerCtl
    {
        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002;
        private const int S_OK = 0;

        int IObjectSafety.GetInterfaceSafetyOptions(ref Guid riid, out int pdwSupportedOptions, out int pdwEnabledOptions)
        {
            pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            return S_OK;   // return S_OK
        }

        int IObjectSafety.SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
        {
            return S_OK;   // return S_OK
        }
    }
}
