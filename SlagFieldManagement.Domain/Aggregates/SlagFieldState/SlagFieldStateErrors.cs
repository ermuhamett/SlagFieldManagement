using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Domain.Aggregates.SlagFieldState;

public static class SlagFieldStateErrors
{
    public static Error PlaceOccupied(Guid placeId) => new(
        "SlagField.PlaceOccupied",
        $"Место {placeId} уже занято или недоступно");
    public static Error NoBucketToEmpty(Guid placeId) => new (
            "SlagFieldState.NoBucketToEmpty", 
            $"На месте с ID {placeId} нет ковша для опустошения.");
    public static Error NoActiveState(Guid placeId) => new(
        "SlagFieldState.NoActiveState", 
        $"Нет активного состояния для места (PlaceId: {placeId}).");
    public static Error InvalidStateForRemoval(Guid placeId) => new (
        "SlagFieldState.InvalidStateForRemoval", 
        $"Данное место (PlaceId: {placeId}) уже занято или не находится в состоянии BucketEmptied.");
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