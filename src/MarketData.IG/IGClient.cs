using Lightstreamer.DotNet.Client;
using MarketData.IG.Models.auth.session.v2;
using MarketData.IG.Models.prices.v1;
using MarketData.IG.Models.search;
using MarketData.IG.RESTApi;
using MarketData.IG.StreamingApi;
using MarketData.IG.StreamingApi.Models;

namespace MarketData.IG;

public class IGClient
{
    readonly IGConfiguration config;
    readonly IDictionary<string, SubscribedTableKey> subscriptions;
    public IgRestApiClient RestApi { get; }
    public IGStreamingApiClient StreamingClient { get; }

    public IGClient(IGConfiguration config)
    {
        RestApi = new IgRestApiClient(config.Environment);
        StreamingClient = new IGStreamingApiClient();
        this.config = config;
        subscriptions = new Dictionary<string, SubscribedTableKey>();

    }

    public async Task Login()
    {
        var auth = new AuthenticationRequest
        {
            identifier = config.UserName,
            password = config.Password
        };

        var igAuthResponse = await RestApi.SecureAuthenticate(auth, config.ApiKey);
        var authResponse = igAuthResponse.Response;

        StreamingClient.Connect(
            authResponse.currentAccountId,
            RestApi.ConversationContext.cst,
            RestApi.ConversationContext.xSecurityToken,
            RestApi.ConversationContext.apiKey,
            authResponse.lightstreamerEndpoint);
    }

    public void Logout()
    {
        StreamingClient.Disconnect();
        RestApi.logout();
    }

    public void SubscribeToMarket(string reference, string[] epics, EventHandler<UpdateArgs<L1LsPriceData>> onUpdate)
    {
        var marketListener = new MarketDetailsTableListener();
        marketListener.Update += onUpdate;
        subscriptions.Add(reference, StreamingClient.SubscribeToMarketDetails(epics, marketListener));
    }

    public void UnsubscribeFromAllMarkets()
    {
        foreach (var subscription in subscriptions)
            UnsubscribeFromMarket(subscription.Key);
    }

    public void UnsubscribeFromMarket(string reference) =>
        StreamingClient.UnsubscribeTableKey(subscriptions[reference]);

    public async Task<IEnumerable<PriceSnapshot>> GetMarketHistory(string epic, string resolution, DateTime startDate, DateTime endDate)
    {
        var igResult = await RestApi.priceSearchByDate(
            epic, 
            resolution, 
            startDate.ToString("yyyy-MM-dd"), 
            endDate.ToString("yyyy-MM-dd"));

        return igResult.Response.prices;
    }

    public async Task<IEnumerable<Market>> SearchMarkets(string searchTerm)
    {
        var data = (await RestApi.searchMarket(searchTerm)).Response;
        return data.markets.Where(m => m.streamingPricesAvailable);
    }
}