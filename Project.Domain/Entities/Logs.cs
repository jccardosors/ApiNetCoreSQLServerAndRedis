namespace Project.Domain.Entities
{
    public class Logs
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Description { get; set; }
        public DateTime Data { get; set; }
    }
}
