using System;
using IrrlichtNETCP;
using OxCore;
using OxCore.Data;

namespace OxRender.Plugin.Debug
{
    public class Point : OxRenderComponentPlugin
    {
        private SceneNode node;

        public Point(Ox ox, Render render)
            : base(ox, render)
        {
        }

        public override void Initialize()
        {
            IOxRenderDebugController debug = (IOxRenderDebugController)Ox.Service.Get(typeof(IOxRenderDebugController));
            debug.CheckedChanged += new EventHandler(debug_CheckedChanged);

            base.Initialize();
        }

        public override void Load()
        {
            node = Render.Scene.AddSphereSceneNode(0.1f, 32, Root);
            node.SetMaterialFlag(MaterialFlag.Lighting, true);

            base.Load();
        }

        public override void Update(ApplicationTime time)
        {
            if (node != null)
            {
                node.Position = Util.ToPositionRH(Ox.DataStore.World.Point.Position);
                node.Visible = true;
                Color c = Color.Black;
                switch (Ox.DataStore.World.Point.Type)
                {
                    case PointData.ObjectType.None:
                        node.Visible = false;
                        break;
                    case PointData.ObjectType.Avatar:
                        c = Color.Blue;
                        break;
                    case PointData.ObjectType.AvatarSelf:
                        c = Color.Purple;
                        break;
                    case PointData.ObjectType.Prim:
                        c = Color.Red;
                        break;
                    case PointData.ObjectType.Ground:
                        c = Color.Gray;
                        break;
                }

                for (int i = 0; i < node.MaterialCount; i++)
                    node.GetMaterial(i).EmissiveColor = c;
            }

            base.Update(time);
        }

        public override void Unload()
        {
            if (Root.Children.Length > 0)
                Root.RemoveAll();

            node = null;

            base.Unload();
        }

        void debug_CheckedChanged(object sender, EventArgs e)
        {
            IOxRenderDebugController debug = (IOxRenderDebugController)Ox.Service.Get(typeof(IOxRenderDebugController));
            Root.Visible = debug.GetVisible(OxRender.Plugin.Debug.Controller.Type.Point);
        }
    }
}
