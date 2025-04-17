using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlagFieldManagement.Domain.Entities;

namespace SlagFieldManagement.Infrastructure.Data.Configurations;

internal sealed class RoleConfiguration:IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(r => r.Description)
            .HasMaxLength(500);
        builder.Property(r => r.CreatedAt)
            .IsRequired();

        // Уникальный индекс для названия роли
        builder.HasIndex(r => r.Name).IsUnique();
    }
}