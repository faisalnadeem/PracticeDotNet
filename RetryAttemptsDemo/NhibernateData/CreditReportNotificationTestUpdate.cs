using System;
using System.Collections.Generic;

namespace NhibernateWithRetry.NhibernateData
{
    public class CreditReportNotificationTestUpdate
    {
        public readonly ITime Time = new Time();
        private int _sendCrNotificationsAppSettingsNotificationsAmountLimit = 100;

        public CreditReportNotification CreateCreditReportNotification()
        {
            DateTime endDate = Time.Now();
            int usersCountWhoShouldGotNotification = 3;
            DateTime startDate = Time.Now().AddDays(-5);
            var userIdsWhoGotFirstNotification = GenerateUserIds(0,1,100005);
            var userIdsWhoGotSecondNotification = GenerateUserIds(0, 2, 1000);
            var userIdsWhoGotThirdNotification = GenerateUserIds(0, 3, 1000);
            var userIdsWhoDidntGetFirstNotification = GenerateUserIds(0, 4, 1000);
            var userIdsWhoDidntGetSecondNotification = GenerateUserIds(0,5,1000);
            var userIdsWhoDidntGetThirdNotification = GenerateUserIds(0, 6, 1000);


            return new CreditReportNotification(
                usersCountWhoShouldGotNotification,
                _sendCrNotificationsAppSettingsNotificationsAmountLimit,
                startDate,
                endDate,
                userIdsWhoGotFirstNotification,
                userIdsWhoGotSecondNotification,
                userIdsWhoGotThirdNotification,
                userIdsWhoDidntGetFirstNotification,
                userIdsWhoDidntGetSecondNotification,
                userIdsWhoDidntGetThirdNotification);
        }

        public List<int> GenerateUserIds(int from, int increaseBy, int to)
        {
            var list = new List<int>();
            for (int i = from; i <= to; i++)
            {
                list.Add(i+increaseBy);
            }
            return list;
        }
    }

    public interface ITime
    {
        DateTime Now();
        DateTime Today();
        DateTime UtcNow();
    }

    public class Time : ITime
    {
        public DateTime Now()
        {
            return DateTime.Now;
        }

        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }

        public DateTime Today()
        {
            return DateTime.Now.Date;
        }
    }

}
