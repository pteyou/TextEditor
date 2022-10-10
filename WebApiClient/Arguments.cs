using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using TextEditor.Configuration;

namespace WebApiClient
{
    public abstract class Arguments
    {
        [JsonIgnore]
        protected static Conf configuration = Hosting.Instance.Config;
        [JsonPropertyName("ModelPath")]
        public abstract string ModelPath { get; }
        [JsonPropertyName("Task")]
        public abstract string Task { get; }
        [JsonPropertyName("InputString")]
        public string InputString { get; set; }
        [JsonPropertyName("OutputField")]
        public abstract string OutputField { get; }
        [JsonIgnore]
        public virtual string GetJsonPayload
        {
            get
            {
                Type type = typeof(JsonSerializer);
                MethodInfo mi = type.GetMethods()
                    .Where(p => 
                    {
                        return p.IsGenericMethod && p.Name == "Serialize" && p.GetParameters().Length == 2 && p.GetParameters().Count(q => q.IsOptional) == 1;
                    }).FirstOrDefault();
                if (mi == null)
                    throw new ApplicationException("WHAATT");
                MethodInfo miConstructed = mi.MakeGenericMethod(GetType());
                return miConstructed.Invoke(null, new object[] { this, null }) as string;
            }
        }
    }

    public class TextGenerationArguments : Arguments
    {
        [JsonPropertyName("OutputSize")]
        public int OutputSize { get; }

        public override string ModelPath => configuration.TextGenerationModelPath;

        public override string Task => "text-generation";

        public override string OutputField => "generated_text";

        public TextGenerationArguments(string inputString, int outputSize)
        {
            InputString = inputString;
            OutputSize = outputSize;
        }
    }
}
