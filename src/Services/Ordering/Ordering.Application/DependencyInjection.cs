using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ordering.Application;

public static class DependencyInjection
{
  public static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {
    // 여기에서 MediatR 이 container 에 등록되므로, infrastructure layer 에서 별도 injection 하지 않아도 된다.
    services.AddMediatR(cfg =>
    {
      cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    });

    return services;
  }
}