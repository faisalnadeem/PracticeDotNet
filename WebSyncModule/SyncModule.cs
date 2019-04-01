using System;
using System.Web;
using System.Web.ModelBinding;

namespace WebSyncModule
{
	//https://support.microsoft.com/en-gb/help/307996/how-to-create-an-asp-net-http-module-using-visual-c-net
	public class SyncModule: IHttpModule
    {
	    public void Init(HttpApplication context)
	    {
			context.BeginRequest += ContextBeginRequest;
		}

	    public void Dispose()
	    {		    
	    }

	    private static void ContextBeginRequest(object sender, EventArgs e)
	    {
		    var application = sender as HttpApplication;
		    if (application == null)
			    return;
		    //var togglesAppSettings = ServiceLocator.GetInstance<IFeatureTogglesAppSettings>();
		    //var appSettings = ServiceLocator.GetInstance<IAppSettings>();
		    var host = application.Request.ServerVariables["HTTP_HOST"].ToLower();
		    bool togglesAppSettingsRedirectToCK= true;
		    string appSettingsNoddleHostName = "noddle.co.uk"; 
		    if (togglesAppSettingsRedirectToCK && host.Contains(appSettingsNoddleHostName.ToLower()))
		    {
			    var cookieProvider = new CustomCookieProvider();//ServiceLocator.GetInstance<ICookieProvider>();
			    var path = application.Request.ServerVariables["URL"];
			    if (cookieProvider.NoRedirectCookieExists() || path.ToLower().Contains("/partners") ||
			        path.ToLower().Contains("/source="))
				    return;
			    var creditKarmaUrlProvider = new CustomCkUrlProvider();
			    var queryString = application.Request.ServerVariables["QUERY_STRING"];
			    var creditKarmaRedirectUrl = creditKarmaUrlProvider.GetUrl(path, queryString);
			    application.Context.Response.Redirect(creditKarmaRedirectUrl.ToString());
		    }
	    }
	}

	internal class CustomCookieProvider
	{
		public bool NoRedirectCookieExists()
		{
			return false;
		}
	}

	public class CustomCkUrlProvider
	{
		private string _appSettingsDomainName = "";

		public Uri GetUrl(string path, string queryString)
		{
			var uriBuilder = new UriBuilder(new Uri(_appSettingsDomainName));
			var lowerPath = path.ToLower();
			var lowerPathWithoutDashes = lowerPath.Replace("-", "");
			if (lowerPath.Contains("/signin"))
			{
				uriBuilder.Path = "/sign-in";
			}
			else if (lowerPath.Contains("/comparison/loans/results/loan/apply") ||
			         lowerPath.Contains("/comparison/loans/wizard/yourstatus"))
			{
				uriBuilder.Path = "/financial-products/loans";
			}
			else if (lowerPath.Contains("/comparison/loans/results"))
			{
				uriBuilder.Path = "/loans";
			}
			else
			{
				uriBuilder.Path = path;
			}

			var REDIRECTED_FROM_NODDLE_QUERY_STRING = "redirectedFromNoddle";
			uriBuilder.Query = string.IsNullOrEmpty(queryString) || queryString.ToLower() == "bid"
				? REDIRECTED_FROM_NODDLE_QUERY_STRING
				: $"{queryString}&{REDIRECTED_FROM_NODDLE_QUERY_STRING}";

			return uriBuilder.Uri;
		}
	}
}
