using System.Threading.Channels;
using Microsoft.Extensions.Caching.Memory;

public class ChannelFactory
{
    private readonly IMemoryCache _cache;

    public ChannelFactory(IMemoryCache cache)
    {
        _cache = cache;
    }
    
    public Channel<T> CreateChannel<T>() where T : class => _cache.GetOrCreate<Channel<T>>(typeof(T).Name,x =>
    {
        x.SlidingExpiration = TimeSpan.FromMinutes(60);
        return Channel.CreateUnbounded<T>();
    }) ?? throw new InvalidOperationException("Channel not found");
}