using System.Net.Sockets;

namespace FxRatesProvider;

public sealed class LightstreamerConnectionSet : IDisposable
{
    private readonly TcpClient _client;
    private readonly NetworkStream _stream;

    private LightstreamerConnectionSet(TcpClient client)
    {
        _client = client;
        _stream = client.GetStream();
    }

    public Stream RequestStream => _stream;
    public Stream ReplyStream => _stream;

    public static async Task<LightstreamerConnectionSet> ConnectDataAdapterAsync(
        DataAdapterConnectionOptions options,
        CancellationToken cancellationToken)
    {
        TcpClient client = await ConnectAsync(options.Host, options.RequestReplyPort, cancellationToken);
        return new LightstreamerConnectionSet(client);
    }

    public static async Task<LightstreamerConnectionSet> ConnectMetadataAdapterAsync(
        MetadataAdapterConnectionOptions options,
        CancellationToken cancellationToken)
    {
        TcpClient client = await ConnectAsync(options.Host, options.RequestReplyPort, cancellationToken);
        return new LightstreamerConnectionSet(client);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    private static async Task<TcpClient> ConnectAsync(string host, int port, CancellationToken cancellationToken)
    {
        TcpClient client = new()
        {
            NoDelay = true
        };

        await client.ConnectAsync(host, port, cancellationToken);
        return client;
    }
}
