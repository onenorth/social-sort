using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OneNorth.SocialSort.GoogleAnalytics;
using Settings = OneNorth.SocialSort.Test.Mocks.Settings;

namespace OneNorth.SocialSort.Test.GoogleAnalytics
{
    [TestFixture]
    public class IntegrationTest
    {
        /// <summary>
        /// This test case is primarily use to step thought the code manually.
        /// Use Test1.html and Test2.html to register page views in Google Analytics
        /// </summary>
        [Test]
        public void Run()
        {
            var settings = Settings.Load();

            var manager = new Manager(settings);
            var service = new Service();

            var testGuid1 = new Guid("{4e8d0e01-15a1-47da-ab59-57c704caf1ed}");
            var testGuid2 = new Guid("{a3c4194f-3a67-456e-beb9-73250cf44f08}");

            Assert.AreEqual(0, service.GetSocialInteractions(testGuid1));
            Assert.AreEqual(0, service.GetPageViews(testGuid1));

            manager.UpdateCaches();

            var socialInteractions = service.GetSocialInteractions(testGuid1);
            Assert.Greater(socialInteractions, 0);

            var pageViews = service.GetPageViews(testGuid1);
            Assert.Greater(pageViews, 0);

            var guids = new List<Guid> {testGuid1, testGuid2};

            var sorted = guids.OrderBy(service.GetPageViews);
        }
    }
}
