using System;
using IrrlichtNETCP;
using OxCore;
using OxCore.Data;

namespace OxRender.Plugin.Debug
{
    public class Axis : OxRenderComponentPlugin
    {
        private const float RADIUS_CENTER = 0.5f;
        private const float RADIUS_LINE = 0.2f;
        private const float LENGTH = 64;
        private const int COUNT = 16;
        private static readonly Vector3D OFFSET = new Vector3D(0, 0, 20);

        public Axis(Ox ox, Render render)
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
            CreateAxis();

            base.Load();
        }

        public override void Unload()
        {
            if (Root.Children.Length > 0)
                Root.RemoveAll();

            base.Unload();
        }

        void debug_CheckedChanged(object sender, EventArgs e)
        {
            IOxRenderDebugController debug = (IOxRenderDebugController)Ox.Service.Get(typeof(IOxRenderDebugController));
            Root.Visible = debug.GetVisible(OxRender.Plugin.Debug.Controller.Type.Axis);
        }

        private void CreateAxis()
        {
            float width = (LENGTH / COUNT);
            SceneNode node;

            // Center
            node = Render.Scene.AddSphereSceneNode(RADIUS_CENTER, 32, Root);
            node.Position = OFFSET;
            node.SetMaterialFlag(MaterialFlag.Lighting, true);
            for (int i = 0; i < node.MaterialCount; i++)
                node.GetMaterial(i).EmissiveColor = Color.Black;

            // X Axis
            for (int i = 0; i < COUNT; i++)
            {
                node = Render.Scene.AddSphereSceneNode(RADIUS_LINE, 8, Root);
                node.Position = OFFSET + new Vector3D(width * i, 0, 0);
                node.SetMaterialFlag(MaterialFlag.Lighting, true);
                for (int j = 0; j < node.MaterialCount; j++)
                    node.GetMaterial(j).EmissiveColor = Color.Red;
            }

            // Y Axis
            for (int i = 0; i < COUNT; i++)
            {
                node = Render.Scene.AddSphereSceneNode(RADIUS_LINE, 8, Root);
                node.Position = OFFSET + Util.ToPositionRH(new float[] { 0, width * i, 0 });
                node.SetMaterialFlag(MaterialFlag.Lighting, true);
                for (int j = 0; j < node.MaterialCount; j++)
                    node.GetMaterial(j).EmissiveColor = Color.Green;
            }

            // Z Axis
            for (int i = 0; i < COUNT; i++)
            {
                node = Render.Scene.AddSphereSceneNode(RADIUS_LINE, 8, Root);
                node.Position = OFFSET + new Vector3D(0, 0, width * i);
                node.SetMaterialFlag(MaterialFlag.Lighting, true);
                for (int j = 0; j < node.MaterialCount; j++)
                    node.GetMaterial(j).EmissiveColor = Color.Blue;
            }
        }
    }
}
