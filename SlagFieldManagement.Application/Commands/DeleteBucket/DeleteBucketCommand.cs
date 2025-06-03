using SlagFieldManagement.Application.Abstractions.Messaging;

namespace SlagFieldManagement.Application.Commands.DeleteBucket;

public record DeleteBucketCommand(Guid Id):ICommand;