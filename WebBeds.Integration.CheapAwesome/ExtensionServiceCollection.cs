using Microsoft.Extensions.DependencyInjection.Extensions;
using Polly;
using Polly.Extensions.Http;
using System;
using WebBeds.Integration.CheapAwesome;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ExtensionServiceCollection
    {
        public static void AddCheapAwesome<TConfiguration>(this IServiceCollection services)
            where TConfiguration : class, IConfigurationCheapBeds
        {
            services.TryAddSingleton<IConfigurationCheapBeds, TConfiguration>();
            services
                .AddHttpClient<IServiceCheapBeds, ServiceCheapBeds>((sp, client) =>
                {
                    IConfigurationCheapBeds configuration
                        = sp.CreateScope().ServiceProvider.GetRequiredService<IConfigurationCheapBeds>();
                    client.BaseAddress = new Uri(configuration.BaseAddress);
                })
                .AddPolicyHandler((sp, _) =>
                {
                    IConfigurationCheapBeds configuration
                        = sp.CreateScope().ServiceProvider.GetRequiredService<IConfigurationCheapBeds>();

                    return HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                        .WaitAndRetryAsync(
                            retryCount: configuration.RetryCount,
                            sleepDurationProvider: retryAttempt =>
                                TimeSpan.FromSeconds(
                                    Math.Pow(configuration.RetryDelayFactor, retryAttempt)));
                })
                .AddPolicyHandler((sp, _) =>
                {
                    IConfigurationCheapBeds configuration
                           = sp.CreateScope().ServiceProvider.GetRequiredService<IConfigurationCheapBeds>();

                    return HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .CircuitBreakerAsync(
                            handledEventsAllowedBeforeBreaking: configuration.HandledEventsBeforeBreaking,
                            durationOfBreak: TimeSpan.FromSeconds(configuration.DurationOfBreakSeconds)
                        );
                });
        }
    }
}
