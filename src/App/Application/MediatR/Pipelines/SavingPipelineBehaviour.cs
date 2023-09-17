using Application.Mediatr.Interfaces.Commands;
using Domain.Interfaces.Infrastructure;
using MediatR;

namespace Application.Mediatr.Pipelines;

/// <summary>
/// Pipeline сохранение данных в бд
/// </summary>
/// <typeparam name="TIn">Входной тип данных</typeparam>
/// <typeparam name="TOut">Тип возвращаемого значения</typeparam>
public class SavingPipelineBehaviour<TIn, TOut> : IPipelineBehavior<TIn, TOut> where TIn : IRequest<TOut>
{
    #region Поле

    /// <summary>
    /// Репозиторий
    /// </summary>
    private readonly IRepository _repository;

    #endregion

    #region Конструктор

    public SavingPipelineBehaviour(IRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Метод

    /// <summary>
    /// Сохранения данных в бд
    /// </summary>
    /// <param name="request">Запрос, который пришел</param>
    /// <param name="next">Метод, который должен выполниться</param>
    /// <param name="cancellationToken">Токен</param>
    /// <returns>Возвращаемый тип, метода, который должен выполниться</returns>
    public async Task<TOut> Handle(TIn request, RequestHandlerDelegate<TOut> next, CancellationToken cancellationToken)
    {
        var result = await next();
        
        if (request is ICommand<TOut>)
        {
            await _repository.SaveAsync();
        }

        return result;
    }

    #endregion
}