using Microsoft.AspNetCore.SignalR;

namespace Notification.API.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        { 

                
            await base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(System.Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
