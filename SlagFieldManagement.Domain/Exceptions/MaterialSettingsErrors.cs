using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Domain.Exceptions;

public sealed class MaterialSettingsErrors
{
    // Ошибки валидации
    public static Error InvalidStageName => new(
        "MaterialSettings.InvalidStageName",
        "Название стадии не может быть пустым.");

    public static Error InvalidDuration => new(
        "MaterialSettings.InvalidDuration",
        "Длительность стадии должна быть положительной.");

    public static Error InvalidTimeRange => new(
        "MaterialSettings.InvalidTimeRange",
        "MinHours должен быть меньше MaxHours.");

    public static Error VisualStateRequiresEventType => new(
        "MaterialSettings.VisualStateRequiresEventType",
        "VisualStateCode требует указания EventType.");

    // Ошибки для операций
    public static Error NotFound(Guid materialId) => new(
        "MaterialSettings.NotFound",
        $"Настройки для материала {materialId} не найдены.");
}