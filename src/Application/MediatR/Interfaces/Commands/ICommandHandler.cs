using MediatR;

namespace Application.Mediatr.Interfaces.Commands;

/// <summary>
/// Интерфейс обработчика команды, с возвращаемым значением
/// </summary>
/// <typeparam name="TIn">Входной тип данных</typeparam>
/// <typeparam name="TOut">Тип возвращаемого значения</typeparam>
public interface ICommandHandler<in TIn, TOut> : IRequestHandler<TIn, TOut> where TIn : ICommand<TOut> { }

/// <summary>
/// Интерфейс обработчика команды
/// </summary>
/// <typeparam name="TIn"> Входной тип данных</typeparam>
public interface ICommandHandler<in TIn> : ICommandHandler<TIn, Unit> where TIn : ICommand { }