using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace NHibernateMigration.DataMigrations.Helpers
{
	public class MigrationHelper
	{
		public static void CreateForeignKeyIfNotExist(
			Migration migration,
			string tableName,
			string columnName,
			string referencedTableName,
			string referencedColumnName)
		{
			migration.Execute.WithConnection(
				(connection, transaction) =>
				{
					string getForeignKeyNameSql = string.Format(@"
SELECT 
    f.name AS ForeignKey
FROM 
    sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.OBJECT_ID = fc.constraint_object_id
where 
    OBJECT_NAME(f.parent_object_id) = '{0}' -- TableName
    and COL_NAME(fc.parent_object_id, fc.parent_column_id) = '{1}' -- ColumnName
    and OBJECT_NAME (f.referenced_object_id) = '{2}' -- ReferenceTableName
    and COL_NAME(fc.referenced_object_id, fc.referenced_column_id) = '{3}'
", tableName, columnName, referencedTableName, referencedColumnName);

					var getForeignKeyNameCmd = connection.CreateCommand();
					getForeignKeyNameCmd.CommandText = getForeignKeyNameSql;
					getForeignKeyNameCmd.Transaction = transaction;
					var foreignKeyName = (string)getForeignKeyNameCmd.ExecuteScalar();

					if (string.IsNullOrWhiteSpace(foreignKeyName))
					{
						var createCommandSql =
							string.Format(
								"ALTER TABLE[dbo].[{0}] ADD CONSTRAINT[FK_{0}_{1}_{2}_{3}] FOREIGN KEY([{1}]) REFERENCES[dbo].[{2}]([{3}])",
								tableName,
								columnName,
								referencedTableName,
								referencedColumnName);
						var createFkCommand = connection.CreateCommand();
						createFkCommand.CommandText = createCommandSql;
						createFkCommand.Transaction = transaction;
						createFkCommand.ExecuteNonQuery();
					}
				});
		}
	}
}
