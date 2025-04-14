using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Domain.Aggregates.SlagFieldPlace;

public static class SlagFieldPlaceErrors
{
    public static Error AlreadyEnabled(Guid placeId) => new(
        "SlagFieldPlace.AlreadyEnabled",
        $"Место {placeId} уже активно");

    public static Error AlreadyDisabled(Guid placeId) => new(
        "SlagFieldPlace.AlreadyDisabled",
        $"Место {placeId} уже отключено");

    public static Error PlaceNotFoundOrDisabled(Guid placeId) => new(
        "SlagFieldPlace.PlaceNotFoundOrDisabled",
        $"Место {placeId} не найдено или отключено");
    
    public static Error PlaceNotFound(Guid placeId) => new(
        "SlagFieldPlace.PlaceNotFound",
        $"Место {placeId} не найдено");

    public static Error InvalidRowFormat = new(
        "SlagFieldPlace.InvalidRow",
        "Название ряда должно содержать только буквы и цифры");

    public static Error InvalidNumber = new(
        "SlagFieldPlace.InvalidNumber",
        "Номер места должен быть положительным числом");

    public static Error HasActiveOperations(Guid placeId) => new(
        "SlagFieldPlace.ActiveOperations",
        $"Место {placeId} нельзя деактивировать: имеются активные операции");
}