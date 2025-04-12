using SlagFieldManagement.Application.Abstractions.Messaging;

namespace SlagFieldManagement.Application.Commands.WentInUse;

public record WentInUseCommand(
    Guid PlaceId):ICommand;