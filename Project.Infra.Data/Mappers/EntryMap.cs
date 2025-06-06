using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;

namespace Project.Infra.Data.Mappers
{
    public class EntryMap : IEntityTypeConfiguration<Entry>
    {
        public void Configure(EntityTypeBuilder<Entry> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(100);
            builder.Property(p => p.ValueEntry).HasPrecision(18, 2);
            builder.Property(p => p.DateEntry).IsRequired();
            builder.Property(p => p.IsCredit).IsRequired();

            builder.HasIndex(p => p.Id).IsUnique();

            builder.ToTable("TB_ENTRY");
        }
    }
}
