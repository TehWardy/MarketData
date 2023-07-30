using MarketData.Hubs;
using MarketData.IG;
using MarketData.IG.StreamingApi.Models;

class Program
{
    // https://www.ig.com/uk/myig/settings/api-keys
    // https://labs.ig.com/gettingstarted
    // https://labs.ig.com/streaming-api-reference
    // https://labs.ig.com/sample-apps/api-companion/index.html
    // https://labs.ig.com/sample-apps/streaming-companion/index.html
    // https://github.com/IG-Group/ig-webapi-dotnet-sample

    static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var builder = WebApplication.CreateBuilder();
        builder.Logging.AddConfiguration(config.GetSection("Logging"));
        RegisterServices(config, builder.Services);

        var app = builder.Build();

        app.MapHub<MarketDataHub>("hubs/markets");
        app.MapControllers();
        app.Run();
    }

    private static void RegisterServices(IConfiguration config, IServiceCollection services)
    {
        var igConfig = new IGConfiguration();
        config.GetSection("IG").Bind(igConfig);

        services.AddSingleton(config);
        services.AddSingleton(igConfig);

        var ig = new IGClient(igConfig);
        ig.Login().Wait();
        services.AddSingleton(ig);

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();

            loggingBuilder.AddSimpleConsole(options =>
            {
                options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
                options.SingleLine = true;
            });
        });

        services.AddControllersWithViews();
        services.AddSignalR();
    }
}