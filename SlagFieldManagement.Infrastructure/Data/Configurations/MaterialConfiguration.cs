using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlagFieldManagement.Domain.Entities;

namespace SlagFieldManagement.Infrastructure.Data.Configurations;

internal sealed class MaterialConfiguration:IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("Materials");
        builder.HasKey(m => m.Id); // Первичный ключ Id типа Guid из базового класса Entity
        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(250); // Поле Name обязательно, максимальная длина 250 символов
        builder.Property(m => m.IsDelete)
            .IsRequired(); // Поле IsDelete обязательно

        // Связь с MaterialSettings (один ко многим)
        // В коде указано поле Settings, но это единичная ссылка, а не коллекция.
        // Предположим, что в будущем может быть коллекция, поэтому настроим как один ко многим
        builder.HasMany<MaterialSettings>()
            .WithOne() // В MaterialSettings нет обратной навигационной ссылки
            .HasForeignKey(ms => ms.MaterialId)
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление
    }
}