using AutoMapper;
using Project.Application.ViewModels;
using Project.Domain.Entities;

namespace Project.Application
{
    public class ConfigurationMapping : Profile
    {
        public ConfigurationMapping()
        {
            CreateMap<Entry, EntryVM>().ReverseMap();
            CreateMap<Entry, ConsolidatedReportResultItemVM>().ReverseMap();
            CreateMap<Logs, LogsVM>().ReverseMap();
        }
    }
}
