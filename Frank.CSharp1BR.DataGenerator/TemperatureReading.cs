namespace Frank.CSharp1BR.DataGenerator;

public record struct TemperatureReading(DateTime Date, double Temperature, EuropeanCapital Capital)
{
    public override string ToString() => $"{Temperature:N2};{Capital}";
}