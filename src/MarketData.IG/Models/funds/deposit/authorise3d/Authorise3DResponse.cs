namespace MarketData.IG.Models.funds.deposit.authorise3d
{

    public class Authorise3DResponse
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
