namespace MarketData.IG.StreamingApi.Models
{
    public class ChartCandelData : ChartDataBase
    {
        public HlocData Offer { get; set; }
        public HlocData Bid { get; set; }
        public HlocData LastTradedPrice { get; set; }
        public bool? EndOfConsolidation { get; set; }
        public int? TickCount { get; set; }
    }

}
