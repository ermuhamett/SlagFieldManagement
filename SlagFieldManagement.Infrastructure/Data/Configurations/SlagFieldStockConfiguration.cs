using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlagFieldManagement.Domain.Entities;

namespace SlagFieldManagement.Infrastructure.Data.Configurations;

internal sealed class SlagFieldStockConfiguration:IEntityTypeConfiguration<SlagFieldStock>
{
    public void Configure(EntityTypeBuilder<SlagFieldStock> builder)
    {
        builder.ToTable("SlagFieldStocks");
        builder.HasKey(s => s.Id); // Первичный ключ Id типа Guid
        builder.Property(s => s.SlagFieldStateId); // SlagFieldStateId необязателен (один к одному)
        builder.Property(s => s.MaterialId)
            .IsRequired(); // MaterialId обязателен
        builder.Property(s => s.Total)
            .HasPrecision(18, 3); // Total с точностью 18,3
        builder.Property(s => s.ApplicationId); // ApplicationId необязателен
        builder.Property(s => s.TransactionType)
            .IsRequired()
            .HasMaxLength(50); // TransactionType обязателен, максимальная длина 50 символов
        builder.Property(s => s.IsDelete)
            .IsRequired(); // IsDelete обязателен

        // Связь с Material (многие к одному)
        builder.HasOne<Material>()
            .WithMany()
            .HasForeignKey(s => s.MaterialId)
            .OnDelete(DeleteBehavior.Restrict); // Ограничение на удаление

        // Связь с SlagFieldState (один к одному) уже настроена в SlagFieldStateConfiguration
    }
}