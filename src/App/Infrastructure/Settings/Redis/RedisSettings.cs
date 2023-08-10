namespace Infrastructure.Settings.Redis;

/// <summary>
/// Настройки Redis
/// </summary>
public class RedisSettings
{
    #region Свойство

    /// <summary>
    /// Подключение к Redis
    /// </summary>
    public string ConnectionString { get; set; }

    #endregion
}