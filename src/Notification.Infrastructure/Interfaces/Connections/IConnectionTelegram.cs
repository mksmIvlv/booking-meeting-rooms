using Notification.Infrastructure.Settings.Telegram;
using Telegram.Bot;

namespace Notification.Infrastructure.Interfaces.Connections;

/// <summary>
/// Подключение к Telegram
/// </summary>
public interface IConnectionTelegram
{
    #region Свойства

    /// <summary>
    /// Bot Telegram 
    /// </summary>
    public TelegramBotClient BotClient { get; }

    /// <summary>
    /// Настройки Telegram
    /// </summary>
    public TelegramSettings Settings { get; }

    #endregion
}