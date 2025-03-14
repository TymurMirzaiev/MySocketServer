using System.Collections.Concurrent;
using MSS.Application.Models;

namespace MSS.Application.Repositories
{
    public class ClientRepository
    {
        public ConcurrentDictionary<string, ClientInfo> Clients { get; } = new ConcurrentDictionary<string, ClientInfo>();
    }
}
