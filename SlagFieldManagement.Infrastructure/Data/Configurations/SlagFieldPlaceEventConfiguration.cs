using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;
using SlagFieldManagement.Infrastructure.EventStores;

namespace SlagFieldManagement.Infrastructure.Data.Configurations;

internal sealed class SlagFieldPlaceEventConfiguration:IEntityTypeConfiguration<SlagFieldPlaceEvent>
{
    public void Configure(EntityTypeBuilder<SlagFieldPlaceEvent> builder)
    {
        builder.ToTable("SlagFieldPlaceEventStore"); // Имя таблицы указано в атрибуте класса
        builder.HasKey(e => e.EventId); // Первичный ключ EventId типа Guid
        builder.Property(e => e.AggregateId)
            .IsRequired(); // AggregateId обязателен
        builder.Property(e => e.EventType)
            .IsRequired()
            .HasMaxLength(255); // EventType обязателен, максимальная длина 255 символов
        builder.Property(e => e.EventData)
            .IsRequired(); // EventData обязателен (JSON-строка)
        builder.Property(e => e.Version)
            .IsRequired(); // Version обязателен
        builder.Property(e => e.Timestamp)
            .IsRequired(); // Timestamp обязателен
        builder.Property(e => e.Metadata); // Metadata необязателен (JSON-строка)

        // Связь с SlagFieldPlace (многие к одному)
        builder.HasOne<SlagFieldPlace>()
            .WithMany() // В SlagFieldPlace нет явной коллекции событий
            .HasForeignKey(e => e.AggregateId)
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление
    }
}