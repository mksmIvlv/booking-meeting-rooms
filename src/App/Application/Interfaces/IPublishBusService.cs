using Contracts.Interface;

namespace Application.Interfaces;

/// <summary>
/// Интерфейс для отправки сообщений в шину
/// </summary>
public interface IPublishBusService<T> where T: IMessage
{
    /// <summary>
    /// Отправка сообщений в очередь
    /// </summary>
    /// <param name="classMessage">Класс - сообщение</param>
    public Task SendMessageAsync(T classMessage);
}