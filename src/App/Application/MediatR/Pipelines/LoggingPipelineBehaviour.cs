using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediatr.Pipelines;

/// <summary>
/// Pipeline записи логов
/// </summary>
/// <typeparam name="TIn">Входной тип данных</typeparam>
/// <typeparam name="TOut">Тип возвращаемого значения</typeparam>
public class LoggingPipelineBehaviour<TIn, TOut> : IPipelineBehavior<TIn, TOut> where TIn : IRequest<TOut>
{
    #region Поле

    /// <summary>
    /// Логгер
    /// </summary>
    private readonly ILogger<LoggingPipelineBehaviour<TIn, TOut>> _logger;

    #endregion

    #region Конструктор

    public LoggingPipelineBehaviour(ILogger<LoggingPipelineBehaviour<TIn, TOut>> logger)
    {
        _logger = logger;
    }

    #endregion
    
    #region Метод

    /// <summary>
    /// Запись логов
    /// </summary>
    /// <param name="request">Запрос, который пришел</param>
    /// <param name="next">Метод, который должен выполниться</param>
    /// <param name="cancellationToken">Токен</param>
    /// <returns>Возвращаемый тип, метода, который должен выполниться</returns>
    public async Task<TOut> Handle(TIn request, RequestHandlerDelegate<TOut> next, CancellationToken cancellationToken)
    {
        _logger.Log(LogLevel.Information,$"Команда {typeof(TIn)} начала выполнение");
        try
        {
            var result = await next();
            _logger.Log(LogLevel.Information,$"Команда {typeof(TIn)} закончило выполнение");

            return result;
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Warning,$"Команда {typeof(TIn)} закончило выполнение");
            throw;
        }
    }

    #endregion
}