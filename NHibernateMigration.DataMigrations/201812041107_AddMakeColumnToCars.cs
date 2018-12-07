using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using NHibernateMigration.DataMigrations.Helpers;

namespace NHibernateMigration.DataMigrations
{
	[Migration(1)]
	public class Add_Make_Column_To_Car : ForwardOnlyMigration
	{
		public override void Up()
		{
			Create.Column("Make").OnTable("Cars").AsString(int.MaxValue).Nullable();
		}

		//public override void Down()
		//{
		//	Delete.Column("Make").FromTable("Cars");
		//}
	}

	[Migration(2)]
	public class Create_EmailQueue_table_with_identity : ForwardOnlyMigration
	{
		public override void Up()
		{
			const string EMAIL_QUEUE_TABLE = "EmailQueue";

			if (!Schema.Table(EMAIL_QUEUE_TABLE).Exists())
			{
				Create.Table(EMAIL_QUEUE_TABLE)
					.WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
					.WithColumn("UserFk").AsInt32().NotNullable()
					.WithColumn("EmailTemplate").AsString().NotNullable()
					.WithColumn("Status").AsString().NotNullable().WithDefaultValue("QUEUED")
					.WithColumn("DateQueued").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
					.WithColumn("SendAfter").AsDateTime().NotNullable().WithDefaultValue(SystemMethods.CurrentDateTime)
					.WithColumn("DateSent").AsDateTime().Nullable()
					.WithColumn("RetryCount").AsInt16().NotNullable().WithDefaultValue(0);

				MigrationHelper.CreateForeignKeyIfNotExist(
					this,
					EMAIL_QUEUE_TABLE,
					"UserFk",
					"Users",
					"Id");

				const string INDEX_NAME = "IX_EmailQueue_Status_SendAfter";

				if (!Schema.Table(EMAIL_QUEUE_TABLE).Index(INDEX_NAME).Exists())
				{
					Execute.Sql(
						//MigrationHelper.GetCurrentMigrationEnvironment() == MigrationEnvironment.Development?
						$"CREATE NONCLUSTERED INDEX {INDEX_NAME} ON dbo.{EMAIL_QUEUE_TABLE}(Status ASC, SendAfter ASC) WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)");
				}
			}
		}
	}
}
