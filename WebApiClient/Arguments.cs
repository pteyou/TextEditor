using System.Text.Json.Serialization;

namespace WebApiClient
{
    internal class Arguments
    {
        [JsonPropertyName("ModelPath")]
        public string ModelPath { get; set; }
        [JsonPropertyName("Task")]
        public string Task { get; set; }
        [JsonPropertyName("InputString")]
        public string InputString { get; set; }
        [JsonPropertyName("OutputSize")]
        public int OutputSize { get; set; }
        [JsonPropertyName("OutputField")]
        public string OutputField { get; set; }
    }
}
