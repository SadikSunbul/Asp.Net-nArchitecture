using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Via.Application.Services.Repositories;
using Via.Persistence.Context;
using Via.Persistence.Repositories;

namespace Via.Persistence;

public static class PersistenceServicesRegisretions
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Veritabanı bağlantıları

        //services.AddDbContext<ViaBaseContext>(cof => cof.UseInMemoryDatabase("ViaInmemoryDatabase"));
        services.AddDbContext<ViaBaseContext>(cof => cof.UseSqlServer(configuration.GetConnectionString("mssqlSadıkDocker")));
        //services.AddDbContext<ViaBaseContext>(cof=>cof.UseSqlServer(configuration.GetConnectionString("mssqlViaSunucu")));



        //RepositroyIoc Kaydı
        services.AddScoped<IDenemeRepository, DenemeRepositroy>();


        return services;
    }
}
