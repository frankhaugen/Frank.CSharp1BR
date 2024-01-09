using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddChannels(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<ChannelFactory>();
        services.AddSingleton(provider => provider.GetRequiredService<ChannelFactory>().CreateChannel<WeatherData>());
        services.AddSingleton(provider => provider.GetRequiredService<ChannelFactory>().CreateChannel<string>());
        return services;
    }
}