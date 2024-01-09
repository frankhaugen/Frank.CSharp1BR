namespace Frank.CSharp1BR.DataGenerator;

public record struct DateTimeRange(DateTime Start, DateTime End)
{
    public DateTime Current { get; private set; } = Start;

    public DateTime GetNext(TimeSpan interval) => Current.Add(interval);
}