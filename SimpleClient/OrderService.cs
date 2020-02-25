using System;
using System.Collections.Generic;
using System.Runtime.Loader;

namespace SimpleClient
{
	public class OrderService
	{
		private readonly List<Order> _orders = new List<Order>();
		public event EventHandler OrderAdded;
		public event EventHandler<string> InfoEvent;

		private bool _revertedTransaction = false;
		
		public void CreateOrder(Order order)
		{
			_orders.Add(order);
			OnOrderAdded(order);
		}

		public void OrderFinishEvent(object sender, EventArgs e)
		{
			if (!_revertedTransaction)
			{
				InfoEvent?.Invoke(this, "Order successfully created");
			}
			else
			{
				InfoEvent?.Invoke(this, "Process of order creation has failed");
			}
		}

		public void StockFaultEventHandler(object sender, EventArgs eventArgs)
		{
			InfoEvent?.Invoke(this, (eventArgs as FaultEventArgs).Error);
			if (_revertedTransaction) return;
			_revertedTransaction = !_revertedTransaction;
		}

		protected virtual void OnOrderAdded(Order order)
		{
			OrderAdded?.Invoke(this, new OrderCreatedEventArgs(order));
		}
	}

	public class OrderCreatedEventArgs : EventArgs
	{
		public Order Order { get; }

		public OrderCreatedEventArgs(Order order)
		{
			Order = order;
		}
	}

	public class Order
	{
		public Guid Guid { get; set; }
		public List<OrderItem> OrderItems { get; set; }
	}

	public class OrderItem
	{
		public Item Item { get; set; }

		public int Count { get; set; }
	}

	public class Item
	{
		public Item(string name)
		{
			this.Name = name;
		}

		public string Name { get; }
	}
}