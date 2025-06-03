using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Domain.Entities;

namespace SlagFieldManagement.Application.Commands.CreateBucket;

public record CreateBucketCommand(
    string Name):ICommand<Bucket>;