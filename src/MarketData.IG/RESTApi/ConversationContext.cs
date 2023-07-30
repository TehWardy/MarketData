namespace MarketData.IG.RESTApi;

public class ConversationContext
{
    public ConversationContext(string cst, string xSecurityToken, string apiKey)
    {
        this.cst = cst;
        this.xSecurityToken = xSecurityToken;
        this.apiKey = apiKey;
    }

    public string cst;
    public string xSecurityToken;
    public string apiKey;
}
