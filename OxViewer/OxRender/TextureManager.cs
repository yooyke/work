using System.Collections.Generic;
using IrrlichtNETCP;

namespace OxRender
{
    public class TextureInfo
    {
        public Texture Texture;
        public bool UseAlpha;
        public bool CheckedPixel;
    }

    public class TextureManager
    {
        private VideoDriver video;
        private Dictionary<string, TextureInfo> texDic = new Dictionary<string, TextureInfo>();

        public int Count { get { return (video == null ? 0 : video.TextureCount); } }

        public TextureManager(VideoDriver video)
        {
            this.video = video;
        }

        public Texture GetTexture(string path)
        {
            TextureInfo info = GetTexture(path, true, false);
            if (info == null)
                return null;

            return info.Texture;
        }

        public TextureInfo GetTexture(string path, bool auto_remove, bool check_pixel)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            if (!auto_remove && texDic.ContainsKey(path))
            {
                TextureInfo tf = texDic[path];
                texDic.Remove(path);
                return tf;
            }

            TextureInfo info;
            Texture tex;
            if (texDic.ContainsKey(path))
            {
                info = texDic[path];
                if (!check_pixel || (check_pixel && info.CheckedPixel))
                    return info;

                tex = info.Texture;
            }
            else
            {
                info = new TextureInfo();
                info.UseAlpha = false;

                tex = video.GetTexture(path);
                if (tex == null)
                    return null;

                info.Texture = tex;
            }

            if (check_pixel)
            {
                for (int x = 0; x < tex.OriginalSize.Width; x++)
                {
                    for (int y = 0; y < tex.OriginalSize.Height; y++)
                    {
                        if (tex.GetPixel(x, y).A < 255)
                        {
                            info.UseAlpha = true;
                            break;
                        }
                    }
                }
            }
            info.CheckedPixel = check_pixel;

            if (auto_remove)
                texDic.Add(path, info);

            return info;
        }

        public Texture AddTexture(string filename, int width, int height, bool auto_remove)
        {
            if (auto_remove && texDic.ContainsKey(filename))
                return texDic[filename].Texture;

            Texture tex = video.AddTexture(new Dimension2D(width, height), filename, ColorFormat.A8R8G8B8);

            if (auto_remove)
            {
                TextureInfo info;
                if (texDic.ContainsKey(filename))
                {
                    info = texDic[filename];
                }
                else
                {
                    info = new TextureInfo();
                    info.CheckedPixel = false;
                }
                info.Texture = tex;

                texDic.Add(filename, info);
            }
            else
            {
                if (texDic.ContainsKey(filename))
                    texDic.Remove(filename);
            }

            return tex;
        }

        /// <summary>
        /// Clear all texture cache and dictionary
        /// </summary>
        internal void Clear()
        {
            if (video != null)
            {
                foreach (TextureInfo info in texDic.Values)
                    video.RemoveTexture(info.Texture);
            }

            texDic.Clear();
        }
    }
}
