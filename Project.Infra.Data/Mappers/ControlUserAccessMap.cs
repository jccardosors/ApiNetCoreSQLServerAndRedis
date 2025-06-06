using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;

namespace Project.Infra.Data.Mappers
{
    public class ControlUserAccessMap : IEntityTypeConfiguration<ControlUserAccess>
    {
        public void Configure(EntityTypeBuilder<ControlUserAccess> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.LastAccess).IsRequired().HasMaxLength(100).HasColumnName("LAST_ACCESS"); ;
            builder.Property(p => p.TryNumber).IsRequired().HasColumnName("TRY_NUMBER");
            builder.Property(p => p.UserEmail).IsRequired().HasColumnName("USER_EMAIL");
            builder.Property(p => p.Blocked).IsRequired();

            builder.HasIndex(p => p.Id).IsUnique();

            builder.ToTable("TB_USUARIO_CTRL_ACESSO");
        }
    }
}
