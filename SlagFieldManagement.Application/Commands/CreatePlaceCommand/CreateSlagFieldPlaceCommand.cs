using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;

namespace SlagFieldManagement.Application.Commands.CreatePlaceCommand;

public record CreateSlagFieldPlaceCommand(
    string Row,
    int Number):ICommand<SlagFieldPlace>;