using System;

namespace OxCore
{
    public class ApplicationTime
    {
        private DateTime startTime;
        private TimeSpan elapsed;

        public DateTime StartTime { get { return startTime; } }
        public TimeSpan ElapsedTime { get { return elapsed; } }

        internal ApplicationTime()
        {
            startTime = DateTime.Now;
        }

        internal void Update(TimeSpan elapsed)
        {
            this.elapsed = elapsed;
        }
    }
}
