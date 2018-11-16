namespace ThreadDemo
{
    internal class UserDispute
    {
        public UserDispute(string description, int userid, int userResidenceId)
        {
            Description = description;
            UserId = userid;
            UserResidenceId = userResidenceId;

        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public int UserResidenceId { get; set; }
    }
}