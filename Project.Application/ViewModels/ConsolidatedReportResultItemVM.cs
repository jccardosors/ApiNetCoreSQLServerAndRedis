namespace Project.Application.ViewModels
{
    public class ConsolidatedReportResultItemVM
    {
        public DateTime DateEntry { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public bool IsCredit { get; set; }
    }
}
