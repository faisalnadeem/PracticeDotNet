namespace GenerateTestUsers
{
	public interface IPasswordHolder
	{
		string HashedPassword { get; }
		string Salt { get; }
	}
}