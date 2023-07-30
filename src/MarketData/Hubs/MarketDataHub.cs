
using MarketData.IG;
using MarketData.IG.StreamingApi.Models;
using Microsoft.AspNetCore.SignalR;

namespace MarketData.Hubs
{
    public class MarketDataHub : Hub
    {
        readonly IGClient ig;
        readonly IHubContext<MarketDataHub> hubContext;

        public MarketDataHub(IGClient ig, IHubContext<MarketDataHub> hubContext)
        {
            this.ig = ig;
            this.hubContext = hubContext;
        }

        public async Task Subscribe(string epic)
        {
            await hubContext.Groups.AddToGroupAsync(Context.ConnectionId, epic);
            await hubContext.Clients.Group(epic).SendAsync("MarketUpdate", "Subscribed", epic);
            ig.SubscribeToMarket(epic, new[] { epic }, (sender, args) => OnMarketUpdate(args,epic));
        }

        public async Task Unsubscribe(string epic)
        {
            await hubContext.Clients.Group(epic).SendAsync("MarketUpdate", "Unsubscribed", epic);
            await hubContext.Groups.RemoveFromGroupAsync(Context.ConnectionId, epic);
            ig.UnsubscribeFromMarket(epic);
        }

        async void OnMarketUpdate(UpdateArgs<L1LsPriceData> e, string epic)
        {
            if (e.UpdateData.UpdateTime is not null)
            {
                var update = new 
                { 
                    e.UpdateData.UpdateTime, 
                    e.ItemName, 
                    e.UpdateData.Bid, 
                    e.UpdateData.Offer, 
                    e.UpdateData.Change, 
                    e.UpdateData.ChangePct 
                };

                await hubContext.Clients.Group(epic).SendAsync("MarketUpdate", "Update", update);
            }
        }
    }
}