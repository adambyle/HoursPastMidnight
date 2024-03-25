using System.Net;

static class Server
{
    public static async void Start(HttpListener listener)
    {
        Game game = new();

        while (true)
        {
            var listenerContext = await listener.GetContextAsync();

            // Reject non-web-socket requests.
            if (!listenerContext.Request.IsWebSocketRequest)
            {
                Console.WriteLine("Connection rejected");
                listenerContext.Response.StatusCode = 400;
                listenerContext.Response.Close();
                continue;
            }

            try
            {
                var webSocketContext = await listenerContext.AcceptWebSocketAsync(null);
                game.Connect(webSocketContext.WebSocket);
            }
            catch (Exception ex)
            {
                listenerContext.Response.StatusCode = 500;
                listenerContext.Response.Close();
                Console.Write($"Error accepting web socket connection: {ex}");
            }
        }
    }
}
