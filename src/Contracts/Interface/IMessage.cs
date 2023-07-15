namespace Contracts.Interface;

/// <summary>
/// Интерфейс для моделей сообщений
/// </summary>
public interface IMessage
{
    #region Свойства

    /// <summary>
    /// Id сообщения
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Id чата для отправки сообщения
    /// </summary>
    public long IdChat { get; set; }

    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string Text { get; set; }

    #endregion
}