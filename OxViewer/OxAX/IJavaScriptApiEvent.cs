using System;
using System.Runtime.InteropServices;

namespace OxAX
{
    // IJavaScriptApiEvent is the COM visible interface that describes the events that can be
    // dispatched TO javascript.
    // Normally these are implemented in javascript in the following manner:
    // <script language="javascript" for="[activexelementid]" event="[functionname(parameterlist)]" type="text/jscript">
    [ComVisible(true)]
    [Guid("9BB9A4EB-4FB2-4105-A424-BA79F74A03F8")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IJavaScriptApiEvent
    {
        [DispId(0)]
        void OnEvent(string message);
    }
}
