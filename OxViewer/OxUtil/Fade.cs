using System;
using System.Threading;

namespace OxUtil
{
    public class Fade
    {
        private Alpha alpha;
        private int wait;
        private bool half = false;
        private Timer timer;

        public event EventHandler OnHalf;
        public event EventHandler OnEnd;

        public float Value { get { return alpha.Value; } }
        public int Value255 { get { return alpha.Value255; } }

        public Fade(float speed)
            : this(speed, 1, Alpha.FpsType.Fps30) { }

        /// <summary>
        /// Fade's custome constructor
        /// </summary>
        /// <param name="speed">Increment value speed</param>
        /// <param name="wait">Wait millisecond</param>
        /// <param name="fps">15, 30, 60</param>
        public Fade(float speed, int wait, Alpha.FpsType fps)
        {
            this.wait = wait;

            alpha = new Alpha(0, 255, speed, fps);
            alpha.OnEnd += new System.EventHandler(alpha_OnEnd);
        }

        void alpha_OnEnd(object sender, System.EventArgs e)
        {
            if (half)
            {
                if (OnEnd != null)
                    OnEnd(this, EventArgs.Empty);
            }
            else
            {
                half = true;

                if (OnHalf != null)
                    OnHalf(this, EventArgs.Empty);

                // callback, state, wait time, span
                timer = new Timer(new TimerCallback(Wait), null, wait, 0);
            }
        }

        private void Wait(object state)
        {
            if (timer != null)
                timer.Dispose();

            alpha.Set(alpha.End, alpha.Start, alpha.Speed);
        }
    }
}
