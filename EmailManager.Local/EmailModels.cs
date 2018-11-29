using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GenerateTestUsers;
using InMemoryFileDemo;
using JetBrains.Annotations;

namespace Yellow.ApplicationServices.Infrastructure.Mail
{
// ReSharper disable InconsistentNaming
    public abstract class EmailModel
    {
        protected EmailModel(IEnumerable<string> recipients, string subject, BrandedAccount? branding)
        {
            Recipients = recipients;
            Fail.IfArgumentNull(subject, "subject");
            Subject = subject;
            BrandedAccount = branding;
        }

        protected EmailModel(IEnumerable<string> recipients, Func<EmailModel, string> subjectRenderer, BrandedAccount? branding)
        {
            Recipients = recipients;
            Fail.IfArgumentNull(subjectRenderer, "subjectRendered");
            SubjectRenderer = subjectRenderer;
            BrandedAccount = branding;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected EmailModel()
        {
        }

        public KeyValuePair<string, byte[]>[] Attachments { get; protected set; }
        public IEnumerable<string> Recipients { get; private set; }
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
        public string EmailType
        {
            get { return GetType().Name.WithoutSuffix("EmailModel"); }
        }

        [PublicAPI]
        public int EmailVersion
        {
            get { return _emailVersion; }
        }

        public virtual void AddEnvironmentInfo()//IAppSettings appSettings)
        {
            EnvironmentName = "appSettings.EnvironmentName";
            EnvironmentComponent = "appSettings.EnvironmentComponent";

            if (From.IsNullOrEmpty())
            {
                From = "noreply@mail.gu";
            }
            RootUrl = "www.google.co.uk";
            HostName = "HostName"; //"appSettings.GetHostNameBy(BrandedAccount)";
			PageName = "PageName"; //"appSettings.GetPageNameBy(BrandedAccount)";
            PageNameWithPoweredSuffix = "PageNameWithPoweredSuffix"; //"appSettings.GetPageNameWithPoweredSuffixBy(BrandedAccount)";
            AccountName = "AccountName"; //"appSettings.GetAccountNameBy(BrandedAccount)";
            ThinFileAccountName = "ThinFileAccountName"; //appSettings.GetThinFileAccountNameBy(BrandedAccount);
            ContactEmail = "ContactEmail"; //appSettings.GetContactEmailBy(BrandedAccount);
            DisputeEmail = "DisputeEmail"; //appSettings.GetDisputeEmailBy(BrandedAccount);
            NoReplyEmail = "NoReplyEmail"; //appSettings.GetNoReplyEmailBy(BrandedAccount);
	        PasswordEmail = "PasswordEmail";//appSettings.GetPasswordEmailBy(BrandedAccount);
	        Signature = "signature";// appSettings.GetSignatureBy(BrandedAccount);
	        ShowSocialMedia = false; //appSettings.ShouldShowSocialMediaInEmailBy(BrandedAccount);
        }

        public virtual string GetTemplateName()
        {
            return GetType().Name.Replace("EmailModel", string.Empty);
        }
    }

	public abstract class MultiTemplateEmailModel : EmailModel
    {
        public string Template { get; private set; }

        protected MultiTemplateEmailModel(IEnumerable<string> recipients, string subject, string template, BrandedAccount? branding = null)
            : base(recipients, subject, branding)
        {
            Template = template;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected MultiTemplateEmailModel()
        {
        }

        protected abstract IEnumerable<string> ProvideSupportedTemplateNames();

        public IEnumerable<string> GetSupportedTemplateNames()
        {
            var result = new List<string>();
            var baseName = base.GetTemplateName();
            foreach (string template in ProvideSupportedTemplateNames())
            {
                result.Add(baseName + template);
            }

            return result;
        }

        public override string GetTemplateName()
        {
            return base.GetTemplateName() + Template;
        }
    }

    public class ActivateAccountEmailModel : MultiTemplateEmailModel
    {
        public const string TEXT = "_vText";
        public const string GRAPHIC = "_vGraphic";
        public const string HALIFAX_THIN_FILE = "_vHalifaxThinFile";
        public const string THIN_FILE = "_vThinFile";

        public string FirstName { get; private set; }

        public ActivateAccountEmailModel(
            string recipient,
            BrandedAccount? branding,
            int activationLinkValidForHours,
            string activateUrl,
            string templateName,
            string firstName)
            : base(new[] {recipient},
                ProvideSubjectName(branding),
                templateName,
                branding)
        {
            ActivationLinkValidForHours = activationLinkValidForHours;
            ActivateUrl = activateUrl;
            FirstName = firstName;

            HeaderBranding = "HeaderBranding";
        }

        public string HeaderBranding { get; set; }


        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected ActivateAccountEmailModel()
        {
        }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string>
            {
                GRAPHIC,
                TEXT,
                HALIFAX_THIN_FILE,
                THIN_FILE
            };
        }

        private static string ProvideSubjectName(string accountName)
        {
            return string.Format("Activate your {0} now", accountName);
        }

