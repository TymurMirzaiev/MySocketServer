using System.Text;
using MSS.Application.Models;
using MSS.Application.Repositories;

namespace MSS.Application.Services
{
    public class ClientHandler
    {
        private readonly ITcpClientWrapper _client;
        private readonly ClientRepository _clientRepository;
        private readonly ClientInfo _clientInfo;

        public ClientHandler(ITcpClientWrapper client, ClientRepository clientRepository)
        {
            _client = client;
            _clientRepository = clientRepository;
            _clientInfo = new ClientInfo
            {
                ConnectionId = Guid.NewGuid().ToString(),
                RemoteEndPoint = client.RemoteEndPoint.ToString()!,
                Sum = 0
            };
            _clientRepository.Clients.TryAdd(_clientInfo.ConnectionId, _clientInfo);
        }

        public async Task HandleClientAsync()
        {
            using (var stream = _client.GetStream())
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            using (var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true })
            {
                await writer.WriteLineAsync($"Welcome! Enter a number or 'list' to see connected clients.");

                while (true)
                {
                    try
                    {
                        var input = await reader.ReadLineAsync();
                        if (input == null) break;

                        var response = ProcessInput(input);
                        await writer.WriteLineAsync(response);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        break;
                    }
                }
            }

            _client.Close();
            _clientRepository.Clients.TryRemove(_clientInfo.ConnectionId, out _);
            Console.WriteLine($"Client {_clientInfo.RemoteEndPoint} disconnected.");
        }

        public string ProcessInput(string input) // can be made internal for testing purposes, but it just simpler for now
        {
            if (input.Equals("list", StringComparison.OrdinalIgnoreCase))
            {
                var sb = new StringBuilder("Connected clients:\n");
                foreach (var client in _clientRepository.Clients.Values)
                {
                    sb.AppendLine($"{client.RemoteEndPoint}: Sum = {client.Sum}");
                }
                return sb.ToString();
            }

            if (int.TryParse(input, out int number))
            {
                _clientInfo.Sum += number;
                return $"Current sum: {_clientInfo.Sum}";
            }

            return "Error: Please enter a valid number or 'list'.";
        }
    }
}
