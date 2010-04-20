using System;

namespace OxCore.Data
{
    public class StatusData
    {
        public const int PROGRESS_MAX = 1000;

        public enum Type
        {
            None,

            Initialize,
            InitializeEnd,

            Login,
            LoginEnd,
            RunningFade,
            RunningBef,
            Running,

            Logout,
            LogoutEnd,
            WaitingFade,
            WaitingBef,
            Waiting,

            Cleanup,
            CleanupEnd,
        }

        public Type Status = Type.None;
        public int Progress = 0;
        public int ProgressForceWaitSecond = 1;

        public float ProgressValue { get { return (float)Progress / PROGRESS_MAX; } }
    }
}
