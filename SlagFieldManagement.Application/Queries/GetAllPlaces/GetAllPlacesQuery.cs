using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;

namespace SlagFieldManagement.Application.Queries.GetAllPlaces;

public sealed record GetAllPlacesQuery():IQuery<List<SlagFieldPlace>>;