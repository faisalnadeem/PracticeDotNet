using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace QuartzSampleFromConfig.Helpers
{
	public class SqlDataHelper
	{
		static string _connectionString = @"Server=PLLLP9435;initial catalog=EmailPoc;Integrated Security=true";

		public static void PopulateEmailQueueTable()
		{
			var sqlQuery = @"  INSERT INTO dbo.EmailQueue ( UserId, EmailTemplate)
								SELECT Id, 'DEFAULT' FROM Yellow.dbo.Users";
			using (var connection = new SqlConnection(_connectionString))
			{
				var command = new SqlCommand(sqlQuery, connection);
				command.Connection.Open();
				command.ExecuteNonQuery();
			}
		}

		public static void DeleteEmailRowById(int id)
		{
			var sqlQuery = $@"DELETE FROM DBO.EmailQUEUE  WHERE ID = {id}";
			using (var connection = new SqlConnection(_connectionString))
			{
				var command = new SqlCommand(sqlQuery, connection);
				command.Connection.Open();
				command.ExecuteNonQuery();
			}
		}

		public static void MarkAsConsumedByEmailQueueId(int id)
		{
			var sqlQuery = $@"Update DBO.EmailQUEUE Set DateSent = @dateSent WHERE ID = @id";
			using (var connection = new SqlConnection(_connectionString))
			{
				var command = new SqlCommand(sqlQuery, connection);
				command.Parameters.AddWithValue("@id", id);
				command.Parameters.AddWithValue("@dateSent", DateTime.Now);

				command.Connection.Open();
				command.ExecuteNonQuery();
			}
		}

		public static EmailQueue GetNextUserIdToSendEmail()
		{
			var emailRows = new List<EmailQueue>();
			var sqlQuery = @"  SELECT TOP 1 eq.Id, eq.UserId, eq.EmailTemplate FROM dbo.EmailQueue eq WITH (ROWLOCK, UPDLOCK) WHERE eq.DateSent IS NULL AND eq.IsErrored = 0";
			using (var connection = new SqlConnection(_connectionString))
			{
				var command = new SqlCommand(sqlQuery, connection);
				command.Connection.Open();
				var dataReader = command.ExecuteReader();
				while (dataReader.Read())
					{
						emailRows.Add(new EmailQueue()
						{
							Id = Convert.ToInt32(dataReader["Id"]),
							UserId = Convert.ToInt32(dataReader["UserId"]),
							EmailTemplate = Convert.ToString(dataReader["EmailTemplate"])
						});
					}
			}

			return emailRows.FirstOrDefault();
		}

	}

	public class EmailQueue
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string EmailTemplate { get; set; }

	}
}
