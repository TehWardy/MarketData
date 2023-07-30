using MarketData.IG.Models;
using MarketData.IG.Models.accountactivity.activity;
using MarketData.IG.Models.accountactivity.transaction;
using MarketData.IG.Models.accountbalance;
using MarketData.IG.Models.accountswitch;
using MarketData.IG.Models.application.operation;
using MarketData.IG.Models.auth.encryptionkey;
using MarketData.IG.Models.browse;
using MarketData.IG.Models.browse.sprintmarkets;
using MarketData.IG.Models.clientsentiment;
using MarketData.IG.Models.confirms;
using MarketData.IG.Models.marketdetails.v2;
using MarketData.IG.Models.positions.close.v1;
using MarketData.IG.Models.positions.create.otc.v1;
using MarketData.IG.Models.positions.edit.v1;
using MarketData.IG.Models.prices.v1;
using MarketData.IG.Models.search;
using MarketData.IG.Models.watchlists.manage.create;
using MarketData.IG.Models.watchlists.manage.delete;
using MarketData.IG.Models.watchlists.manage.edit;
using MarketData.IG.Models.watchlists.retrieve;
using MarketData.IG.Models.workingorders.create.v1;
using MarketData.IG.Models.workingorders.delete.v1;
using MarketData.IG.Models.workingorders.edit.v1;

namespace MarketData.IG.RESTApi
{
    public partial class IgRestApiClient
    {
        public ConversationContext ConversationContext { get; set; }
        readonly IgRestService _igRestService;

        public IgRestApiClient(string environment) =>
            _igRestService = new IgRestService(environment);

        public async Task<IgResponse<Models.auth.session.v2.AuthenticationResponse>> SecureAuthenticate(Models.auth.session.v2.AuthenticationRequest ar, string apiKey)
        {
            ConversationContext = new ConversationContext(null, null, apiKey);
            var encryptedPassword = await SecurePassword(ar.password);

            if (encryptedPassword == ar.password)
            {
                ar.encryptedPassword = false;
            }
            else
            {
                ar.encryptedPassword = true;
            }
            ar.password = encryptedPassword;
            return await authenticate(ar);
        }

        private async Task<string> SecurePassword(string rawPassword)
        {
            var encryptedPassword = rawPassword;

            //Try encrypting password. If we can encrypt it, do so...                                                                            
            var secureResponse = await fetchEncryptionKey();

            if (secureResponse.Response != null)
            {
                byte[] encryptedBytes;

                // get a public key to ENCRYPT...
                Rsa rsa = new Rsa(Convert.FromBase64String(secureResponse.Response.encryptionKey), true);

                encryptedBytes = rsa.RsaEncrypt(string.Format("{0}|{1}", rawPassword, secureResponse.Response.timeStamp));
                encryptedPassword = Convert.ToBase64String(encryptedBytes);
            }

            return encryptedPassword;
        }

        ///<Summary>
        ///Creates a trading session, obtaining session tokens for subsequent API access.
        ///<p>
        ///   Please note that region-specific <a href=/loginrestrictions>login restrictions</a> may apply.
        ///</p>
        ///@param authenticationRequest Client login credentials
        ///@return Client summary account information
        ///</Summary>

        public async Task<IgResponse<Models.auth.session.v2.AuthenticationResponse>> authenticate(Models.auth.session.v2.AuthenticationRequest authenticationRequest)
        {
            return await _igRestService.RestfulService<Models.auth.session.v2.AuthenticationResponse>("/gateway/deal/session", HttpMethod.Post, "2", ConversationContext, authenticationRequest);
        }


        ///<Summary>
        ///Creates a trading session, obtaining session tokens for subsequent API access
        ///@return the encryption key to be used to encode the credentials
        ///</Summary>

        public async Task<IgResponse<EncryptionKeyResponse>> fetchEncryptionKey()
        {
            return await _igRestService.RestfulService<EncryptionKeyResponse>("/gateway/deal/session/encryptionKey", HttpMethod.Get, "1", ConversationContext);
        }

