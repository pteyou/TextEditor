namespace TextEditor.Configuration
{
    public class Conf
    {
        public string Hostname { get; set; }
        public string Port { get; set; }
        public string TextGenerationModelPath { get; set; }
        public int MaxTextGenCacheSize { get; set; }
        public int MaxTextGenCacheWorkers { get; set; }
    }
}
