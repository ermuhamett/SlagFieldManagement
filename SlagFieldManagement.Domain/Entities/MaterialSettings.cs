using SlagFieldManagement.Domain.Abstractions;
using SlagFieldManagement.Domain.Exceptions;

namespace SlagFieldManagement.Domain.Entities;

public sealed class MaterialSettings:Entity
{
    public Guid MaterialId { get; private set; }
    public string StageName { get; private set; }
    public string? EventType { get; private set; }
    public int Duration { get; private set; }
    public string? VisualStateCode { get; private set; }
    public decimal? MinHours { get; private set; }
    public decimal? MaxHours { get; private set; }
    public bool IsDelete { get; private set; }
    
    private MaterialSettings(
        Guid id,
        Guid materialId,
        string stageName,
        string? eventType,
        int duration,
        string? visualStateCode,
        decimal? minHours,
        decimal? maxHours)
        : base(id)
    {
        MaterialId = materialId;
        StageName = stageName;
        EventType = eventType;
        Duration = duration;
        VisualStateCode = visualStateCode;
        MinHours = minHours;
        MaxHours = maxHours;
        IsDelete = false;
    }
    
    /// <summary>
    /// Создает новый экземпляр MaterialSettings с валидацией входных данных.
    /// </summary>
    public static Result<MaterialSettings> Create(
        Guid materialId,
        string stageName,
        string? eventType,
        int duration,
        string? visualStateCode,
        decimal? minHours,
        decimal? maxHours)
    {
        var validationResult = Validate(stageName, duration, minHours, maxHours);
        if (validationResult.IsFailure)
            return Result.Failure<MaterialSettings>(validationResult.Error);

        return Result.Success(new MaterialSettings(
            Guid.NewGuid(),
            materialId,
            stageName,
            eventType,
            duration,
            visualStateCode,
            minHours,
            maxHours
        ));
    }
    
    private static Result Validate(
        string stageName,
        int duration,
        decimal? minHours,
        decimal? maxHours)
    {
        if (string.IsNullOrWhiteSpace(stageName))
            return Result.Failure(MaterialSettingsErrors.InvalidStageName);

        if (duration <= 0)
            return Result.Failure(MaterialSettingsErrors.InvalidDuration);

        if (minHours.HasValue && maxHours.HasValue && minHours >= maxHours)
            return Result.Failure(MaterialSettingsErrors.InvalidTimeRange);

        return Result.Success();
    }

    /// <summary>
    /// Возвращает визуальное состояние на основе времени события.
    /// </summary>
    public string? GetVisualState(DateTime? eventStartTime)
    {
        if (eventStartTime == null || MinHours == null || MaxHours == null)
            return VisualStateCode;

        var hoursPassed = (DateTime.UtcNow - eventStartTime.Value).TotalHours;
        return (hoursPassed >= (double)MinHours && hoursPassed <= (double)MaxHours)
            ? VisualStateCode
            : null;
    }

    /// <summary>
    /// Помечает настройки материала как удаленные.
    /// </summary>
    public void Delete() => IsDelete = true;
}