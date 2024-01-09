using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Microsoft.Extensions.Hosting;

public class LineParserService : BackgroundService
{
    private readonly Channel<string> _channel;
    private readonly Channel<WeatherData> _weatherDataChannel;


    public LineParserService(Channel<string> channel, Channel<WeatherData> weatherDataChannel)
    {
        _channel = channel;
        _weatherDataChannel = weatherDataChannel;
    }
    
    private static ulong _lineCount;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var line in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            _lineCount++;
            if (_lineCount % 1_000_000 == 0)
            {
                Console.WriteLine($"LineParserService: {_lineCount}");
            }
            await _weatherDataChannel.Writer.WriteAsync(ParseLine(line), stoppingToken);
        }
    }
        
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static WeatherData ParseLine(string line)
    {
        var parts = line.Split(';');
        return new WeatherData
        {
            StationName = parts[1],
            Temperature = double.Parse(parts[0])
        };
    }
}