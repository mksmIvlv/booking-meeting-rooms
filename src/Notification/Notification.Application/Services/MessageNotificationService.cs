using Contracts.Models;
using MassTransit;
using Notification.Application.Interfaces;

namespace Notification.Application.Services;

/// <summary>
/// Сервис для обработки входящих сообщений, MassTransit
/// </summary>
public class MessageNotificationService: IConsumer<MessageNotification>
{
    #region Поле

    /// <summary>
    /// Сервис для отправки уведомлений
    /// </summary>
    private readonly INotification _notification;

    #endregion

    #region Конструктор

    public MessageNotificationService(INotification notification)
    {
        _notification = notification;
    }

    #endregion

    #region Метод

    /// <summary>
    /// Обработка входящих сообщений и отправка оповещения
    /// </summary>
    /// <param name="context">Модель</param>
    public Task Consume(ConsumeContext<MessageNotification> context)
    {
        var message = context.Message;
        _notification.SendMessage(message.IdChat, message.Text);
        
        return Task.CompletedTask;
    }

    #endregion
}