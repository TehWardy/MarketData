using System;

namespace IGWebApiClient
{
    public class UpdateArgs<T> : EventArgs
	{
		public T UpdateData { get; set; }
		public int ItemPosition { get; set; }
		public string ItemName { get; set; }
	}

}