        private static string ProvideSubjectName(BrandedAccount? branding)
        {
	        return "ProvideSubjectName";
			//if (branding.IsAnyHomeAndLegacyBrandedAccount())
			//    return ProvideSubjectName(appSettings.HomeAndLegacyEmailLabel);
			//return ProvideSubjectName(appSettings.GetAccountNameBy(branding));
        }

        public int ActivationLinkValidForHours { get; private set; }
        public string ActivateUrl { get; private set; }
    }

    public class ResetPasswordEmailModel : MultiTemplateEmailModel
    {
        public const string DEFAULT = "";

        public ResetPasswordEmailModel(
            string recipient,
            BrandedAccount? branding,
            string pageName,
            int recoveryTokenValidityTimeInHours,
            string recoveryUrl,
            string templateName,
            string firstName)
            : base(new[] {recipient}, string.Format("Resetting your {0} password", pageName), templateName, branding)
        {
            RecoveryTokenValidityTimeInHours = recoveryTokenValidityTimeInHours;
            RecoveryUrl = recoveryUrl;
            FirstName = firstName;
        }

        public static string GetTemplateName(BrandedAccount? branding)
        {
            return DEFAULT;
        }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string>
            {
                DEFAULT
            };
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected ResetPasswordEmailModel()
        {
        }

        public int RecoveryTokenValidityTimeInHours { get; private set; }
        public string RecoveryUrl { get; private set; }
        public string FirstName { get; private set; }
    }

    public class FraudAlertEmailModel : EmailModel
    {
        public FraudAlertEmailModel(IEnumerable<string> recipients, string keywordFound, int noddleDisputeId)
            : base(recipients, string.Format("Fraud alert - found: '{0}' in dispute no: {1}", keywordFound, noddleDisputeId), null)
        {
            KeywordFound = keywordFound;
            NoddleDisputeId = noddleDisputeId;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected FraudAlertEmailModel()
        {
        }

        public string KeywordFound { get; private set; }
        public int NoddleDisputeId { get; private set; }
    }

    public abstract class NewCreditReportNotification : MultiTemplateEmailModel
    {
        protected NewCreditReportNotification(String recipient, Version version, BrandedAccount? branding)
            : base(new[] {recipient}, GetSubjectForVersion(version), GetTemplateForMagicString(version), branding)
        {
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected NewCreditReportNotification()
        {
        }

        private const string HALIFAX = "_vHalifax";
        private const string DEFAULT = "";

        private static string GetTemplateForMagicString(Version version)
        {
            switch (version)
            {
                case Version.Halifax:
                    return HALIFAX;
                case Version.Default:
                case Version.HomeAndLegacy:
                case Version.SevenDays:
                    return DEFAULT;
                default:
                    throw new InvalidEnumArgumentException("Uncrecognized Credit Report Notification version");
            }
        }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string> {HALIFAX, DEFAULT};
        }

        private static string GetSubjectForVersion(Version version)
        {
            switch (version)
            {
                case Version.Halifax:
                    return "Your new Halifax Credit Checker report is ready";
                case Version.HomeAndLegacy:
                    return "Your new report from Noddle in partnership with Home & Legacy is ready";
                case Version.Default:
                case Version.SevenDays:
                    return "Your new Noddle credit report is ready";
                default:
                    throw new InvalidEnumArgumentException("Uncrecognized Credit Report Notification version");
            }
        }

        public enum Version
        {
            Default, // Monthly for Brands
            Halifax,
            HomeAndLegacy,
            SevenDays
        };
    }
    
    public class FirstCreditReportNotificationEmailModel : NewCreditReportNotification
    {
        public FirstCreditReportNotificationEmailModel(string recipient, string firstName, Version version, BrandedAccount? branding)
            : base(recipient, version, branding)
        {
            FirstName = firstName;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected FirstCreditReportNotificationEmailModel()
        {
        }

        public string FirstName { get; private set; }
    }

    public class FirstCreditReportNotificationSevenDaysEmailModel : EmailModel
    {
        public FirstCreditReportNotificationSevenDaysEmailModel(string recipient, string firstName)
            : base(new List<string> { recipient }, "Your new Noddle credit report is ready", null)
        {
            FirstName = firstName;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected FirstCreditReportNotificationSevenDaysEmailModel()
        {
        }

        public string FirstName { get; private set; }
    }

    public class SecondCreditReportNotificationEmailModel : NewCreditReportNotification
    {
        public SecondCreditReportNotificationEmailModel(string recipient, string firstName, Version version, BrandedAccount? branding)
            : base(recipient, version, branding)
        {
            FirstName = firstName;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected SecondCreditReportNotificationEmailModel()
        {
        }

        public string FirstName { get; private set; }
    }

    public class SecondCreditReportNotificationSevenDaysEmailModel : EmailModel
    {
        public SecondCreditReportNotificationSevenDaysEmailModel(string recipient, string firstName)
            : base(new List<string> { recipient }, "Your new Noddle credit report is ready", null)
        {
            FirstName = firstName;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected SecondCreditReportNotificationSevenDaysEmailModel()
        {
        }

        public string FirstName { get; private set; }
    }

