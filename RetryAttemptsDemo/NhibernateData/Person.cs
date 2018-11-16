namespace NhibernateWithRetry.NhibernateData
{
    public class Person
    {
        public virtual int Id { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual Car Car { get; set; }
    }
}