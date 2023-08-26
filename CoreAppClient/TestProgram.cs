namespace TextEditor.CoreAppClient;

public class Program
{
    public static async Task Main(string[] args) 
    {
        var response = await SanityChecker.Call();
        System.Console.WriteLine($"call to health check returns {response.Ok}");
        if (!response.Ok)
        {
            Console.WriteLine($"{response.ServiceInfo.StatusMessage}");
        }
        
        var genTextResponse = await TextGenerator.Call("hello world I am here", 10, 5);
        System.Console.WriteLine($"call to text generator returns {genTextResponse.ServiceInfo.Ok}");
        if (!genTextResponse.ServiceInfo.Ok)
        {
            Console.WriteLine($"{genTextResponse.ServiceInfo.StatusMessage}");
        }
        else
        {
            foreach (var textOutput in genTextResponse.OuptutText)
            {
                Console.WriteLine($"Generated : {textOutput}");
            }
        }
    }

}