using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Application.DTO;

namespace SlagFieldManagement.Application.Queries.GetSlagFieldState;

public sealed record GetSlagFieldStateQuery:IQuery<List<SlagFieldStateResponse>>;