using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebSessionDemo.Models;

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

		public ActionResult TestCrossDomainCookie()
		{
			var model = new CrossDomainCookieViewModel() {Name = "XDomainCookieVm", Description = "XDomainCookieVmDesc"};

			return View(model);
		}
		
		[HttpPost]
		public ActionResult TestCrossDomainCookie(CrossDomainCookieViewModel model)
		{
			string cookieName = "cookiename";
			string uid = "uidddd";
			string pwd = "peessleeiisl";
			string cookiePath = "pathatest";
			string cookieDomain = ".noddle.co.uk";
			string cookieSecure = "true";

			Response.AddHeader("Set-Cookie",
				$"{cookieName}={uid}|{pwd};path={cookiePath}domain={cookieDomain}secure={cookieSecure}");

			//var httpCookie = new HttpCookie("xdomain-cookie", model.Description)
			//{
			//	Domain = ".noddle.co.uk",
			//	HttpOnly = true,
			//	Expires = DateTime.Now.AddHours(1)
			//};

			//Response.Cookies.Add(httpCookie);
			//HttpContext.Current.Response.Cookies.Add(httpCookie);
			return RedirectPermanent("https://www.noddle.co.uk/account/sign-in");
			//return View(model);
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