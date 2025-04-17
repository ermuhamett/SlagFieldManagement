using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;

namespace SlagFieldManagement.Infrastructure.Data.Configurations;

internal sealed class SlagFieldPlaceConfiguration:IEntityTypeConfiguration<SlagFieldPlace>
{
    public void Configure(EntityTypeBuilder<SlagFieldPlace> builder)
    {
        builder.ToTable("SlagFieldPlaces");
        builder.HasKey(p => p.Id); // Первичный ключ Id типа Guid
        builder.Property(p => p.Row)
            .IsRequired()
            .HasMaxLength(50); // Row обязателен, максимальная длина 50 символов
        builder.Property(p => p.Number)
            .IsRequired(); // Number обязателен
        builder.Property(p => p.IsEnable)
            .IsRequired(); // IsEnable обязателен
        builder.Property(p => p.IsDelete)
            .IsRequired(); // IsDelete обязателен
    }
}