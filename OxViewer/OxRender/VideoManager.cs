using System.Collections.Generic;
using IrrlichtNETCP;

namespace OxRender
{
    public class VideoManager
    {
        private VideoDriver video;

        public VideoManager(VideoDriver video)
        {
            this.video = video;
        }

        public void Draw2DImage(Texture image, ref Position2D destPos, ref Rect rect, ref Color color, bool useAlphaChannel)
        {
            video.Draw2DImage(image, destPos, rect, color, useAlphaChannel);
        }
    }
}
