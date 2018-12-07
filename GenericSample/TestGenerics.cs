using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace GenericSample
{
    public class TestGenerics : ITestGenerics
	{
	    private readonly IDbConnectionFactory _dbConnectionFactory;

	    public TestGenerics(IDbConnectionFactory dbConnectionFactory)
	    {
		    _dbConnectionFactory = dbConnectionFactory;
	    }

	    public void Test()
	    {
		    var stopWatch = Stopwatch.StartNew();
		    var connection = CreateSqlConnection();
			Console.WriteLine($"Non generice: time elapsed {stopWatch.ElapsedTicks}" );
			stopWatch.Stop();
	    }

		public void TestGeneric()
	    {
		    var stopWatch = Stopwatch.StartNew();
		    var connection = _dbConnectionFactory.CreateConnection<SqlConnection>();
			Console.WriteLine($"Generice: time elapsed {stopWatch.ElapsedTicks}" );
			stopWatch.Stop();
	    }

		private SqlConnection CreateSqlConnection()
		{
			return (SqlConnection) _dbConnectionFactory.CreateConnection();
			//return _dbConnectionFactory.CreateConnection<SqlConnection>();
		}
	}
}
