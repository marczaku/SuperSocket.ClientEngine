using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Linq;


namespace SuperSocket.ClientEngine
{
	public class SyncTcpClientSession : TcpClientSession
	{
		//OnDataReceived call this when we get data
		
		private byte[] _buffer = new byte[1024];

		public SyncTcpClientSession(EndPoint remoteEndPoint)
			: base(remoteEndPoint)
		{

		}

		protected override void SocketEventArgsCompleted(object sender, SocketAsyncEventArgs e)
		{
			throw new NotImplementedException();
		}

		protected override void OnGetSocket(SocketAsyncEventArgs e)
		{
			throw new NotImplementedException();
		}

		protected override void SendInternal(PosList<ArraySegment<byte>> items)
		{
			foreach (var item in items)
			{
				Client.Send(item.Array);
			}
		}

		public override void Connect()
		{
			Client = new Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
			Client.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Tcp, System.Net.Sockets.SocketOptionName.NoDelay, 1);
			Client.Connect(RemoteEndPoint);
			if (Client.Connected == false)
				throw new Exception("socket timed out");

			OnConnected();

			Client.BeginReceive(_buffer, 0, _buffer.Length, System.Net.Sockets.SocketFlags.None, OnDataReceived, _buffer);
		}

		private void OnDataReceived(IAsyncResult ar)
		{
			try
			{
				var bytes = Client.EndReceive(ar);

				if (bytes > 0)
				{
					var data = _buffer;
					_buffer = new byte[1024];
					Client.BeginReceive(_buffer, 0, _buffer.Length, System.Net.Sockets.SocketFlags.None, OnDataReceived, _buffer);
					OnDataReceived(data, 0, bytes);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
