using DataStructures;
using System;
using System.Threading.Tasks;
using WebApiClient;

namespace TextEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            TextGenerationCacheTree TGCT = new TextGenerationCacheTree();
            TGCT.AddPropositions("bonjour je vais tres bien et vous");
            TGCT.AddPropositions("bonjour je vais tres mal et vous");
            TGCT.AddPropositions("bonjour tu vas tres mal et vous");

            var res = TGCT.GetPropositions(3);
            var resFi = TGCT.GetPropositions(5, true, "bonjour je vais");


            System.Console.WriteLine(string.Join(Environment.NewLine, res));
            Console.WriteLine($"{Environment.NewLine} {Environment.NewLine} --- {Environment.NewLine} {Environment.NewLine}");
            System.Console.WriteLine(string.Join(Environment.NewLine, resFi));

            Console.WriteLine("Entrer to quit");
            Console.ReadLine();
        }
        public static async Task Test()
        {
            var args = new TextGenerationArguments("Hello", 4);
            Response r = await TextGeneration.Instance.Run(args);
            Console.WriteLine($"status code : {r.StatusCode}");
            if (r.Field != null)
            {
                Console.WriteLine($"Text : {r.Text}");
            }
        }
    }
}
