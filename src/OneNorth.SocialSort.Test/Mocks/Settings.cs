using System.Configuration;
using OneNorth.SocialSort.Configuration;

namespace OneNorth.SocialSort.Test.Mocks
{
    public class Settings : ISettings
    {
        public static ISettings Load()
        {
            return new Settings
                {
                    GoogleAnalyticsProfileId = ConfigurationManager.AppSettings["OneNorth.SocialSort.GoogleAnalytics.ProfileId"],
                    GoogleAnalyticsServiceAccountEmail = ConfigurationManager.AppSettings["OneNorth.SocialSort.GoogleAnalytics.ServiceAccountEmail"],
                    GoogleAnalyticsKeyFilePath = ConfigurationManager.AppSettings["OneNorth.SocialSort.GoogleAnalytics.KeyFilePath"],
                    WindowInDays = int.Parse(ConfigurationManager.AppSettings["OneNorth.SocialSort.WindowInDays"])
                };
        }

        public string GoogleAnalyticsProfileId { get; set; }
        public string GoogleAnalyticsServiceAccountEmail { get; set; }
        public string GoogleAnalyticsKeyFilePath { get; set; }
        public int WindowInDays { get; set; }
    }
}
