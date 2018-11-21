using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace GenerateTestUsers
{
    class Program
    {
        static void Main(string[] args)
        {
	        Console.WriteLine("Generate test users: Press any key to exit...");
	        Console.ReadKey();
		}
    }

    public class DapperConnectionHelper
    {
        public void ExecuteSql()
        {
            string sql = "INSERT INTO Customers (CustomerName) Values (@CustomerName);";

            using (var connection = new SqlConnection("Data Source=SqlCe_W3Schools.sdf"))
            {
                var affectedRows = connection.Execute(sql, new { CustomerName = "Mark" });

                Console.WriteLine(affectedRows);

                //var customer = connection.Query<Customer>("Select * FROM CUSTOMERS WHERE CustomerName = 'Mark'").ToList();
                //FiddleHelper.WriteTable(customer);
            }
        }
    }
}
