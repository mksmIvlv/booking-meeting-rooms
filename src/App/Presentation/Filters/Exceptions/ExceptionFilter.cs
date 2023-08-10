using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Filters.Exceptions;

/// <summary>
/// Фильтр для ошибок
/// </summary>
public class ExceptionFilter : Attribute, IExceptionFilter
{
    #region Метод

    /// <summary>
    /// Метод обработки исключений
    /// </summary>
    /// <param name="context">Контекст</param>
    public void OnException(ExceptionContext context)
    {
        string? actionName = context.ActionDescriptor.DisplayName;
        string exceptionMessage = context.Exception.Message;
        string message = $"В методе {actionName} возникло исключение:\n{exceptionMessage}";

        if (context.Exception is FormatException)
        {
            message = $"В методе {actionName} возникло исключение:\nне верный формат данных.";
        }
        
        context.Result = new ContentResult
        {
            Content = message
        };
        context.ExceptionHandled = true;
    }

    #endregion
}