using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace WebSessionDemo.Controllers
{
    public class RedirectTestController : Controller
    {
        // GET: RedirectTest
        public ActionResult From()
        {
            return View();
        }
        public ActionResult To()
        {
	        string referrer = Request.UrlReferrer.ToString();
			return View();
        }
    }
}