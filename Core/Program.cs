using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Models;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, configuration) => {
        configuration.Sources.Clear();
        var env = hostingContext.HostingEnvironment;
        configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
        var configurationRoot = configuration.Build();
        Settings settings = new();
        configurationRoot.GetSection(nameof(Settings)).Bind(settings);        
    })
    .Build();

await host.RunAsync();