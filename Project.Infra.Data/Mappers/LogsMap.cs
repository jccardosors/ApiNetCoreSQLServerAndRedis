using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;

namespace Project.Infra.Data.Mappers
{
    public class LogsMap : IEntityTypeConfiguration<Logs>
    {
        public void Configure(EntityTypeBuilder<Logs> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(100);
            builder.Property(p => p.IdUser).HasColumnName("ID_USUARIO"); ;
            builder.Property(p => p.Data).IsRequired();

            builder.HasIndex(p => p.Id).IsUnique();

            builder.ToTable("TB_LOGS");
        }
    }
}
