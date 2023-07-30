using System;

namespace IGWebApiClient
{
    public class ChartTickTableListerner : TableListenerAdapterBase<ChartTickData>
	{
		protected override ChartTickData LoadUpdate(IUpdateInfo update)
		{
			try
			{
				var updateData = new ChartTickData
				{
					Bid = StringToNullableDecimal(update.GetNewValue("BID")),
					Offer = StringToNullableDecimal(update.GetNewValue("OFR")),
					LastTradedPrice = StringToNullableDecimal(update.GetNewValue("LTP")),
					LastTradedVolume = StringToNullableDecimal(update.GetNewValue("LTV")),
					IncrimetalTradingVolume = StringToNullableDecimal(update.GetNewValue("TTV")),
					UpdateTime = EpocStringToNullableDateTime(update.GetNewValue("UTM")),
					DayMidOpenPrice = StringToNullableDecimal(update.GetNewValue("DAY_OPEN_MID")),
					DayChange = StringToNullableDecimal(update.GetNewValue("DAY_NET_CHG_MID")),
					DayChangePct = StringToNullableDecimal(update.GetNewValue("DAY_PERC_CHG_MID")),
					DayHigh = StringToNullableDecimal(update.GetNewValue("DAY_HIGH")),
					DayLow = StringToNullableDecimal(update.GetNewValue("DAY_LOW"))
				};
				return updateData;
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
