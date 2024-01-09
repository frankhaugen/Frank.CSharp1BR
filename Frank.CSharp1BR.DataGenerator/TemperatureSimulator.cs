using System.Runtime.CompilerServices;

namespace Frank.CSharp1BR.DataGenerator;

/// <summary>
/// A utility class for simulating temperature based on date and time.
/// </summary>
public static class TemperatureSimulator
{
    /// <summary>
    /// Get the current temperature based on the date and time.
    /// </summary>
    /// <param name="dateTime">The date and time for which to get the temperature.</param>
    /// <returns>The current temperature in degrees Celsius.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double GetTemperature(DateTime dateTime)
    {
        // Base temperature based on month (example for a temperate climate)
        var baseTemp = GetMonthlyBaseTemperature(dateTime.Month);

        // Daily fluctuation using sine wave
        var dailyFluctuation = Math.Sin((dateTime.Hour + dateTime.Minute / 60.0) / 24.0 * Math.PI * 2) * 10; // 10 degrees of fluctuation

        // Random variation
        var randomVariation = Random.Shared.NextDouble() * 4 - 2; // Random value between -2 and 2

        var temperature = baseTemp + dailyFluctuation + randomVariation;
        
        return Math.Round(temperature, 2);
    }
    
    // Simplified average base temperatures per month
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double GetMonthlyBaseTemperature(int month)
    {
        double[] monthlyTemps = {0, 2, 5, 10, 15, 20, 25, 25, 20, 15, 10, 5};
        return monthlyTemps[(month - 1) % 12];
    }
}