        ///<Summary>
        ///Log out of the current session
        ///</Summary>

        public async void logout()
        {
            await _igRestService.RestfulService("/gateway/deal/session", HttpMethod.Delete, "1", ConversationContext);
        }

        ///<Summary>
        ///Returns all top-level nodes (market categories) in the market navigation hierarchy.
        ///</Summary>

        public async Task<IgResponse<BrowseMarketsResponse>> browseRoot()
        {
            return await _igRestService.RestfulService<BrowseMarketsResponse>("/gateway/deal/marketnavigation", HttpMethod.Get, "1", ConversationContext);
        }

        ///<Summary>
        ///Returns all sub-nodes of the given node in the market navigation hierarchy
        ///@return the children of the selected node
        ///@throws BrowseMarketsException
        ///@pathParam nodeId the identifier of the node to browse
        ///</Summary>

        public async Task<IgResponse<BrowseMarketsResponse>> browse(string nodeId)
        {
            return await _igRestService.RestfulService<BrowseMarketsResponse>("/gateway/deal/marketnavigation/" + nodeId, HttpMethod.Get, "1", ConversationContext);
        }

        ///<Summary>
        ///Returns all open positions for the active account
        ///</Summary>

        public async Task<IgResponse<Models.positions.get.otc.v2.PositionsResponse>> getOTCOpenPositionsV2()
        {
            return await _igRestService.RestfulService<Models.positions.get.otc.v2.PositionsResponse>("/gateway/deal/positions", HttpMethod.Get, "2", ConversationContext);
        }

        ///<Summary>
        ///Returns all watchlists belonging to the active account
        ///</Summary>

        public async Task<IgResponse<ListOfWatchlistsResponse>> listOfWatchlists()
        {
            return await _igRestService.RestfulService<ListOfWatchlistsResponse>("/gateway/deal/watchlists", HttpMethod.Get, "1", ConversationContext);
        }

        ///<Summary>
        ///Returns the given watchlists markets
        ///@pathParam watchlistId Watchlist id
        ///</Summary>

        public async Task<IgResponse<WatchlistInstrumentsResponse>> instrumentsForWatchlist(string watchlistId)
        {
            return await _igRestService.RestfulService<WatchlistInstrumentsResponse>("/gateway/deal/watchlists/" + watchlistId, HttpMethod.Get, "1", ConversationContext);
        }

        ///<Summary>
        ///Returns all open working orders for the active account
        ///</Summary>

        public async Task<IgResponse<Models.workingorders.get.v2.WorkingOrdersResponse>> workingOrdersV2()
        {
            return await _igRestService.RestfulService<Models.workingorders.get.v2.WorkingOrdersResponse>("/gateway/deal/workingorders", HttpMethod.Get, "2", ConversationContext);
        }



        ///<Summary>
        ///Returns the account activity history for the last specified period
        ///@pathParam lastPeriod Interval in milliseconds
        ///</Summary>

        public async Task<IgResponse<ActivityHistoryResponse>> lastActivityPeriod(string lastPeriod)
        {
            return await _igRestService.RestfulService<ActivityHistoryResponse>("/gateway/deal/history/activity/" + lastPeriod, HttpMethod.Get, "1", ConversationContext);
        }

        ///<Summary>
        ///Returns the account activity history for the given date range
        ///@pathParam fromDateStr Start date in dd-mm-yyyy format
        ///@pathParam toDateStr End date in dd-mm-yyyy format
        ///</Summary>

        public async Task<IgResponse<ActivityHistoryResponse>> lastActivityTimeRange(string fromDate, string toDate)
        {
            return await _igRestService.RestfulService<ActivityHistoryResponse>("/gateway/deal/history/activity/" + fromDate + "/" + toDate, HttpMethod.Get, "1", ConversationContext);
        }


        ///<Summary>
        ///Returns the transaction history for the specified transaction type and period
        ///@pathParam transactionType Transaction type (( ALL, ALL_DEAL, DEPOSIT, WITHDRAWAL ) )
        ///@pathParam lastPeriod Interval in milliseconds
        ///</Summary>

