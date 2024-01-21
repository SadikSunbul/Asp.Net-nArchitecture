using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ViabelliWebProject.Packages.Core.Security.Entities;

namespace Via.Persistence.EfCoreConfiguration;

public class OtpAuthenticatorConfiguration : IEntityTypeConfiguration<OtpAuthenticator>
{
    public void Configure(EntityTypeBuilder<OtpAuthenticator> builder)
    {
        builder.ToTable("OtpAuthenticators").HasKey(oa => oa.Id);

        builder.Property(oa => oa.Id).HasColumnName("Id").IsRequired();
        builder.Property(oa => oa.UserId).HasColumnName("UserId").IsRequired();
        builder.Property(oa => oa.SecretKey).HasColumnName("SecretKey").IsRequired();
        builder.Property(oa => oa.IsVerified).HasColumnName("IsVerified").IsRequired();
        builder.Property(oa => oa.CreateDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(oa => oa.CreateDate).HasColumnName("UpdatedDate");
        builder.Property(oa => oa.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(oa => !oa.DeletedDate.HasValue);

        builder.HasOne(oa => oa.User);
    }
}