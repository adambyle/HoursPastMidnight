using System.Net;
using System.Net.Sockets;

class Program
{

    public static void Main(string[] args)
    {
        var Game = new Game();

        string ip = args[0];
        int port = int.Parse(args[1]);

        var listener = new HttpListener();
        listener.Prefixes.Add($"http://{ip}:{port}/");
        listener.Start();
        Server.Start(listener);
        Console.WriteLine($"Listening at {ip}:{port}...");
        Console.WriteLine("Press any key to quit.");
        Console.ReadKey();
    }
}
