namespace MarketData.IG.StreamingApi.Models
{
    public class ChartTickData : ChartDataBase
    {
        /// <summary>
        /// Bid price
        /// </summary>
        public decimal? Bid { get; set; }
        /// <summary>
        /// Offer price
        /// </summary>
        public decimal? Offer { get; set; }
        /// <summary>
        /// Last traded price
        /// </summary>
        public decimal? LastTradedPrice { get; set; }
    }

}
