using Microsoft.Extensions.Options;
using Notification.Infrastructure.Interfaces.Connections;
using Notification.Infrastructure.Settings;
using Notification.Infrastructure.Settings.Telegram;
using Telegram.Bot;

namespace Notification.Infrastructure.Connections;

/// <summary>
/// Подключение к Telegram
/// </summary>
public class ConnectionTelegram: IConnectionTelegram
{
    #region Свойства

    /// <summary>
    /// Настройки Telegram
    /// </summary>
    public TelegramSettings Settings { get; }
    
    /// <summary>
    /// Чат бот
    /// </summary>
    public TelegramBotClient BotClient { get; }

    #endregion

    #region Конструктор

    public ConnectionTelegram(IOptions<NotificationInfrastructureSettings> settings)
    {
        Settings = settings.Value.TelegramSettings;
        BotClient = new TelegramBotClient(Settings.TokenBot);
    }

    #endregion
}