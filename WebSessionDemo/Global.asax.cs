using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Castle.Windsor;

namespace WebSessionDemo
{
	public class MvcApplication : System.Web.HttpApplication
	{
		private static WindsorContainer _container;
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			_container = new WindsorContainer();
			WebComponentsConfig.ConfigureContainer(_container);

		}

		protected void Session_Start()
		{
			// ensure username cookie is expired if it still exists
			_container.Resolve<ICookieProvider>().RemoveUsername();

			// If user is authenticated and new session is started it means that the user hasn't used application
			// for the last N minutes. We need to reautenticate him via login page.
			var currentUser = _container.Resolve<ICurrentUser>();
			if (!currentUser.IsAuthenticated)
				return;

			currentUser.SignOut();
			if (new HttpRequestWrapper(HttpContext.Current.Request).IsAjaxRequest())
			{
				HttpContext.Current.Response.Write("{\"Unauthorized\":\"true\"}");
				HttpContext.Current.Response.ContentType = "application/json";
			}
			else
			{
				FormsAuthentication.RedirectToLoginPage();
			}

			HttpContext.Current.Response.EndIfStillConnected();
		}
	}

	public static class HttpContextExtensions
	{
		public static void EndIfStillConnected(this HttpResponse response)
		{
			// In order not to receive "The remote host closed the connection" errors, let's check if client is still connected before 
			// sending him the content. Otherwise this error will occur.
			if (response.IsClientConnected)
				response.End();
		}
	}

	public interface ICookieProvider
	{
		void AddHttpOnlyCookie(string name, string value);
		void AddCookie(string name, string value);
		void SetCookieAsExpired(HttpCookie cookie);
		void SetSourceParamsCookieAsExpired();
		void SetSourceCookieValue(string source);
		string GetSourceCookieValue();
		void SetSourceCookieAsExpired();
		void RememberUsername(string username);
		string GetRememberedUsername();
		void RemoveUsername();
		IEnumerable<HttpCookie> GetAll();
		bool IsShowDaysLeftPopupOnCallReportSetInSessionSettingsCookie();
		void SetShowDaysLeftPopupOnCallReportInSessionSettingsCookie();
		HttpCookie GetCookie(string name);
		bool IsSetCookieAsExpired(string cookieName);
	}

	// this class has flaw in its design as it contains complex logic of serializing/deserializing objects to cookies. There should be external logic 
	// that handles this and this class should be pure abstraction over cookies - so that it does not require unit tests. 
	// However this class has few of them. This indicates that it has a logic that shouldn't be here.
	// ReSharper disable ClassNeverInstantiated.Global
	public class CookieProvider : ICookieProvider
	// ReSharper restore ClassNeverInstantiated.Global
	{
		private const string SOURCE_PARAMS_COOKIE_NAME = "source-params";
		private const string SOURCE_COOKIE_NAME = "source";
		private const string REMEMBERED_USERNAME_COOKIE_NAME = "rUsername";
		private const string SESSION_SETTINGS_COOKIE_NAME = "SessionSettings";
		private const string SHOW_DAYS_LEFT_POPUP_ON_CALL_REPORT_SETTED_VALUE = "showDaysLeftPopupOnCallReport=true";
		// ReSharper disable once InconsistentNaming
		private static readonly string[] VALID_COOKIES_PATHS = { "/" };

		public virtual void AddHttpOnlyCookie(string name, string value)
		{
			var httpCookie = new HttpCookie(name, value)
			{
				HttpOnly = true
			};

			HttpContext.Current.Response.Cookies.Add(httpCookie);
		}

		public virtual void AddCookie(string name, string value)
		{
			var httpCookie = new HttpCookie(name, value)
			{
				HttpOnly = false
			};

			HttpContext.Current.Response.Cookies.Add(httpCookie);
		}

		public virtual HttpCookie GetCookie(string name)
		{
			return HttpContext.Current.Request.Cookies[name];
		}

		protected virtual string GetCookieValue(string name)
		{
			var cookie = HttpContext.Current.Request.Cookies[name];
			return cookie != null ? cookie.Value : null;
		}

		protected virtual void SetCookieAsExpired(string name)
		{
			// How to correctly delete cookies by: http://msdn.microsoft.com/en-us/library/ms178195.aspx
			if (HttpContext.Current.Request.Cookies[name] != null)
			{
				var httpCookie = new HttpCookie(name)
				{
					Expires = DateTime.Now.AddDays(-1)
				};
				HttpContext.Current.Response.SetCookie(httpCookie);
			}
		}

		public virtual void SetCookieAsExpired(HttpCookie cookie)
		{
			if (VALID_COOKIES_PATHS.Contains(cookie.Path))
			{
				var httpCookie = new HttpCookie(cookie.Name)
				{
					Expires = DateTime.Now.AddDays(-1),
					Path = cookie.Path
				};
				HttpContext.Current.Response.SetCookie(httpCookie);
			}
		}
		

		public void SetSourceParamsCookieAsExpired()
		{
			SetCookieAsExpired(SOURCE_PARAMS_COOKIE_NAME);
		}

		public void SetSourceCookieValue(string source)
		{
			AddCookie(SOURCE_COOKIE_NAME, source);
		}

		public string GetSourceCookieValue()
		{
			return GetCookieValue(SOURCE_COOKIE_NAME);
		}

		public void SetSourceCookieAsExpired()
		{
			SetCookieAsExpired(SOURCE_COOKIE_NAME);
		}

		public void RememberUsername(string username)
		{
			var httpCookie = new HttpCookie(REMEMBERED_USERNAME_COOKIE_NAME, username)
			{
				Expires = DateTime.Now.AddYears(20),
				HttpOnly = true
			};
			HttpContext.Current.Response.Cookies.Add(httpCookie);
		}

		public string GetRememberedUsername()
		{
			HttpCookie cookieWithName = GetCookie(REMEMBERED_USERNAME_COOKIE_NAME);

			return cookieWithName != null ? cookieWithName.Value : string.Empty;
		}

		public void RemoveUsername()
		{
			SetCookieAsExpired(REMEMBERED_USERNAME_COOKIE_NAME);
		}

		public IEnumerable<HttpCookie> GetAll()
		{
			for (int i = 0; i < HttpContext.Current.Request.Cookies.Count; i++)
			{
				yield return HttpContext.Current.Request.Cookies.Get(i);
			}
		}

		public bool IsShowDaysLeftPopupOnCallReportSetInSessionSettingsCookie()
		{
			var sessionSettingsCookieValue = GetCookieValue(SESSION_SETTINGS_COOKIE_NAME);
			return sessionSettingsCookieValue != null &&
				   sessionSettingsCookieValue.Contains(SHOW_DAYS_LEFT_POPUP_ON_CALL_REPORT_SETTED_VALUE);
		}

		public void SetShowDaysLeftPopupOnCallReportInSessionSettingsCookie()
		{
			AddHttpOnlyCookie(SESSION_SETTINGS_COOKIE_NAME, SHOW_DAYS_LEFT_POPUP_ON_CALL_REPORT_SETTED_VALUE);
		}

		public bool IsSetCookieAsExpired(string cookieName)
		{
			bool isSetExpired = false;

			if (GetCookie(cookieName) != null)
			{
				SetCookieAsExpired(new HttpCookie(cookieName));
				isSetExpired = true;
			}

			return isSetExpired;
		}
	}
}
