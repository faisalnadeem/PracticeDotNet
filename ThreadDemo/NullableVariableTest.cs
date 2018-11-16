namespace ThreadDemo
{
    public class NullableVariableTest
    {
        public static void TestNullable()
        {
            var user = new User(){Id = 1, Name = "Testuser", ResidenceId = 123};

            var userDispute = new UserDispute("dispute desc", user.Id, user.ResidenceId.GetValueOrDefault());

        }
    }
}
