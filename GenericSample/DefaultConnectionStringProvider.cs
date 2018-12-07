namespace GenericSample
{
	public class DefaultConnectionStringProvider: IConnectionStringProvider
	{
		public string GetConnectionString()
		{
			return @"Server=PLLLP9435;initial catalog=EmailPoc;Integrated Security=true";
			//return ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
		}
	}
}