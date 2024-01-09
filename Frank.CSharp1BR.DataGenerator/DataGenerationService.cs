using System.Runtime.CompilerServices;
using System.Text;

namespace Frank.CSharp1BR.DataGenerator;

public class DataGenerationService
{
    private readonly DataGeneratorSettings _settings;
    private readonly IEnumerable<EuropeanCapital> _capitals;

    public DataGenerationService(DataGeneratorSettings settings)
    {
        _settings = settings;
        var capitols = Enum.GetValues<EuropeanCapital>();
        Random.Shared.Shuffle(capitols);
        _capitals = capitols;
    }
    
    public static ulong Iteations { get; set; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task GenerateDataAsync(CancellationToken cancellationToken)
    {
        await using var fileStream = File.OpenWrite(_settings.FilePath);
        await using var streamWriter = new StreamWriter(fileStream);

        var temperatureReadings = Enumerable.Range(0, (int)_settings.MaxNumberOfLines)
            .SelectMany(i =>
            {
                var date = _settings.DateRange.Start + _settings.Interval.Multiply(i);
                return _capitals.Select(capital => GenerateReading(date, capital));
            });

        Console.WriteLine("Writing data...");

        var bufferedReadings = new StringBuilder();
        foreach (var reading in temperatureReadings)
        {
            Iteations++;
            if (Iteations % 1_000_000 == 0)
            {
                Console.WriteLine($"Writing line {Iteations}");
                await streamWriter.WriteAsync(bufferedReadings.ToString());
                bufferedReadings.Clear();
            }

            bufferedReadings.AppendLine(reading.ToString());
        }

        await streamWriter.WriteAsync(bufferedReadings.ToString());
    }

    
    public async Task GenerateDataAsyncV1(CancellationToken cancellationToken)
    {
        await using var fileStream = File.OpenWrite(_settings.FilePath);
        await using var streamWriter = new StreamWriter(fileStream);

        var temperatureReadings = Enumerable.Range(0, (int)_settings.MaxNumberOfLines)
            .AsParallel()
            .WithCancellation(cancellationToken)
            .SelectMany(i =>
            {
                var date = _settings.DateRange.Start + _settings.Interval.Multiply(i);
                return _capitals.Select(capital =>
                {
                    // Console.WriteLine($"Generating data for {capital} at {date}");
                    return GenerateReading(date, capital);
                });
            });
        
        Console.WriteLine("Writing data...");

        IEnumerable<Task> readings = temperatureReadings 
            .Select(async reading => await WriteLineAsync(streamWriter, reading));
            
        await Task.WhenAll(readings);
    }

    
    private static Task WriteLineAsync(TextWriter streamWriter, TemperatureReading reading)
    {
        Iteations++;
        Console.WriteLine($"Writing line {Iteations}");
        return streamWriter.WriteLineAsync(reading.ToString());
    }

    private TemperatureReading GenerateReading(DateTime date, EuropeanCapital capitol)
    {
        var temperature = TemperatureSimulator.GetTemperature(date);
        return new TemperatureReading
        {
            Date = date,
            Temperature = temperature,
            Capital = capitol
        };
    }
}