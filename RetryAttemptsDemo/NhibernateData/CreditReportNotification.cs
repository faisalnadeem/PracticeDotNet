using System;
using System.Collections.Generic;
using System.Linq;

namespace NhibernateWithRetry.NhibernateData
{
    //[Serializable]
    //public abstract class Entity : EntityWithTypedId<int>
    //{
    //}
    public class CreditReportNotification 
    {
        public CreditReportNotification()
        {
        }

        public CreditReportNotification(
            int usersCountWhoShouldGetNotification,
            int notificationsAmountLimit,
            DateTime sendingDate,
            DateTime sendingEndDate,
            IEnumerable<int> firstNotificationUserIds,
            IEnumerable<int> secondNotificationUserIds,
            IEnumerable<int> thirdNotificationUserIds,
            IEnumerable<int> firstNotificationFailureUserIds,
            IEnumerable<int> secondNotificationFailureUserIds,
            IEnumerable<int> thirdNotificationFailureUserIds)
        {
            UsersCountWhoShouldGetNotification = usersCountWhoShouldGetNotification;
            NotificationsAmountLimit = notificationsAmountLimit;
            SendingDate = sendingDate;
            SendingEndDate = sendingEndDate;
            SetFirstCrNotificationUserIds(firstNotificationUserIds);
            SetSecondCrNotificationUserIds(secondNotificationUserIds);
            SetThirdCrNotificationUserIds(thirdNotificationUserIds);
            SetFirstCrNotificationFailureUserIds(firstNotificationFailureUserIds);
            SetSecondCrNotificationFailureUserIds(secondNotificationFailureUserIds);
            SetThirdCrNotificationFailureUserIds(thirdNotificationFailureUserIds);

            UsersCountWhoGotNotification = GetFirstCrNotificationUserIds().Count +
                                           GetSecondCrNotificationUserIds().Count +
                                           GetThirdCrNotificationUserIds().Count;
            UsersCountWhoDidntGetNotificationBecauseOfFail = GetFirstCrNotificationFailureUserIds().Count +
                                                             GetSecondCrNotificationFailureUserIds().Count +
                                                             GetThirdCrNotificationFailureUserIds().Count;
        }
        public virtual int Id { get; set; }
        public virtual DateTime SendingDate { get; set; }
        public virtual DateTime SendingEndDate { get; set; }
        public virtual int NotificationsAmountLimit { get; set; }
        public virtual int UsersCountWhoShouldGetNotification { get; set; }
        public virtual int UsersCountWhoGotNotification { get; set; }
        public virtual int UsersCountWhoDidntGetNotificationBecauseOfFail { get; set; }

        public virtual string FirstCrNotificationUserIds { get; set; }
        public virtual string SecondCrNotificationUserIds { get; set; }
        public virtual string ThirdCrNotificationUserIds { get; set; }
        public virtual string FirstCrNotificationFailureUserIds { get; set; }
        public virtual string SecondCrNotificationFailureUserIds { get; set; }
        public virtual string ThirdCrNotificationFailureUserIds { get; set; }

        public virtual HashSet<int> GetFirstCrNotificationUserIds()
        {
            return GetUserIds(FirstCrNotificationUserIds);
        }

        public virtual HashSet<int> GetSecondCrNotificationUserIds()
        {
            return GetUserIds(SecondCrNotificationUserIds);
        }

        public virtual HashSet<int> GetThirdCrNotificationUserIds()
        {
            return GetUserIds(ThirdCrNotificationUserIds);
        }

        public virtual HashSet<int> GetFirstCrNotificationFailureUserIds()
        {
            return GetUserIds(FirstCrNotificationFailureUserIds);
        }

        public virtual HashSet<int> GetSecondCrNotificationFailureUserIds()
        {
            return GetUserIds(SecondCrNotificationFailureUserIds);
        }

        public virtual HashSet<int> GetThirdCrNotificationFailureUserIds()
        {
            return GetUserIds(ThirdCrNotificationFailureUserIds);
        }

        public virtual void SetFirstCrNotificationUserIds(IEnumerable<int> userIds)
        {
            FirstCrNotificationUserIds = string.Join(",", userIds);
        }

        public virtual void SetSecondCrNotificationUserIds(IEnumerable<int> userIds)
        {
            SecondCrNotificationUserIds = string.Join(",", userIds);
        }

        public virtual void SetThirdCrNotificationUserIds(IEnumerable<int> userIds)
        {
            ThirdCrNotificationUserIds = string.Join(",", userIds);
        }

        public virtual void SetFirstCrNotificationFailureUserIds(IEnumerable<int> userIds)
        {
            FirstCrNotificationFailureUserIds = string.Join(",", userIds);
        }

        public virtual void SetSecondCrNotificationFailureUserIds(IEnumerable<int> userIds)
        {
            SecondCrNotificationFailureUserIds = string.Join(",", userIds);
        }

        public virtual void SetThirdCrNotificationFailureUserIds(IEnumerable<int> userIds)
        {
            ThirdCrNotificationFailureUserIds = string.Join(",", userIds);
        }

        private HashSet<int> GetUserIds(string userIds)
        {
            if (string.IsNullOrEmpty(userIds))
                return new HashSet<int>();

            return userIds
                .Split(',')
                .Select(int.Parse)
                .ToHashSet();
        }
    }

    public static class CollectionExtensions
    {
       public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }
    }
}
