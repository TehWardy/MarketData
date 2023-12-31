namespace MarketData.IG.Models.funds.withdraw
{

    public class WithdrawResponse
    {
        public enum Status
        {

            ///<Summary>
            ///Success
            ///</Summary>

            SUCCESS,
            ///<Summary>
            ///Pending manual authorisation
            ///</Summary>

            PENDING,
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
