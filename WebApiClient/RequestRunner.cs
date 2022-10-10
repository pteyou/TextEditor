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
    class RequestRunner
    {
        private static Conf configuration { get; }
        private static string HostName { get; }
        private static string Port { get; }
        private static HttpClient client { get; }
        private Arguments _arguments;

        static RequestRunner()
        {
            configuration = Hosting.Instance.Config;
            HostName = configuration.Hostname;
            Port = configuration.Port;
            client = new HttpClient();
        }

        public RequestRunner(Arguments args = null)
        {
            _arguments = args;
        }

        public Arguments Args
        {
            set
            {
                _arguments = value;
            }
        }
        public async Task<Response> ProcessRequest(bool get)
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
                var jsonPayload = _arguments.GetJsonPayload;
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
    }
}