    public class ThirdCreditReportNotificationEmailModel : NewCreditReportNotification
    {
        public ThirdCreditReportNotificationEmailModel(string recipient, string firstName, Version version, BrandedAccount? branding)
            : base(recipient, version, branding)
        {
            FirstName = firstName;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected ThirdCreditReportNotificationEmailModel()
        {
        }

        public string FirstName { get; private set; }
    }

    public class ThirdCreditReportNotificationSevenDaysEmailModel : EmailModel
    {
        public ThirdCreditReportNotificationSevenDaysEmailModel(string recipient, string firstName)
            : base(new List<string> { recipient }, "Your new Noddle credit report is ready", null)
        {
            FirstName = firstName;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected ThirdCreditReportNotificationSevenDaysEmailModel()
        {
        }

        public string FirstName { get; private set; }
    }

    public class RegistrationReminderEmailModel : MultiTemplateEmailModel
    {
        public const string TEXT = "_vText";
        public const string GRAPHIC = "_vGraphic";

        public RegistrationReminderEmailModel(
            string recipient,
            BrandedAccount? branding,
            string accountName,
            int activateAccountTokenValidForHours,
            string templateName,
            string signUpUrl)
            : base(
                new[] {recipient},
                string.Format("Your {0} activation link has expired.", accountName),
                templateName,
                branding)
        {
            ActivateAccountTokenValidForHours = activateAccountTokenValidForHours;
            SignUpUrl = signUpUrl;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected RegistrationReminderEmailModel()
        {
        }

        public static string GetTemplateName(BrandedAccount? branding)
        {
            return branding == null ? GRAPHIC : TEXT;
        }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string>
            {
                GRAPHIC,
                TEXT
            };
        }

        public int ActivateAccountTokenValidForHours { get; private set; }
        public string SignUpUrl { get; set; }
    }

    public class RecoverUsernameEmailModel : MultiTemplateEmailModel
    {
        public const string DEFAULT = "";

        public RecoverUsernameEmailModel(
            string recipient,
            BrandedAccount? branding,
            string pageName,
            int tokenValidForHours,
            string tokenIdentifier,
            string templateName,
            string firstName)
            : base(new[] {recipient}, string.Format("Recover your {0} username", pageName), templateName, branding)
        {
            TokenValidForHours = tokenValidForHours;
            TokenIdentifier = tokenIdentifier;
            FirstName = firstName;
        }

        public static string GetTemplateName(BrandedAccount? branding)
        {
            return DEFAULT;
        }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string>
            {
                DEFAULT
            };
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected RecoverUsernameEmailModel()
        {
        }

        public int TokenValidForHours { get; private set; }
        public string TokenIdentifier { get; private set; }
        public string FirstName { get; private set; }
    }

    public class RejectedDisputeEmailModel : MultiTemplateEmailModel
    {
        public const string THAT_DOES_NOT_BELONG_TO_RECEIVER = "ThatDoesntBelongToReceiver";
        public const string BECAUSE_OF_DUPLICATED_DISPUTE = "BecauseOfDuplicatedDispute";
        public const string BECAUSE_DEFAULTED_ACCOUNT_WITH_INSOLVENCY = "BecauseDefaultedAccountWithInsolvency";
        public const string BECAUSE_REQUIRED_CLOSE_TIME_NOT_PASSED = "BecauseRequiredCloseTimeNotPassed";
        public const string BECAUSE_CREDIT_REPORT_REQUEST_SEARCH = "BecauseCreditReportRequestSearch";
        public const string BECAUSE_ADDRESS_LINK_EXISTS = "BecauseAddressLinkExists";

        public RejectedDisputeEmailModel(
            string recipient,
            int disputeReferenceNo,
            string firstName,
            string from,
            string template,
            BrandedAccount? branding,
            string pageName)
            : base(new[] {recipient}, string.Format("{0} credit report dispute no. {1}", pageName, disputeReferenceNo), template, branding)
        {
            DisputeReferenceNo = disputeReferenceNo;
            FirstName = firstName;
            From = from;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected RejectedDisputeEmailModel()
        {
        }

        public int DisputeReferenceNo { get; private set; }
        public string FirstName { get; private set; }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string>
            {
                THAT_DOES_NOT_BELONG_TO_RECEIVER,
                BECAUSE_OF_DUPLICATED_DISPUTE,
                BECAUSE_DEFAULTED_ACCOUNT_WITH_INSOLVENCY,
                BECAUSE_REQUIRED_CLOSE_TIME_NOT_PASSED,
                BECAUSE_CREDIT_REPORT_REQUEST_SEARCH,
                BECAUSE_ADDRESS_LINK_EXISTS
            };
        }
    }

    public abstract class CallReportEmailModel : EmailModel
    {
        protected CallReportEmailModel(
            IEnumerable<string> recipients,
            string subject,
            string environmentUrl)
            : base(recipients, subject, null)
        {
            EnvironmentUrl = environmentUrl;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected CallReportEmailModel()
        {
        }

        public string EnvironmentUrl { get; private set; }
    }

    public class CallReportPasswordChangedEmailModel : CallReportEmailModel
    {
        public CallReportPasswordChangedEmailModel(IEnumerable<string> recipients, string environmentUrl) :
            base(recipients, string.Format("CallReport password changed on: {0}", environmentUrl), environmentUrl)
        {
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected CallReportPasswordChangedEmailModel()
        {
        }
    }

