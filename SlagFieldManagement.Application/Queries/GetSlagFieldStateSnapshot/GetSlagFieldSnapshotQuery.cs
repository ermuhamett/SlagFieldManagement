using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Application.DTO;

namespace SlagFieldManagement.Application.Queries.GetSlagFieldStateSnapshot;

public record GetSlagFieldSnapshotQuery(DateTime SnapshotTime):IQuery<List<SlagFieldStateResponse>>;