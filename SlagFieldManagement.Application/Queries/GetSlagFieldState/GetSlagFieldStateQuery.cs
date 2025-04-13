using SlagFieldManagement.Application.Abstractions.Messaging;

namespace SlagFieldManagement.Application.Queries.GetSlagFieldState;

public sealed record GetSlagFieldStateQuery:IQuery<List<SlagFieldStateResponse>>;