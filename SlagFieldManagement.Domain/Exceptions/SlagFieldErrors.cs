using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Domain.Exceptions;

public static class SlagFieldErrors
{
    public static Error BucketNotFound(Guid bucketId) => new(
        "SlagFieldErrors.BucketNotFound",
        $"Ковш {bucketId} не найдено");
}