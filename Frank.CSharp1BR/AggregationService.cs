using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Microsoft.Extensions.Hosting;

public class AggregationService : BackgroundService
{
    private readonly ConcurrentDictionary<string, WeatherDataAggregator> _results;
    private readonly Channel<WeatherData> _channel;

    public AggregationService(Channel<WeatherData> channel)
    {
        _channel = channel;
        _results = new ConcurrentDictionary<string, WeatherDataAggregator>();
    }
    
    private static ulong _lineCount;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var weatherData in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            _lineCount++;
            if (_lineCount % 1_000_000 == 0)
            {
                Console.WriteLine($"AggregationService: {_lineCount}");
                Console.WriteLine($"AggregationService:\n{weatherData.StationName}\n{_results[weatherData.StationName].GetResult()}");
            }
            if (!_results.ContainsKey(weatherData.StationName))
            {
                _results[weatherData.StationName] = new WeatherDataAggregator();
            }
            _results[weatherData.StationName].Accumulate(weatherData);
        }
    }
}