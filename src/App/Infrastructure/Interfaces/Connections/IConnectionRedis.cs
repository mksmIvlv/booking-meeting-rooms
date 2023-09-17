using Infrastructure.Settings.Redis;
using RedLockNet.SERedis;

namespace Infrastructure.Interfaces.Connections;

/// <summary>
/// Интерфейс для подключения к Redis
/// </summary>
public interface IConnectionRedis
{
    #region Свойства

    /// <summary>
    /// Фабрика к которой было созданно подключение
    /// </summary>
    public RedLockFactory RedLockFactory { get; }

    /// <summary>
    /// Настройки Redis
    /// </summary>
    public RedisSettings Settings { get; }

    #endregion
}