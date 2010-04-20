namespace OxJson
{
    public struct JsonMessage
    {
        public string key;
        public string value;

        public JsonMessage(string type, string value)
        {
            this.key = type;
            this.value = value;
        }
    }
}
