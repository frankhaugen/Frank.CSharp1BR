// See https://aka.ms/new-console-template for more information

using Frank.CSharp1BR.DataGenerator;

Console.WriteLine("Generating data...");

var settings = new DataGeneratorSettings
{
    FilePath = "../../../../data.csv",
    DateRange = new DateTimeRange(new DateTime(2010, 1, 1), new DateTime(2019, 12, 31)),
    MaxNumberOfLines = 1_000_000_000,
    Interval = TimeSpan.FromSeconds(1)
};

var service = new DataGenerationService(settings);
await service.GenerateDataAsync(CancellationToken.None);