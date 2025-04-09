using SlagFieldManagement.Application.Abstractions.Messaging;

namespace SlagFieldManagement.Application.Commands.CreatePlaceCommand;

public record CreateSlagFieldPlaceCommand(
    string Row,
    int Number):ICommand<Guid>;