using SlagFieldManagement.Application.Abstractions.Messaging;

namespace SlagFieldManagement.Application.Commands.EmptyBucket;

public record EmptyBucketCommand(
    Guid PlaceId,
    DateTime EndDate):ICommand;