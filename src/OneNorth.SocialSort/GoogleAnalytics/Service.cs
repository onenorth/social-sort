using System;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace OneNorth.SocialSort.GoogleAnalytics
{
    public class Service : IService
    {
        public int GetPageViews(Item item)
        {
            return GetPageViews(item.ID);
        }

        public int GetPageViews(ID id)
        {
            return GetPageViews(id.Guid);
        }

        public int GetPageViews(Guid id)
        {
            return Cache.GetPageViews(id);
        }

        public int GetSocialInteractions(Item item)
        {
            return GetSocialInteractions(item.ID);
        }

        public int GetSocialInteractions(ID id)
        {
            return GetSocialInteractions(id.Guid);
        }

        public int GetSocialInteractions(Guid id)
        {
            return Cache.GetSocialInteractions(id);
        }
    }
}