        public async Task<IgResponse<TransactionHistoryResponse>> lastTransactionPeriod(string transactionType, string lastPeriod)
        {
            return await _igRestService.RestfulService<TransactionHistoryResponse>("/gateway/deal/history/transactions/" + transactionType + "/" + lastPeriod, HttpMethod.Get, "1", ConversationContext);
        }

        ///<Summary>
        ///Returns the transaction history for the specified transaction type and given date range
        ///@pathParam transactionType Transaction type (( ALL, ALL_DEAL, DEPOSIT, WITHDRAWAL ) )
        ///@pathParam fromDate Start date in dd-mm-yyyy format
        ///@pathParam toDate End date in dd-mm-yyyy format
        ///</Summary>

        public async Task<IgResponse<TransactionHistoryResponse>> lastTransactionTimeRange(string transactionType, string fromDate, string toDate)
        {
            return await _igRestService.RestfulService<TransactionHistoryResponse>("/gateway/deal/history/transactions/" + transactionType + "/" + fromDate + "/" + toDate, HttpMethod.Get, "1", ConversationContext);
        }


        ///<Summary>
        ///Returns a list of accounts belonging to the logged-in client
        ///</Summary>

        public async Task<IgResponse<AccountDetailsResponse>> accountBalance()
        {
            return await _igRestService.RestfulService<AccountDetailsResponse>("/gateway/deal/accounts", HttpMethod.Get, "1", ConversationContext);
        }


        ///<Summary>
        ///Switches active accounts, optionally setting the default account
        ///@param accountSwitchRequest Account switch request
        ///</Summary>

        public async Task<IgResponse<AccountSwitchResponse>> accountSwitch(AccountSwitchRequest accountSwitchRequest)
        {
            return await _igRestService.RestfulService<AccountSwitchResponse>("/gateway/deal/session", HttpMethod.Put, "1", ConversationContext, accountSwitchRequest);
        }


        ///<Summary>
        ///Alters the details of a given user application
        ///@param updateApplicationRequest application update request
        ///</Summary>

        public async Task<IgResponse<Application>> update(UpdateApplicationRequest updateApplicationRequest)
        {
            return await _igRestService.RestfulService<Application>("/gateway/deal/operations/application", HttpMethod.Put, "1", ConversationContext, updateApplicationRequest);
        }

        ///<Summary>
        ///Disables the current application key from processing further requests.  Disabled keys may be reenabled via the My Account section on our web dealing platform.
        ///</Summary>

        public async Task<IgResponse<Application>> disableApplication()
        {
            return await _igRestService.RestfulService<Application>("/gateway/deal/operations/application/disable", HttpMethod.Put, "1", ConversationContext);
        }

        ///<Summary>
        ///Returns a list of client-owned applications
        ///</Summary>

        public async Task<IgResponse<List<Application>>> findClientApplications()
        {
            return await _igRestService.RestfulService<List<Application>>("/gateway/deal/operations/application", HttpMethod.Get, "1", ConversationContext);
        }


        ///<Summary>
        ///Creates a trading session, obtaining session tokens for subsequent API access
        ///@param authenticationRequest Client login credentials
        ///@return Client summary account information
        ///</Summary>

        private async Task<IgResponse<Models.auth.session.AuthenticationResponse>> authenticate(Models.auth.session.AuthenticationRequest authenticationRequest)
        {
            return await _igRestService.RestfulService<Models.auth.session.AuthenticationResponse>("/gateway/deal/session", HttpMethod.Post, "1", ConversationContext, authenticationRequest);
        }

        ///<Summary>
        ///</Summary>

        public async Task<IgResponse<SprintMarketsSearchResponse>> findAll()
        {
            return await _igRestService.RestfulService<SprintMarketsSearchResponse>("/gateway/deal/sprintmarkets", HttpMethod.Get, "1", ConversationContext);
        }


        ///<Summary>
        ///Returns the client sentiment for the given instrument's market
        ///@pathParam marketId Market identifier
        ///</Summary>

