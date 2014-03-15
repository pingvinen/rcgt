using System;
using System.Net.Sockets;
using System.Text;

namespace RcGt.TalkToRuby
{
	class MainClass
	{
		static Encoding encoding = Encoding.UTF8;

		public static void Main(string[] args)
		{
			Console.WriteLine("Hello Mr. Bond");

			using (TcpClient tcp = new TcpClient("localhost", 2000))
			{
				tcp.NoDelay = true;

				bool doRun = true;
				while (doRun)
				{
					ConsoleKeyInfo cki = Console.ReadKey();
					switch (cki.Key)
					{
						case ConsoleKey.UpArrow:
							Send(tcp, "Speeder:100\n");
							break;

						case ConsoleKey.DownArrow:
							Send(tcp, "Brake:100\n");
							break;

						case ConsoleKey.Q:
							doRun = false;
							break;
					}
				}

				//Console.WriteLine("Received: '{0}'", Receive(tcp));
			}

			Console.WriteLine("All done");
		}

		private static void Send(TcpClient tcp, string msg)
		{
			byte[] buffer = encoding.GetBytes(msg);

			int startTickCount = Environment.TickCount;
			int sent = 0;  // how many bytes is already sent
			do
			{
				try
				{
					sent += tcp.Client.Send(buffer, sent, buffer.Length - sent, SocketFlags.None);
				}
				catch (SocketException ex)
				{
					if (ex.SocketErrorCode == SocketError.WouldBlock ||
					    ex.SocketErrorCode == SocketError.IOPending ||
					    ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
					{
						// socket buffer is probably full, wait and try again
						System.Threading.Thread.Sleep(30);
					}
					else
						throw ex;  // any serious error occurr
				}
			}
			while (sent < buffer.Length);


		}

		private static string Receive(TcpClient tcp)
		{
			byte[] buffer = new byte[tcp.Available];
			tcp.Client.Receive(buffer);
			string str = encoding.GetString(buffer);
			return str;
		}
	}
}
