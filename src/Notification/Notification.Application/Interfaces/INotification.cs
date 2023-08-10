namespace Notification.Application.Interfaces;

/// <summary>
/// Интерфейс для отправки оповещений
/// </summary>
public interface INotification
{
    #region Свойство

    /// <summary>
    /// Отправить оповещение
    /// </summary>
    /// <param name="idChat">Id чата</param>
    /// <param name="message">Сообщение</param>
    public Task SendMessage(long idChat, string message);

    #endregion
}