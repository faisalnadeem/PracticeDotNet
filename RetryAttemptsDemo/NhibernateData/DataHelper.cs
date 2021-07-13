namespace NhibernateWithRetry.NhibernateData
{
    public class DataHelper
    {
        //public void Test()
        //{
        //    var configuration = ConfigureNhibernate();
        //    InstantiateDatabase(configuration, true);

        //    var creditReportNotification = new CreditReportNotificationTestUpdate().CreateCreditReportNotification();
        //    var sessionFactory = configuration.BuildSessionFactory();
        //    using (var session = sessionFactory.OpenSession())
        //    {
        //        using (var tx = session.BeginTransaction())
        //        {
        //            RetryHelper.ExecuteTaskWithRetryAttempts(() => session.Save(creditReportNotification));
        //            //session.Save(creditReportNotification);
        //            tx.Commit();
        //        }
        //    }

        //}

        //public Configuration ConfigureNhibernate()
        //{
        //    var configuration = new Configuration();
        //    configuration.DataBaseIntegration(db =>
        //    {
        //        db.ConnectionString = @"Server=PLLLP9435;initial catalog=NHibernateTest;Integrated Security=true";
        //        db.Dialect<NHibernate.Dialect.MsSql2012Dialect>();
        //        db.Driver<NHibernate.Driver.SqlClientDriver>();
        //    });

        //    return configuration;
        //}

        //public void InstantiateDatabase(Configuration configuration, bool updateSchema = false)
        //{
        //    var modelMapper = new ModelMapper();
        //    modelMapper.AddMapping<PersonMapping>();
        //    modelMapper.AddMapping<CarMapping>();
        //    modelMapper.AddMapping<CreditReportNotificationMap>();
        //    modelMapper.AddMapping<CloseAccountMapping>();
        //    modelMapper.AddMapping<SubscriptionMapping>();

        //    var mapping = modelMapper.CompileMappingForAllExplicitlyAddedEntities();
        //    configuration.AddDeserializedMapping(mapping, "Test");
        //    var schema = new SchemaExport(configuration);
        //    if(updateSchema)
        //        schema.Execute(false, true, false);
        //}      
    }
}