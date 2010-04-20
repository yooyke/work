using OxLoader;
using OxCore;

namespace OxViewer
{
    public interface IOxViewerPlugin : IPlugin
    {
    }

    public abstract class OxViewerPlugin : OxComponent, IOxViewerPlugin
    {
        public OxViewerPlugin(Ox ox)
            : base(ox)
        {
        }
    }
}
