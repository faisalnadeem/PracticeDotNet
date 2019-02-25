using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebSessionDemo.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
		
		public ActionResult TestHostName()
		{
			var urlToTest = new Uri("https://rebrand.noddle.cig.local/");
			var currentHostname = urlToTest.Host;
			var configHostName = "rebrand.noddle.cig.local";

			var isok = currentHostname.Contains(configHostName, StringComparison.OrdinalIgnoreCase);
			return View();
		}

		public async Task<ActionResult> TestIndexAsync()
		{
			await Task.Delay(TimeSpan.FromSeconds(5));
			return View("TestIndexAsync");
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}