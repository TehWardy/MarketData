using Lightstreamer.DotNet.Client;
using MarketData.IG.StreamingApi.Models;

namespace MarketData.IG.StreamingApi.Listeners
{
    public class AccountDetailsTableListerner : TableListenerAdapterBase<StreamingAccountData>
    {
        protected override StreamingAccountData LoadUpdate(IUpdateInfo update)
        {

            try
            {
                var streamingAccountData = new StreamingAccountData
                {
                    ProfitAndLoss = StringToNullableDecimal(update.GetNewValue("PNL")),
                    Deposit = StringToNullableDecimal(update.GetNewValue("DEPOSIT")),
                    UsedMargin = StringToNullableDecimal(update.GetNewValue("USED_MARGIN")),
                    AmountDue = StringToNullableDecimal(update.GetNewValue("AMOUNT_DUE")),
                    AvailableCash = StringToNullableDecimal(update.GetNewValue("AVAILABLE_CASH"))
                };
                return streamingAccountData;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

}
