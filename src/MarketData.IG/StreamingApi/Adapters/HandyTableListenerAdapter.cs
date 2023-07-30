using Lightstreamer.DotNet.Client;
using MarketData.IG.StreamingApi.Models;
using System.Globalization;
using System.Text;

namespace MarketData.IG.StreamingApi.Adapters
{
    public class HandyTableListenerAdapter : TableListenerAdapterBase
    {
        public L1LsPriceData L1LsPriceUpdateData(int itemPos, string itemName, IUpdateInfo update)
        {
            var lsL1PriceData = new L1LsPriceData();
            try
            {
                var midOpen = update.GetNewValue("MID_OPEN");
                var high = update.GetNewValue("HIGH");
                var low = update.GetNewValue("LOW");
                var change = update.GetNewValue("CHANGE");
                var changePct = update.GetNewValue("CHANGE_PCT");
                var updateTime = update.GetNewValue("UPDATE_TIME");
                var marketDelay = update.GetNewValue("MARKET_DELAY");
                var marketState = update.GetNewValue("MARKET_STATE");
                var bid = update.GetNewValue("BID");
                var offer = update.GetNewValue("OFFER");

                if (!string.IsNullOrEmpty(midOpen))
                {
                    lsL1PriceData.MidOpen = Convert.ToDecimal(midOpen, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(high))
                {
                    lsL1PriceData.High = Convert.ToDecimal(high, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(low))
                {
                    lsL1PriceData.Low = Convert.ToDecimal(low, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(change))
                {
                    lsL1PriceData.Change = Convert.ToDecimal(change, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(changePct))
                {
                    lsL1PriceData.ChangePct = Convert.ToDecimal(changePct, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(updateTime))
                {
                    lsL1PriceData.UpdateTime = updateTime;
                }
                if (!string.IsNullOrEmpty(marketDelay))
                {
                    lsL1PriceData.MarketDelay = Convert.ToInt32(marketDelay, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(marketState))
                {
                    lsL1PriceData.MarketState = marketState;
                }
                if (!string.IsNullOrEmpty(bid))
                {
                    lsL1PriceData.Bid = Convert.ToDecimal(bid, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(offer))
                {
                    lsL1PriceData.Offer = Convert.ToDecimal(offer, CultureInfo.InvariantCulture);
                }
            }
            catch (Exception)
            {
            }
            return lsL1PriceData;
        }

        public StreamingAccountData StreamingAccountDataUpdates(int itemPos, string itemName, IUpdateInfo update)
        {
            var streamingAccountData = new StreamingAccountData();
            try
            {
                var pnl = update.GetNewValue("PNL");
                var deposit = update.GetNewValue("DEPOSIT");
                var usedMargin = update.GetNewValue("USED_MARGIN");
                var amountDue = update.GetNewValue("AMOUNT_DUE");
                var availableCash = update.GetNewValue("AVAILABLE_CASH");

                if (!string.IsNullOrEmpty(pnl))
                {
                    streamingAccountData.ProfitAndLoss = Convert.ToDecimal(pnl, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(deposit))
                {
                    streamingAccountData.Deposit = Convert.ToDecimal(deposit, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(usedMargin))
                {
                    streamingAccountData.UsedMargin = Convert.ToDecimal(usedMargin, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(amountDue))
                {
                    streamingAccountData.AmountDue = Convert.ToDecimal(amountDue, CultureInfo.InvariantCulture);
                }
                if (!string.IsNullOrEmpty(availableCash))
                {
                    streamingAccountData.AmountDue = Convert.ToDecimal(availableCash, CultureInfo.InvariantCulture);
                }

            }
            catch (Exception)
            {
            }
            return streamingAccountData;
        }

        public string L1PriceUpdates(int itemPos, string itemName, IUpdateInfo update)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Item Position: " + itemPos);
            sb.AppendLine("Item Name: " + itemName);

            try
            {
                var midOpen = update.GetNewValue("MID_OPEN");
                var high = update.GetNewValue("HIGH");
                var low = update.GetNewValue("LOW");
                var change = update.GetNewValue("CHANGE");
                var changePct = update.GetNewValue("CHANGE_PCT");
                var updateTime = update.GetNewValue("UPDATE_TIME");
                var marketDelay = update.GetNewValue("MARKET_DELAY");
                var marketState = update.GetNewValue("MARKET_STATE");
                var bid = update.GetNewValue("BID");
                var offer = update.GetNewValue("OFFER");

                if (!string.IsNullOrEmpty(midOpen))
                {
                    sb.AppendLine("mid open: " + midOpen);
                }
                if (!string.IsNullOrEmpty(high))
                {
                    sb.AppendLine("high: " + high);
                }
                if (!string.IsNullOrEmpty(low))
                {
                    sb.AppendLine("low: " + low);
                }
                if (!string.IsNullOrEmpty(change))
                {
                    sb.AppendLine("change: " + change);
                }
                if (!string.IsNullOrEmpty(changePct))
                {
                    sb.AppendLine("change percent: " + changePct);
                }
                if (!string.IsNullOrEmpty(updateTime))
                {
                    sb.AppendLine("update time: " + updateTime);
                }
                if (!string.IsNullOrEmpty(marketDelay))
                {
                    sb.AppendLine("market delay: " + marketDelay);
                }
                if (!string.IsNullOrEmpty(marketState))
                {
                    sb.AppendLine("market state: " + marketState);
                }
                if (!string.IsNullOrEmpty(bid))
                {
                    sb.AppendLine("bid: " + bid);
                }
                if (!string.IsNullOrEmpty(offer))
                {
                    sb.AppendLine("offer: " + offer);
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine("Exception in L1 Prices");
                sb.AppendLine(ex.Message);
            }
            return sb.ToString();
        }
    }

}
