namespace Core.Enums;

public static class WaterFrequencyExtensions
{
    public static int GetFrequencyDays(this WaterFrequency frequency)
    { 
        switch (frequency)
        {
            case WaterFrequency.Daily:
                return 1;
            case WaterFrequency.EveryTwoDays:
                return 2;
            case WaterFrequency.TwiceAWeek:
                return 3;
            case WaterFrequency.Weekly:
            default:
                return 7;
            case WaterFrequency.EveryTwoWeeks:
                return 14;
            case WaterFrequency.Monthly:
                return 30;
        };
    }
}
