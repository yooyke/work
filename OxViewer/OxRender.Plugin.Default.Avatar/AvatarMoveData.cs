using System;
using IrrlichtNETCP;

namespace OxRender.Plugin.Default.Avatar
{
    public partial class Avatar
    {
        private class AvatarMoveData
        {
            private Vector3D target;
            private Vector3D current;
            private int count = 1;
            private int span = 1;
            private float length;

            public Vector3D Target { get { return target; } }
            public Vector3D Current { get { return current; } set { current = value; } }
            public float Length { get { return length; } }
            public int Span { get { return span; } }

            public AvatarMoveData(ref Vector3D position)
            {
                this.target = position;
                this.current = position;
            }

            public void Update(ref Vector3D target, float length)
            {
                this.target = target;
                this.length = length;
                this.span = count;
                this.count = 1;
            }

            public void IncrementCount() { count++; }
        }
    }
}
