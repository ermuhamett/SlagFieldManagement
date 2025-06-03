using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Domain.Entities;

namespace SlagFieldManagement.Application.Queries.GetAllBuckets;

public record GetAllBucketsQuery():IQuery<List<Bucket>>;