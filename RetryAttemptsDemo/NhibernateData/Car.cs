namespace NhibernateWithRetry.NhibernateData
{
    public class Car
    {
        public virtual int Id { get; set; }
        public virtual string Make { get; set; }
        public virtual string Model { get; set; }
        public virtual string Year { get; set; }
    }
}