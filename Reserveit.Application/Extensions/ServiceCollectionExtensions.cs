using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Reserveit.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddAutoMapper(cfg => { },
                       applicationAssembly);

        services.AddValidatorsFromAssembly(applicationAssembly);
    }
}
