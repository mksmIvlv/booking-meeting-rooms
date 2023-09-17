using Grpc.Net.Client;

namespace Infrastructure.Interfaces.Connections;

/// <summary>
/// Интерфейс для подключения к gRPC
/// </summary>
public interface IConnectionGrpc
{
    #region Свойство

    /// <summary>
    /// Канал gRPC
    /// </summary>
    public GrpcChannel Channel { get; }

    #endregion
}