using System;
using System.IO;
using IrrlichtNETCP;
using OxCore;

namespace OxRender.Plugin.Default
{
    public class Sky : OxRenderComponentPlugin
    {
        private string dir;

        public Sky(Ox ox, Render render)
            : base(ox, render)
        {
            Priority = (int)PriorityBase.RenderSky;

            dir = Path.Combine(Path.Combine(Ox.Paths.Application, "media"), "skybox");
        }

        public override void Load()
        {
            if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                CreateSkybox();

            base.Load();
        }

        public override void Unload()
        {
            if (Root.Children.Length > 0)
                Root.RemoveAll();

            base.Unload();
        }

        private void CreateSkybox()
        {
            bool exists = true;
            string[] tex_name = new string[] { "sea_sky_UP.jpg", "sea_sky_DN.jpg", "sea_sky_LF.jpg", "sea_sky_RT.jpg", "sea_sky_FR.jpg", "sea_sky_BK.jpg" };
            for (int i = 0; i < tex_name.Length; i++)
            {
                tex_name[i] = Path.Combine(dir, tex_name[i]);
                if (!File.Exists(tex_name[i]))
                {
                    exists = false;
                    break;
                }
            }

            if (exists)
            {
                int index = 0;
                SceneNode node = Render.Scene.AddSkyBoxSceneNode(Root, new Texture[] {
                    Render.Texture.GetTexture(tex_name[index++]),
                    Render.Texture.GetTexture(tex_name[index++]),
                    Render.Texture.GetTexture(tex_name[index++]),
                    Render.Texture.GetTexture(tex_name[index++]),
                    Render.Texture.GetTexture(tex_name[index++]),
                    Render.Texture.GetTexture(tex_name[index++]),
                }, -1);

                node.Rotation = Util.ROTATION_OFFSET;
            }
        }
    }
}
