using System;
using System.Collections.Generic;

namespace OneNorth.SocialSort.GoogleAnalytics
{
    internal class Cache
    {
        private static Dictionary<Guid, Metrics> _metrics;

        static Cache()
        {
            _metrics = new Dictionary<Guid, Metrics>();
        }

        internal static void CacheMetrics(Dictionary<Guid, Metrics> metrics)
        {
            // This is atomic and we dont need locking.
            _metrics = metrics;
        }

        public static int GetPageViews(Guid id)
        {
            Metrics metrics;
            return (_metrics.TryGetValue(id, out metrics)) ? metrics.PageViews : 0;
        }

        public static int GetSocialInteractions(Guid id)
        {
            Metrics metrics;
            return (_metrics.TryGetValue(id, out metrics)) ? metrics.SocialInteractions : 0;
        }
    }
}