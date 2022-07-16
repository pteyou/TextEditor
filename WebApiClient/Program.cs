using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TextEditor.Configuration;

namespace WebApiClient
{
    class Program
    {
        private static Conf configuration = Hosting.Instance.Config;
        private static string _ModelPath = configuration.TextGenerationModelPath;
        private static string HostName => "localhost";
        private static string Port => "8000";
        private static readonly HttpClient client = new HttpClient();
        private static async Task<Response> ProcessRequest(bool get)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            if (get)
            {
                response = await client.GetAsync($"http://{HostName}:{Port}/");
            }
            else
            {
                var payload = new Arguments
                {
                    ModelPath = _ModelPath,
                    Task = "text-generation",
                    InputString = "Hello i am starting a great journey for my new application development, it is quite exciting",
                    OutputSize = 5,
                    OutputField = "generated_text"
                };
                var jsonPayload = JsonSerializer.Serialize(payload);
                var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                response = await client.PostAsync($"http://{HostName}:{Port}/api/v1", httpContent);
            }
            
            var jsonReponse = JsonSerializer.Deserialize<Response>(await response.Content.ReadAsStringAsync());
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.BadRequest:
                    return new Response { ErrorMessage = jsonReponse.ErrorMessage, StatusCode = (int)response.StatusCode };
                case System.Net.HttpStatusCode.OK:
                    return new Response { Text = jsonReponse.Text, Field = jsonReponse.Field, StatusCode = (int)response.StatusCode };
                default:
                    return null;
            }
        }
        static async Task Main(string[] args)
        {
            Response r = await ProcessRequest(false);
            Console.WriteLine($"status code : {r.StatusCode}");
            if(r.Field != null)
            {
                Console.WriteLine($"Text : {r.Text}");
            }
        }
    }
}

