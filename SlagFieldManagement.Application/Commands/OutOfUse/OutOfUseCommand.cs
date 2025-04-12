using SlagFieldManagement.Application.Abstractions.Messaging;

namespace SlagFieldManagement.Application.Commands.OutOfUse;

public record OutOfUseCommand(
    Guid PlaceId):ICommand;