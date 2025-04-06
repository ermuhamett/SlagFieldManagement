using MediatR;
using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Application.Abstractions.Messaging;

/// <summary>
/// Интерфейс для обработчика запросов, возвращающих результат с типом <typeparamref name="TResponse"/>.
/// Запросы предназначены для получения данных без изменения состояния.
/// </summary>
/// <typeparam name="TQuery">Тип запроса, который обрабатывает данный обработчик.</typeparam>
/// <typeparam name="TResponse">Тип данных, возвращаемых запросом.</typeparam>
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}