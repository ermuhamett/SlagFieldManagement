namespace SlagFieldManagement.Domain.Abstractions;

public record Error(string Code, string Message)
{
    // Статическое поле, представляющее отсутствие ошибки.
    public static Error None => new(string.Empty, string.Empty);
    // Статическое поле, представляющее ошибку, связанную с null значением.
    public static Error NullValue = new("Error.NullValue", "Null value was provided");
}