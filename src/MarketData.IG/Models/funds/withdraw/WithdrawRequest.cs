namespace MarketData.IG.Models.funds.withdraw
{

    public class WithdrawRequest
    {
        ///<Summary>
        ///Target card identifier
        ///</Summary>
        public string cardId { get; set; }
        ///<Summary>
        ///Special instructions, if applicable
        ///</Summary>
        public string specialInstructions { get; set; }
        ///<Summary>
        ///Client password
        ///</Summary>
        public char[] password { get; set; }
        ///<Summary>
        ///Withdrawal amount
        ///</Summary>
        public decimal amount { get; set; }
    }
}
