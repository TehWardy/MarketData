using Lightstreamer.DotNet.Client;
using MarketData.IG.StreamingApi.Models;

namespace MarketData.IG.StreamingApi
{
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
            return decimal.TryParse(value, out number) ? number : null;
        }

        protected int? StringToNullableInt(string value)
        {
            int number;
            return int.TryParse(value, out number) ? number : null;
        }

        protected DateTime? EpocStringToNullableDateTime(string value)
        {
            ulong epoc;
            if (!ulong.TryParse(value, out epoc))
            {
                return null;
            }
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(epoc);
        }
    }

    public abstract class TableListenerAdapterBase<T> : TableListenerAdapterBase
    {
        public override void OnUpdate(int itemPos, string itemName, IUpdateInfo update)
        {
            OnUpdate(new UpdateArgs<T> { UpdateData = LoadUpdate(update), ItemPosition = itemPos, ItemName = itemName });
        }

        protected abstract T LoadUpdate(IUpdateInfo update);

        public event EventHandler<UpdateArgs<T>> Update;

        protected virtual void OnUpdate(UpdateArgs<T> e)
        {
            var handler = Update;

            if (handler != null) 
                handler(this, e);
        }
    }
}