        public async Task<IgResponse<ClientSentiment>> getClientSentiment(string marketId)
        {
            return await _igRestService.RestfulService<ClientSentiment>("/gateway/deal/clientsentiment/" + marketId, HttpMethod.Get, "1", ConversationContext);
        }

        ///<Summary>
        ///Returns a list of related (what others have traded) client sentiment for the given instrument's market
        ///@pathParam marketId Market identifier
        ///</Summary>

        public async Task<IgResponse<ClientSentimentList>> getRelatedClientSentiment(string marketId)
        {
            return await _igRestService.RestfulService<ClientSentimentList>("/gateway/deal/clientsentiment/related/" + marketId, HttpMethod.Get, "1", ConversationContext);
        }


        ///<Summary>
        ///Returns a deal confirmation for the given deal reference
        ///@pathParam dealReference Deal reference
        ///</Summary>

        public async Task<IgResponse<ConfirmsResponse>> retrieveConfirm(string dealReference)
        {
            return await _igRestService.RestfulService<ConfirmsResponse>("/gateway/deal/confirms/" + dealReference, HttpMethod.Get, "1", ConversationContext);
        }


        ///<Summary>
        ///Returns the details of the given market
        ///@pathParam epic The epic of the market to be retrieved
        ///</Summary>

        public async Task<IgResponse<Models.marketdetails.v1.MarketDetailsResponse>> marketDetails(string epic)
        {
            return await _igRestService.RestfulService<Models.marketdetails.v1.MarketDetailsResponse>("/gateway/deal/markets/" + epic, HttpMethod.Get, "1", ConversationContext);
        }

        ///<Summary>
        ///Returns the details of the given market
        ///@pathParam epic The epic of the market to be retrieved
        ///</Summary>

        public async Task<IgResponse<MarketDetailsResponse>> marketDetailsV2(string epic)
        {
            return await _igRestService.RestfulService<MarketDetailsResponse>("/gateway/deal/markets/" + epic, HttpMethod.Get, "2", ConversationContext);
        }

        ///<Summary>
        ///Returns the details of the given markets.
        ///@pathParam epics The epics of the market to be retrieved, separated by a comma. Max number of epics is limited to 50.
        ///</Summary>

        public async Task<IgResponse<MarketDetailsListResponse>> marketDetailsMulti(string epicsList)
        {
            return await _igRestService.RestfulService<MarketDetailsListResponse>("/gateway/deal/markets?epics=" + epicsList, HttpMethod.Get, "1", ConversationContext);
        }


        ///<Summary>
        ///Creates an OTC position
        ///@param createPositionRequest the request for creating a position
        ///@return OTC create position response
        ///</Summary>

        public async Task<IgResponse<CreatePositionResponse>> createPositionV1(CreatePositionRequest createPositionRequest)
        {
            return await _igRestService.RestfulService<CreatePositionResponse>("/gateway/deal/positions/otc", HttpMethod.Post, "1", ConversationContext, createPositionRequest);
        }

        ///<Summary>
        ///Creates an OTC position
        ///@param createPositionRequest the request for creating a position
        ///@return OTC create position response
        ///</Summary>

        public async Task<IgResponse<CreatePositionResponse>> createPositionV2(Models.positions.create.otc.v2.CreatePositionRequest createPositionRequest)
        {
            return await _igRestService.RestfulService<CreatePositionResponse>("/gateway/deal/positions/otc", HttpMethod.Post, "2", ConversationContext, createPositionRequest);
        }

        ///<Summary>
        ///Updates an OTC position
        ///@pathParam dealId Deal reference identifier
        ///@param editPositionRequest the request for updating a position
        ///@return OTC edit position response
        ///</Summary>

        public async Task<IgResponse<EditPositionResponse>> editPositionV1(string dealId, EditPositionRequest editPositionRequest)
        {
            return await _igRestService.RestfulService<EditPositionResponse>("/gateway/deal/positions/otc/" + dealId, HttpMethod.Put, "1", ConversationContext, editPositionRequest);
        }

