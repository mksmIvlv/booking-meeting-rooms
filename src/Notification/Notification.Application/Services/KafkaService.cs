using Confluent.Kafka;
using Contracts.Models;
using Newtonsoft.Json;
using Notification.Application.Interfaces;
using Notification.Infrastructure.Interfaces.Connections;

namespace Notification.Application.Services;

public class KafkaService: IConsumerBus
{
    #region Поля

    /// <summary>
    /// Отправка уведомлений
    /// </summary>
    private INotification _notification;
    
    /// <summary>
    /// Подключение к шине
    /// </summary>
    private IConnectionKafka _connect;

    #endregion

    #region Конструктор

    public KafkaService(INotification notification, IConnectionKafka connect)
    {
        _notification = notification;
        _connect = connect;
    }

    #endregion

    #region Метод

    /// <summary>
    /// Получить новые сообщения из шины
    /// </summary>
    public void Listen()
    {
        try
        {
            _connect.Consumer.Subscribe(_connect.Settings.TopicName);
            CancellationTokenSource cts = new CancellationTokenSource();
            
            while (true)
            {
                try
                {
                    var content = _connect.Consumer.Consume(cts.Token);
                    var message = JsonConvert.DeserializeObject<MessageNotification>(content.Value);

                    _notification.SendMessage(message.IdChat, message.Text);
                }
                catch (ConsumeException)
                {
                    _connect.Consumer.Close();
                }
            }
        }
        catch (OperationCanceledException)
        {
            _connect.Consumer.Close();
        }
    }

    #endregion
}