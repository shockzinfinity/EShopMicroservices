using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration.GetConnectionString("Database");

    services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
    services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

    services.AddDbContext<ApplicationDbContext>((sp, options) =>
    {
      // 이렇게 injection 하면 MediatR 또 injection 해야하는 방식이므로
      // DI 에서 자동으로 해결하게 변경
      //options.AddInterceptors(new AuditableEntityInterceptor(), new DispatchDomainEventsInterceptor());
      options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
      options.UseSqlServer(connectionString);
    });

    return services;
  }
}