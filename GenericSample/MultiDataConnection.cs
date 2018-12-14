using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = System.Console;

namespace GenericSample
{
	public interface IDatabase
	{
		void SaveToDatabase();
		void ReadFromDatabase();
	}

	public class MySQLDatabase : IDatabase
	{
		public MySQLDatabase()
		{
			//init stuff
			Console.WriteLine($"{this.GetType().FullName}");
		}

		public void SaveToDatabase()
		{
			//MySql implementation
			Console.WriteLine($"{this.GetType().FullName} - SaveToDatabase");
		}
		public void ReadFromDatabase()
		{
			//MySql implementation
			Console.WriteLine($"{this.GetType().FullName} - ReadFromDatabase");
		}
	}

	public class SQLLiteDatabase : IDatabase
	{
		public SQLLiteDatabase()
		{
			//init stuff
			Console.WriteLine($"{this.GetType().FullName}");
		}

		public void SaveToDatabase()
		{
			//SQLLite implementation
			Console.WriteLine($"{this.GetType().FullName} - SaveToDatabase");
		}
		public void ReadFromDatabase()
		{
			//SQLLite implementation
			Console.WriteLine($"{this.GetType().FullName} - ReadFromDatabase");
		}
	}

	//Application
	public class TestDbConnection
	{
		private readonly string _dbConnectionType;
		public IDatabase db;

		public TestDbConnection(string dbConnectionType)
		{
			_dbConnectionType = dbConnectionType;
			db = GetDatabase();
		}

		public void SaveData()
		{
			db.SaveToDatabase();
		}
		
		public void ReadData()
		{
			db.ReadFromDatabase();
		}

		private IDatabase GetDatabase()
		{
			//var _dbConnectionType = "Sql";
			if (_dbConnectionType == "Sql")
				return new MySQLDatabase();
			else if (_dbConnectionType == "SqlLite")
				return new SQLLiteDatabase();

			throw new Exception("You forgot to configure the database!");
		}
	}
}
