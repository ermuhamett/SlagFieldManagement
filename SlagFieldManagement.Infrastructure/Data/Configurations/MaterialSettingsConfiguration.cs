using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlagFieldManagement.Domain.Entities;

namespace SlagFieldManagement.Infrastructure.Data.Configurations;

internal sealed class MaterialSettingsConfiguration:IEntityTypeConfiguration<MaterialSettings>
{
    public void Configure(EntityTypeBuilder<MaterialSettings> builder)
    {
        builder.ToTable("MaterialSettings");
        builder.HasKey(ms => ms.Id); // Первичный ключ Id типа Guid из базового класса Entity
        builder.Property(ms => ms.MaterialId)
            .IsRequired(); // Внешний ключ MaterialId обязателен
        builder.Property(ms => ms.StageName)
            .IsRequired()
            .HasMaxLength(50); // StageName обязателен, максимальная длина 50 символов
        builder.Property(ms => ms.EventType)
            .HasMaxLength(50); // EventType необязателен (nullable), максимальная длина 50 символов
        builder.Property(ms => ms.Duration)
            .IsRequired(); // Duration обязателен
        builder.Property(ms => ms.VisualStateCode)
            .HasMaxLength(50); // VisualStateCode необязателен (nullable), максимальная длина 50 символов
        builder.Property(ms => ms.MinHours)
            .HasPrecision(18, 2); // MinHours необязателен (nullable), точность 18,2
        builder.Property(ms => ms.MaxHours)
            .HasPrecision(18, 2); // MaxHours необязателен (nullable), точность 18,2
        builder.Property(ms => ms.IsDelete)
            .IsRequired(); // IsDelete обязателен

        // Связь с Material (многие к одному)
        builder.HasOne<Material>()
            .WithMany() // Обратной коллекции в Material пока нет, но связь один ко многим
            .HasForeignKey(ms => ms.MaterialId)
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление
    }
}