        ///<Summary>
        ///Updates an OTC position
        ///@pathParam dealId Deal reference identifier
        ///@param editPositionRequest the request for updating a position
        ///@return OTC edit position response
        ///</Summary>

        public async Task<IgResponse<EditPositionResponse>> editPositionV2(string dealId, Models.positions.edit.v2.EditPositionRequest editPositionRequest)
        {
            return await _igRestService.RestfulService<EditPositionResponse>("/gateway/deal/positions/otc/" + dealId, HttpMethod.Put, "2", ConversationContext, editPositionRequest);
        }

        ///<Summary>
        ///Closes one or more OTC positions
        ///@param closePositionRequest the request for closing one or more positions
        ///@return OTC close position response
        ///</Summary>

        public async Task<IgResponse<ClosePositionResponse>> closePosition(ClosePositionRequest closePositionRequest)
        {
            return await _igRestService.RestfulService<ClosePositionResponse>("/gateway/deal/positions/otc", HttpMethod.Delete, "1", ConversationContext, closePositionRequest);
        }

        ///<Summary>
        ///Returns all open positions for the active account
        ///</Summary>

        public async Task<IgResponse<Models.positions.get.otc.v1.PositionsResponse>> getOTCOpenPositionsV1()
        {
            return await _igRestService.RestfulService<Models.positions.get.otc.v1.PositionsResponse>("/gateway/deal/positions", HttpMethod.Get, "1", ConversationContext);
        }


        ///<Summary>
        ///Returns an open position for the active account by deal identifier
        ///@pathParam dealId Deal reference identifier
        ///</Summary>

        public async Task<IgResponse<Models.positions.get.otc.v1.OpenPosition>> getOTCOpenPositionByDealIdV1(string dealId)
        {
            return await _igRestService.RestfulService<Models.positions.get.otc.v1.OpenPosition>("/gateway/deal/positions/" + dealId, HttpMethod.Get, "1", ConversationContext);
        }

        ///<Summary>
        ///Returns an open position for the active account by deal identifier
        ///@pathParam dealId Deal reference identifier
        ///</Summary>

        public async Task<IgResponse<Models.positions.get.otc.v2.OpenPosition>> getOTCOpenPositionByDealIdV2(string dealId)
        {
            return await _igRestService.RestfulService<Models.positions.get.otc.v2.OpenPosition>("/gateway/deal/positions/" + dealId, HttpMethod.Get, "2", ConversationContext);
        }


        ///<Summary>
        ///Returns a list of historical prices for the given epic, resolution and date range.
        ///@pathParam epic Instrument epic
        ///@pathParam resolution Price resolution (MINUTE, MINUTE_2, MINUTE_3, MINUTE_5, MINUTE_10, MINUTE_15, MINUTE_30, HOUR, HOUR_2, HOUR_3, HOUR_4, DAY, WEEK, MONTH)
        ///@requestParam startdate Start date (yyyy:MM:dd-HH:mm:ss)
        ///@requestParam enddate End date (yyyy:MM:dd-HH:mm:ss). Must be later then the start date.
        ///</Summary>

        public async Task<IgResponse<PriceList>> priceSearchByDate(string epic, string resolution, string startdate, string enddate)
        {
            return await _igRestService.RestfulService<PriceList>("/gateway/deal/prices/" + epic + "?resolution=" + resolution + "&from=" + startdate + "&to=" + enddate + "&pageSize=1000&pageNumber=1", HttpMethod.Get, "1", ConversationContext);
        }

        ///<Summary>
        ///Returns a list of historical prices for the given epic, resolution and number of data points
        ///@pathParam epic Instrument epic
        ///@pathParam resolution Price resolution (MINUTE, MINUTE_2, MINUTE_3, MINUTE_5, MINUTE_10, MINUTE_15, MINUTE_30, HOUR, HOUR_2, HOUR_3, HOUR_4, DAY, WEEK, MONTH)
        ///@pathParam numPoints Number of data points required
        ///</Summary>

