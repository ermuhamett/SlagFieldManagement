using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;
using SlagFieldManagement.Domain.Aggregates.SlagFieldState;
using SlagFieldManagement.Domain.Entities;

namespace SlagFieldManagement.Infrastructure.Data.Configurations;

public class SlagFieldStateConfiguration:IEntityTypeConfiguration<SlagFieldState>
{
    public void Configure(EntityTypeBuilder<SlagFieldState> builder)
    {
        builder.ToTable("SlagFieldStates");
        builder.HasKey(s => s.Id); // Первичный ключ Id типа Guid
        builder.Property(s => s.PlaceId)
            .IsRequired(); // PlaceId обязателен
        builder.Property(s => s.BucketId); // BucketId необязателен
        builder.Property(s => s.StartDate)
            .IsRequired(); // StartDate обязателен
        builder.Property(s => s.EndDate); // EndDate необязателен
        builder.Property(s => s.MaterialId); // MaterialId необязателен
        builder.Property(s => s.SlagWeight)
            .HasPrecision(18, 3); // SlagWeight с точностью 18,3
        builder.Property(s => s.State)
            .IsRequired()
            .HasConversion(
                v => v.ToString(),          // Конвертация в строку при сохранении в БД
                v => (StateFieldType)Enum.Parse(typeof(StateFieldType), v) // Обратная конвертация
            )
            .HasMaxLength(15); // State обязателен, максимальная длина 15 символов
        builder.Property(s => s.Description)
            .HasMaxLength(250); // Description необязателен, максимальная длина 250 символов
        builder.Property(s => s.IsDelete)
            .IsRequired(); // IsDelete обязателен

        // Связь с SlagFieldPlace (многие к одному)
        builder.HasOne<SlagFieldPlace>()
            .WithMany()
            .HasForeignKey(s => s.PlaceId)
            .OnDelete(DeleteBehavior.Restrict); // Ограничение на удаление

        // Связь с Bucket (многие к одному)
        builder.HasOne<Bucket>()
            .WithMany()
            .HasForeignKey(s => s.BucketId)
            .OnDelete(DeleteBehavior.Restrict); // Ограничение на удаление

        // Связь с Material (многие к одному)
        builder.HasOne<Material>()
            .WithMany()
            .HasForeignKey(s => s.MaterialId)
            .OnDelete(DeleteBehavior.Restrict); // Ограничение на удаление

        // Связь с SlagFieldStock (один к одному)
        builder.HasOne<SlagFieldStock>()
            .WithOne()
            .HasForeignKey<SlagFieldStock>(stock => stock.SlagFieldStateId)
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление
    }
}