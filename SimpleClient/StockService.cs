using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleClient
{
	public class StockService
	{
		public event EventHandler FaultEvent;
		public event EventHandler StockFinishEvent;

		private List<(int Count, Item Item)> _items = new List<(int Count, Item Item)>
		{
			(5, new Item("Monitor")),
			(6, new Item("Keyboard"))
		};

		public void OrderAdded(object sender, EventArgs eventArgs)
		{
			Order order = (eventArgs as OrderCreatedEventArgs).Order;
			foreach (OrderItem t in order.OrderItems)
			{
				(int Count, Item Item) itemInStock =
					_items.FirstOrDefault(stockItem => stockItem.Item.Name == t.Item.Name);
				if (itemInStock == default((int Count, Item Item)))
				{
					EmmitErrorInStock($"Item {t.Item.Name} is not in stock");
					continue;
				}

				if (itemInStock.Count < t.Count)
				{
					EmmitErrorInStock($"Not enough {t.Item.Name} to complete order");
				}
			}

			EmmitStockFinishEvent();
		}

		private void EmmitStockFinishEvent()
		{
			StockFinishEvent?.Invoke(this, new EventArgs());
		}
		
		private void EmmitErrorInStock(string error)
		{
			FaultEvent?.Invoke(this, new FaultEventArgs(error));
		}
	}

	public class FaultEventArgs : EventArgs
	{
		public FaultEventArgs(string error)
		{
			Error = error;
		}

		public string Error { get; }


	}
}