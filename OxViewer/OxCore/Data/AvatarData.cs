namespace OxCore.Data
{
    public class AvatarData : ObjectData
    {
        private string first;
        private string last;
        private Animation[] animations;

        public class Animation
        {
            public string id;
            public int sequenceID;

            public Animation(string id, int sequenceID)
            {
                this.id = id;
                this.sequenceID = sequenceID;
            }
        }

        public string First { get { return first; } }
        public string Last { get { return last; } }
        public string Name { get { return first + " " + last; } }
        public Animation[] Animations { get { return animations; } }

        public AvatarData(string id, string first, string last, string parentID, float[] position, float[] rotation, float[] scale, float[] velocity, bool myself)
            : base(id, parentID, position, rotation, scale, velocity, myself)
        {
            this.first = first;
            this.last = last;
            this.animations = null;
        }

        public void UpdateAnimation(Animation[] animations)
        {
            this.animations = animations;
        }
    }
}
