using SlagFieldManagement.Application.Abstractions.Messaging;

namespace SlagFieldManagement.Application.Commands.Invalid;

public record InvalidCommand(
    Guid PlaceId,
    string Description):ICommand;