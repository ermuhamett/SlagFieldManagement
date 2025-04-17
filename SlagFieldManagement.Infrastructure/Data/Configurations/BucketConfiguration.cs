using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlagFieldManagement.Domain.Entities;

namespace SlagFieldManagement.Infrastructure.Data.Configurations;

internal sealed class BucketConfiguration:IEntityTypeConfiguration<Bucket>
{
    public void Configure(EntityTypeBuilder<Bucket> builder)
    {
        builder.ToTable("Buckets");
        builder.HasKey(b => b.Id); // Первичный ключ Id типа Guid
        builder.Property(b => b.Description)
            .IsRequired();
        builder.Property(b => b.IsDelete)
            .IsRequired(); // IsDelete обязателен
    }
}