    public class CallReportPasswordChangeFailureEmailModel : CallReportEmailModel
    {
        public string ErrorMessage { get; private set; }

        public CallReportPasswordChangeFailureEmailModel(IEnumerable<string> recipients, string environmentUrl, string errorMessage) :
            base(recipients, string.Format("CallReport password couldn't have been changed on: {0}", environmentUrl), environmentUrl)
        {
            ErrorMessage = errorMessage;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected CallReportPasswordChangeFailureEmailModel()
        {
        }
    }

    public class CallReportCredentialsSwitchedEmailModel : CallReportEmailModel
    {
        public CallReportCredentialsSwitchedEmailModel(IEnumerable<string> recipients, string environmentUrl) :
            base(recipients, string.Format("CallReport credentials have been switched on: {0}", environmentUrl), environmentUrl)
        {
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected CallReportCredentialsSwitchedEmailModel()
        {
        }
    }

    public class CallReportCredentialsSwitchFailureEmailModel : CallReportEmailModel
    {
        public string ErrorMessage { get; private set; }

        public CallReportCredentialsSwitchFailureEmailModel(IEnumerable<string> recipients, string environmentUrl, string errorMessage) :
            base(recipients, string.Format("CallReport credentials couldn't have been switched on: {0}", environmentUrl), environmentUrl)
        {
            ErrorMessage = errorMessage;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected CallReportCredentialsSwitchFailureEmailModel()
        {
        }
    }

    public class NoddleWebWatchAlertEmailModel : EmailModel
    {
        public NoddleWebWatchAlertEmailModel(string recipient, BrandedAccount? branding, string name)
            : base(new[] {recipient}, "We've sent you a Noddle Web Watch alert", branding)
        {
            Name = name;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected NoddleWebWatchAlertEmailModel()
        {
        }

        public string Name { get; private set; }
    }

    public class MessageLimitsReachedEmailModel : EmailModel
    {
        public MessageLimitsReachedEmailModel(
            string recipient,
            string from,
            string occurrences,
            string firstTimeSpottedOn,
            string occurrencesLimit,
            string timeLimit,
            string body,
            string ambient)
            : base(
                new[] {recipient},
                email => string.Format("[Yellow-error][{0}][{1}] Message limits reached", email.EnvironmentName, email.EnvironmentComponent),
                null)
        {
            From = from;
            Occurrences = occurrences;
            FirstTimeSpottedOn = firstTimeSpottedOn;
            OccurrencesLimit = occurrencesLimit;
            TimeLimit = timeLimit;
            Body = body;
            Ambient = ambient;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected MessageLimitsReachedEmailModel()
        {
        }

        public string Occurrences { get; private set; }
        public string FirstTimeSpottedOn { get; private set; }
        public string OccurrencesLimit { get; private set; }
        public string TimeLimit { get; private set; }
        public string Body { get; private set; }
        public string Ambient { get; private set; }
    }

    public class WelcomeToNoddleEmailModel : MultiTemplateEmailModel
    {
        public const string TEMPLATE_vASDA = "_vASDA";
        public const string TEMPLATE_vAsdaCustomers = "_vAsdaCustomers";
        public const string TEMPLATE_vHalifax = "_vHalifax";
        public const string TEMPLATE_vNationwide = "_vNationwide";
        public const string TEMPLATE_vSavCardHolders = "_vSavCardHolders";
        public const string TEMPLATE_vSavDeclinedUsers = "_vSavDeclinedUsers";
        public const string TEMPLATE_vTestDesktop = "_vTestDesktop";
        public const string TEMPLATE_vTesco = "_vTesco";
        public const string TEMPLATE_vTescoStaff = "_vTescoStaff";
        public const string TEMPLATE_vNew = "_vNew";
        public const string TEMPLATE_vRBS = "_vRBS";
        public const string TEMPLATE_vThinFile = "_vThinFile";
        public const string TEMPLATE_vThorne = "_vThorne";
        public const string TEMPLATE_vHomeAndLegacy = "_vHomeAndLegacy";

        public WelcomeToNoddleEmailModel(string recipient, string template, BrandedAccount? branding, string pageNameWithPoweredSuffix) :
            base(new[] {recipient}, string.Format("Welcome to {0}", pageNameWithPoweredSuffix), template, branding)
        {
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected WelcomeToNoddleEmailModel()
        {
        }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string>
            {
                TEMPLATE_vASDA,
                TEMPLATE_vAsdaCustomers,
                TEMPLATE_vHalifax,
                TEMPLATE_vNationwide,
                TEMPLATE_vSavCardHolders,
                TEMPLATE_vSavDeclinedUsers,
                TEMPLATE_vTestDesktop,
                TEMPLATE_vTesco,
                TEMPLATE_vTescoStaff,
                TEMPLATE_vNew,
                TEMPLATE_vRBS,
                TEMPLATE_vThinFile,
                TEMPLATE_vThorne,
                TEMPLATE_vHomeAndLegacy
            };
        }
    }

