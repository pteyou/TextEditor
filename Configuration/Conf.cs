namespace TextEditor.Configuration
{
    public class Conf
    {
        public string Hostname { get; set; }
        public int GrpcCorePort { get; set; }
        public int MaxTextGenCacheSize { get; set; }
        public int MaxTextGenCacheWorkers { get; set; }
    }
}
