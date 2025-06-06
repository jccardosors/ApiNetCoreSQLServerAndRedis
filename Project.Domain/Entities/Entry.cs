namespace Project.Domain.Entities
{
    public class Entry
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal ValueEntry { get; set; }
        public DateTime DateEntry { get; set; }
        public bool IsCredit { get; set; }

    }
}
