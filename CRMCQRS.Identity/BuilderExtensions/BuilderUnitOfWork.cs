using CRMCQRS.Infrastructure.Database;
using CRMCQRS.Infrastructure.UnitOfWork;

namespace CRMCQRS.Identity.BuilderExtensions
{
    public static class BuilderUnitOfWork 
    {
        public static void ConfigureUnitOfWorkServices(this IServiceCollection services)
        {
            services.AddUnitOfWork<DefaultDbContext>();
        }
    }
}
