using Microsoft.Extensions.DependencyInjection;
using Project.Application.Interfaces;
using Project.Application.Servicos;
using Project.Domain;
using Project.Infra.Data.Repositories;

namespace Project.Infra.Ioc
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //application
            services.AddScoped<IEntryService, EntryService>();
            // services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILogsService, LogsService>();
            //services.AddScoped<IControlUserAccessService, ControlUserAccessService>();
            services.AddScoped<IAutenticateService, AutenticateService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IConsolidatedReportService, ConsolidatedReportService>();

            //domain and Infra.data
            services.AddScoped<IEntryRepository, EntryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILogsRepository, LogsRepository>();
            services.AddScoped<IControlUserAccessRepository, ControlUserAccessRepository>();


        }
    }
}
