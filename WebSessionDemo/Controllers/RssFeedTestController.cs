using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace WebSessionDemo.Controllers
{
    public class RssFeedTestController : Controller
    {
        // GET: RSSFeedTest
        public ActionResult Index()
        {
		    var parliamentNewUrsl = "https://www.parliament.uk/g/RSS/news-feed/?pageInstanceId=209&limit=20";
	        var newsFeed = GetAsync(parliamentNewUrsl);

			//	        var feed = new RssFeedTestViewModel
			//	        {
			//				Channel = new List<RssChannelItem>
			//				{
			//					new RssChannelItem{Title = }
			//				}
			//,
			//	        };
			return View();
        }

	    static async Task<RssFeedTestViewModel> GetAsync(string path)
	    {
		    RssFeedTestViewModel feedAsync = null;
		    var httpClient = new HttpClient();
		    httpClient.DefaultRequestHeaders.Accept.Clear();
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

		    HttpResponseMessage response = await httpClient.GetAsync(path);
		    if (response.IsSuccessStatusCode)
		    {
			    feedAsync = await response.Content.ReadAsAsync<RssFeedTestViewModel>();
		    }
		    return feedAsync;
	    }
	}

	public class RssFeedTestViewModel
	{
		public IEnumerable<RssChannelItem> Item { get; set; }
	}

	public class RssChannelItem
	{
		public string Title { get; set; }
	}
}