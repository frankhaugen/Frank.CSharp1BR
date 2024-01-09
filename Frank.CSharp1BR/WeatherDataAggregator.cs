public class WeatherDataAggregator
{
    public double MinTemperature { get; private set; } = double.MaxValue;
    public double MaxTemperature { get; private set; } = double.MinValue;
    public double TotalTemperature { get; private set; }
    public long Count { get; private set; }

    public void Accumulate(WeatherData data)
    {
        MinTemperature = Math.Min(MinTemperature, data.Temperature);
        MaxTemperature = Math.Max(MaxTemperature, data.Temperature);
        TotalTemperature += data.Temperature;
        Count++;
    }

    public AggregatedResult GetResult() =>
        new()
        {
            MinTemperature = MinTemperature,
            MaxTemperature = MaxTemperature,
            TotalTemperature = TotalTemperature / Count,
            Count = Count
        };
}