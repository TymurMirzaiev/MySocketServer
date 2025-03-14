using System.Net;
using System.Net.Sockets;

namespace MSS.Application.Models
{
    public interface ITcpClientWrapper
    {
        EndPoint RemoteEndPoint { get; }
        NetworkStream GetStream();
        void Close();
    }

    public class TcpClientWrapper : ITcpClientWrapper
    {
        private readonly TcpClient _tcpClient;

        public TcpClientWrapper(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
        }

        public EndPoint RemoteEndPoint => _tcpClient.Client.RemoteEndPoint!;
        public NetworkStream GetStream() => _tcpClient.GetStream();
        public void Close() => _tcpClient.Close();
    }
}
