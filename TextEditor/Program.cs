using System;
using PythonExecutor;
namespace TextEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            var textGenerator = new TextGenerationCaller();
            var result =
                textGenerator.Launch(
                    "\"Hello i am starting a great journey for my new application development, it is quite exciting\"",
                    out string errors);
            Console.WriteLine(result);
            Console.WriteLine(errors);
        }
    }
}
