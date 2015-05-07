using System;

namespace OneNorth.SocialSort.GoogleAnalytics
{
    public interface IService
    {
        int GetPageViews(Guid id);
        int GetSocialInteractions(Guid id);
    }
}
