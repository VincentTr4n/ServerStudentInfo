using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
	class Client
	{
		static void Main(string[] args)
		{
			byte[] data = new byte[1<<10];
			IPEndPoint ipe = new IPEndPoint(IPAddress.Loopback, 9090);
			Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			client.Connect(ipe);

			NetworkStream stream = new NetworkStream(client);

			if (stream.CanRead)
			{
				int rec = stream.Read(data, 0, data.Length);
				Console.WriteLine("Server :" + Encoding.ASCII.GetString(data, 0, rec));
			}
			while (true)
			{
				try
				{

					string input = Console.ReadLine();
					if (input == "exit") break;

					if (stream.CanWrite)
					{
						data = Encoding.ASCII.GetBytes(input);
						stream.Write(data, 0, data.Length);
						stream.Flush();

					}
					if (stream.CanRead)
					{
						data = new byte[1 << 10];
						int rec = stream.Read(data, 0, data.Length);
						if (rec == 0) break;
						string rep = Encoding.ASCII.GetString(data, 0, rec);
						Console.WriteLine("Server :" + rep);
						if(rep == "begin edit")
						{
							string id = Console.ReadLine();
							string name = Console.ReadLine();
							string diem = Console.ReadLine();
							data = Encoding.ASCII.GetBytes(id+" "+name+" "+diem);
							stream.Write(data, 0, data.Length);
							stream.Flush();

							data = new byte[1 << 10];
							rec = stream.Read(data, 0, data.Length);
							if (rec == 0) break;
							rep = Encoding.ASCII.GetString(data, 0, rec);
							Console.WriteLine("Server :" + rep);
						}
					}
				}
				catch { Console.WriteLine("Can't send!"); }
			}
			client.Close();

		}
	}
}
