using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;

namespace WebSessionDemo.Controllers
{
    public class SyndicationTestController : Controller
    {
		// GET: SyndicationTest
		public ActionResult Index()
        {
			//https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.syndication.syndicationcategory?view=netframework-4.7.2
	        SyndicationFeed myFeed = new SyndicationFeed("My Test Feed",
		        "This is a test feed",
		        new Uri("http://FeedServer/Test"), "MyFeedId", DateTime.Now);
	        SyndicationItem myItem = new SyndicationItem("Item One Title",
		        "Item One Content",
		        new Uri("http://FeedServer/Test/ItemOne"));
	        myItem.Categories.Add(new SyndicationCategory("MyCategory"));
	        Collection<SyndicationItem> items = new Collection<SyndicationItem>();
	        items.Add(myItem);
	        myFeed.Items = items;
			return View(myFeed);
        }
    }
}