    public class DisputeConfirmationEmailModel : EmailModel
    {
        public DisputeConfirmationEmailModel(string recipient, BrandedAccount? branding, int userDisputeId, int daysToResolveDispute)
            : base(new[] {recipient}, "Confirmation of your dispute reference number", branding)
        {
            UserDisputeId = userDisputeId;
            DaysToResolveDispute = daysToResolveDispute;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected DisputeConfirmationEmailModel()
        {
        }

        public int UserDisputeId { get; private set; }
        public int DaysToResolveDispute { get; private set; }
    }

    public class SignupTemporarilyBlockedEmailModel : EmailModel
    {
        public SignupTemporarilyBlockedEmailModel(string recipient, BrandedAccount? branding, string pageName)
            : base(new[] {recipient}, string.Format("Thank you for registering with {0}", pageName), branding)
        {
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected SignupTemporarilyBlockedEmailModel()
        {
        }
    }

    public class EmailAddressChangedEmailModel : MultiTemplateEmailModel
    {
        public const string DEFAULT = "";

        public EmailAddressChangedEmailModel(
            string recipient,
            BrandedAccount? branding,
            string templateName,
            string firstName)
            : base(new[] {recipient}, ProvideSubjectName(branding), templateName, branding)
        {
            FirstName = firstName;
        }

        public static string GetTemplateName(BrandedAccount? branding)
        {
            return DEFAULT;
        }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string>
            {
                DEFAULT
            };
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected EmailAddressChangedEmailModel()
        {
        }

        private static string ProvideSubjectName(BrandedAccount? branding)
        {
            return "Confirmation of your email address change";
        }

        public string FirstName { get; private set; }
    }

    public class AddressChangedEmailModel : MultiTemplateEmailModel
    {
        public const string DEFAULT = "";

        public AddressChangedEmailModel(
            string recipient,
            BrandedAccount? branding,
            string templateName,
            string firstName)
            : base(new[] {recipient}, "Confirmation of your address change", templateName, branding)
        {
            FirstName = firstName;
        }

        public static string GetTemplateName(BrandedAccount? branding)
        {
            return DEFAULT;
        }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string>
            {
                DEFAULT
            };
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected AddressChangedEmailModel()
        {
        }

        public string FirstName { get; private set; }
    }

    public class ReportsEmailModel : EmailModel
    {
        public ReportsEmailModel(IEnumerable<string> recipients, DateTime reportsForDay, KeyValuePair<string, byte[]>[] attachments)
            : base(recipients, string.Format("Reports for {0}", reportsForDay.ToShortDateString()), null)
        {
            Attachments = attachments;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected ReportsEmailModel()
        {
        }
    }

    public class NoddleWebWatchNoAlertEmailModel : EmailModel
    {
        public NoddleWebWatchNoAlertEmailModel(string recipient, BrandedAccount? branding, string name)
            : base(new[] {recipient}, "Good news from Noddle Web Watch – you have no new alerts", branding)
        {
            Name = name;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected NoddleWebWatchNoAlertEmailModel()
        {
        }

        public string Name { get; private set; }
    }

    public class NoddleAlertNotificationEmailModel : EmailModel
    {
        public NoddleAlertNotificationEmailModel(string recipient, BrandedAccount? branding, string name)
            : base(new[] {recipient}, "We've sent you a Noddle alert notification", branding)
        {
            Name = name;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected NoddleAlertNotificationEmailModel()
        {
        }

        public string Name { get; private set; }
    }

    public class NoddleNoAlertNotificationEmailModel : EmailModel
    {
        public Guid CustomerId { get; set; }
        public string Name { get; private set; }

        public NoddleNoAlertNotificationEmailModel(string recipient, 
            BrandedAccount? branding, 
            string name,
            Guid customerId)
            : base(new[] {recipient}, "Good news from Noddle – you have no new alerts", branding)
        {
            Name = name;
            CustomerId = customerId;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected NoddleNoAlertNotificationEmailModel()
        {
        }

    }

    public class WelcomeToNoddleImproveEmailModel : EmailModel
    {
        public WelcomeToNoddleImproveEmailModel(string recipient, BrandedAccount? branding, string name)
            : base(new[] {recipient}, "Thank you for buying Noddle Improve", branding)
        {
            Name = name;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected WelcomeToNoddleImproveEmailModel()
        {
        }

        public string Name { get; private set; }
    }

    public class WelcomeToNoddleWebWatchEmailModel : EmailModel
    {
        public WelcomeToNoddleWebWatchEmailModel(string recipient, BrandedAccount? branding, string name)
            : base(new[] {recipient}, "Thank you for buying Noddle Web Watch", branding)
        {
            Name = name;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected WelcomeToNoddleWebWatchEmailModel()
        {
        }

        public string Name { get; private set; }
    }

    public class WelcomeToNoddleAlertsEmailModel : EmailModel
    {
        public WelcomeToNoddleAlertsEmailModel(string recipient, BrandedAccount? branding, string name)
            : base(new[] {recipient}, "Thank you for buying Noddle Alerts", branding)
        {
            Name = name;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected WelcomeToNoddleAlertsEmailModel()
        {
        }

        public string Name { get; private set; }
    }

