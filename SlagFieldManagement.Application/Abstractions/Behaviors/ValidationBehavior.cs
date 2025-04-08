using FluentValidation;
using MediatR;
using SlagFieldManagement.Application.Abstractions.Messaging;
using SlagFieldManagement.Application.Exceptions;

namespace SlagFieldManagement.Application.Abstractions.Behaviors;

/// <summary>
/// Поведение для валидации запросов перед их обработкой в конвейере MediatR.
/// Проверяет запрос на соответствие правилам валидации, и если есть ошибки, выбрасывает исключение.
/// </summary>
/// <typeparam name="TRequest">Тип запроса, который должен реализовывать интерфейс <see cref="IBaseCommand"/>.</typeparam>
/// <typeparam name="TResponse">Тип ответа, возвращаемого обработчиком запроса.</typeparam>
public class ValidationBehavior<TRequest, TResponse>
    :IPipelineBehavior<TRequest, TResponse>
    where TRequest:IBaseCommand
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Конструктор, принимающий коллекцию валидаторов для проверки запросов.
    /// </summary>
    /// <param name="validators">Коллекция валидаторов для типа <typeparamref name="TRequest"/>.</param>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <summary>
    /// Метод обработки запроса в конвейере MediatR.
    /// Выполняет валидацию запроса, и если есть ошибки, выбрасывает исключение <see cref="ValidationException"/>.
    /// Если ошибок нет, передает запрос следующему обработчику в конвейере.
    /// </summary>
    /// <param name="request">Запрос, который подлежит валидации.</param>
    /// <param name="next">Делегат, передающий управление следующему обработчику в конвейере.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат обработки запроса, если валидация прошла успешно.</returns>
    /// <exception cref="ValidationException">Исключение, выбрасываемое при наличии ошибок валидации.</exception>
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        // Если для запроса нет валидаторов, пропускаем валидацию и передаем запрос дальше.
        if (!_validators.Any())
        {
            return await next();
        }
        // Создаем контекст валидации для текущего запроса.
        var context = new ValidationContext<TRequest>(request);
        var validationErrors = _validators
            .Select(_validator => _validator.Validate(context))// Проверка запроса каждым валидатором.
            .Where(validationResult => validationResult.Errors.Any())// Оставляем только результаты с ошибками.
            .SelectMany(validationResult => validationResult.Errors)// Собираем все ошибки.
            .Select(validationFailure => new ValidationError(
                validationFailure.PropertyName, // Имя свойства с ошибкой.
                validationFailure.ErrorMessage)) // Сообщение об ошибке.
            .ToList();
        // Если найдены ошибки валидации, выбрасываем ValidationException.
        if (validationErrors.Any())
        {
            throw new Exceptions.ValidationException(validationErrors);
        }
        return await next();
    }
}