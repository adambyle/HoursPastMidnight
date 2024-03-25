using System.Net;
// Thanks to Paul Batum.
// https://github.com/paulbatum/WebSocket-Samples/blob/master/HttpListenerWebSocketEcho/Server/Server.cs
static class HttpListenerContextGetter
{
    public static Task<HttpListenerContext> GetContextAsync(this HttpListener listener)
    {
        return Task.Factory.FromAsync(listener.BeginGetContext, listener.EndGetContext, TaskCreationOptions.None);
    }
}
