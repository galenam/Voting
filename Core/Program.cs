using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Serilog;
using Service;

using IHost host = Host.CreateDefaultBuilder(args)/*
    .ConfigureLogging((context, logBuilder) =>
    {
        var logger = new LoggerConfiguration()
          //.WriteTo.File("log.txt",
            //outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")            
          .ReadFrom.Configuration(context.Configuration)
          .CreateLogger();
        logBuilder.AddSerilog(logger, true);
    })*/
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
    .UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext())
    .Build();

using IServiceScope serviceScope = host.Services.CreateScope();
var provider = serviceScope.ServiceProvider;
var fService = provider.GetRequiredService<IFillDataService>();
fService.FillDb();
//await host.RunAsync();

