using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
	class Program
	{
		static void Main(string[] args)
		{
			var socket = new WebSocket4Net.WebSocket("ws://10.47.89.97/poll/5", "Chat", new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("ABBCX", "7") });
			socket.MessageReceived += new EventHandler<WebSocket4Net.MessageReceivedEventArgs>(socket_MessageReceived);
			socket.Open();


			Console.ReadKey();
		}

		static void socket_MessageReceived(object sender, WebSocket4Net.MessageReceivedEventArgs e)
		{
			Console.WriteLine(e.Message);
		}
	}
}
