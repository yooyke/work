using System;
using OxCore;
using OxCore.Data;
using OxJson;

namespace OxViewer
{
    class Progress : OxComponent
    {
        private const float IDEAL_LOGIN_SECOND = 2.0f;
        private const float IDEAL_LANDPATCH_SECOND = 8.0f;
        private const float IDEAL_LOGOUT_SECOND = 5.0f;
        private const float RATE_LOGIN = 0.2f;
        private const float RATE_LANDPATCH = 0.4f;
        private const float RATE_CONTENT = 0.4f;

        private float auto_increment_rate = 1;
        private float auto_increment_counter = 0;
        private float content_load_counter = 0;
        private float force_wait_second = 1;

        public Progress(Ox ox)
            : base(ox)
        {
            Ox.OnEvent += new OxLoader.OxEventHandler(Ox_OnEvent);
        }

        public override void Update(ApplicationTime time)
        {
            LoginProcess(time);
            LogoutProcess(time);

            base.Update(time);
        }

        void Ox_OnEvent(string message)
        {
            JsonMessage param;
            JsonType type = JsonUtil.DeserializeMessage(message, out param);
            switch (type)
            {
                case JsonType.StateInside:
                    MessageState(param.value);
                    break;
            }
        }

        private void MessageState(string message)
        {
            JsonStateInside j = (JsonStateInside)JsonUtil.Deserialize<JsonStateInside>(message);
            switch (j.state)
            {
                case (int)StatusData.Type.Login:
                    Ox.DataStore.World.Status.Progress = 0;
                    auto_increment_counter = 0;
                    content_load_counter = 0;
                    force_wait_second = (Ox.DataStore.World.Status.ProgressForceWaitSecond > 1 ? Ox.DataStore.World.Status.ProgressForceWaitSecond : 1);
                    auto_increment_rate = (IDEAL_LOGIN_SECOND + IDEAL_LANDPATCH_SECOND + force_wait_second);
                    break;

                case (int)StatusData.Type.LoginEnd:
                    if (auto_increment_counter < RATE_LOGIN * StatusData.PROGRESS_MAX)
                        auto_increment_counter = RATE_LOGIN * StatusData.PROGRESS_MAX;
                    break;
                case (int)StatusData.Type.Logout:
                    Ox.DataStore.World.Status.Progress = StatusData.PROGRESS_MAX;
                    auto_increment_counter = StatusData.PROGRESS_MAX;
                    auto_increment_rate = IDEAL_LOGOUT_SECOND;
                    break;
            }
        }

        private void LoginProcess(ApplicationTime time)
        {
            if (Ox.DataStore.World.Status.Status < StatusData.Type.Login || StatusData.Type.Running < Ox.DataStore.World.Status.Status)
                return;

            float progress_max = StatusData.PROGRESS_MAX;
            float progress_now = 0;

            if (StatusData.Type.Login < Ox.DataStore.World.Status.Status)
                progress_now += (RATE_LOGIN * progress_max);

            SimData sim;
            if (Ox.DataStore.World.TryGetCurrentSim(out sim))
            {
                float content_max = (force_wait_second * Ox.DataStore.Core.FpsActiveTarget);
                progress_now += (RATE_LANDPATCH * progress_max * ((float)sim.Terrain.PatchCount / TerrainData.PATCH_MAX));

                if (sim.Terrain.PatchCount == TerrainData.PATCH_MAX)
                {
                    progress_now += RATE_LANDPATCH * progress_max * (content_load_counter / content_max);

                    if (content_load_counter < content_max)
                        content_load_counter++;
                }
            }

            if (progress_now < auto_increment_counter)
                progress_now = auto_increment_counter;

            if (progress_now > progress_max)
                progress_now = progress_max;
            Ox.DataStore.World.Status.Progress = (int)progress_now;

            auto_increment_counter += (StatusData.PROGRESS_MAX / auto_increment_rate) * (float)time.ElapsedTime.TotalSeconds;
        }

        private void LogoutProcess(ApplicationTime time)
        {
            if (Ox.DataStore.World.Status.Status < StatusData.Type.Logout || StatusData.Type.Waiting < Ox.DataStore.World.Status.Status)
                return;

            float progress_now = auto_increment_counter;

            if (progress_now < 0)
                progress_now = 0;
            Ox.DataStore.World.Status.Progress = (int)progress_now;

            auto_increment_counter -= (StatusData.PROGRESS_MAX / auto_increment_rate) * (float)time.ElapsedTime.TotalSeconds;
        }
    }
}
