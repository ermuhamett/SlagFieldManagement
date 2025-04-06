using MediatR;
using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Application.Abstractions.Messaging;

/// <summary>
/// Базовый интерфейс для команд, возвращающих <see cref="Result"/>.
/// Команды используются для выполнения действий, изменяющих состояние.
/// </summary>
public interface ICommand : IRequest<Result>, IBaseCommand
{
}

/// <summary>
/// Интерфейс для команд, возвращающих результат с типом <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TResponse">Тип данных, возвращаемый командой.</typeparam>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
{
}

/// <summary>
/// Маркерный интерфейс для команд, используемый для группировки всех команд.
/// </summary>
public interface IBaseCommand
{
}