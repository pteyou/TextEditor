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
    }

}