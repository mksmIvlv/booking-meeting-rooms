using Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Interfaces;

namespace Notification.Application.Services;

public class HttpService
{
    #region Поле

    /// <summary>
    /// Отправка уведомлений 
    /// </summary>
    private INotification _notification;

    #endregion

    #region Конструктор

    public HttpService(INotification notification)
    {
        _notification = notification;
    }

    #endregion
    
    #region Метод
    
    public void ReceivingMessage(MessageNotification content)
    {
        _notification.SendMessage(content.IdChat, content.Text);
    }

    #endregion
}