using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using WebGrease;

namespace WebSessionDemo
{
	

	public class CurrentUser : ICurrentUser
	{
		private readonly IWebSession _session;
		//private readonly IHttpContextAbstraction _httpContext;

		public CurrentUser(IWebSession session)
		{
			_session = session;
		}

		public IUserCredentials Credentials { get; }
		public DateTime LastLoginDate { get; }
		public bool IsAuthenticated { get; }
		public bool IsPasswordResetForced { get; }
		public bool IsPasswordResetForcedByAdmin { get; }
		public void SignIn(string ticketUsername)
		{
			throw new NotImplementedException();
		}

		public void SignOut()
		{
			throw new NotImplementedException();
		}

		public void DestroySessionAndSessionCookieIfExists()
		{
			throw new NotImplementedException();
		}
	}

	
	public class WebSession : IWebSession
	{
		//private ILog _rootLogger = LogManager.GetLogger("root");
		public void Set<T>(T value) where T : class
		{
			string key = GetKey<T>();
			Set(key, value);
		}

		public T Get<T>(bool failIfNull) where T : class
		{
			string key = GetKey<T>();
			var value = Get<T>(key);
			if (failIfNull)
			{
				//Fail.IfNull(value, "{0} is not stored in session", key);
				throw new Exception(string.Format("{0} is not stored in session", key));
			}
			return value;
		}

		public bool IsAvailable()
		{
			return
				HttpContext.Current != null &&
				HttpContext.Current.Session != null &&
				HttpContext.Current.Items.Contains("sessionWasAbandoned") == false;
		}

		public void Abandon()
		{
			HttpContext.Current.Session.Abandon();

			// make sure that any further calls to session IsAvailable during current request, will return false
			HttpContext.Current.Items["sessionWasAbandoned"] = true;
		}

		public void DestroySessionCookie()
		{
			if (HttpContext.Current.Request.Cookies["C2"] != null)
			{
				//_rootLogger.DebugFormat($"Destroying session cookie. Stack trace: {new StackTrace()}");
				HttpContext.Current.Response.Cookies["C2"].Expires = new DateTime(1999, 11, 11);
			}

			if (HttpContext.Current.Request.Cookies["source"] != null &&
				HttpContext.Current.Request.Cookies["source"].Value.Equals("noddle"))
			{
				HttpContext.Current.Response.Cookies["source"].Expires = new DateTime(1999, 11, 11);
			}
		}

		public string SessionId
		{
			get { return HttpContext.Current.Session.SessionID; }
		}

		private void Set<T>(string key, T value) where T : class
		{
			HttpContext.Current.Session[key] = value;
		}

		private T Get<T>(string key) where T : class
		{
			object value = HttpContext.Current.Session[key];
			//Fail.IfNotCastable<T>(value);
			return (T)value;
		}

		private string GetKey<T>() where T : class
		{
			return typeof(T).FullName;
		}
	}
}