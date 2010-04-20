using System;
using IrrlichtNETCP;

namespace OxRender
{
    public static partial class Util
    {
        public static readonly Vector3D ROTATION_OFFSET = new Vector3D(90, 0, 0);
        public static readonly Vector3D ROTATION_AND_3DS_OFFSET = new Vector3D(90, 0, 90);

        public static Vector3D ToVector3D(float[] array)
        {
            return new Vector3D(array[0], array[1], array[2]);
        }

        public static Vector3D ToScale(float[] rotArray)
        {
            return new Vector3D(rotArray[0], rotArray[2], rotArray[1]);
        }

        public static float[] ToPositionArrayFromIrrlicht(ref Vector3D position)
        {
            return new float[] { position.X, -position.Y, position.Z };
        }

        public static float ToAngleRH(float angle)
        {
            return -angle;
        }

        public static Vector3D ToPositionRH(float[] posArray)
        {
            return new Vector3D(posArray[0], -posArray[1], posArray[2]);
        }

        public static Vector3D ToRotationRH(float[] rotArray)
        {
            return new Vector3D(rotArray[0], rotArray[1], -rotArray[2]);
        }

        public static Vector3D ToRotationRH(ref Vector3D rotation)
        {
            return new Vector3D(rotation.X, rotation.Y, -rotation.Z);
        }

        public static Colorf ToColorf(Color color)
        {
            return ToColorf(color.A, color.R, color.G, color.B);
        }

        public static Colorf ToColorf(int a, int r, int g, int b)
        {
            return new Colorf((float)a / 255.0f, (float)r / 255.0f, (float)g / 255.0f, (float)b / 255.0f);
        }

        public static Color ToColor(Colorf color)
        {
            return ToColor(color.A, color.R, color.G, color.B);
        }

        public static Color ToColor(float a, float r, float g, float b)
        {
            return new Color((int)(a * 255), (int)(r * 255), (int)(g * 255), (int)(b * 255));
        }
    }
}
