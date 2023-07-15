using Grpc.Net.Client;
using Infrastructure.Interfaces.Connections;
using Infrastructure.Settings;
using Infrastructure.Settings.Grpc;
using Microsoft.Extensions.Options;

namespace Infrastructure.Connections;

public class ConnectionGrpc : IConnectionGrpc
{
    #region Свойства

    /// <summary>
    /// Канал к которому создано подключение
    /// </summary>
    public GrpcChannel Channel { get; private set; }

    #endregion

    #region Поле

    /// <summary>
    /// Настройки gRPC
    /// </summary>
    private GrpcSettings _settings;

    #endregion

    #region Конструктор

    public ConnectionGrpc(IOptions<InfrastructureSettings> settings)
    {
        _settings = settings.Value.GrpcSettings;
        ConnectGrpc();
    }

    #endregion

    #region Метод

    /// <summary>
    /// Подключение к каналу gRPC
    /// </summary>
    private void ConnectGrpc()
    {
        Channel = GrpcChannel.ForAddress(_settings.Address);
    }

    #endregion
}