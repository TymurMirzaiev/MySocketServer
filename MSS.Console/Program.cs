using Microsoft.AspNetCore.SignalR.Client;
using MSS.Application.Models;

var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chat")
                .Build();

connection.On<int>("ReceiveSum", (sum) =>
{
    Console.WriteLine($"Current sum: {sum}");
});

connection.On<List<ClientInfo>>("ReceiveClientList", (clientList) =>
{
    Console.WriteLine("Connected clients:");
    foreach (var client in clientList)
    {
        Console.WriteLine($"{client.ConnectionId}: Sum = {client.Sum}");
    }
});

await connection.StartAsync();
Console.WriteLine("Connected to the server. Enter a number or 'list' to see connected clients.");

while (true)
{
    var input = Console.ReadLine();
    if (input?.Equals("list", StringComparison.OrdinalIgnoreCase) ?? false)
    {
        await connection.InvokeAsync("RequestClientList");
    }
    else if (int.TryParse(input, out int number))
    {
        await connection.InvokeAsync("SendNumber", number);
    }
    else
    {
        Console.WriteLine("Error: Please enter a valid number or 'list'.");
    }
}