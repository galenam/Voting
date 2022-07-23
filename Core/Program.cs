using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Serilog;
using Service;

Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Verbose()
  .WriteTo.File("log.txt",
    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
  .CreateLogger();
Log.Information("Hello, Serilog!");
using IHost host = Host.CreateDefaultBuilder(args)
.ConfigureLogging(tmp => tmp.AddSerilog())
    .ConfigureAppConfiguration((hostingContext, configuration) =>
    {
        configuration.Sources.Clear();
        var env = hostingContext.HostingEnvironment;
        configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
        IConfigurationRoot configurationRoot = configuration.Build();
        var options =
            configurationRoot.GetSection(nameof(Settings))
                             .Get<Settings>();
    })
    .ConfigureServices((context, services) =>
    {
        var configurationRoot = context.Configuration;
        services.AddSingleton<IFillDataService, FillDataService>();

    })
    .Build();

using IServiceScope serviceScope = host.Services.CreateScope();
var provider = serviceScope.ServiceProvider;
var fService = provider.GetRequiredService<IFillDataService>();
fService.FillDb();
Log.CloseAndFlush();
//await host.RunAsync();

