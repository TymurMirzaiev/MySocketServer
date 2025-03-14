using System;
using System.Net;
using Moq;
using MSS.Application.Models;
using MSS.Application.Repositories;
using MSS.Application.Services;
using Xunit;

namespace MSS.UnitTests
{
    public class ClientHandlerTests
    {
        private readonly ClientHandler _clientHandler;
        private readonly ClientRepository _clientRepository;

        public ClientHandlerTests()
        {
            _clientRepository = new ClientRepository();

            var mockClient = new Mock<ITcpClientWrapper>();
            var mockEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);

            mockClient.Setup(c => c.RemoteEndPoint).Returns(mockEndPoint);

            _clientHandler = new ClientHandler(mockClient.Object, _clientRepository);
        }

        [Fact]
        public void ProcessInput_ValidNumber_UpdatesSum()
        {
            // Act
            var response1 = _clientHandler.ProcessInput("10");
            var response2 = _clientHandler.ProcessInput("20");

            // Assert
            Assert.Equal("Current sum: 10", response1);
            Assert.Equal("Current sum: 30", response2);
        }

        [Fact]
        public void ProcessInput_InvalidInput_ReturnsError()
        {
            // Act
            var response = _clientHandler.ProcessInput("invalid");

            // Assert
            Assert.Equal("Error: Please enter a valid number or 'list'.", response);
        }

        [Fact]
        public void ProcessInput_ListCommand_ReturnsConnectedClients()
        {
            // Arrange
            _clientHandler.ProcessInput("10");
            _clientHandler.ProcessInput("20");

            // Act
            var response = _clientHandler.ProcessInput("list");

            // Assert
            Assert.Contains("Connected clients:", response);
            Assert.Contains("127.0.0.1:5000: Sum = 30", response);
        }
    }
}
