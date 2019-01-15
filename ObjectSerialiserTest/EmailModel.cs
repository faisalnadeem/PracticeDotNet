using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ObjectSerialiserTest
{
	public abstract class EmailModel //: ISerializable
	{
		protected EmailModel(IEnumerable<string> recipients, string subject, BrandedAccount? branding)
		{
			Recipients = recipients;
			//Fail.IfArgumentNull(subject, "subject");
			Subject = subject;
			BrandedAccount = branding;
		}

		protected EmailModel(IEnumerable<string> recipients, Func<EmailModel, string> subjectRenderer, BrandedAccount? branding)
		{
			Recipients = recipients;
			//Fail.IfArgumentNull(subjectRenderer, "subjectRendered");
			SubjectRenderer = subjectRenderer;
			BrandedAccount = branding;
		}

		[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
		protected EmailModel()
		{
		}

		public KeyValuePair<string, byte[]>[] Attachments { get; protected set; }
		public IEnumerable<string> Recipients { get; protected set; }
		public Func<EmailModel, string> SubjectRenderer { get; private set; }
		protected int _emailVersion = 1;

		[PublicAPI]
		public string From { get; protected set; }

		[PublicAPI]
		public string To
		{
			get { return Recipients.FirstOrDefault(); }
		}

		[PublicAPI]
		public string Subject { get; protected set; }

		[PublicAPI]
		public string RootUrl { get; protected set; }

		[PublicAPI]
		public string HostName { get; protected set; }

		[PublicAPI]
		public string EnvironmentName { get; protected set; }

		[PublicAPI]
		public string EnvironmentComponent { get; protected set; }

		[PublicAPI]
		public BrandedAccount? BrandedAccount { get; protected set; }

		[PublicAPI]
		public string PageName { get; protected set; }

		[PublicAPI]
		public string PageNameWithPoweredSuffix { get; protected set; }

		[PublicAPI]
		public string AccountName { get; protected set; }

		[PublicAPI]
		public string ThinFileAccountName { get; protected set; }

		[PublicAPI]
		public string ContactEmail { get; protected set; }

		[PublicAPI]
		public string DisputeEmail { get; protected set; }

		[PublicAPI]
		public string NoReplyEmail { get; protected set; }

		[PublicAPI]
		public string PasswordEmail { get; protected set; }

		[PublicAPI]
		public string Signature { get; protected set; }

		[PublicAPI]
		public bool ShowSocialMedia { get; protected set; }

		[PublicAPI]
		public string AlertsProductName { get; protected set; }

		[PublicAPI]
		public string WebWatchProductName { get; protected set; }

		[PublicAPI]
		public int CreditReportRefreshDays { get; protected set; }

		[PublicAPI]
		public string Valediction { get; protected set; }

		[PublicAPI]
		public string EmailType
		{
			get { return GetType().Name.WithoutSuffix("EmailModel"); }
		}

		[PublicAPI]
		public int EmailVersion
		{
			get { return _emailVersion; }
		}

		public virtual string GetTemplateName()
		{
			return GetType().Name.Replace("EmailModel", string.Empty);
		}

		//public EmailModel(SerializationInfo info, StreamingContext context)
		//{
		//	Recipients = (IEnumerable<string>) info.GetValue("recipients", typeof(IEnumerable<string>));
		//	From = (string) info.GetValue("from", typeof(string));
		//}
		

		//public void GetObjectData(SerializationInfo info, StreamingContext context)
		//{
		//	info.AddValue("recipients", Recipients);
		//	info.AddValue("from", From);
			
		//}
	}


	public static class StringExtensions
	{
		/// <exception cref = "ArgumentNullException">When <c>toTrim</c> is null.</exception>
		/// <exception cref = "ArgumentException"><c>ArgumentException</c>.</exception>
		public static string WithoutSuffix(this string toTrim, string suffix)
		{
			if (toTrim == null)
				throw new ArgumentNullException("toTrim", "String to trim the end from was null");

			if (string.IsNullOrEmpty(suffix))
				return toTrim;

			if (!toTrim.EndsWith(suffix))
				throw new ArgumentException(
					String.Format("The: {0} string does not end with: {1}, and so was the intention of the code.", toTrim, suffix));

			return toTrim.Substring(0, toTrim.LastIndexOf(suffix, StringComparison.Ordinal));
		}
	}

	[Conditional("JETBRAINS_ANNOTATIONS")]
	[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
	public sealed class PublicAPIAttribute : Attribute
	{
		public PublicAPIAttribute()
		{
		}

		public PublicAPIAttribute([NotNull] string comment)
		{
			this.Comment = comment;
		}

		public string Comment { get; private set; }
	}

	public class Test1EmailModel : EmailModel
	{
		//model = new NoddleNoAlertNotificationEmailModel("test@testfn.co",
		//	null,
		//	"FirstName",
		//Guid.NewGuid());

	}

	[Serializable]
	public class NoddleNoAlertNotificationEmailModel : EmailModel
	{
		public Guid CustomerId { get; set; }

		public string Name { get; private set; }


		public NoddleNoAlertNotificationEmailModel(string recipient,
			BrandedAccount? branding,
			string name,
			Guid customerId,
			string pageName	= "")
			: base(new[] { recipient }, "Good news from Noddle – you have no new alerts", branding)
		{
			Name = name;
			CustomerId = customerId;
		}

		//[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
		protected NoddleNoAlertNotificationEmailModel()
		{
		}

	}

	public class NoAlertNotificationEmailModel : EmailModel
	{
		public Guid CustomerId { get; set; }

		public string Name { get; private set; }


		public NoAlertNotificationEmailModel(string recipient,
			BrandedAccount? branding,
			string name,
			Guid customerId,
			string pageName	= "")
			: base(new[] { recipient }, "Good news from Noddle – you have no new alerts", branding)
		{
			Name = name;
			CustomerId = customerId;
		}

		//[UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
		protected NoAlertNotificationEmailModel()
		{
		}

	}

	public enum BrandedAccount
	{
		Test1,
		Test2,
		Test3
	}
}