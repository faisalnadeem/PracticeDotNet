using System;
using System.Data;
using System.Data.SqlClient;

namespace GenericSample
{
	public class SqlConnectionFactory : IDbConnectionFactory
	{
		private readonly IConnectionStringProvider _connectionStringProvider;

		public SqlConnectionFactory(IConnectionStringProvider connectionStringProvider)
		{
			_connectionStringProvider = connectionStringProvider;
		}

		public IDbConnection CreateConnection()
		{
			var conn = new SqlConnection(_connectionStringProvider.GetConnectionString());
			conn.Open();
			return conn;
		}

		public T CreateConnection<T>()
		{
			IDbConnection connection = null;
			if (typeof(T) ==  new SqlConnection().GetType())
			{
				connection = new SqlConnection(_connectionStringProvider.GetConnectionString());
				connection.Open();
			}

			return (T) Convert.ChangeType(connection, typeof(T));
		}
	}
}