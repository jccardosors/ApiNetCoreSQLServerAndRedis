namespace Project.Application.ViewModels
{
    public class ConsolidatedReportResultVM
    {
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public decimal TotalValue { get; set; }
        public List<ConsolidatedReportResultItemVM> Items { get; set; }
    }
}
