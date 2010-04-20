using System;

namespace OxUtil
{
    public static class MathHelper
    {
        public const float DEGTORAD = 0.0174533f;
        public const float RADTODEG = 57.2958f;
        public const float TwoPI = (float)Math.PI * 2;
        public const float PI = (float)Math.PI;
        public const float PIOver2 = (float)Math.PI / 2;
        public const float PIOver4 = (float)Math.PI / 4;
        /// <summary>
        /// 0.066667f
        /// </summary>
        public const float FPS15 = 0.066667f;
        /// <summary>
        /// 0.033333f
        /// </summary>
        public const float FPS30 = 0.033333f;
        /// <summary>
        /// 0.016667f
        /// </summary>
        public const float FPS60 = 0.016667f;

        public static int Clamp(int value, int min, int max)
        {
            return (int)ClampCalc(value, min, max); ;
        }

        public static float Clamp(float value, float min, float max)
        {
            return (float)ClampCalc(value, min, max);
        }

        public static double Clamp(double value, double min, double max)
        {
            return ClampCalc(value, min, max);
        }

        private static double ClampCalc(double value, double min, double max)
        {
            value = (value > max) ? max : value;
            value = (value < min) ? min : value;
            return value;
        }

        public static float Length(float[] vector)
        {
            float lenSq = LengthSQ(vector);
            if (lenSq > 0)
                return (float)Math.Sqrt(lenSq);

            return 0;
        }

        public static float LengthSQ(float[] vector)
        {
            return (vector[0] * vector[0] + vector[1] * vector[1] + vector[2] * vector[2]);
        }

        public static float[] Normalize(float[] vector)
        {
            float len = LengthSQ(vector);
            if (len == 0)
                return null;
            return new float[] { vector[0] / len, vector[1] / len, vector[2] / len };
        }

        public static float[] RotateVect(float[] matrix4, float[] vector3)
        {
            float[] vect = new float[3];
            vect[0] = vector3[0] * matrix4[0] + vector3[1] * matrix4[4] + vector3[2] * matrix4[8];
            vect[1] = vector3[0] * matrix4[1] + vector3[1] * matrix4[5] + vector3[2] * matrix4[9];
            vect[2] = vector3[0] * matrix4[2] + vector3[1] * matrix4[6] + vector3[2] * matrix4[10];
            return vect;
        }

        public static float[] RotationDegree(float[] matrix4)
        {
            double Y = -Math.Asin(matrix4[2]);
            double C = Math.Cos(Y);
            Y *= (180.0 / Math.PI);

            double rotx, roty, X, Z;

            if (Math.Abs(C) > 0.0005f)
            {
                rotx = matrix4[10] / C;
                roty = matrix4[6] / C;

                X = Math.Atan2(roty, rotx) * (180.0 / Math.PI);

                rotx = matrix4[0] / C;
                roty = matrix4[1] / C;
                Z = Math.Atan2(roty, rotx) * (180.0 / Math.PI);
            }
            else
            {
                X = 0.0f;
                rotx = matrix4[5];
                roty = -matrix4[4];
                Z = Math.Atan2(roty, rotx) * (180.0 / Math.PI);
            }

            // fix values that get below zero
            // before it would set (!) values to 360
            // that where above 360:
            if (X < 0.00) X += 360.00;
            if (Y < 0.00) Y += 360.00;
            if (Z < 0.00) Z += 360.00;

            return new float[] { (float)X, (float)Y, (float)Z };
        }

        public static float Dot(float[] a, float[] b)
        {
            return (a[0] * b[0] + a[1] * b[1] + a[2] * b[2]);
        }

        public static float[] Cross(float[] a, float[] b)
        {
            return new float[] { a[1] * b[2] - a[2] * b[1], a[2] * b[0] - a[0] * b[2], a[0] * b[1] - a[1] * b[0] };
        }

        public static float CalcTargetFromPosition(float[] target, float[] position)
        {
            return CalcTargetFromPosition(target, position, null);
        }

        public static float CalcTargetFromPosition(float[] target, float[] position, float[] front)
        {
            float[] f = new float[] { 1, 0, 0 };
            if (front != null && front.Length == 3)
                f = front;

            if (target.Length != 3 || position.Length != 3 || f.Length != 3)
                return 0;

            float[] tar = new float[] { target[0] - position[0], target[1] - position[1], 0 };
            float lengthSQ = tar[0] * tar[0] + tar[1] * tar[1];
            if (lengthSQ > 0)
            {
                float length = (float)Math.Sqrt(lengthSQ);
                tar[0] = tar[0] / length;
                tar[1] = tar[1] / length;
            }

            float dotAngle = f[0] * tar[0] + f[1] * tar[1] + 0;
            float angle = (float)Math.Acos(dotAngle);
            if ((f[0] * tar[1] - f[1] * tar[0]) < 0)
                angle = MathHelper.TwoPI - angle;

            return angle;
        }
    }
}
