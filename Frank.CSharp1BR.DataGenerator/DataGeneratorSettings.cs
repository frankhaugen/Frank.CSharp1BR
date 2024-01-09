namespace Frank.CSharp1BR.DataGenerator;

public record DataGeneratorSettings
{
    public string FilePath { get; init; }
    public ulong MaxNumberOfLines { get; init; }
    public DateTimeRange DateRange { get; set; }
    public TimeSpan Interval { get; set; }
}