        public async Task<IgResponse<PriceList>> priceSearchByNum(string epic, string resolution, string numPoints)
        {
            return await _igRestService.RestfulService<PriceList>("/gateway/deal/prices/" + epic + "/" + resolution + "/" + numPoints, HttpMethod.Get, "1", ConversationContext);
        }


        ///<Summary>
        ///Returns a list of historical prices for the given epic, resolution and date range.
        ///@pathParam epic Instrument epic
        ///@pathParam resolution Price resolution (MINUTE, MINUTE_2, MINUTE_3, MINUTE_5, MINUTE_10, MINUTE_15, MINUTE_30, HOUR, HOUR_2, HOUR_3, HOUR_4, DAY, WEEK, MONTH)
        ///@pathParam startDate Start date (yyyy-MM-dd HH:mm:ss)
        ///@pathParam endDate End date (yyyy-MM-dd HH:mm:ss). Must be later then the start date.
        ///</Summary>

        public async Task<IgResponse<PriceList>> priceSearchByDateV2(string epic, string resolution, string startDate, string endDate)
        {
            return await _igRestService.RestfulService<PriceList>("/gateway/deal/prices/" + epic + "/" + resolution + "/" + startDate + "/" + endDate, HttpMethod.Get, "2", ConversationContext);
        }

        ///<Summary>
        ///Returns a list of historical prices for the given epic, resolution and number of data points
        ///@pathParam epic Instrument epic
        ///@pathParam resolution Price resolution (MINUTE, MINUTE_2, MINUTE_3, MINUTE_5, MINUTE_10, MINUTE_15, MINUTE_30, HOUR, HOUR_2, HOUR_3, HOUR_4, DAY, WEEK, MONTH)
        ///@pathParam numPoints Number of data points required
        ///</Summary>

        public async Task<IgResponse<PriceList>> priceSearchByNumV2(string epic, string resolution, string numPoints)
        {
            return await _igRestService.RestfulService<PriceList>("/gateway/deal/prices/" + epic + "/" + resolution + "/" + numPoints, HttpMethod.Get, "2", ConversationContext);
        }


        ///<Summary>
        ///Returns all markets matching the search term
        ///@return market search result
        ///@throws SearchMarketsException
        ///@requestParam searchTerm The term to be used in the search
        ///</Summary>

        public async Task<IgResponse<SearchMarketsResponse>> searchMarket(string searchTerm)
        {
            return await _igRestService.RestfulService<SearchMarketsResponse>("/gateway/deal/markets?searchTerm=" + searchTerm, HttpMethod.Get, "1", ConversationContext);
        }


        ///<Summary>
        ///Deletes a watchlist
        ///@pathParam watchlistId Watchlist id
        ///</Summary>

        public async Task<IgResponse<DeleteWatchlistResponse>> deleteWatchlist(string watchlistId)
        {
            return await _igRestService.RestfulService<DeleteWatchlistResponse>("/gateway/deal/watchlists/" + watchlistId, HttpMethod.Delete, "1", ConversationContext);
        }

        ///<Summary>
        ///Creates a watchlist
        ///@param createWatchlistRequest Watchlist create request
        ///</Summary>

        public async Task<IgResponse<CreateWatchlistResponse>> createWatchlist(CreateWatchlistRequest createWatchlistRequest)
        {
            return await _igRestService.RestfulService<CreateWatchlistResponse>("/gateway/deal/watchlists", HttpMethod.Post, "1", ConversationContext, createWatchlistRequest);
        }

        ///<Summary>
        ///Adds a market to a watchlist
        ///@pathParam watchlistId Watchlist id
        ///@param addInstrumentToWatchlistRequest Add market to watchlist request
        ///</Summary>

        public async Task<IgResponse<AddInstrumentToWatchlistResponse>> addInstrumentToWatchlist(string watchlistId, AddInstrumentToWatchlistRequest addInstrumentToWatchlistRequest)
        {
            return await _igRestService.RestfulService<AddInstrumentToWatchlistResponse>("/gateway/deal/watchlists/" + watchlistId, HttpMethod.Put, "1", ConversationContext, addInstrumentToWatchlistRequest);
        }

