using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

class Game
{
    private List<Player> _players = [];
    private List<Connection> _connections = [];
    private int _nextPlayerId = 0;
    private Mutex _connectionsListMutex = new();

    public IReadOnlyList<Player> Players => _players;
    public bool Started { get; private set; } = false;

    public async void Connect(WebSocket client)
    {
        var connection = new Connection(client);
        _connectionsListMutex.WaitOne();
        _connections.Add(connection);
        _connectionsListMutex.ReleaseMutex();
        Console.WriteLine("Connection made");

        byte[] data = [];

        while (client.State == WebSocketState.Open)
        {
            // Collect the next 1KB chunk of data.
            var chunk = new byte[1024];
            var receipt = await client.ReceiveAsync(
                new ArraySegment<byte>(chunk),
                CancellationToken.None);
            data = data.Concat(chunk[..receipt.Count]).ToArray();

            // Keep collecting if we're not done.
            if (!receipt.EndOfMessage)
                continue;

            var message = Encoding.UTF8.GetString(data);
            using (var document = JsonDocument.Parse(message))
            {

            }
            data = [];
        }

        Console.WriteLine("Connection lost");
        _connectionsListMutex.WaitOne();
        _connections.Remove(connection);
        _connectionsListMutex.ReleaseMutex();
    }

    private void SendAll(string data)
    {
        _connectionsListMutex.WaitOne();
        foreach (var connection in _connections)
            connection.Send(data);
        _connectionsListMutex.ReleaseMutex();
    }

    private IEnumerable<Connection> PlayerConnections()
    {
        return from connection in _connections
               where connection.Player != null
               select connection;
    }

    record class Connection(WebSocket WebSocket)
    {
        public Mutex Mutex { get; } = new();
        public Player? Player { get; set; } = null;

        public async void Send(string data)
        {
            Mutex.WaitOne();
            while (WebSocket.State == WebSocketState.Connecting) ;
            if (WebSocket.State == WebSocketState.Open)
            {
                await WebSocket.SendAsync(new ArraySegment<byte>(
                    Encoding.UTF8.GetBytes(data)),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);
            }
            Mutex.ReleaseMutex();
        }
    }
}