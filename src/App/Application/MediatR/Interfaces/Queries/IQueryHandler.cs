using MediatR;

namespace Application.Mediatr.Interfaces.Queries;

/// <summary>
/// Интерфейс обработчика запросов
/// </summary>
/// <typeparam name="TIn">Входной тип данных</typeparam>
/// <typeparam name="TOut">Тип возвращаемого значения</typeparam>
public interface IQueryHandler<in TIn, TOut> : IRequestHandler<TIn, TOut> where TIn : IQuery<TOut> { }