    public class NoddleWebWatchRegistrationReminderEmailModel : EmailModel
    {
        public NoddleWebWatchRegistrationReminderEmailModel(string recipient, BrandedAccount? branding, string name)
            : base(new[] {recipient}, "Noddle Web Watch - you've not yet completed your registration", branding)
        {
            Name = name;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected NoddleWebWatchRegistrationReminderEmailModel()
        {
        }

        public string Name { get; private set; }
    }

    public class NoddleWebWatchMonitoredInformationChangedEmailModel : EmailModel
    {
        public NoddleWebWatchMonitoredInformationChangedEmailModel(string recipient, BrandedAccount? branding, string name)
            : base(new[] {recipient}, "Your Noddle Web Watch monitored information has been changed", branding)
        {
            Name = name;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected NoddleWebWatchMonitoredInformationChangedEmailModel()
        {
        }

        public string Name { get; private set; }
    }

    public class NoddleWebWatchAllMonitoredInformationDeletedEmailModel : EmailModel
    {
        public NoddleWebWatchAllMonitoredInformationDeletedEmailModel(string recipient, BrandedAccount? branding, string name)
            : base(new[] {recipient}, "Noddle Web Watch - please enter some monitored information", branding)
        {
            Name = name;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected NoddleWebWatchAllMonitoredInformationDeletedEmailModel()
        {
        }

        public string Name { get; private set; }
    }

    public class ChaserEmailModel : MultiTemplateEmailModel
    {
        public const string TEMPLATE_1_SignUp_vSavCardHolders = "1ToSignUpInNoddle_vSavCardHolders";
        public const string TEMPLATE_1_SignUp_vSavDeclinedUsers = "1ToSignUpInNoddle_vSavDeclinedUsers";
        public const string TEMPLATE_2_SignUp_vSavCardHolders = "2ToSignUpInNoddle_vSavCardHolders";
        public const string TEMPLATE_2_SignUp_vSavDeclinedUsers = "2ToSignUpInNoddle_vSavDeclinedUsers";
        public const string TEMPLATE_1_SwitchAccount_vSavCardHolders = "1ToSwitchAccount_vSavCardHolders";
        public const string TEMPLATE_1_SwitchAccount_vSavDeclinedUsers = "1ToSwitchAccount_vSavDeclinedUsers";
        public const string TEMPLATE_2_SwitchAccount_vSavCardHolders = "2ToSwitchAccount_vSavCardHolders";
        public const string TEMPLATE_2_SwitchAccount_vSavDeclinedUsers = "2ToSwitchAccount_vSavDeclinedUsers";

        public ChaserEmailModel(
            string recipient,
            string template,
            string lastName,
            string title,
            string signUpUrl,
            string switchAccountUrl,
            string from,
            string tacUrl)
            : base(new[] {recipient}, "Your invitation to sign up for aqua Credit Checker, powered by Noddle", template)
        {
            LastName = lastName;
            Title = title;
            Email = recipient;
            SignUpUrl = signUpUrl;
            SwitchAccountUrl = switchAccountUrl;
            TACUrl = tacUrl;
            From = from;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected ChaserEmailModel()
        {
        }

        public string LastName { get; private set; }
        public string Title { get; private set; }
        public string Email { get; private set; }
        public string SignUpUrl { get; private set; }
        public string SwitchAccountUrl { get; private set; }
        public string TACUrl { get; private set; }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string>
            {
                TEMPLATE_1_SignUp_vSavCardHolders,
                TEMPLATE_1_SignUp_vSavDeclinedUsers,
                TEMPLATE_2_SignUp_vSavCardHolders,
                TEMPLATE_2_SignUp_vSavDeclinedUsers,
                TEMPLATE_1_SwitchAccount_vSavCardHolders,
                TEMPLATE_1_SwitchAccount_vSavDeclinedUsers,
                TEMPLATE_2_SwitchAccount_vSavCardHolders,
                TEMPLATE_2_SwitchAccount_vSavDeclinedUsers
            };
        }
    }

    public class InviteEmailModel : MultiTemplateEmailModel
    {
        public const string TEMPLATE_SignUp_vSavCardHolders = "ToNoddle_vSavCardHolders";
        public const string TEMPLATE_SignUp_vSavDeclinedUsers = "ToNoddle_vSavDeclinedUsers";
        public const string TEMPLATE_SignUp_vExistingSavCardHolders = "ToNoddle_vExistingSavCardHolders";
        public const string TEMPLATE_SignUp_vExistingSavDeclinedUsers = "ToNoddle_vExistingSavDeclinedUsers";
        public const string TEMPLATE_Switch_vSavCardHolders = "ToSwitch_vSavCardHolders";
        public const string TEMPLATE_Switch_vSavDeclinedUsers = "ToSwitch_vSavDeclinedUsers";
        public const string TEMPLATE_Switch_vExistingSavCardHolders = "ToSwitch_vExistingSavCardHolders";
        public const string TEMPLATE_Switch_vExistingSavDeclinedUsers = "ToSwitch_vExistingSavDeclinedUsers";

