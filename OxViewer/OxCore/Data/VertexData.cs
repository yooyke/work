using System;
using System.Collections.Generic;
using System.Text;

namespace OxCore.Data
{
    public struct VertexData
    {
        public float[] Position;
        public float[] Normal;
        public float[] UV;
        public float[] Color;

        public VertexData(float[] position, float[] normal, float[] uv, float[] color)
        {
            Position = position;
            Normal = normal;
            UV = uv;
            Color = color;
        }
    }
}
