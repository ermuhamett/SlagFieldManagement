using SlagFieldManagement.Application.Abstractions.Messaging;

namespace SlagFieldManagement.Application.Commands.PlaceBucket;

public record PlaceBucketCommand(
    Guid PlaceId,
    Guid BucketId,
    Guid MaterialId,
    decimal SlagWeight,
    DateTime StartDate):ICommand;