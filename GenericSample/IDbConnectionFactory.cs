using System.Data;

namespace GenericSample
{
	public interface IDbConnectionFactory
	{
		IDbConnection CreateConnection();
		T CreateConnection<T>();
	}
}