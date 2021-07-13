using FluentNHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace NhibernateWithRetry.NhibernateData
{
    public class SubscriptionMapping : ClassMapping<Subscription>
    {
        public SubscriptionMapping()
        {
            Id(s => s.Id, im => im.Generator(Generators.Identity));  // primary key mapping
            Property(s => s.Description, pm => pm.NotNullable(true));
        }
    }
    public class CloseAccountMapping : ClassMapping<CloseAccount>
    {
        public CloseAccountMapping()
        {
            Id(s => s.Id, im => im.Generator(Generators.Identity));  // primary key mapping
            Property(s => s.Description, pm => pm.NotNullable(true));
        }
    }

    public class PersonMapping : ClassMapping<Person>
    {
        public PersonMapping()
        {
            Id(s => s.Id, im => im.Generator(Generators.Identity));  // primary key mapping
            Property(s => s.FirstName, pm => pm.NotNullable(true));
            Property(s => s.LastName, pm => pm.NotNullable(true));
            ManyToOne(s => s.Car, mom => mom.Cascade(Cascade.Persist));
        }
    }

    public class CarMapping : ClassMapping<Car>
    {
        public CarMapping()
        {
            Id(s => s.Id, im => im.Generator(Generators.Identity));
            Property(s => s.Make, pm => pm.NotNullable(true));
            Property(s => s.Model, pm => pm.NotNullable(true));
            Property(s => s.Year, pm => pm.NotNullable(true));
        }
    }

    public class
        CreditReportNotificationMap : ClassMapping<CreditReportNotification> //, IAutoMappingOverride<CreditReportNotification>
    {
        public CreditReportNotificationMap()
        {
            Id(s => s.Id, im => im.Generator(Generators.Identity)); // primary key mapping
            Property(notification => notification.SendingDate, pm => pm.NotNullable(true));
            Property(notification => notification.SendingEndDate, pm => pm.NotNullable(true));
            Property(s => s.FirstCrNotificationFailureUserIds, pm =>
                {
                    pm.NotNullable(false);
                    pm.Length(10000);
                });

            Property(s => s.SecondCrNotificationUserIds,
                pm =>
                {
                    pm.NotNullable(true);
                    pm.Length(10000);
                });
            Property(s => s.ThirdCrNotificationUserIds,
                pm =>
                {
                    pm.NotNullable(true);
                    pm.Length(10000);
                });
            Property(s => s.FirstCrNotificationFailureUserIds,
                pm =>
                {
                    pm.NotNullable(true);
                    pm.Length(10000);
                });
            Property(s => s.SecondCrNotificationFailureUserIds,
                pm =>
                {
                    pm.NotNullable(true);
                    pm.Length(10000);
                });
            Property(s => s.ThirdCrNotificationFailureUserIds,
                pm =>
                {
                    pm.NotNullable(true);
                    pm.Length(10000);
                });
            Property(x => x.UsersCountWhoShouldGetNotification, pm => pm.NotNullable(true));
            Property(x => x.UsersCountWhoGotNotification, pm => pm.NotNullable(true));
            Property(x => x.UsersCountWhoDidntGetNotificationBecauseOfFail, pm => pm.NotNullable(true));
            Property(x => x.NotificationsAmountLimit, pm => pm.NotNullable(true));
        }
    }

    public static class MappingExtensions
    {
        public static PropertyPart WithMaxLength(this PropertyPart map)
        {
            return map.Length(10000);
        }
    }
}
//Property(s =>  ((MemberExpression) Reveal.Member<CreditReportNotification>("FirstCrNotificationUserIds").Body).Member.Name,
//    pm =>
//    {
//        pm.NotNullable(false);
//        pm.Length(10000);
//    });
//Property(s => Reveal.Member<CreditReportNotification>("SecondCrNotificationUserIds").ToMember().Name,
//    pm =>
//    {
//        pm.NotNullable(true);
//        pm.Length(10000);
//    });
//Property(s => Reveal.Member<CreditReportNotification>("ThirdCrNotificationUserIds").ToMember().Name,
//    pm =>
//    {
//        pm.NotNullable(true);
//        pm.Length(10000);
//    });
//Property(s => Reveal.Member<CreditReportNotification>("FirstCrNotificationFailureUserIds").ToMember().Name,
//    pm =>
//    {
//        pm.NotNullable(true);
//        pm.Length(10000);
//    });
//Property(s => Reveal.Member<CreditReportNotification>("SecondCrNotificationFailureUserIds").ToMember().Name,
//    pm =>
//    {
//        pm.NotNullable(true);
//        pm.Length(10000);
//    });
//Property(s => Reveal.Member<CreditReportNotification>("ThirdCrNotificationFailureUserIds").ToMember().Name,
//    pm =>
//    {
//        pm.NotNullable(true);
//        pm.Length(10000);
//    });