        ///<Summary>
        ///Remove a market from a watchlist
        ///@pathParam watchlistId Watchlist id
        ///@pathParam epic Market epic
        ///</Summary>

        public async Task<IgResponse<RemoveInstrumentFromWatchlistResponse>> removeInstrumentFromWatchlist(string watchlistId, string epic)
        {
            return await _igRestService.RestfulService<RemoveInstrumentFromWatchlistResponse>("/gateway/deal/watchlists/" + watchlistId + "/" + epic, HttpMethod.Delete, "1", ConversationContext);
        }



        ///<Summary>
        ///Returns all open working orders for the active account
        ///</Summary>

        public async Task<IgResponse<Models.workingorders.get.v1.WorkingOrdersResponse>> workingOrdersV1()
        {
            return await _igRestService.RestfulService<Models.workingorders.get.v1.WorkingOrdersResponse>("/gateway/deal/workingorders", HttpMethod.Get, "1", ConversationContext);
        }


        ///<Summary>
        ///Creates an OTC working order
        ///@deprecated Use version 2 of the service instead
        ///@param createWorkingOrderRequest Create working order request data
        ///</Summary>

        public async Task<IgResponse<CreateWorkingOrderResponse>> createWorkingOrderV1(CreateWorkingOrderRequest createWorkingOrderRequest)
        {
            return await _igRestService.RestfulService<CreateWorkingOrderResponse>("/gateway/deal/workingorders/otc", HttpMethod.Post, "1", ConversationContext, createWorkingOrderRequest);
        }

        ///<Summary>
        ///Creates an OTC working order
        ///@param createWorkingOrderRequest Create working order request data
        ///</Summary>

        public async Task<IgResponse<CreateWorkingOrderResponse>> createWorkingOrderV2(Models.workingorders.create.v2.CreateWorkingOrderRequest createWorkingOrderRequest)
        {
            return await _igRestService.RestfulService<CreateWorkingOrderResponse>("/gateway/deal/workingorders/otc", HttpMethod.Post, "2", ConversationContext, createWorkingOrderRequest);
        }

        ///<Summary>
        ///Updates an OTC working order
        ///@deprecated Use version 2 of the service instead
        ///@pathParam dealId Deal identifier
        ///@param editWorkingOrderRequest Update working order request data
        ///</Summary>

        public async Task<IgResponse<EditWorkingOrderResponse>> editWorkingOrderV1(string dealId, EditWorkingOrderRequest editWorkingOrderRequest)
        {
            return await _igRestService.RestfulService<EditWorkingOrderResponse>("/gateway/deal/workingorders/otc/" + dealId, HttpMethod.Put, "1", ConversationContext, editWorkingOrderRequest);
        }

        ///<Summary>
        ///Updates an OTC working order
        ///@pathParam dealId Deal identifier
        ///@param editWorkingOrderRequest Update working order request data
        ///</Summary>

        public async Task<IgResponse<EditWorkingOrderResponse>> editWorkingOrderV2(string dealId, Models.workingorders.edit.v2.EditWorkingOrderRequest editWorkingOrderRequest)
        {
            return await _igRestService.RestfulService<EditWorkingOrderResponse>("/gateway/deal/workingorders/otc/" + dealId, HttpMethod.Put, "2", ConversationContext, editWorkingOrderRequest);
        }

        ///<Summary>
        ///Deletes an OTC working order
        ///@pathParam dealId Deal identifier
        ///@param deleteWorkingOrderRequest Delete working order request data
        ///</Summary>

        public async Task<IgResponse<DeleteWorkingOrderResponse>> deleteWorkingOrder(string dealId, DeleteWorkingOrderRequest deleteWorkingOrderRequest)
        {
            return await _igRestService.RestfulService<DeleteWorkingOrderResponse>("/gateway/deal/workingorders/otc/" + dealId, HttpMethod.Delete, "1", ConversationContext, deleteWorkingOrderRequest);
        }

    }
}
