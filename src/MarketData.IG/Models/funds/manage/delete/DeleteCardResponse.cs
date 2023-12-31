namespace MarketData.IG.Models.funds.manage.delete
{

    public class DeleteCardResponse
    {
        public enum Status
        {

            ///<Summary>
            ///Success
            ///</Summary>

            SUCCESS,
            ///<Summary>
            ///Service not available
            ///</Summary>

            NOT_AVAILABLE,
            ///<Summary>
            ///General failure
            ///</Summary>

            FAILURE,
        }
        ///<Summary>
        ///Status
        ///</Summary>
        public string status { get; set; }
    }
}
