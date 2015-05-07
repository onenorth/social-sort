using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using OneNorth.SocialSort.Configuration;

namespace OneNorth.SocialSort.GoogleAnalytics
{
    public class Manager : IManager
    {
        private readonly ISettings _settings;

        public Manager() : this(new Settings())
        {
            
        }

        internal Manager(ISettings settings)
        {
            _settings = settings;
        }

        public void UpdateCaches()
        {
            var service = GetService();
            var request = GetRequest(service);
            var metrics = Execute(request);
            Cache.CacheMetrics(metrics);
        }

        private AnalyticsService GetService()
        {
            // Google Analytics API Service Account Authentication 
            var keyFilePath = _settings.GoogleAnalyticsKeyFilePath;    // found in developer console under APIs & auth / Credentials
            var serviceAccountEmail = _settings.GoogleAnalyticsServiceAccountEmail;  // found in developer console under APIs & auth / Credentials
            var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.Exportable); // notasecret is the standard password for the key file.
            var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
            {
                Scopes = new[] { AnalyticsService.Scope.AnalyticsReadonly }
            }.FromCertificate(certificate));

            // Google Analytics Service
            var service = new AnalyticsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "OneNorth.SocialSort", // This can be whatever you want
            });

            return service;
        }

        private DataResource.GaResource.GetRequest GetRequest(AnalyticsService service)
        {
            // format the profile id
            var profileId = _settings.GoogleAnalyticsProfileId;
            if (!profileId.Contains("ga:"))
                profileId = string.Format("ga:{0}", profileId);

            var window = _settings.WindowInDays;
            var startDate = DateTime.Now.AddDays(-window);
            var endDate = DateTime.Now;

            var request = service.Data.Ga.Get(profileId, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"), "ga:pageviews,ga:socialInteractions");
            request.Dimensions = "ga:pageTitle";

            return request;
        }

        private Dictionary<Guid, Metrics> Execute(DataResource.GaResource.GetRequest request)
        {
       
            // Retrieve data, performing paging if necessary.
            var metrics = new Dictionary<Guid, Metrics>();
            GaData response = null;
            do
            {
                var startIndex = 1;
                if (response != null && !string.IsNullOrEmpty(response.NextLink))
                {
                    var uri = new Uri(response.NextLink);
                    var paramerters = uri.Query.Split('&');
                    var s = paramerters.First(i => i.Contains("start-index")).Split('=')[1];
                    startIndex = int.Parse(s);
                }

                request.StartIndex = startIndex;
                response = request.Execute();
                ProcessData(response, metrics);

            } while (!string.IsNullOrEmpty(response.NextLink));

            return metrics;
        }

        private void ProcessData(GaData response, Dictionary<Guid, Metrics> metrics)
        {
            var pageTitleIndex = 0;
            var pageViewsIndex = 0;
            var socialInteractionsIndex = 0;

            // Find associated columns
            for (var index = 0; index < response.ColumnHeaders.Count; index++)
            {
                var header = response.ColumnHeaders[index];

                if (string.Equals(header.Name, "ga:pageTitle", StringComparison.OrdinalIgnoreCase))
                    pageTitleIndex = index;
                else if (string.Equals(header.Name, "ga:pageviews", StringComparison.OrdinalIgnoreCase))
                    pageViewsIndex = index;
                else if (string.Equals(header.Name, "ga:socialInteractions", StringComparison.OrdinalIgnoreCase))
                    socialInteractionsIndex = index;
            }

            foreach (var row in response.Rows)
            {
                // Try to get the item id from the page title
                var pageTitle = row[pageTitleIndex];
                var parts = pageTitle.Split('|').Select(x => x.Trim()).ToArray();

                Guid itemId;
                if (!Guid.TryParse(parts.Last(), out itemId))
                    continue;

                // Get page views
                int pageViews;
                if (!int.TryParse(row[pageViewsIndex], out pageViews))
                    pageViews = 0;

                // Get Social Interactions
                int socialInteractions;
                if (!int.TryParse(row[socialInteractionsIndex], out socialInteractions))
                    socialInteractions = 0;

                if (!metrics.ContainsKey(itemId))
                    metrics.Add(itemId, new Metrics { PageViews = pageViews, SocialInteractions = socialInteractions });
                else
                {
                    var entry = metrics[itemId];
                    entry.PageViews += pageViews;
                    entry.SocialInteractions += socialInteractions;
                }
            }
        }
    }
}