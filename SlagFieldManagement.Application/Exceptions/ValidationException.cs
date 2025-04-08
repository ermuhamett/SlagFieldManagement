namespace SlagFieldManagement.Application.Exceptions;

/// <summary>
/// Исключение, выбрасываемое при наличии ошибок валидации запроса.
/// Содержит коллекцию ошибок валидации.
/// </summary>
public class ValidationException:Exception
{
    /// <summary>
    /// Конструктор, инициализирующий исключение с коллекцией ошибок валидации.
    /// </summary>
    /// <param name="errors">Коллекция ошибок валидации.</param>
    public ValidationException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }

    /// <summary>
    /// Коллекция ошибок валидации.
    /// </summary>
    public IEnumerable<ValidationError> Errors { get; }
}