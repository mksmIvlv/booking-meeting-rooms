using Grpc.Core;
using Notification.Application.Interfaces;
using Notification.Infrastructure;

namespace Notification.Application.Services;

public class GrpcService : PushMessageNotification.PushMessageNotificationBase
{
    #region Поле

    /// <summary>
    /// Отправка уведомлений
    /// </summary>
    private INotification _notification;

    #endregion

    #region Конструктор

    public GrpcService(INotification notification)
    {
        _notification = notification;
    }

    #endregion

    #region Метод

    /// <summary>
    /// Переопределенный метод Push, который получает сообщение
    /// </summary>
    /// <param name="request">Сообщение для дальнейшей отправки как оповещение</param>
    /// <param name="context"></param>
    /// <returns>Пустое сообщение</returns>
    public override Task<Response> Push(Request request, ServerCallContext context)
    {
        _notification.SendMessage(request.IdChat, request.Text);
       
        // отправляем ответ
        return Task.FromResult(new Response());
    }

    #endregion
}