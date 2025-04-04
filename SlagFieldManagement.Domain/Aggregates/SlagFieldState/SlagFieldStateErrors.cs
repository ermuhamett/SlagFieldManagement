using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Domain.Aggregates.SlagFieldState;

public class SlagFieldStateErrors
{
    public static Error PlaceOccupied(Guid placeId) => new(
        "SlagField.PlaceOccupied",
        $"Место {placeId} уже занято или недоступно");

    public static Error InvalidStateTransition = new(
        "SlagField.InvalidStateTransition",
        "Недопустимое изменение состояния");

    public static Error BucketNotEmpty = new(
        "SlagField.BucketNotEmpty",
        "Невозможно снять неопустошенный ковш");

    public static Error EmptyingNotAllowed = new(
        "SlagField.EmptyingNotAllowed",
        "Опорожнение недоступно в текущем состоянии");

    public static Error RemovalNotAllowed = new(
        "SlagField.RemovalNotAllowed",
        "Снятие ковша недоступно в текущем состоянии");
}