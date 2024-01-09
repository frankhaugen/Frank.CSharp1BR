// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

Console.WriteLine("Hello, World!");

var settings = new Settings()
{
    FilePath = "../../../../data.csv"
};

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostContext, configApp) =>
    {
        configApp.AddInMemoryCollection(new Dictionary<string, string>
        {
            {"Settings:FilePath", settings.FilePath}
        });
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<Settings>(hostContext.Configuration.GetSection(nameof(Settings)));
        services.AddChannels();
        services.AddHostedService<AggregationService>();
        services.AddHostedService<LineParserService>();
        services.AddHostedService<DataProcessingService>();
    });
    
await builder.RunConsoleAsync();