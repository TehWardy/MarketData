using System;

namespace IGWebApiClient
{
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
			if (handler != null) handler(this, e);
		}
	}
}
