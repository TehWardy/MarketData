namespace MarketData.IG.Models.funds.manage.transfer
{

    public class TransferFundsResponse
    {
        public enum Status
        {

            ///<Summary>
            ///Success
            ///</Summary>

            SUCCESS,
            ///<Summary>
            ///Failure
            ///</Summary>

            FAILURE,
        }
        ///<Summary>
        ///Status
        ///</Summary>
        public string status { get; set; }
    }
}
