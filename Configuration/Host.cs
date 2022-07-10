using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace TextEditor.Configuration
{
    public sealed class Hosting
    {
        private static readonly Hosting instance = new Hosting();
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

        public static Hosting Instance => instance;

        public Conf Config => _configuration;
        private static Conf LoadConfiguration()
        {
            var options = new Conf();
            IHost host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, configuration) =>
                {
                    configuration.Sources.Clear();
                    IHostEnvironment env = hostingContext.HostingEnvironment;
                    configuration.AddJsonFile("settings.json", optional: false, reloadOnChange: true);
                    IConfigurationRoot configurationRoot = configuration.Build();
                    configurationRoot.GetSection(nameof(Conf)).Bind(options);
                }).Build();
            host.Start();
            return options;
        }
    }
}
