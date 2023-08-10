using MediatR;

namespace Application.Mediatr.Interfaces.Queries;

/// <summary>
/// Интерфейс для запросов
/// </summary>
/// <typeparam name="TOut">Тип возвращаемого значения</typeparam>
public interface IQuery<out TOut> : IRequest<TOut> { }