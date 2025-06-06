namespace Project.Application.ViewModels
{
    public class ConsolidatedReportVM
    {
        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }

        public bool CreditAndDebit { get; set; } = true;

        public bool OnlyCredit { get; set; } = false;
    }
}
