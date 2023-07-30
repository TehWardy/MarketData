using Lightstreamer.DotNet.Client;
using MarketData.IG.StreamingApi.Models;

namespace MarketData.IG.StreamingApi
{
    public class ChartCandleTableListerner : TableListenerAdapterBase<ChartCandelData>
    {
        protected override ChartCandelData LoadUpdate(IUpdateInfo update)
        {
            try
            {
                var updateData = new ChartCandelData
                {
                    Bid = new HlocData { High = StringToNullableDecimal(update.GetNewValue("BID_HIGH")), Low = StringToNullableDecimal(update.GetNewValue("BID_LOW")), Open = StringToNullableDecimal(update.GetNewValue("BID_OPEN")), Close = StringToNullableDecimal(update.GetNewValue("BID_CLOSE")) },
                    Offer = new HlocData { High = StringToNullableDecimal(update.GetNewValue("OFR_HIGH")), Low = StringToNullableDecimal(update.GetNewValue("OFR_LOW")), Open = StringToNullableDecimal(update.GetNewValue("OFR_OPEN")), Close = StringToNullableDecimal(update.GetNewValue("OFR_CLOSE")) },
                    LastTradedPrice = new HlocData { High = StringToNullableDecimal(update.GetNewValue("LTP_HIGH")), Low = StringToNullableDecimal(update.GetNewValue("LTP_LOW")), Open = StringToNullableDecimal(update.GetNewValue("LTP_OPEN")), Close = StringToNullableDecimal(update.GetNewValue("LTP_CLOSE")) },
                    LastTradedVolume = StringToNullableDecimal(update.GetNewValue("LTV")),
                    IncrimetalTradingVolume = StringToNullableDecimal(update.GetNewValue("TTV")),
                    UpdateTime = EpocStringToNullableDateTime(update.GetNewValue("UTM")),
                    DayMidOpenPrice = StringToNullableDecimal(update.GetNewValue("DAY_OPEN_MID")),
                    DayChange = StringToNullableDecimal(update.GetNewValue("DAY_NET_CHG_MID")),
                    DayChangePct = StringToNullableDecimal(update.GetNewValue("DAY_PERC_CHG_MID")),
                    DayHigh = StringToNullableDecimal(update.GetNewValue("DAY_HIGH")),
                    DayLow = StringToNullableDecimal(update.GetNewValue("DAY_LOW")),
                    TickCount = StringToNullableInt(update.GetNewValue("CONS_TICK_COUNT"))
                };
                var conEnd = StringToNullableInt(update.GetNewValue("CONS_END"));
                updateData.EndOfConsolidation = conEnd.HasValue ? conEnd > 0 : null;
                return updateData;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

}
