namespace MarketData.IG.Models.auth.silentlogin;

public class SilentLoginRequest
{
    ///<Summary>
    ///Whether this is a newly created client logging into demo for the very first time after account creation.
    ///</Summary>
    public bool firstTimeDemoLogin { get; set; }
}