        public InviteEmailModel(
            string recipient,
            string template,
            string lastName,
            string title,
            string signUpUrl,
            string switchAccountUrl,
            string from,
            string tacUrl) :
            base(new[] { recipient }, "Your invitation to sign up for aqua Credit Checker, powered by Noddle", template)

        {
            LastName = lastName;
            Title = title;
            Email = recipient;
            SignUpUrl = signUpUrl;
            SwitchAccountUrl = switchAccountUrl;
            From = from;
            TACUrl = tacUrl;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected InviteEmailModel()
        {
        }

        public string LastName { get; private set; }
        public string Title { get; private set; }
        public string Email { get; private set; }
        public string SignUpUrl { get; private set; }
        public string SwitchAccountUrl { get; private set; }
        public string TACUrl { get; private set; }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string>
            {
                TEMPLATE_SignUp_vSavCardHolders,
                TEMPLATE_SignUp_vSavDeclinedUsers,
                TEMPLATE_SignUp_vExistingSavCardHolders,
                TEMPLATE_SignUp_vExistingSavDeclinedUsers,
                TEMPLATE_Switch_vSavCardHolders,
                TEMPLATE_Switch_vSavDeclinedUsers,
                TEMPLATE_Switch_vExistingSavCardHolders,
                TEMPLATE_Switch_vExistingSavDeclinedUsers
            };
        }
    }

    public class InviteToSwitch_ThinFileEmailModel : EmailModel
    {
        public InviteToSwitch_ThinFileEmailModel(string recipient, string switchAccountUrl, BrandedAccount? brandedAccount)
            : base(new[] { recipient }, "Your credit report is now available", brandedAccount)
        {
            SwitchAccountUrl = switchAccountUrl;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected InviteToSwitch_ThinFileEmailModel()
        {
        }

        public string SwitchAccountUrl { get; private set; }
    }

    public class StillThinFileEmailModel : EmailModel
    {
        public StillThinFileEmailModel(string recipient, string logInUrl, BrandedAccount? brandedAccount)
            : base(new[] { recipient }, "Your credit report is still not available", brandedAccount)
        {
            LogInUrl = logInUrl;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected StillThinFileEmailModel()
        {
        }

        public string LogInUrl { get; private set; }
    }

    public class SwitchAccountConfirmationEmailModel : MultiTemplateEmailModel
    {
        public const string SAV = "_vSav";
        public const string TESCO = "_vTesco";

        public SwitchAccountConfirmationEmailModel(
            string recipient,
            string template,
            BrandedAccount? branding,
            string pageNameWithPoweredSuffix,
            string name) :
                base(new[] {recipient}, string.Format("{0} - Account Switch Confirmation", pageNameWithPoweredSuffix), template, branding)
        {
            Name = name;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected SwitchAccountConfirmationEmailModel()
        {
        }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string> {SAV, TESCO};
        }

        public string Name { get; private set; }
    }

    public class KbaNotAnsweredReminderEmailModel : MultiTemplateEmailModel
    {
        public const string DEFAULT = "";

        public KbaNotAnsweredReminderEmailModel(
            string recipient,
            BrandedAccount? branding,
            string pageName,
            string name,
            string continueSignUpUrl,
            string templateName)
            : base(
                new[] {recipient},
                string.Format("{0} – come back and complete your application", pageName),
                templateName,
                branding)
        {
            ContinueSignUpUrl = continueSignUpUrl;
            Name = name;
        }

        public static string GetTemplateName(BrandedAccount? branding)
        {
            return DEFAULT;
        }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string>
            {
                DEFAULT
            };
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected KbaNotAnsweredReminderEmailModel()
        {
        }

        public string ContinueSignUpUrl { get; private set; }
        public string Name { get; private set; }
    }

    public class DisputeWasUpdatedEmailModel : EmailModel
    {
        public DisputeWasUpdatedEmailModel(string recipient, BrandedAccount? branding, int disputeId) : base(
            new[] {recipient},
            email => string.Format("Update dispute reference number: {0}", disputeId),
            branding)
        {
            DisputeId = disputeId;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected DisputeWasUpdatedEmailModel()
        {
        }

        public int DisputeId { get; private set; }
    }

    public class UnlockAccountEmailModel : MultiTemplateEmailModel
    {
        public const string DEFAULT = "";

        public UnlockAccountEmailModel(
            string recipient,
            BrandedAccount? branding,
            string accountName,
            int tokenValidForHours,
            string confirmationUrl,
            string templateName,
            string firstName)
            : base(new[] {recipient}, string.Format("Unlock your {0}", accountName), templateName, branding)
        {
            TokenValidForHours = tokenValidForHours;
            ConfirmationUrl = confirmationUrl;
            FirstName = firstName;
        }

        public static string GetTemplateName(BrandedAccount? branding)
        {
            return DEFAULT;
        }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string>
            {
                DEFAULT
            };
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected UnlockAccountEmailModel()
        {
        }

        public int TokenValidForHours { get; private set; }
        public string ConfirmationUrl { get; private set; }
        public string FirstName { get; private set; }
    }

