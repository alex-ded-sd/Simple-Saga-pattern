using System;
using System.Collections.Generic;

namespace SimpleClient
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello! enter first product:");
			string product = Console.ReadLine();
			Console.WriteLine("Enter count:");
			int count = Convert.ToInt32(Console.ReadLine());
			Console.WriteLine("Enter second product:");
			string product2 = Console.ReadLine();
			Console.WriteLine("Enter count:");
			int count2 = Convert.ToInt32(Console.ReadLine());
			Order order = new Order
			{
				Guid = Guid.NewGuid(),
				OrderItems = new List<OrderItem>
				{
					new OrderItem {Count = count, Item = new Item(product)},
					new OrderItem {Count = count2, Item = new Item(product2)}
				}
			};
			OrderService orderService = new OrderService();
			StockService stockService = new StockService();
			orderService.OrderAdded += stockService.OrderAdded;
			stockService.FaultEvent += orderService.StockFaultEventHandler;
			stockService.StockFinishEvent += orderService.OrderFinishEvent;
			orderService.InfoEvent += (sender, s) => Console.WriteLine(s);
			orderService.CreateOrder(order);
			Console.ReadLine();
		}
	}
}