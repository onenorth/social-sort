
# Overview
This module supports sorting Sitecore items by popularity using Google Analytics.  Items can be sorted by the number of page views and/or by the number of [social interactions](https://developers.google.com/analytics/devguides/platform/social-interactions-overview).  The data provided by this module can be applied to the results of Lucene searches, fast queries and other lists regardless of source.

![Sort By](https://raw.githubusercontent.com/onenorth/social-sort/master/img/SortBy.png)

The module provides an API to access the total number of views and social interactions for each item publicly displayed on a Sitecore site.

Coming Soon: See the associated blog post at [onenorth.com/blog](http://www.onenorth.com/blog/#/grid) for the reasons and thoughts behind creating this module.

# Google Analytics
Google Analytics is used to provide the data for the number of page views and social interactions.

## Account Setup
If you don't already have a Google Analytics account for your site, you will need to set one up using the following instructions: https://support.google.com/analytics/answer/1008015?hl=en

Second, we will need to configure access to the Google Analytics API.  We will do this through the Google Developers Console.

 1. Enable API Access
	 1. Login to https://console.developers.google.com/project
	 1. Select an existing or create a new project.
	 1. Navigate to **APIs & auth** / **APIs** in the left navigation.
	 1. Find and enable the **Analytics API**.
 1. Setup credentials
	 2.  Navigate to **APIs & auth** / **Credentials**
	 3. Under **OAuth**, click **Create new Client ID**
	 4. Select **Service Account**.
	 5. Click **Create Client ID**
	 6. The JSON key should download automatically.
	 7. In our case we need the P12 key.  Click **Generate new P12 key**.
	 8. The .P12 file should download automatically.
		 9. Notice the private key's password is **notasecret**
	 10. Make note of the Service Account Email address for later use.

Now we need to configure the new Service Account to have access to Google Analytics.

 1. Login to https://www.google.com/analytics
 2. Click the **Admin** menu item.
 3. Select the analytics account you would like to associate with the new service account with.
 4. Click on **User Management**
 5. Enter the email address for the service account that was just created in the **Add permissions for** field.
 6. Select Read & Analyze as the access level as we don't need the other permissions.
 7. Click **Add**.

## Add the Page View Tracking Code

The next step is to add the Google Analytics tracking code to the pages within the Sitecore Site.  This allows Google Analytics to track the visits to each page.  To get the tracking code:

 1. Navigate to https://www.google.com/analytics and sign in.  
 2. Click on the **Admin** menu item in the top menu bar.
 3. Select the appropriate **ACCOUNT** and **PROPERTY** in the dropdowns.
 4. Click on **.js Tracking Info** then **Tracking Code** in the PROPERTY column.

You should see something similar to the following:

    <script>
		(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
	  (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
		m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
		})(window,document,'script','//www.google-analytics.com/analytics.js','ga');

		ga('create', 'UA-XXXXX-Y', 'auto');
		ga('send', 'pageview');

	</script>

This code can be pasted onto every webpage you want to track.  Before you start pasting this code into your renderings we need to make one change.  We want to track each time an item is viewed.  The best way to do this is to tell Google Analytics the ID of the item you are viewing.  We can add the ID of the item being viewed to the the script.  In our case we are going to append the Item ID to the title of the page. 

	ga('set', 'title', document.title + ' | {4e8d0e01-15a1-47da-ab59-57c704caf1ed}');

The GUID in the above sample is the Item ID.  This Item ID should be replaced with the ID of the Item currently being viewed. 

> The approach for getting the Item ID varies based on MVC, Web Forms, xDB, and/or the implementation.  

The complete Tracking Code is as follows:

    <script>
		(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
	  (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
		m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
		})(window,document,'script','//www.google-analytics.com/analytics.js','ga');

		ga('create', 'UA-XXXXX-Y', 'auto');
		ga('set', 'title', document.title + ' | {4e8d0e01-15a1-47da-ab59-57c704caf1ed}');
		ga('send', 'pageview');

	</script>

Alternate examples of the Tracking Code are [here](https://github.com/onenorth/social-sort/blob/master/src/OneNorth.SocialSort.Test/Test1.html) and [here](https://github.com/onenorth/social-sort/blob/master/src/OneNorth.SocialSort.Test/Test2.html).  This code is used for the unit tests and uses a newer pattern.

Once the tracking code is added.  You can navigate the site to increase the view counts within Google Analytics.

## Add the Social Interactions Tracking Code

Adding the Tracking Code to handle social interactions is documented here: https://developers.google.com/analytics/devguides/collection/analyticsjs/social-interactions.  This code is dependent on how the social interactions are implemented.

Adding the following code to a button click event will register a social interaction when clicked.  The code from above needs to be present on the page to properly register the associated title and item ID with the social interaction.

    ga('send', 'social', 'facebook', 'like', window.location.href);

# OneNorth.SocialSort API

The OneNorth.SocialSort API provides an easy and performant way to obtain the number of page views and social interactions a Sitecore Item has.  The API uses caching to improve the performance of looking up the data.  The cache is refreshed once an hour.  The SocialSort API makes use of Sitecore Scheduling to run a task that queries for the view and social data from Google Analytics.  This is done once an hour.  

The OneNorth.SocialSort API has an associated assembly and configuration file.  The assembly and configuration file can be downloaded from here: https://github.com/onenorth/social-sort/tree/master/release.  This content can just be xcopied into your Sitecore instance.

## OneNorth.SocialSort.Config

The configuration file is located at \App_Config\Include\OneNorth.SocialSort.Config.  Out of the box, the configuration file looks like this:

    ﻿<?xml version="1.0" encoding="utf-8"?>
	<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
	  <sitecore>
	    <settings>
	      <setting name="OneNorth.SocialSort.GoogleAnalytics.ProfileId" value="" />
	      <setting name="OneNorth.SocialSort.GoogleAnalytics.ServiceAccountEmail" value="" />
	      <setting name="OneNorth.SocialSort.GoogleAnalytics.KeyFilePath" value="" />
	      <setting name="OneNorth.SocialSort.WindowInDays" value="30" />
	    </settings>
	  </sitecore>
	  <scheduling>
	    <agent type="OneNorth.SocialSort.GoogleAnalytics.Task, OneNorth.SocialSort" method="Run" interval="01:00:00" />
	  </scheduling>
	</configuration>

In order for the SocialSort API to connect to Google Analytics, we need to provide the appropriate configuration values above.  The settings are their values are described here:

 - **OneNorth.SocialSort.GoogleAnalytics.ProfileId** - This is the View ID from Google Analytics.  The View ID can be obtained from the *Admin* menu in https://www.google.com/analytics.  Navigate to *ACCOUNT* > *PROPERTY* > *VIEW* / *View Settings*.  You will see the View ID under *Reporting View Settings* / *Basic Settings*
 - **OneNorth.SocialSort.GoogleAnalytics.ServiceAccountEmail** - This is the service account email address that was generated when configuring the Service Account.  You should have made note of this from above.
 - **OneNorth.SocialSort.GoogleAnalytics.KeyFilePath** - This is the location of the .p12 file you downloaded when configuring the Service Account.  This is the physical location on disk.
 - **OneNorth.SocialSort.WindowInDays** - This is how big of a window in days to retrieve Google Analytics data for.  By default this is set to 30 days.

The scheduing agent is set to run the **OneNorth.SocialSort.GoogleAnalytics.Task** once every hour.  You can change the time value if you feel once an hour is not appropriate.

Once the above configuration changes have been made, the API is ready to use.

## API

The API was designed to be fairly easy to use.  The primary namespace for using the API is **OneNorth.SocialSort.GoogleAnalytics**.  To obtain the number of page views and social interactions for an Item, you can do the following:

    var service = new OneNorth.SocialSort.GoogleAnalytics.Service();

	// Obtain the number of page views for an item
	var pageViews = service.GetPageViews(<Item>);
    
    // Obtain the number of social interactions for an item
    var socialInteractions = service.GetSocialInteractions(<Item>);

As you can see, it is very simple to get the number of page views and social interactions for an item.  These methods pull from cache and therefore perform well.  We can expand this example to sorting a list of Sitecore Items based on the number of views.

    var service = new OneNorth.SocialSort.GoogleAnalytics.Service();
    
    List<Item> items = <Get list of items from somewhere>;
    var sortedItems = items.OrderBy(service.GetPageViews);
    
From the above example, you can take any list of items.  This also allows sorting of results from Lucene queries and fast queries.

> Note: In the example above, the sorting is performed after the results of a query are returned.  If there are pages worth of results, it will perform better if the sort is performed on the IDs/GUIDs before the items are actually retrieved.  The API allows sorting based on ID and GUID also.  Here are the available signatures:

    int GetPageViews(Item item);
    int GetPageViews(ID id);
    int GetPageViews(Guid id);
    int GetSocialInteractions(Item item);
    int GetSocialInteractions(ID id);
    int GetSocialInteractions(Guid id);

  
#License
The associated code is released under the terms of the [MIT license](http://onenorth.mit-license.org).
