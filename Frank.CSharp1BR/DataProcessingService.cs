using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

public class DataProcessingService : BackgroundService
{
    private readonly IOptions<Settings> _settings;
    private readonly Channel<string> _channel;
    
    public DataProcessingService(IOptions<Settings> settings, Channel<string> channel)
    {
        _settings = settings;
        _channel = channel;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Starting DataProcessingService");
        var stopWatch = Stopwatch.StartNew();
        await ReadFileAsync(_settings.Value.FilePath, _channel.Writer, stoppingToken);
        stopWatch.Stop();
        Console.WriteLine($"DataProcessingService completed in {stopWatch.Elapsed}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static async Task ReadFileAsync(string filePath, ChannelWriter<string> writer, CancellationToken cancellationToken)
    {
        const FileOptions fileOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;
        await using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 81920, fileOptions);
        var buffer = new byte[81920];
        var decoder = Encoding.UTF8.GetDecoder();
        var stringBuilder = new StringBuilder();
        var charBuffer = new char[81920];
        var read = 0;
        while ((read = await fileStream.ReadAsync(buffer, cancellationToken)) > 0)
        {
            var charCount = decoder.GetChars(buffer, 0, read, charBuffer, 0);
            stringBuilder.Append(charBuffer, 0, charCount);
            var lines = stringBuilder.ToString().Split(Environment.NewLine);
            for (var i = 0; i < lines.Length - 1; i++)
            {
                await writer.WriteAsync(lines[i], cancellationToken);
            }
            stringBuilder.Clear();
            stringBuilder.Append(lines[^1]);
        }
        await writer.WriteAsync(stringBuilder.ToString(), cancellationToken);
        writer.Complete();
    }
}