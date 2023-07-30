namespace MarketData.IG.StreamingApi.Models
{
    public class LsTradeSubscriptionData
    {
        public StreamingDirectionEnum? direction;
        public string limitLevel; // if this is null we get an exception  - should be a decimal
        public string dealId;
        public string affectedDealId;
        public string stopLevel; // should be decimal but throws an exception if null.
        public string expiry;
        public string size; // should be decimal ...
        public StreamingStatusEnum? status;
        public string epic;
        public string level; // decimal
        public bool? guaranteedStop;
        public string dealReference;
        public StreamingDealStatusEnum? dealStatus;
        public List<AffectedDeals> affectedDeals;
    }

}
