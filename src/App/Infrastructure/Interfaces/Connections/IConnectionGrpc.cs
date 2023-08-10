using Grpc.Net.Client;

namespace Infrastructure.Interfaces.Connections;

public interface IConnectionGrpc
{
    public GrpcChannel Channel { get; }
}