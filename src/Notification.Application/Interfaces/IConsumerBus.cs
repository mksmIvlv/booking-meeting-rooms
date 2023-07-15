namespace Notification.Application.Interfaces;

/// <summary>
/// Интерфейс для получения сообщений из шины
/// </summary>
public interface IConsumerBus
{
    #region Свойство

    /// <summary>
    /// Получить новые сообщения из шины
    /// </summary>
    public void Listen();

    #endregion
}