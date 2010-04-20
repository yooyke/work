using System;
using System.IO;
using IrrlichtNETCP;
using OxCore;
using OxCore.Data;
using OxJson;

namespace OxRender.Plugin.Default.Terrain
{
    public class Terrain : OxRenderTerrainPlugin
    {
        string dir;

        public Terrain(Ox ox, Render render)
            : base(ox, render)
        {
            Priority = (int)PriorityBase.RenderTerrain;

            dir = Path.Combine(Path.Combine(Ox.Paths.Application, "media"), "terrain");
        }

        public override void Load()
        {
            if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                CreateTerrain();

            base.Load();
        }

        public override void Unload()
        {
            if (Root.Children.Length > 0)
                Root.RemoveAll();

            base.Unload();
        }

        private void CreateTerrain()
        {
            string path = Path.Combine(dir, "sand01.jpg");
            Texture tex = Render.Texture.GetTexture(path);
            if (tex == null)
                tex = Render.RenderData.BlankTexture;

            TerrainSceneNode node = Render.Scene.AddTerrainSceneNodeFromRawData(
                TerrainData.GetDefaultHeightData(),
                256,
                Root,
                -1,
                new Vector3D(0, 0, 0),
                new Vector3D(0, 0, 0),
                new Vector3D(1, 1, 1),
                Color.White,
                4,
                TerrainPatchSize.TPS9,
                4
                );

            node.Position = new Vector3D(0, -node.TerrainCenter.Y - node.TerrainCenter.Z, node.TerrainCenter.Y - node.TerrainCenter.Z);
            node.Rotation = new Vector3D(-90, 0, 0);
            node.SetMaterialTexture(0, tex);
            node.ScaleTexture(16, 16);
            node.SetMaterialFlag(MaterialFlag.Lighting, true);
        }
    }
}
