using System;
using System.Collections.Generic;
using System.Linq;

namespace IGWebApiClient
{
    public class IGStreamingApiClient : IConnectionListener
    {

        private LSClient lsClient;

        public IGStreamingApiClient()
        {
            try
            {
                lsClient = new LSClient();
            }
            catch (Exception)
            {
                
            }
        }

        public bool Connect(string username, string cstToken, string xSecurityToken, string apiKey, string lsHost)
        {
            bool connectionEstablished = false;

            ConnectionInfo connectionInfo = new ConnectionInfo();
            connectionInfo.Adapter = "DEFAULT";
            connectionInfo.User = username;
			connectionInfo.Password = string.Format("CST-{0}|XST-{1}", cstToken, xSecurityToken);
            connectionInfo.PushServerUrl = lsHost;
            try
            {
                if (lsClient != null)
                {
                    lsClient.OpenConnection(connectionInfo, this);
                    connectionEstablished = true;
                }
            }
            catch (Exception)
            {
                connectionEstablished = false;
            }
            return connectionEstablished;
        }

		[Obsolete("Use 'Disconnect' instead, this will be removed in future versions")]
        public void disconnect()
        {
			Disconnect();
		}

		public void Disconnect()
		{
            if (lsClient != null)
            {
                lsClient.CloseConnection();
            }
        }

        /// <summary>
        /// account details subscription
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="tableListener"></param>
		[Obsolete("Use 'SubscribeToAccountDetails' instead, this will be removed in future versions")]
        public void subscribeToAccountDetails(string accountId, IHandyTableListener tableListener)
        {
			SubscribeToAccountDetails(accountId, tableListener);
        }

        /// <summary>
        /// account details subscription
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="tableListener"></param>
		[Obsolete("Use 'SubscribeToAccountDetails' instead, this will be removed in future versions")]
		public void subscribeToAccountDetails(string accountId, IHandyTableListener tableListener, IEnumerable<string> fields)
		{
			SubscribeToAccountDetails(accountId, tableListener, fields);
		}

		[Obsolete("Use 'SubscribeToAccountDetails' instead, this will be removed in future versions")]
		public SubscribedTableKey subscribeToAccountDetailsKey(string accountId, IHandyTableListener tableListener)
        {
			return SubscribeToAccountDetails(accountId, tableListener);
        }

		public SubscribedTableKey SubscribeToAccountDetails(string accountId, IHandyTableListener tableListener)
		{
			return SubscribeToAccountDetails(accountId, tableListener, new[] { "PNL", "DEPOSIT", "USED_MARGIN", "AMOUNT_DUE", "AVAILABLE_CASH" });
		}

		[Obsolete("Use 'SubscribeToAccountDetails' instead, this will be removed in future versions")]
		public SubscribedTableKey subscribeToAccountDetailsKey(string accountId, IHandyTableListener tableListener,
			IEnumerable<string> fields)
        {
			return SubscribeToAccountDetails(accountId, tableListener, fields);
        }

		public SubscribedTableKey SubscribeToAccountDetails(string accountId, IHandyTableListener tableListener, IEnumerable<string> fields)
        {
            ExtendedTableInfo extTableInfo = new ExtendedTableInfo(
				new[] { "ACCOUNT:" + accountId },
                "MERGE",
				fields.ToArray(),
                true
                );

            return lsClient.SubscribeTable(extTableInfo, tableListener, false);
        }


        /// <summary>
        /// L1 Prices subscription
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="tableListener"></param>
		public SubscribedTableKey SubscribeToMarketDetails(IEnumerable<string> epics, IHandyTableListener tableListener)
        {
			return SubscribeToMarketDetails(epics, tableListener,
				new[] { 
                    "MID_OPEN", "HIGH", "LOW", "CHANGE", "CHANGE_PCT", "UPDATE_TIME", 
                    "MARKET_DELAY", "MARKET_STATE", "BID", "OFFER" 
                    
                    /*, "BID_QUOTE_ID", "OFR_QUOTE_ID"*/
                });
        }

        /// <summary>
        /// L1 Prices subscription
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="tableListener"></param>
		[Obsolete("Use 'SubscribeToMarketDetails' instead, this will be removed in future versions")]
		public SubscribedTableKey subscribeToMarketDetails(IEnumerable<string> epics, IHandyTableListener tableListener, IEnumerable<string> fields)
        {
			return SubscribeToMarketDetails(epics, tableListener, fields);
		}

		public SubscribedTableKey SubscribeToMarketDetails(IEnumerable<string> epics, IHandyTableListener tableListener, IEnumerable<string> fields)
            {

			string[] items = epics.Select(e => string.Format("L1:{0}", e)).ToArray();
			ExtendedTableInfo extTableInfo = new ExtendedTableInfo(
				items,
				"MERGE",
				fields.ToArray(),
				true
				);
			return lsClient.SubscribeTable(extTableInfo, tableListener, false);
		}

