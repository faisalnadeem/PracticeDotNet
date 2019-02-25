using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace WebSessionDemo
{
	public class WebComponentsConfig
	{
		public static void ConfigureContainer(WindsorContainer container)
		{
			container.Register(
				Component.For<ICookieProvider>().ImplementedBy<CookieProvider>(),
				Component.For<IWebSession>().ImplementedBy<WebSession>(),
				Component.For<ICurrentUser>().ImplementedBy<CurrentUser>()
			);

		}
	}
}