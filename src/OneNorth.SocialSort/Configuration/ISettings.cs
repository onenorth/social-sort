namespace OneNorth.SocialSort.Configuration
{
    public interface ISettings
    {
        string GoogleAnalyticsProfileId { get; }
        string GoogleAnalyticsServiceAccountEmail { get; }
        string GoogleAnalyticsKeyFilePath { get; }
        int WindowInDays { get; }
    }
}
