namespace MarketData.IG.Models.application.operation;

public class UpdateApplicationRequest
{
    ///<Summary>
    ///</Summary>
    public string apiKey { get; set; }
    ///<Summary>
    ///</Summary>
    public string status { get; set; }
    ///<Summary>
    ///</Summary>
    public int allowanceAccountTrading { get; set; }
    ///<Summary>
    ///</Summary>
    public int allowanceAccountOverall { get; set; }
}
