using System;

namespace IGWebApiClient
{
    public class MarketDetailsTableListerner : TableListenerAdapterBase<L1LsPriceData>
	{
		protected override L1LsPriceData LoadUpdate(IUpdateInfo update)
		{
			try
			{
				var lsL1PriceData = new L1LsPriceData
				{
					MidOpen = StringToNullableDecimal(update.GetNewValue("MID_OPEN")),
					High = StringToNullableDecimal(update.GetNewValue("HIGH")),
					Low = StringToNullableDecimal(update.GetNewValue("LOW")),
					Change = StringToNullableDecimal(update.GetNewValue("CHANGE")),
					ChangePct = StringToNullableDecimal(update.GetNewValue("CHANGE_PCT")),
					UpdateTime = update.GetNewValue("UPDATE_TIME"),
					MarketDelay = StringToNullableInt(update.GetNewValue("MARKET_DELAY")),
					MarketState = update.GetNewValue("MARKET_STATE"),
					Bid = StringToNullableDecimal(update.GetNewValue("BID")),
					Offer = StringToNullableDecimal(update.GetNewValue("OFFER"))
				};
				return lsL1PriceData;
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
