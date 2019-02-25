using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSessionDemo
{
	public interface ICurrentUser
	{
		IUserCredentials Credentials { get; }

		DateTime LastLoginDate { get; }
		bool IsAuthenticated { get; }
		bool IsPasswordResetForced { get; }
		bool IsPasswordResetForcedByAdmin { get; }
		void SignIn(string ticketUsername);
		void SignOut();
		void DestroySessionAndSessionCookieIfExists();
	}

	public interface IUserCredentials
	{
		int? AdminSecurityLevel { get; }
		int Id { get; }
		bool IsAdmin { get; }

	}

	public interface IWebSession
	{
		void Set<T>(T value) where T : class;

		T Get<T>(bool failIfNull = false) where T : class;

		bool IsAvailable();

		void Abandon();

		void DestroySessionCookie();

		string SessionId { get; }
	}
}