    public class UnlockAccountUnsuccessfulAttemptEmailModel : MultiTemplateEmailModel
    {
        public const string DEFAULT = "";

        public UnlockAccountUnsuccessfulAttemptEmailModel(
            string recipient,
            BrandedAccount? branding,
            string accountName,
            string templateName,
            string firstName)
            : base(new[] {recipient},
                  ProvideSubjectName(accountName),
                  templateName,
                  branding)
        {
            FirstName = firstName;
        }

        public static string GetTemplateName(BrandedAccount? branding)
        {
            return DEFAULT;
        }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string>
            {
                DEFAULT
            };
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected UnlockAccountUnsuccessfulAttemptEmailModel()
        {
        }

        private static string ProvideSubjectName(string accountName)
        {
            return string.Format("Your {0} – Still Locked", accountName);
        }

        public string FirstName { get; private set; }
    }

    public class UnlockAccountSuccessfulEmailModel : MultiTemplateEmailModel
    {
        public const string DEFAULT = "";

        public UnlockAccountSuccessfulEmailModel(
            string recipient,
            BrandedAccount? branding,
            string accountName,
            string templateName,
            string firstName)
            : base(new[] {recipient},
                  ProvideSubjectName(accountName),
                  templateName,
                  branding)
        {
            FirstName = firstName;
        }

        public static string GetTemplateName(BrandedAccount? branding)
        {
            return DEFAULT;
        }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string>
            {
                DEFAULT
            };
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected UnlockAccountSuccessfulEmailModel()
        {
        }

        private static string ProvideSubjectName(string accountName)
        {
            return string.Format("Your {0} – Successfully Unlocked", accountName);
        }

        public string FirstName { get; private set; }
    }

    public abstract class RenewalReminderEmailModel : MultiTemplateEmailModel
    {
        public const string TEXT = "_vText";
        public const string GRAPHIC = "_vGraphic";

        protected RenewalReminderEmailModel(string recipient, string subject, BrandedAccount? branding, string firstName, string productPageUrl, string template)
            : base(new[] { recipient }, subject, template, branding)
        {
            FirstName = firstName;
            ProductPageUrl = productPageUrl;
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected RenewalReminderEmailModel()
        {
        }

        public string FirstName { get; private set; }
        public string ProductPageUrl { get; private set; }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string> {TEXT, GRAPHIC};
        }
    }

    public class PremiumProductsAutoRenewedEmailModel : MultiTemplateEmailModel
    {
        public const string TEMPLATE_vHalifax = "_vHalifax";
        public const string DEFAULT = "";

        public string FirstName { get; private set; }
        public string ProductType { get; private set; }

        public PremiumProductsAutoRenewedEmailModel(string recipient, string userFirstName, string template, string productType)
            : base(new[] {recipient}, GetSubjectForTemplate(template, productType), template, GetBrandingForTemplate(template))
        {
            FirstName = userFirstName;
            ProductType = productType;
        }

        private static BrandedAccount? GetBrandingForTemplate(string template)
        {
            switch (template)
            {              
                case DEFAULT:
                    return null;
                default:
                    throw Fail.Because("Unsupported template: {0}", template);
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected PremiumProductsAutoRenewedEmailModel()
        {
        }

        private static string GetSubjectForTemplate(string template, string productType)
        {
            switch (template)
            {
                case TEMPLATE_vHalifax:
                    return "Your Halifax Credit Checker service will be renewed free of charge";
                case DEFAULT:
                    return $"Your {productType} subscription will be renewed free of charge";
                default:
                    throw Fail.Because("Unsupported template: {0}", template);
            }
        }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string>
            {
                TEMPLATE_vHalifax,
                DEFAULT
            };
        }
    }

    public class SwitchedAccountToNoddleEmailModel : MultiTemplateEmailModel
    {
        public const string TEMPLATE_NoddleThinFile = "_vThinFile";
        public const string DEFAULT = "";

        public string FirstName { get; private set; }
        public string BrandName { get; private set; }

        public SwitchedAccountToNoddleEmailModel(string recipient, string userFirstName, string template, string brand)
            : base(new[] {recipient}, GetSubjectForTemplate(template, brand), template, GetBrandingForTemplate(template))
        {
            FirstName = userFirstName;
            BrandName = brand;
        }

        private static BrandedAccount? GetBrandingForTemplate(string template)
        {
            switch (template)
            {
                case DEFAULT:
                    return null;
                default:
                    throw Fail.Because("Unsupported template: {0}", template);
            }
        }

        [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
        protected SwitchedAccountToNoddleEmailModel()
        {
        }

        private static string GetSubjectForTemplate(string template, string brand)
        {
            switch (template)
            {
                case DEFAULT:
                case TEMPLATE_NoddleThinFile:
                    return $"Information about your {brand} account";
                default:
                    throw Fail.Because("Unsupported template: {0}", template);
            }
        }

        protected override IEnumerable<string> ProvideSupportedTemplateNames()
        {
            return new List<string>
            {
                TEMPLATE_NoddleThinFile,
                DEFAULT
            };
        }
    }

    // ReSharper restore InconsistentNaming
}