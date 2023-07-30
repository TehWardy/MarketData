namespace MarketData.IG.StreamingApi.Models
{
    public class UpdateArgs<T> : EventArgs
    {
        public T UpdateData { get; set; }
        public int ItemPosition { get; set; }
        public string ItemName { get; set; }
    }

}
