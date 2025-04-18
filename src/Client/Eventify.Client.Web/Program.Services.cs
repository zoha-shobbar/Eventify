﻿using Eventify.Client.Web.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Eventify.Client.Web;

public static partial class Program
{
    public static void ConfigureServices(this WebAssemblyHostBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;
        // The following services are blazor web assembly only.

        builder.Logging.ConfigureLoggers(configuration);

        services.AddClientWebProjectServices(configuration);

        Uri.TryCreate(configuration.GetServerAddress(), UriKind.RelativeOrAbsolute, out var serverAddress);

        if (serverAddress!.IsAbsoluteUri is false)
        {
            serverAddress = new Uri(new Uri(builder.HostEnvironment.BaseAddress), serverAddress);
        }

        services.AddScoped(sp =>
        {
            var httpClient = new HttpClient(sp.GetRequiredService<HttpMessageHandler>())
            {
                BaseAddress = serverAddress
            };

            httpClient.DefaultRequestHeaders.Add("X-Origin", builder.HostEnvironment.BaseAddress);

            return httpClient;
        });
        services.AddKeyedScoped<HttpMessageHandler, HttpClientHandler>("PrimaryHttpMessageHandler");
    }

    public static void AddClientWebProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddClientCoreProjectServices(configuration);
        // The following services work both in blazor web assembly and server side for pre-rendering and blazor server.

        services.AddTransient<IPrerenderStateService, WebPrerenderStateService>();

        services.AddScoped<IBitDeviceCoordinator, WebDeviceCoordinator>();
        services.AddScoped<IExceptionHandler, WebExceptionHandler>();
        services.AddScoped<IStorageService, BrowserStorageService>();
        services.AddScoped<IPushNotificationService, WebPushNotificationService>();

        services.AddSingleton(sp =>
        {
            ClientWebSettings settings = new();
            configuration.Bind(settings);
            return settings;
        });

        services.AddOptions<ClientWebSettings>()
            .Bind(configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
