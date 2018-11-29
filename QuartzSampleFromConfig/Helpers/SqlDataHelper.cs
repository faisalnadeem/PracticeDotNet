using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using IsolationLevel = System.Data.IsolationLevel;

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

		public static void MarkAsErroredByEmailQueueId(int id)
		{
			var sqlQuery = $@"Update DBO.EmailQUEUE Set DateSent = NULL , IsErrored = 1 WHERE ID = @id";
			using (var connection = new SqlConnection(_connectionString))
			{
				var command = new SqlCommand(sqlQuery, connection);
				command.Parameters.AddWithValue("@id", id);	
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

		public static void MarkAsConsumedByEmailQueueId(int id, SqlConnection connection)
		{
			var sqlQuery = $@"Update DBO.EmailQUEUE Set DateSent = @dateSent WHERE ID = @id";
				var command = new SqlCommand(sqlQuery, connection);
				command.Parameters.AddWithValue("@id", id);
				command.Parameters.AddWithValue("@dateSent", DateTime.Now);

				//command.Connection.Open();
				command.ExecuteNonQuery();
		}

		public static void MarkAsConsumedByEmailQueueIdHistory(int id, string threadName)
		{
			var sqlQuery = $@"insert into EmailQueueUpdateHistory (EmailQueueId, UpdateByThreadName, UpdatedDate)
								values (@emailRowId, @threadname, @updateddate)";
			using (var connection = new SqlConnection(_connectionString))
			{
				var command = new SqlCommand(sqlQuery, connection);
				command.Parameters.AddWithValue("@emailRowId", id);
				command.Parameters.AddWithValue("@threadname", threadName);
				command.Parameters.AddWithValue("@updateddate", DateTime.Now);

				command.Connection.Open();
				command.ExecuteNonQuery();
			}
		}

		public static void MarkAsConsumedByEmailQueueIdHistory(int id, string threadName, SqlConnection connection)
		{
			var sqlQuery = $@"insert into EmailQueueUpdateHistory (EmailQueueId, UpdateByThreadName, UpdatedDate)
								values (@emailRowId, @threadname, @updateddate)";
				var command = new SqlCommand(sqlQuery, connection);
				command.Parameters.AddWithValue("@emailRowId", id);
				command.Parameters.AddWithValue("@threadname", threadName);
				command.Parameters.AddWithValue("@updateddate", DateTime.Now);

				//command.Connection.Open();
				command.ExecuteNonQuery();
		}

		public static EmailQueue GetNextUserIdToSendEmail()
		{
			//using (var scope = new TransactionScope())
			//{
			var emailRows = new List<EmailQueue>();
			var sqlQuery =
				@"  SELECT TOP 1 eq.Id, eq.UserId, eq.EmailTemplate FROM dbo.EmailQueue eq WITH (ROWLOCK, UPDLOCK) WHERE eq.DateSent IS NULL AND eq.IsErrored = 0";
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

			//scope.Complete();
			return emailRows.FirstOrDefault();
			//}
		}

		public static EmailQueue SelectAndUpdateNextUserIdToSendEmail(string threadName)
		{
			using (var scope = new TransactionScope())
			{
				var emailRows = new List<EmailQueue>();
				EmailQueue emailRow = null;
				var sqlQuery = "";

				using (var connection = new SqlConnection(_connectionString))
				{
					connection.Open();
					//using (var tx = connection.BeginTransaction(IsolationLevel.Serializable))
					//{
					//	tx.Commit();
					//}
					sqlQuery =
						@"SELECT TOP 1 eq.Id, eq.UserId, eq.EmailTemplate FROM dbo.EmailQueue eq WITH (ROWLOCK, UPDLOCK) WHERE eq.DateSent IS NULL AND eq.IsErrored = 0";
					using (var command = new SqlCommand(sqlQuery, connection))
					{
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

						dataReader.Close();
					}

					emailRow = emailRows.FirstOrDefault();

					if (emailRow != null)
					{
						sqlQuery = $@"Update DBO.EmailQUEUE Set DateSent = @dateSent WHERE ID = @id";

						using (var command = new SqlCommand(sqlQuery, connection))
						{
							command.Parameters.AddWithValue("@id", emailRow.Id);
							command.Parameters.AddWithValue("@dateSent", DateTime.Now);
							//command.Connection.Open();
							command.ExecuteNonQuery();
						}

						sqlQuery = $@"insert into EmailQueueUpdateHistory (EmailQueueId, UpdateByThreadName, UpdatedDate)
								values (@emailRowId, @threadname, @updateddate)";
						using (var command = new SqlCommand(sqlQuery, connection))
						{
							command.Parameters.AddWithValue("@emailRowId", emailRow.Id);
							command.Parameters.AddWithValue("@threadname", threadName);
							command.Parameters.AddWithValue("@updateddate", DateTime.Now);
							//command.Connection.Open();
							command.ExecuteNonQuery();
						}
					}
				}

				scope.Complete();
				return emailRow;
			}
		}

		public static EmailQueue SelectAndUpdateNextUserIdToSendEmailInlineTxn(string threadName)
		{
			var emailRows = new List<EmailQueue>();
			EmailQueue emailRow = null;

			using (var connection = new SqlConnection(_connectionString))
			{
				var sqlQuery = @"
						Begin Transaction
						declare @eid int
						SELECT TOP 1 @eid = eq.Id FROM dbo.EmailQueue eq WITH ( UPDLOCK, READPAST) WHERE eq.DateSent IS NULL AND eq.IsErrored = 0
						if (@eid is not null)
							begin
								update EmailPoc.dbo.EmailQueue set DateSent = GETDATE() where Id = @eid
								insert into dbo.EmailQueueUpdateHistory (EmailQueueId, UpdateByThreadName, UpdatedDate)
								values (@eid, @threadname, GETDATE())
							end
						SELECT eq.Id, eq.UserId, eq.EmailTemplate FROM dbo.EmailQueue eq WHERE eq.id = @eid
						Commit Transaction
						";
				using (var command = new SqlCommand(sqlQuery, connection))
				{
					command.Parameters.AddWithValue("@threadname", threadName);
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

					dataReader.Close();
				}

				emailRow = emailRows.FirstOrDefault();
			}

			return emailRow;
		}

		public static EmailQueue GetNextUserIdToSendEmail(SqlConnection connection)
		{
			//using (var scope = new TransactionScope())
			//{
			var emailRows = new List<EmailQueue>();
			var sqlQuery =
				@"  SELECT TOP 1 eq.Id, eq.UserId, eq.EmailTemplate FROM dbo.EmailQueue eq WITH (ROWLOCK, UPDLOCK) WHERE eq.DateSent IS NULL AND eq.IsErrored = 0";
			//command.Connection.Open();
			using (var command = new SqlCommand(sqlQuery, connection))
			using (var dataReader = command.ExecuteReader())
			{
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

			//scope.Complete();
			return emailRows.FirstOrDefault();
			//}
		}

	}

	public class EmailQueue
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public string EmailTemplate { get; set; }

	}
}
