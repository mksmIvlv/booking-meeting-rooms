using Contracts.Model;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Interfaces;
using Notification.Application.Services;

namespace Notification.Presentation.Controllers;

[ApiController]
[Route("api/[action]")]
public class NotificationController: ControllerBase
{
    #region Поле

    /// <summary>
    /// Сервис для обработки сообщения и дальнейшей отправки
    /// </summary>
    private HttpService _httpService;

    #endregion

    #region Конструктор

    public NotificationController(HttpService httpService)
    {
        _httpService = httpService;
    }

    #endregion
    
    #region Api - Метод
    
    [HttpPost]
    public void PostMessageNotification(MessageNotification content)
    {
        _httpService.ReceivingMessage(content);
    }

    #endregion
}