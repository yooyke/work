using System;
using System.IO;
using IrrlichtNETCP;
using OxCore;

namespace OxRender.Plugin.Default
{
    public class Sea : OxRenderComponentPlugin
    {
        private const float TILE_W_NUMBER = 96;
        private const float TILE_H_NUMBER = 96;
        private const float TILE_SPAN = 16;

        string dir;

        public Sea(Ox ox, Render render)
            : base(ox, render)
        {
            Priority = (int)PriorityBase.RenderSea;

            dir = Path.Combine(Path.Combine(Ox.Paths.Application, "media"), "sea");
        }

        public override void Load()
        {
            if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                CreateSea();

            base.Load();
        }

        public override void Unload()
        {
            if (Root.Children.Length > 0)
                Root.RemoveAll();

            base.Unload();
        }

        private void CreateSea()
        {
            Mesh mesh = new Mesh();
            MeshBuffer mb = new MeshBuffer(VertexType.Standard);
            for (int h = 0; h < TILE_H_NUMBER; h++)
            {
                for (int w = 0; w < TILE_W_NUMBER; w++)
                {
                    mb.SetVertex((uint)(h * TILE_H_NUMBER + TILE_W_NUMBER), new Vertex3D(
                        new Vector3D(w * TILE_SPAN, h * TILE_SPAN, 19.8f),
                        new Vector3D(0, 0, 1),
                        Color.White,
                        new Vector2D((float)w / TILE_W_NUMBER, (float)h / TILE_H_NUMBER)
                        ));
                }
            }

            uint index = 0;
            for (int h = 0; h < (int)(TILE_H_NUMBER - 1); h++)
            {
                for (int w = 0; w < (int)(TILE_W_NUMBER - 1); w++)
                {
                    mb.SetIndex(index++, (ushort)((h + 0) * TILE_H_NUMBER + (w + 0)));
                    mb.SetIndex(index++, (ushort)((h + 0) * TILE_H_NUMBER + (w + 1)));
                    mb.SetIndex(index++, (ushort)((h + 1) * TILE_H_NUMBER + (w + 0)));

                    mb.SetIndex(index++, (ushort)((h + 1) * TILE_H_NUMBER + (w + 0)));
                    mb.SetIndex(index++, (ushort)((h + 0) * TILE_H_NUMBER + (w + 1)));
                    mb.SetIndex(index++, (ushort)((h + 1) * TILE_H_NUMBER + (w + 1)));
                }
            }
            mb.Material.Texture1 = Render.Texture.GetTexture(Path.Combine(dir, "sea.jpg"));
            mb.Material.BackfaceCulling = false;
            mesh.AddMeshBuffer(mb);

            MeshSceneNode node = Render.Scene.AddMeshSceneNode(mesh, Root, -1);
            node.Position = new Vector3D(-(TILE_W_NUMBER * TILE_SPAN / 2), -(TILE_H_NUMBER * TILE_SPAN / 2), 0);
            node.AutomaticCulling = CullingType.Off;
        }
    }
}
