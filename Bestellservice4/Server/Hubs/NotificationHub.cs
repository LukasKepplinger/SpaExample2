using Microsoft.AspNetCore.SignalR;

namespace Bestellservice4.Server.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNewDishNotification(int dishId)
        {
            await Clients.All.SendAsync("NewDishNotification", dishId);
        }
    }
}
