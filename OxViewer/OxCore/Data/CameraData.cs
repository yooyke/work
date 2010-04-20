namespace OxCore.Data
{
    public class CameraData
    {
        private float[] angle = new float[2];
        private float[] origin = new float[2];

        public float Distance;
        public float DistanceMax;
        public float DistanceMin;
        public float[] Angle { get { return angle; } }
        public float[] Origin { get { return origin; } }

        public void SetAngle(float x, float y)
        {
            angle[0] = x;
            angle[1] = y;
        }

        public void SetOrigin(float x, float y)
        {
            origin[0] = x;
            origin[1] = y;
        }
    }
}
