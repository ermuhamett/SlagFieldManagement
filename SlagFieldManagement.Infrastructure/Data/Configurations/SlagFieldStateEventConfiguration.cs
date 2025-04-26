using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlagFieldManagement.Domain.Aggregates.SlagFieldState;
using SlagFieldManagement.Infrastructure.EventStores;

namespace SlagFieldManagement.Infrastructure.Data.Configurations;

internal sealed class SlagFieldStateEventConfiguration:IEntityTypeConfiguration<SlagFieldStateEvent>
{
    public void Configure(EntityTypeBuilder<SlagFieldStateEvent> builder)
    {
        builder.ToTable("SlagFieldStateEventStore"); // Имя таблицы указано в атрибуте класса
        builder.HasKey(e => e.EventId); // Первичный ключ EventId типа Guid
        builder.Property(e => e.AggregateId)
            .IsRequired(); // AggregateId обязателен
        builder.Property(e => e.EventType)
            .IsRequired()
            .HasMaxLength(255);// EventType обязателен
        builder.Property(e => e.EventData)
            .IsRequired(); // EventData обязателен (JSON-строка)
        builder.Property(e => e.Version)
            .IsRequired(); // Version обязателен
        builder.Property(e => e.Timestamp)
            .IsRequired(); // Timestamp обязателен

        builder.HasIndex(e => e.AggregateId); // Для поиска по AggregateId
        builder.HasIndex(e => e.Timestamp);   // Для фильтрации по времени
        
        // Связь с SlagFieldState (многие к одному)
        builder.HasOne<SlagFieldState>()
            .WithMany() // В SlagFieldState нет явной коллекции событий
            .HasForeignKey(e => e.AggregateId)
            .OnDelete(DeleteBehavior.Cascade); // Каскадное удаление
    }
}