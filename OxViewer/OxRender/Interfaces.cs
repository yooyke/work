using System.Reflection;
using OxLoader;
using OxCore;
using IrrlichtNETCP;

namespace OxRender
{
    public interface IOxRenderComponentPlugin : IPlugin
    {
        void Load();
        void Draw();
        void Unload();
    }

    public abstract class OxRenderComponentPlugin : OxComponent, IOxRenderComponentPlugin
    {
        private static int id_count = 0;
        private Render render;
        private SceneNode root;

        protected Render Render { get { return render; } }
        protected SceneNode Root { get { return root; } }

        public OxRenderComponentPlugin(Ox ox, Render render)
            : base(ox)
        {
            this.render = render;
        }

        public override void Initialize()
        {
            base.Initialize();

            System.Type type = this.GetType();

            root = Render.Scene.AddEmptySceneNode(Render.Root, id_count++);
            root.Name = type.FullName;
        }

        public virtual void Load() { }
        public virtual void Draw() { }
        public virtual void Unload() { }
    }

    #region Object Plugin
    public interface IOxRenderPluginObject : IOxRenderComponentPlugin
    {
        void Collision();
    }

    public abstract class OxRenderObjectPlugin : OxRenderComponentPlugin, IOxRenderPluginObject
    {
        public OxRenderObjectPlugin(Ox ox, Render render)
            : base(ox, render) { }

        public virtual void Collision() { }
    }
    #endregion

    #region Avatar Plugin
    public interface IOxRenderPluginAvatar : IOxRenderPluginObject
    {
        SceneNode GetAvatarScneNode(string key);
    }

    public abstract class OxRenderAvatarPlugin : OxRenderObjectPlugin, IOxRenderPluginAvatar
    {
        public OxRenderAvatarPlugin(Ox ox, Render render)
            : base(ox, render)
        {
            Ox.Service.Add(typeof(IOxRenderPluginAvatar), this);
        }

        public abstract SceneNode GetAvatarScneNode(string key);
    }
    #endregion

    #region Prim Plugin
    public interface IOxRenderPluginPrim : IOxRenderPluginObject
    {
        SceneNode GetPrimScneNode(string key);
    }

    public abstract class OxRenderPrimPlugin : OxRenderObjectPlugin, IOxRenderPluginPrim
    {
        public OxRenderPrimPlugin(Ox ox, Render render)
            : base(ox, render)
        {
            Ox.Service.Add(typeof(IOxRenderPluginPrim), this);
        }

        public abstract SceneNode GetPrimScneNode(string key);
    }
    #endregion

    #region Terraion Plugin
    public interface IOxRenderPluginTerrain : IOxRenderComponentPlugin
    {
    }

    public abstract class OxRenderTerrainPlugin : OxRenderComponentPlugin, IOxRenderPluginTerrain
    {
        public OxRenderTerrainPlugin(Ox ox, Render render)
            : base(ox, render)
        {
            Ox.Service.Add(typeof(IOxRenderPluginTerrain), this);
        }
    }
    #endregion

    #region TilePicker Plugin
    public interface IOxRenderPluginTilePicker : IOxRenderPluginObject
    {
    }

    public abstract class OxRenderTilePickerPlugin : OxRenderObjectPlugin, IOxRenderPluginTilePicker
    {
        public OxRenderTilePickerPlugin(Ox ox, Render render)
            : base(ox, render)
        {
            Ox.Service.Add(typeof(IOxRenderPluginTilePicker), this);
        }
    }
    #endregion
}
