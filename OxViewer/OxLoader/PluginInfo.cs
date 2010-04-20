namespace OxLoader
{
    public struct PluginInfo
    {
        public string Path;
        public string ClassName;

        public PluginInfo(string path, string className)
        {
            Path = path;
            ClassName = className;
        }
    }
}
