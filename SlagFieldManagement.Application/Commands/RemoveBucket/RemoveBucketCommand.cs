using SlagFieldManagement.Application.Abstractions.Messaging;

namespace SlagFieldManagement.Application.Commands.RemoveBucket;

public record RemoveBucketCommand(
    Guid PlaceId) : ICommand;