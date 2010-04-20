namespace OxCore.Data
{
    public class OxData
    {
        private const float DEFAULT_FPS_ACTIVE_TARGET = 30;
        private const float DEFAULT_FPS_DEACTIVE_TARGET = 5;

        public float FpsActiveTarget = DEFAULT_FPS_ACTIVE_TARGET;
        public float FpsDeactiveTarget = DEFAULT_FPS_DEACTIVE_TARGET;
        public long CacheMax;
    }
}
