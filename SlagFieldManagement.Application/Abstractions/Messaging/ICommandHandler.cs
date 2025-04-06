using MediatR;
using SlagFieldManagement.Domain.Abstractions;

namespace SlagFieldManagement.Application.Abstractions.Messaging;

/// <summary>
/// Интерфейс для обработчика команд, которые не возвращают данные, а просто выполняют операцию.
/// </summary>
/// <typeparam name="TCommand">Тип команды, которую обрабатывает данный обработчик.</typeparam>
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

/// <summary>
/// Интерфейс для обработчика команд, которые возвращают результат с типом <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TCommand">Тип команды, которую обрабатывает данный обработчик.</typeparam>
/// <typeparam name="TResponse">Тип данных, возвращаемых командой.</typeparam>
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}