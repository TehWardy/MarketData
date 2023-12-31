namespace MarketData.IG.Models.marketdetails.v2
{

    public class MarketDetailsResponse
    {
        ///<Summary>
        ///Instrument data
        ///</Summary>
        public InstrumentData instrument { get; set; }
        ///<Summary>
        ///Dealing rule data
        ///</Summary>
        public DealingRulesData dealingRules { get; set; }
        ///<Summary>
        ///Market snapshot data
        ///</Summary>
        public MarketSnapshotData snapshot { get; set; }
    }
}
