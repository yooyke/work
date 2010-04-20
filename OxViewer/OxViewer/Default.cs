namespace OxViewer
{
    static class Default
    {
        public const string LOGIN_USER_AGENT = "OxViewer";
        public const string LOGIN_USER_VERSION = "1.0.0.0";

        public const float CORE_FPS_ACTIVE_TARGET = 30;
        public const float CORE_FPS_DEACTIVE_TARGET = 5;
        public const long CORE_CACHE_MAX = 1 * 1024 * 1024 * 1024;

        public const bool AGENT_ALWAYS_RUN = false;
        public const double AGENT_HEAD = 0;
        public const float AGENT_TURN_SPEED = 0.10f;
        public const float CAMERA_START_DISTANCE = 4;
        public const float CAMERA_MIN_DISTANCE = 1.8f;
        public const float CAMERA_MAX_DISTANCE = 10;
        public const float CAMERA_DELTA_SPEED = 0.25f;
        public const float CAMERA_ROTATION_SPEED = 0.01f;
    }
}
