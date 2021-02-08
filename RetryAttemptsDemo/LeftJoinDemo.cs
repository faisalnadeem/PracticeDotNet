using NhibernateWithRetry.NhibernateData;

namespace NhibernateWithRetry
{
    public class LeftJoinDemo
	{
		public void Test()
		{
			var configuration = new DataHelper().ConfigureNhibernate();
			var sessionFactory = configuration.BuildSessionFactory();
			using (var session = sessionFactory.OpenSession())
			{
				using (var tx = session.BeginTransaction())
				{
					session.Save(new CloseAccount(){Description = "user1"});
					//session.Save(creditReportNotification);
					tx.Commit();
				}
			}



		}
	}

	public class CloseAccount
	{
		public virtual int Id { get; set; }
		public virtual string Description { get; set; }

	}
	public class Subscription{
		public virtual int Id { get; set; }
		public virtual string Description { get; set; }
		public virtual CloseAccount CloseAccount { get; set; }
	}
}
