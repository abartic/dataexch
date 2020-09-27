using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace DataExch.Hubs
{
    public class CommunicationHub : Hub
    {

        public async Task SendMessage(string message)
        {
	        await Clients.AllExcept(this.Context.ConnectionId).SendAsync("ReceiveMessage", message);
        }

    }
}

