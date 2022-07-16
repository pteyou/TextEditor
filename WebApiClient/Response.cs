using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApiClient
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string Text { get; set; }
        public string Field { get; set; }
        public string ErrorMessage { get; set; }
    }
}
