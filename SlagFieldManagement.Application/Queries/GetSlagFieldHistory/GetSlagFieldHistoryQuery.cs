using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Application.DTO;

namespace SlagFieldManagement.Application.Queries.GetSlagFieldHistory;

public record GetSlagFieldHistoryQuery(DateTime Timestamp)
    : IQuery<List<SlagFieldEventHistoryResponse>>;