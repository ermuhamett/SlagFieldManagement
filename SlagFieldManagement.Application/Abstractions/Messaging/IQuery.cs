using MediatR;
using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Application.Abstractions.Messaging;

/// <summary>
/// Интерфейс для запросов, возвращающих результат с типом <typeparamref name="TResponse"/>.
/// Запросы используются для получения данных без изменения состояния.
/// </summary>
/// <typeparam name="TResponse">Тип данных, возвращаемых запросом.</typeparam>
public interface IQuery<TResponse>:IRequest<Result<TResponse>>
{
    
}