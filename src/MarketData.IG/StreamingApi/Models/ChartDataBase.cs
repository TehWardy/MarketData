namespace MarketData.IG.StreamingApi.Models
{
    public class ChartDataBase
    {

        /// <summary>
        /// Last traded volume
        /// </summary>
        public decimal? LastTradedVolume { get; set; }
        /// <summary>
        /// Incremental trading volume
        /// </summary>
        public decimal? IncrimetalTradingVolume { get; set; }
        /// <summary>
        /// Update time (as milliseconds from the Epoch)
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// Mid open price for the day
        /// </summary>
        public decimal? DayMidOpenPrice { get; set; }
        /// <summary>
        /// Change from open price to current (MID price)
        /// </summary>

        public decimal? DayChange { get; set; }
        /// <summary>
        /// Daily percentage change (MID price)
        /// </summary>
        public decimal? DayChangePct { get; set; }
        /// <summary>
        /// Daily high price (MID)
        /// </summary>
        public decimal? DayHigh { get; set; }
        /// <summary>
        /// Daily low price (MID)
        /// </summary>
        public decimal? DayLow { get; set; }
    }

}
