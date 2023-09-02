using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Via.Domain.Entities;

namespace Via.Persistence.EfCoreConfiguration;

public class DenemeConfiguration : IEntityTypeConfiguration<Deneme>
{
    public void Configure(EntityTypeBuilder<Deneme> builder)
    {
        builder.ToTable("Denemes").HasKey(i => i.Id);
        builder.HasIndex(b => b.Name).IsUnique();
        builder.HasQueryFilter(i => !i.DeletedDate.HasValue);
    }
}
