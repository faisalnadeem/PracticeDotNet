using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Automapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Cfg;
using Configuration = NHibernate.Cfg.Configuration;
using FluentNHibernate.Data;
using FluentNHibernate.Utils;
using NHibernate.Mapping.ByCode.Conformist;

namespace NhibernateTransformersSammple
{
    public class DataHelper
    {
        public void Test()
        {
            Update_an_existing_database_schema();
            //return;
            var configuration = ConfigureNhibernate();
            
            var sefact = configuration.BuildSessionFactory();
            using (var session = sefact.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    
                        var testClass = new TestClass() {Description = $"Test-1"};
                        session.Save(testClass);
                        tx.Commit();
                }
            }

        }

        public Configuration ConfigureNhibernate()
        {
            var configuration = new Configuration();
            configuration.DataBaseIntegration(db =>
            {
                db.ConnectionString = @"Server=.\SQLEXPRESS;initial catalog=Test1;Integrated Security=true";
                db.Dialect<NHibernate.Dialect.MsSql2012Dialect>();
                db.Driver<NHibernate.Driver.SqlClientDriver>();
            });

            return configuration;
        }

        public void InstantiateDatabase(Configuration configuration, bool updateSchema = false)
        {
            var modelMapper = new ModelMapper();
            //modelMapper.AddMapping<PersonMapping>();
            //modelMapper.AddMapping<CarMapping>();
            //modelMapper.AddMapping<CreditReportNotificationMap>();

            var mapping = modelMapper.CompileMappingForAllExplicitlyAddedEntities();
            configuration.AddDeserializedMapping(mapping, "Test1");
            var schema = new SchemaExport(configuration);
            if (updateSchema)
                schema.Execute(false, true, false);
        }

        public void Update_an_existing_database_schema()
        {
            var config = ConfigureNhibernate();
            //var mapper = new ConventionModelMapper();
            var modelMapper = new ModelMapper();
            modelMapper.AddMapping<TestClassMapping>();
            modelMapper.AddMapping<TestChildMapping>();

            var mapping = modelMapper.CompileMappingForAllExplicitlyAddedEntities();
            config.AddDeserializedMapping(mapping, "Test1");
            //AutoMap.AssemblyOf<Entity>()
            //    .IgnoreBase(typeof(BaseEntity<>));
            //config.AddMapping(mapper.CompileMappingFor(new[] { typeof(TestClass), typeof(TestChild)}));
            //config.AddAssembly(Assembly.LoadFrom("NhibernateTransformersSammple.dll"));
            var update = new SchemaUpdate(config);
            try
            {
                update.Execute(false, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    //public abstract class BaseEntity<T>
    //{
    //    public T Id { get; set; }
    //}

    public class TestClass 
    {
        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public TestChild TestChild { get; set; }
    }

    public class TestChild
    {
        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
    }

    public class TestClassMapping : ClassMapping<TestClass>
    {
        public TestClassMapping()
        {
            Id(s => s.Id, im => im.Generator(Generators.Identity));  // primary key mapping
            Property(s => s.Description, pm => pm.NotNullable(true));
            Property(s => s.TestChild, pm => pm.NotNullable(false));
            
        }
    }
    public class TestChildMapping : ClassMapping<TestChild>
    {
        public TestChildMapping()
        {
            Id(s => s.Id, im => im.Generator(Generators.Identity));  // primary key mapping
            Property(s => s.Description, pm => pm.NotNullable(true));
        }
    }

}
