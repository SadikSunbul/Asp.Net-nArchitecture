using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Via.Domain.Entities;

namespace Via.Persistence.Context;

public class ViaBaseContext:DbContext
{
    public IConfiguration Configuration { get; }
    public ViaBaseContext(DbContextOptions opt,IConfiguration configuration) : base(opt)
    {
        Configuration = configuration;
        
    }

    #region DbSet<> Properties
    public DbSet<Deneme> Denemes { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
