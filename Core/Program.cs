﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Serilog;
using Services;

using IHost host = Host.CreateDefaultBuilder(args)
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
        services.AddSingleton<IGetDataFromSourceService, GetDataFromSourceService>();
        services.AddOptions<Settings>()
            .Bind(configurationRoot.GetSection(nameof(Settings)));

    })
    .UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext())
    .Build();

using IServiceScope serviceScope = host.Services.CreateScope();
var provider = serviceScope.ServiceProvider;
var fService = provider.GetRequiredService<IFillDataService>();
await fService.FillDb();
//await host.RunAsync();

