namespace Project.Domain.Entities
{
    public class ControlUserAccess
    {
        public int Id { get; set; }
        public DateTime LastAccess { get; set; }
        public int TryNumber { get; set; }
        public bool Blocked { get; set; }
        public string UserEmail { get; set; }
    }
}
