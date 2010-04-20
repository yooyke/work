using System;
using System.Collections.Generic;
using System.Text;

namespace OxCore.Data
{
    public class MeshData
    {
        public VertexData[] Vertices;
        public uint[] Indices;
        public string Texture1;
        public bool Texture1DownLoaded;
        public float[] Color;
    }
}
