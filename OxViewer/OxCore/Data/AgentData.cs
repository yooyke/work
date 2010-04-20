namespace OxCore.Data
{
    public class AgentData
    {
        public string AssetServerUri = string.Empty;
        public string AuthToken = string.Empty;

        /// <summary>
        /// [uuid] Agent ID
        /// </summary>
        public string ID = string.Empty;
        /// <summary>
        /// [uuid] Current sim ID
        /// </summary>
        public string SimID = string.Empty;
        public string SimName = string.Empty;
        public string First = string.Empty;
        public string Last = string.Empty;

        public bool AlwaysRun = false;
        public double Head = 0;
        public float LengthFromPoint = 0;

        public int LoginFailedCount = 0;
        public string LoginStatus = string.Empty;
        public string LoginMessage = string.Empty;

        internal void Reset()
        {
            AssetServerUri = string.Empty;
            AuthToken = string.Empty;
            ID = string.Empty;
            SimID = string.Empty;
            SimName = string.Empty;
            AlwaysRun = false;
            Head = 0;
            LengthFromPoint = 0;
            LoginStatus = string.Empty;
            LoginMessage = string.Empty;
        }
    }
}
