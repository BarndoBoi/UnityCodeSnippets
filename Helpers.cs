using System;
public static class Helpers
{
    public static int GenerateRandomRange(int minValue, int maxValue)
    {
        // Simulate a random range function using System.Random
        Random random = new Random();
        return random.Next(minValue, maxValue);
    }

    public static float GenerateRandomRange(double minValue, double maxValue)
    {
        Random random = new Random();
        return (float)(minValue + (maxValue - minValue) * random.NextDouble());
    }
}