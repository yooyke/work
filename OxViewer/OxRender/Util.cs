using System;
using IrrlichtNETCP;
using OxCore;
using OxUtil;

namespace OxRender
{
    public static partial class Util
    {
        public static void CreateBox3DFromNode(SceneNode node, out Box3D box)
        {
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float minZ = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            float maxZ = float.MinValue;

            Vector3D[] edges;
            node.BoundingBox.GetEdges(out edges);
            Matrix4 m = new Matrix4();
            m.RotationDegrees = node.Rotation;
            m.Translation = node.Position;
            for (int i = 0; i < edges.Length; i++)
            {
                Vector3D v = edges[i] * node.Scale;
                v = m.TransformVect(ref v);

                if (v.X < minX)
                    minX = v.X;

                if (v.Y < minY)
                    minY = v.Y;

                if (v.Z < minZ)
                    minZ = v.Z;

                if (v.X > maxX)
                    maxX = v.X;

                if (v.Y > maxY)
                    maxY = v.Y;

                if (v.Z > maxZ)
                    maxZ = v.Z;
            }

            box = new Box3D(minX, minY, minZ, maxX, maxY, maxZ);
        }

        public static float CalcTargetFromPosition(ref Vector3D target, ref Vector3D position)
        {
            Vector3D? front = null;
            return CalcTargetFromPosition(ref target, ref position, ref front);
        }

        public static float CalcTargetFromPosition(ref Vector3D target, ref Vector3D position, ref Vector3D? front)
        {
            Vector3D f = new Vector3D(1, 0, 0);
            if (front != null)
                f = (Vector3D)front;

            Vector3D tar = target - position;
            tar.Z = 0;
            if (tar.LengthSQ > 0)
                tar.Normalize();

            float dotAngle;
            dotAngle = f.DotProduct(tar);
            //Vector3D.Dot(ref front, ref tar, out dotAngle);

            Vector3D cross;
            cross = f.CrossProduct(tar);
            //Vector3D.Cross(ref front, ref tar, out cross);

            float angle = (float)Math.Acos(dotAngle);
            if (cross.Z < 0)
                angle = MathHelper.TwoPI - angle;

            return angle;
        }

        public static Matrix4 AbsoluteMatrix(SceneNode bone)
        {
            Matrix4 mat = bone.RelativeTransformation;

            if (bone.Parent != null)
                mat *= AbsoluteMatrix(bone.Parent);

            return mat;
        }
    }
}
