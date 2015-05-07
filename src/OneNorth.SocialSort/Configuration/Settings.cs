namespace OneNorth.SocialSort.Configuration
{
    public class Settings : ISettings
    {
        public string GoogleAnalyticsProfileId
        {
            get { return Sitecore.Configuration.Settings.GetSetting("OneNorth.SocialSort.GoogleAnalytics.ProfileId", ""); }
        }

        public string GoogleAnalyticsServiceAccountEmail
        {
            get { return Sitecore.Configuration.Settings.GetSetting("OneNorth.SocialSort.GoogleAnalytics.ServiceAccountEmail", ""); }
        }

        public string GoogleAnalyticsKeyFilePath
        {
            get { return Sitecore.Configuration.Settings.GetSetting("OneNorth.SocialSort.GoogleAnalytics.KeyFilePath", ""); }
        }

        public int WindowInDays
        {
            get { return Sitecore.Configuration.Settings.GetIntSetting("OneNorth.SocialSort.WindowInDays", 30); }
        }
    }
}