using System;

namespace OneNorth.SocialSort.GoogleAnalytics
{
    public class Service : IService
    {
        public int GetPageViews(Guid id)
        {
            return Cache.GetPageViews(id);
        }

        public int GetSocialInteractions(Guid id)
        {
            return Cache.GetSocialInteractions(id);
        }
    }
}