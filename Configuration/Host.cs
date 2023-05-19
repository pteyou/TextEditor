using System;
using Microsoft.Extensions.Configuration;

namespace TextEditor.Configuration
{
    public sealed class Hosting
    {
        private static readonly Hosting _instance = new Hosting();
        private static readonly Conf _configuration;
        static Hosting()
        {
            try
            {
                _configuration = LoadConfiguration();
            }
            catch (Exception e)
            {
                Console.WriteLine($"FATAL Error, unable to load configuration file {Environment.NewLine}{e.Message}");
                System.Environment.Exit(1);
            }
            
        }

        private Hosting()
        {

        }

        public static Hosting Instance => _instance;

        public Conf Config => _configuration;

        private static Conf LoadConfiguration()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("editorSettings.json", optional: false, reloadOnChange: true)
                .Build();
            var options = config.GetRequiredSection(nameof(Conf)).Get<Conf>();
            return options;
        }
    }
}
