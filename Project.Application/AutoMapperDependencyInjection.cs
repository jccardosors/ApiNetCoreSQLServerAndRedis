using Microsoft.Extensions.DependencyInjection;

namespace Project.Application
{
    public static class AutoMapperDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ConfigurationMapping));
            return services;
        }
    }
}
