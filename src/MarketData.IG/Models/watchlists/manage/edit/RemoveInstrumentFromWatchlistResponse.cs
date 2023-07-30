namespace MarketData.IG.Models.watchlists.manage.edit
{

    public class RemoveInstrumentFromWatchlistResponse
    {
        public enum Status
        {

            ///<Summary>
            ///Success
            ///</Summary>

            SUCCESS,
        }
        ///<Summary>
        ///Status
        ///</Summary>
        public string status { get; set; }
    }
}
