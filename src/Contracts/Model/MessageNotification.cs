using Contracts.Interface;

namespace Contracts.Model;

public class MessageNotification : IMessage
{
    #region Свойства

    /// <summary>
    /// Id сообщения
    /// </summary>
    public Guid Id { get; private  set; }
    
    /// <summary>
    /// Id чата для отправки сообщения
    /// </summary>
    public long IdChat { get;  set; }
    
    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string Text { get;  set; }

    #endregion

    #region Конструктор

    public MessageNotification(long idChat, string text)
    {
        Id = Guid.NewGuid();
        IdChat = idChat;
        Text = text;
    }

    #endregion
}