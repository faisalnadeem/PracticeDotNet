using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SequenceNoElements
{
    public sealed class RegistrationProgress : PrimitiveValue<string>
    {
        private RegistrationProgress(string value) : base(value) { }

        // ReSharper disable InconsistentNaming
        public static RegistrationProgress DataCollection = new RegistrationProgress("Data Collection Idv Error");
        public static RegistrationProgress AboutYou = new RegistrationProgress("AboutYou");
        public static RegistrationProgress AddressHistory = new RegistrationProgress("AddressHistory");
        public static RegistrationProgress BlackBox = new RegistrationProgress("BlackBox");
        public static RegistrationProgress IdentityValidation = new RegistrationProgress("IdentityValidation");
        public static RegistrationProgress Matching = new RegistrationProgress("Matching Idv Error");
        public static RegistrationProgress MatchingCheck = new RegistrationProgress("Matching Check Idv Error");
        public static RegistrationProgress PredictiveAnalytics = new RegistrationProgress("Predictive Analytics Idv Error");
        public static RegistrationProgress PredictiveAnalyticsCheck = new RegistrationProgress("Predictive Analytics Check Idv Error");
        public static RegistrationProgress IdEnhanced = new RegistrationProgress("Id Enhanced Idv Error");
        public static RegistrationProgress IdEnhancedCheck = new RegistrationProgress("Id Enhanced Check Idv Error");
        public static RegistrationProgress RTFA = new RegistrationProgress("RTFA Idv Error");
        public static RegistrationProgress RTFACheck = new RegistrationProgress("RTFA Check Idv Error");
        public static RegistrationProgress KbaStarted = new RegistrationProgress("KBA 1 Started");
        public static RegistrationProgress Kba = new RegistrationProgress("Kba Idv Error");
        public static RegistrationProgress GetKBAQuestions = new RegistrationProgress("Get KBA Questions Idv Error");
        public static RegistrationProgress EnoughQuestionsRound1 = new RegistrationProgress("Enough Questions Round 1 Idv Error");
        public static RegistrationProgress KBA1 = new RegistrationProgress("KBA 1 Idv Error");
        public static RegistrationProgress KBA1Check = new RegistrationProgress("KBA 1 Check Idv Error");
        public static RegistrationProgress EnoughQuestionsRound2 = new RegistrationProgress("Enough Questions Round 2 Idv Error");
        public static RegistrationProgress Kba2Started = new RegistrationProgress("KBA 2 Started");
        public static RegistrationProgress KBA2 = new RegistrationProgress("KBA 2 Idv Error");
        public static RegistrationProgress KBA2Check = new RegistrationProgress("KBA 2 Check Idv Error");
        public static RegistrationProgress DeviceRiskCheck = new RegistrationProgress("Device Risk Check Idv Error");
        public static RegistrationProgress EmailRisk = new RegistrationProgress("Email Risk Idv Error");
        public static RegistrationProgress EmailRiskCheck = new RegistrationProgress("Email Risk Check Idv Error");
        public static RegistrationProgress MobileRisk = new RegistrationProgress("Mobile Risk Idv Error");
        public static RegistrationProgress MobileRiskCheck = new RegistrationProgress("Mobile Risk Check Idv Error");
        public static RegistrationProgress AccountDetails = new RegistrationProgress("AccountDetails");
        public static RegistrationProgress Activation = new RegistrationProgress("Activation");
        public static RegistrationProgress Registered = new RegistrationProgress("Registered");

        public static RegistrationProgress Failed = new RegistrationProgress("Failed");
        public static RegistrationProgress NotUniqueUser = new RegistrationProgress("NotUniqueUser");
        public static RegistrationProgress NotUniqueEmail = new RegistrationProgress("NotUniqueEmail");
        public static RegistrationProgress EmailBlacklistFailed = new RegistrationProgress("EmailBlacklistFailed");
        public static RegistrationProgress IpBlacklistFailed = new RegistrationProgress("IpBlacklistFailed");

        public static RegistrationProgress InvalidIdvNotification = new RegistrationProgress("InvalidIdvNotification");
        // ReSharper restore InconsistentNaming

        public bool IsFailed =>
            IsCompleted &&
            Value != Registered;

        public bool IsCompleted =>
            Value != AboutYou &&
            Value != AddressHistory &&
            Value != BlackBox &&
            Value != IdentityValidation &&
            Value != KbaStarted &&
            Value != Kba2Started &&
            Value != AccountDetails &&
            Value != Activation;

        private static readonly IEnumerable<RegistrationProgress> idvErrors = new List<RegistrationProgress>
        {
            DataCollection,
            Matching,
            MatchingCheck,
            PredictiveAnalytics,
            PredictiveAnalyticsCheck,
            IdEnhanced,
            IdEnhancedCheck,
            RTFA,
            RTFACheck,
            GetKBAQuestions,
            EnoughQuestionsRound1,
            KBA1,
            KBA1Check,
            EnoughQuestionsRound2,
            KBA2,
            KBA2Check,
            DeviceRiskCheck,
            EmailRisk,
            EmailRiskCheck,
            MobileRisk,
            MobileRiskCheck
        };

        public static RegistrationProgress GetIdvErrorByDescription(string description)
        {
            return idvErrors.First(x => x.Value.StartsWith(description, StringComparison.CurrentCultureIgnoreCase) || x.Value == description);
        }
    }

    public abstract class PrimitiveValue<T>
    {
        public PrimitiveValue(T value)
        {
            Value = value;
        }

        public T Value { get; }

        public static implicit operator T(PrimitiveValue<T> value)
        {
            if (ReferenceEquals(value, null))
                return default(T);

            return value.Value;
        }

        public static bool operator ==(PrimitiveValue<T> @this, PrimitiveValue<T> other)
        {
            if (ReferenceEquals(@this, null))
                return ReferenceEquals(other, null);

            if (ReferenceEquals(other, null))
                return false;

            return @this.Value.Equals(other.Value);
        }

        public static bool operator !=(PrimitiveValue<T> @this, PrimitiveValue<T> other)
        {
            return !(@this == other);
        }

        public override bool Equals(object obj)
        {
            var other = obj as PrimitiveValue<T>;

            if (ReferenceEquals(other, null))
                return false;

            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Value}";
        }
    }
}
