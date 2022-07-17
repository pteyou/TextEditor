using System;
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
        static async Task Main(string[] args)
        {
            RequestRunner rr = new RequestRunner(new TextGenerationArguments("Hello i am starting a great journey for my new application development, it is quite exciting", 5));
            Response r = await rr.ProcessRequest(false);
            Console.WriteLine($"status code : {r.StatusCode}");
            if(r.Field != null)
            {
                Console.WriteLine($"Text : {r.Text}");
            }
        }
    }
}

