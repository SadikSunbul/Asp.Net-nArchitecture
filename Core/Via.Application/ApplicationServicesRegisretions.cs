using FluentValidation;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ViabelliWebProject.Packages.Core.Application.Piplines.Cachings;
using ViabelliWebProject.Packages.Core.Application.Piplines.FluentValidation;
using ViabelliWebProject.Packages.Core.Application.Piplines.Logging;
using ViabelliWebProject.Packages.Core.Application.Piplines.TransectionScopes;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.Rules;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.SeriLog;
using ViabelliWebProject.Packages.Core.CrossCuttingConcerns.SeriLog.Loggers;

namespace Via.Application;

public static class ApplicationServicesRegisretions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services.AddMediatR(configure =>
        {
            configure.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());

            configure.AddOpenBehavior(typeof(RequestValidaterBehrivor<,>));
            configure.AddOpenBehavior(typeof(TransectionScopeBehavior<,>));
            configure.AddOpenBehavior(typeof(CachengBehavior<,>));
            configure.AddOpenBehavior(typeof(CacheRemoverRequestBehevior<,>));
            configure.AddOpenBehavior(typeof(LogginBehavior<,>));
        });

        //services.AddSingleton<LoggerServiceBase, FileLogger>();
        services.AddSingleton<LoggerServiceBase, MsSqlLogger>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddSubClasesOfType(Assembly.GetExecutingAssembly(), typeof(BaseRules));

        return services;
    }

    public static IServiceCollection AddSubClasesOfType(
        this IServiceCollection services,
        Assembly assembly,
        Type type,
        Func<IServiceCollection, Type, IServiceCollection>? adWithLifeCycle = null)
    {
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();

        foreach (var item in types)
        {
            if (adWithLifeCycle == null)
            {
                services.AddScoped(item);
            }
            else
            {
                adWithLifeCycle(services, type);
            }
        }
        return services;

    }
}
