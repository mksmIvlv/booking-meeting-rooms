using Application.Interfaces;
using Contracts.Interface;
using Infrastructure;
using Infrastructure.Interfaces.Connections;

namespace Application.Services;

/// <summary>
/// Сервис для отправки сообщений с помощью gRPC
/// </summary>
public class GrpcService<T>: IPublishBusService<T> where T: IMessage
{
    #region Поле

    /// <summary>
    /// Подключение к gRPC
    /// </summary>
    private IConnectionGrpc _connection;

    #endregion

    #region Конструктор

    public GrpcService(IConnectionGrpc connectionGrpc)
    {
        _connection = connectionGrpc;
    }

    #endregion

    #region Метод

    /// <summary>
    /// Отправка сообщений
    /// </summary>
    /// <param name="classMessage">Класс - сообщение</param>
    public async Task SendMessageAsync(T classMessage)
    {
        // создаем клиента
        var client = new PushMessageNotification.PushMessageNotificationClient(_connection.Channel);

        // формируем сообщение для отправки
        Request request = new Request
        {
            Id = classMessage.Id.ToString(),
            IdChat = classMessage.IdChat,
            Text = classMessage.Text
        };
        
        // отправляем сообщение
        await client.PushAsync(request);
    }

    #endregion
}