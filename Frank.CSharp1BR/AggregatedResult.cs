public class AggregatedResult
{
    public double MinTemperature { get; set; }
    public double MaxTemperature { get; set; }
    public double TotalTemperature { get; set; }
    public long Count { get; set; }
    
    public override string ToString() =>
        $"Min: {MinTemperature}, Max: {MaxTemperature}, Avg: {TotalTemperature / Count}, Count: {Count}";
}