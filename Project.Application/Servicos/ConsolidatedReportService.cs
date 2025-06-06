using AutoMapper;
using Project.Application.Interfaces;
using Project.Application.Utils;
using Project.Application.ViewModels;
using Project.Domain;

namespace Project.Application.Servicos
{
    public class ConsolidatedReportService : IConsolidatedReportService
    {
        private readonly IMapper _mapper;
        private readonly IEntryRepository _entryRepository;
        private readonly ILogsService _logsService;

        public ConsolidatedReportService(IMapper mapper, IEntryRepository entryRepository, ILogsService logsService)
        {
            _mapper = mapper;
            _entryRepository = entryRepository;
            _logsService = logsService;
        }

        public async Task<CustomResult<ConsolidatedReportResultVM>> GenerateReport(string email, ConsolidatedReportVM parameters)
        {
            try
            {
                await _logsService.Add(email, "ConsolidatedReportService", "GenerateReport", string.Empty);

                var result = new ConsolidatedReportResultVM
                {
                    InitialDate = parameters.InitialDate,
                    FinalDate = parameters.FinalDate,
                    Items = new List<ConsolidatedReportResultItemVM>()
                };

                var resulSet = await _entryRepository.GetAllByPeriod(parameters.InitialDate, parameters.FinalDate);

                if (!parameters.CreditAndDebit)
                {
                    if (parameters.OnlyCredit)
                    {
                        resulSet = resulSet.Where(x => x.IsCredit);
                    }
                    else
                    {
                        resulSet = resulSet.Where(x => !x.IsCredit);
                    }
                }

                foreach (var item in resulSet)
                {
                    var itemMap = _mapper.Map<ConsolidatedReportResultItemVM>(item);
                    result.Items.Add(itemMap);
                }

                result.TotalValue = result.Items.Sum(p => p.Value);

                return CustomResult<ConsolidatedReportResultVM>.Success(result);
            }
            catch (Exception ex)
            {
                await _logsService.Add(email, "ConsolidatedReportService", "GenerateReport", ex.Message);

                return CustomResult<ConsolidatedReportResultVM>.Failure(CustomError.ExceptionError(ex.Message));
            }
        }
    }
}
