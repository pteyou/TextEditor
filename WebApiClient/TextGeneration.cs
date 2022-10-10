using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiClient
{
    public class TextGeneration
    {
        private static TextGeneration instance = new TextGeneration();
        private static readonly RequestRunner Rr;
        private TextGeneration()
        {

        }
        static TextGeneration()
        {
            Rr = new RequestRunner();
        }
        public static TextGeneration Instance
        {
            get => instance;
        }

        public async Task<Response> Run(TextGenerationArguments args)
        {
            Rr.Args = args;
            return await Rr.ProcessRequest(false);
        }
    }
}
