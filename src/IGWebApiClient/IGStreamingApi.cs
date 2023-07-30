using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Lightstreamer.DotNet.Client;

namespace IGWebApiClient
{
    public enum StreamingDirectionEnum
    {
        BUY,
        SELL
    }

    public enum StreamingStatusEnum
    {
        OPEN,
        UPDATED,
        AMENDED,
        CLOSED,
        DELETED,
    }

    public enum StreamingDealStatusEnum
    {
        ACCEPTED,
        REJECTED,
    }

    public enum TradeSubscriptionTypeEnum
    {
        WOU = 0,
        OPU = 1,
        TRADE = 2,
    }

	public class AffectedDeals
	{
		public string dealId;
		public string status;
	}

	public enum ChartScale
	{
		OneSecond,
		OneMinute,
		FiveMinute,
		OneHour,
	}

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

    public class L1LsPriceData
    {
        public decimal? MidOpen;
        public decimal? High;
        public decimal? Low;
        public decimal? Change;
        public decimal? ChangePct;
        public string UpdateTime;
        public int? MarketDelay;
        public string MarketState;
        public decimal? Bid;
        public decimal? Offer;       
    }

    public class StreamingAccountData
    {
        public decimal? ProfitAndLoss;
        public decimal? Deposit;
        public decimal? UsedMargin;
        public decimal? AmountDue;
        public decimal? AvailableCash;
    }   

    public class MarketStatus
    {
        public string marketstatus { get; set; }       
    }
   
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

                if (!String.IsNullOrEmpty(midOpen))               
                {
                    lsL1PriceData.MidOpen = Convert.ToDecimal(midOpen, CultureInfo.InvariantCulture);
                }
                if (!String.IsNullOrEmpty(high))        
                {
                    lsL1PriceData.High = Convert.ToDecimal(high, CultureInfo.InvariantCulture);
                }
                if (!String.IsNullOrEmpty(low))  
                {
                    lsL1PriceData.Low = Convert.ToDecimal(low, CultureInfo.InvariantCulture);
                }
                if (!String.IsNullOrEmpty(change))
                {
                    lsL1PriceData.Change = Convert.ToDecimal(change, CultureInfo.InvariantCulture);
                }
                if (!String.IsNullOrEmpty(changePct))
                {
                    lsL1PriceData.ChangePct = Convert.ToDecimal(changePct, CultureInfo.InvariantCulture);
                }
                if (!String.IsNullOrEmpty(updateTime))               
                {
                    lsL1PriceData.UpdateTime = updateTime;
                }
                if (!String.IsNullOrEmpty(marketDelay))
                {
                    lsL1PriceData.MarketDelay = Convert.ToInt32(marketDelay, CultureInfo.InvariantCulture);
                }
                if (!String.IsNullOrEmpty(marketState))
                {              
                    lsL1PriceData.MarketState = marketState;
                }
                if (!String.IsNullOrEmpty(bid))
                {
                    lsL1PriceData.Bid = Convert.ToDecimal(bid, CultureInfo.InvariantCulture);
                }
                if (!String.IsNullOrEmpty(offer))
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
                                       
                if (!String.IsNullOrEmpty(pnl))
                {
                    streamingAccountData.ProfitAndLoss = Convert.ToDecimal(pnl, CultureInfo.InvariantCulture);
                }
                if (!String.IsNullOrEmpty(deposit))
                {
                    streamingAccountData.Deposit = Convert.ToDecimal(deposit, CultureInfo.InvariantCulture);
                }
                if (!String.IsNullOrEmpty(usedMargin))
                {
                    streamingAccountData.UsedMargin = Convert.ToDecimal(usedMargin, CultureInfo.InvariantCulture);
                }
                if (!String.IsNullOrEmpty(amountDue))
                {
                    streamingAccountData.AmountDue = Convert.ToDecimal(amountDue, CultureInfo.InvariantCulture);
                }
                if (!String.IsNullOrEmpty(availableCash))
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

                if (!String.IsNullOrEmpty(midOpen))
                {
                    sb.AppendLine("mid open: " + midOpen);
                }
                if (!String.IsNullOrEmpty(high))
                {
                    sb.AppendLine("high: " + high);
                }
                if (!String.IsNullOrEmpty(low))
                {
                    sb.AppendLine("low: " + low);
                }
                if (!String.IsNullOrEmpty(change))
                {
                    sb.AppendLine("change: " + change);
                }
                if (!String.IsNullOrEmpty(changePct))
                {
                    sb.AppendLine("change percent: " + changePct);
                }
                if (!String.IsNullOrEmpty(updateTime))
                {
                    sb.AppendLine("update time: " + updateTime);
                }
                if (!String.IsNullOrEmpty(marketDelay))
                {
                    sb.AppendLine("market delay: " + marketDelay);
                }
                if (!String.IsNullOrEmpty(marketState))
                {
                    sb.AppendLine("market state: " + marketState);
                }
                if (!String.IsNullOrEmpty(bid))
                {
                    sb.AppendLine("bid: " + bid);
                }
                if (!String.IsNullOrEmpty(offer))
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
       
	public class TableListenerAdapterBase : IHandyTableListener
	{
        public virtual void OnRawUpdatesLost(int itemPos, string itemName, int lostUpdates)
        {
        }

        public virtual void OnSnapshotEnd(int itemPos, string itemName)
        {
        }

        public virtual void OnUnsubscr(int itemPos, string itemName)
        {
        }

        public virtual void OnUnsubscrAll()
        {
        }

        public virtual void OnUpdate(int itemPos, string itemName, IUpdateInfo update)
        {
		}

		protected decimal? StringToNullableDecimal(string value)
		{
			decimal number;
			return decimal.TryParse(value, out number) ? number : (decimal?)null;
		}

		protected int? StringToNullableInt(string value)
		{
			int number;
			return int.TryParse(value, out number) ? number : (int?)null;
		}

		protected DateTime? EpocStringToNullableDateTime(string value)
		{
			ulong epoc;
			if (!ulong.TryParse(value, out epoc))
			{
				return null;
			}
			return new DateTime(1970,1,1,0,0,0,0).AddMilliseconds(epoc);
		}
		
	}
}
