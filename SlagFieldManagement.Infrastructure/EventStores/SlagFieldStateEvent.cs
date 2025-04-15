using System.ComponentModel.DataAnnotations.Schema;

namespace SlagFieldManagement.Infrastructure.EventStores;

[Table("SlagFieldStateEventStore")]
public class SlagFieldStateEvent
{
    public Guid EventId { get; set; }
    public Guid AggregateId { get; set; }
    public string EventType { get; set; } // Пример: "BucketPlaced"
    public string EventData { get; set; } // JSON-данные
    public int Version { get; set; }
    public DateTime Timestamp { get; set; }
    public string Metadata { get; set; } // JSON-метаданные
}