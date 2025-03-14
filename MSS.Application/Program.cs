using System.Net;
using System.Net.Sockets;
using MSS.Application.Models;
using MSS.Application.Repositories;
using MSS.Application.Services;


if (args.Length != 1 || !int.TryParse(args[0], out int port))
{
    Console.WriteLine("Usage: SocketServer <port>");
    return;
}

var clientRepository = new ClientRepository();
var listener = new TcpListener(IPAddress.Any, port);
listener.Start();
Console.WriteLine($"Server started on port {port}. Waiting for connections...");

while (true)
{
    var client = await listener.AcceptTcpClientAsync();
    Console.WriteLine("New client connected.");
    _ = Task.Run(() => new ClientHandler(new TcpClientWrapper(client), clientRepository).HandleClientAsync());
}