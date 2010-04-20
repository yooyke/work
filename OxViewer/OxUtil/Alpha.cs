using System;
using System.Threading;

namespace OxUtil
{
    public class Alpha
    {
        private const float MIN = 0;
        private const float MAX = 255;
        private const float DEFAULT_SPEED = 1;

        public enum FpsType
        {
            Fps15,
            Fps30,
            Fps60,
        }

        private float start;
        private float end;
        private float speed;
        private float value;
        private float vector;
        private bool isEnd;
        private Timer timer;
        private FpsType fps = FpsType.Fps30;

        public event EventHandler OnEnd;

        public float Start { get { return start; } }
        public float End { get { return end; } }
        public float Speed { get { return speed; } }
        public bool IsEnd { get { return isEnd; } }
        public float Value { get { return (value / MAX); } }
        public int Value255 { get { return (int)value; } }

        /// <summary>
        /// Min : 0, Max : 255, Speed : 1
        /// </summary>
        public Alpha()
            : this(MIN, MAX, DEFAULT_SPEED) { }

        public Alpha(float start, float end, float speed)
            : this(MIN, MAX, DEFAULT_SPEED, FpsType.Fps30) { }

        /// <summary>
        /// AlphaController's custome constructor
        /// </summary>
        /// <param name="start">Start alpha value</param>
        /// <param name="end">Target alpha value</param>
        /// <param name="speed">Increment value speed</param>
        /// <param name="fps">Timer update frame per second</param>
        public Alpha(float start, float end, float speed, FpsType fps)
        {
            Set(start, end, speed);

            this.fps = fps;
        }

        public void Set(float start, float end, float speed)
        {
            start = MathHelper.Clamp(start, MIN, MAX);
            end = MathHelper.Clamp(end, MIN, MAX);

            this.start = start;
            this.value = start;
            this.end = end;
            this.speed = speed;

            isEnd = false;

            CalcVector();

            int period = 33;
            switch (fps)
            {
                case FpsType.Fps15:
                    period = (int)(MathHelper.FPS15 * 1000);
                    break;
                case FpsType.Fps30:
                    period = (int)(MathHelper.FPS30 * 1000);
                    break;
                case FpsType.Fps60:
                    period = (int)(MathHelper.FPS60 * 1000);
                    break;
            }

            if (timer != null)
                timer.Dispose();

            timer = new Timer(new TimerCallback(Update), null, period, period);
        }

        private void Update(object state)
        {
            if (isEnd)
                return;

            value += vector * speed;

            if ((vector > 0 && value >= end))
            {
                value = end;
                isEnd = true;

                if (timer != null)
                    timer.Dispose();

                if (OnEnd != null)
                    OnEnd(this, EventArgs.Empty);
            }

            if ((vector < 0 && value <= end))
            {
                value = end;
                isEnd = true;

                if (OnEnd != null)
                    OnEnd(this, EventArgs.Empty);
            }
        }

        private void CalcVector()
        {
            vector = (end > start) ? 1 : -1;
        }
    }
}
