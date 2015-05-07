using System;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace OneNorth.SocialSort.GoogleAnalytics
{
    public interface IService
    {
        int GetPageViews(Item item);
        int GetPageViews(ID id);
        int GetPageViews(Guid id);
        int GetSocialInteractions(Item item);
        int GetSocialInteractions(ID id);
        int GetSocialInteractions(Guid id);
    }
}
