using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SmartApartmentSystem.API.Infrastructure
{
    public class SignalRHub : Hub
    {
        private readonly ILogger<SignalRHub> _logger;

        public SignalRHub(ILogger<SignalRHub> logger)
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation("User connected {0}", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {

            _logger.LogInformation("User disconnected {0} - {1}", Context.ConnectionId, exception);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
