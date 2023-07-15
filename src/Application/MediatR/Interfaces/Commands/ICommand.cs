using MediatR;

namespace Application.Mediatr.Interfaces.Commands;

/// <summary>
/// Интерфейс для команд, возвращающих значение
/// </summary>
/// <typeparam name="TOut">Тип возвращаемого значения</typeparam>
public interface ICommand <out TOut> : IRequest<TOut> { }

/// <summary>
/// Интерфейс для команд
/// </summary>
public interface ICommand : ICommand<Unit> { }