		public SubscribedTableKey SubscribeToChartTicks(IEnumerable<string> epics, IHandyTableListener tableListener)
		{
			return SubscribeToChartTicks(epics, tableListener,
				new[] { 
                    "BID", "OFR", "LTP", "LTV", "TTV", "UTM", 
                    "DAY_OPEN_MID", "DAY_NET_CHG_MID", "DAY_PERC_CHG_MID", "DAY_HIGH", "DAY_LOW"
                });
		}

		public SubscribedTableKey SubscribeToChartTicks(IEnumerable<string> epics, IHandyTableListener tableListener, string[] fields)
		{
			string[] items = epics.Select(e => string.Format("CHART:{0}:TICK", e)).ToArray();
			ExtendedTableInfo extTableInfo = new ExtendedTableInfo(
				items,
				"DISTINCT",
				fields,
				true
				);
			return lsClient.SubscribeTable(extTableInfo, tableListener, false);
		}

		public SubscribedTableKey SubscribeToChartCandleData(IEnumerable<string> epics, ChartScale scale, IHandyTableListener tableListener)
		{
			return SubscribeToChartCandleData(epics, scale, tableListener,
				new[] { 
                    "LTV", "TTV", "UTM", 
					"DAY_OPEN_MID", "DAY_NET_CHG_MID", "DAY_PERC_CHG_MID", "DAY_HIGH", "DAY_LOW", 
					"OFR_OPEN", "OFR_HIGH", "OFR_LOW", "OFR_CLOSE",
					"BID_OPEN", "BID_HIGH", "BID_LOW", "BID_CLOSE",
					"LTP_OPEN", "LTP_HIGH", "LTP_LOW", "LTP_CLOSE",
					"CONS_END", "CONS_TICK_COUNT",
                });
            }


		public SubscribedTableKey SubscribeToChartCandleData(IEnumerable<string> epics, ChartScale scale, IHandyTableListener tableListener, string[] fields)
		{
			string[] items = epics.Select(e => string.Format("CHART:{0}:{1}", e, GetScale(scale))).ToArray();
            ExtendedTableInfo extTableInfo = new ExtendedTableInfo(
                items,
                "MERGE",
                fields,
                true
                );
            return lsClient.SubscribeTable(extTableInfo, tableListener, false);
        }

		private string GetScale(ChartScale scale)
		{
			switch (scale)
			{
				case ChartScale.OneSecond:
					return "SECOND";
				case ChartScale.OneMinute:
					return "1MINUTE";
				case ChartScale.FiveMinute:
					return "5MINUTE";
				case ChartScale.OneHour:
					return "HOUR";
				default:
					throw new ArgumentOutOfRangeException("scale");
			}
		}

        /// <summary>
        /// trade subscription details
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="tableListener"></param>
		[Obsolete("Use 'SubscribeToTradeSubscription' instead, this will be removed in future versions")]
		public SubscribedTableKey subscribeToTradeSubscription(string accountId, IHandyTableListener tableListener)
		{
			return SubscribeToTradeSubscription(accountId, tableListener);
		}

		public SubscribedTableKey SubscribeToTradeSubscription(string accountId, IHandyTableListener tableListener)
        {
            return subscribeToTradeSubscription(accountId, tableListener,
				new[] { 
                    "CONFIRMS", "OPU", "WOU"
                });
        }

		private SubscribedTableKey subscribeToTradeSubscription(string accountId, IHandyTableListener tableListener,
			string[] fields)
		{
			return SubscribeToTradeSubscription(accountId, tableListener, fields);
		}
		public SubscribedTableKey SubscribeToTradeSubscription(string accountId, IHandyTableListener tableListener, IEnumerable<string> fields)
        {
            ExtendedTableInfo extTableInfo = new ExtendedTableInfo(
				new[] { "TRADE:" + accountId },
                "DISTINCT",
				fields.ToArray(),
                true
                );
            return lsClient.SubscribeTable(extTableInfo, tableListener, false);
        }

        public void UnsubscribeTableKey(SubscribedTableKey stk)
        {
            try
            {              
                if (lsClient != null)
                {
                    lsClient.UnsubscribeTable(stk);
                }
            }
            catch (Exception)
            {
                
            }
        }


        public virtual void OnActivityWarning(bool warningOn)
        {

        }

        public virtual void OnClose()
        {
            if (lsClient != null)
            {
                lsClient.CloseConnection();
            }
        }

        public virtual void OnConnectionEstablished()
        {

        }

        public virtual void OnDataError(PushServerException e)
        {

        }

        public virtual void OnEnd(int cause)
        {

        }

        public virtual void OnFailure(PushConnException e)
        {

        }

        public virtual void OnFailure(PushServerException e)
        {

        }

        public virtual void OnNewBytes(long bytes)
        {

        }

        public virtual void OnSessionStarted(bool isPolling)
        {            
        }

    }

}
