using OxLoader;

namespace OxCore
{
    public interface IOxRenderPlugin : IPlugin
    {
    }

    public abstract class OxRenderPlugin : OxDrawableComponent, IOxRenderPlugin
    {
        public OxRenderPlugin(Ox ox)
            : base(ox) { }
    }
}
