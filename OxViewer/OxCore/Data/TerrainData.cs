namespace OxCore.Data
{
    public class TerrainData
    {
        private const int WIDTH = 256;
        private const int HEIGHT = 256;
        private const int PATCH_WIDTH = 16;
        private const int PATCH_HEIGHT = 16;

        public const int PATCH_MAX = PATCH_WIDTH * PATCH_HEIGHT;

        private uint positionX;
        private uint positionY;
        private float[,] heightData;
        private bool[] patch_flag;
        private int patch_count;

        public uint PositionX { get { return positionX; } }
        public uint PositionY { get { return positionY; } }
        public float[,] HeightData { get { return heightData; } }
        public int PatchCount { get { return patch_count; } }

        public TerrainData(uint positionX, uint positionY)
        {
            this.positionX = positionX;
            this.positionY = positionY;

            heightData = TerrainData.GetDefaultHeightData();
            patch_flag = new bool[PATCH_MAX];
        }

        public void SetData(int patchX, int patchY, float[] data)
        {
            if (data.Length < 256)
                return;

            for (int y = 0; y < PATCH_HEIGHT; y++)
                for (int x = 0; x < PATCH_WIDTH; x++)
                    heightData[patchX * PATCH_HEIGHT + x, patchY * 16 + y] = data[y * PATCH_HEIGHT + x];

            if (!patch_flag[patchY * PATCH_HEIGHT + patchX])
            {
                patch_flag[patchY * PATCH_HEIGHT + patchX] = true;
                patch_count++;
            }
        }

        public static float[,] GetDefaultHeightData()
        {
            float[,] hd = new float[WIDTH, HEIGHT];
            for (int y = 0; y < HEIGHT; y++)
                for (int x = 0; x < WIDTH; x++)
                    hd[x, y] = 20;

            return hd;
        }

        internal void Reset()
        {
            for (int i = 0; i < patch_flag.Length; i++)
                patch_flag[i] = false;

            patch_count = 0;
        }
    }
}
