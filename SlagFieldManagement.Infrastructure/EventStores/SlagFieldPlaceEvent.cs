using System.ComponentModel.DataAnnotations.Schema;

namespace SlagFieldManagement.Infrastructure.EventStores;

[Table("SlagFieldPlaceEventStore")]
public class SlagFieldPlaceEvent
{
    public Guid EventId { get; set; }
    public Guid AggregateId { get; set; }
    public string EventType { get; set; } // Пример: "WentInUse"
    public string EventData { get; set; }
    public int Version { get; set; }
    public DateTime Timestamp { get; set; }
    public string Metadata { get; set; }
}