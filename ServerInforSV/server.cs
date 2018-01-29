using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ServerInforSV
{
	class server
	{
		static void Main(string[] args)
		{
			byte[] data = new byte[1<<10];
			IPEndPoint ipe = new IPEndPoint(IPAddress.Any, 9090);
			Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			server.Bind(ipe);
			server.Listen(5);

			List<SV> mlist = new List<SV>();
			mlist.Add(new SV { ID = "1", Name = "Vincent Tran1", DTB = "9" });
			mlist.Add(new SV { ID = "2", Name = "Vincent Tran2", DTB = "9" });
			mlist.Add(new SV { ID = "3", Name = "Vincent Tran3", DTB = "9" });
			mlist.Add(new SV { ID = "4", Name = "Vincent Tran4", DTB = "9" });
			mlist.Add(new SV { ID = "5", Name = "Vincent Tran5", DTB = "9" });
			mlist.Add(new SV { ID = "6", Name = "Vincent Tran6", DTB = "9" });
			mlist.Add(new SV { ID = "7", Name = "Vincent Tran7", DTB = "9" });
			mlist.Add(new SV { ID = "8", Name = "Vincent Tran8", DTB = "9" });

			//var load = System.IO.File.ReadAllText("data.txt");

			//List<SV> mlist = new List<SV>(JsonConvert.DeserializeObject<SV[]>(load));

			bool ok = false;

			Console.WriteLine("Server is setting...");
			
			Socket client = server.Accept();

			var rep1 = Encoding.ASCII.GetBytes("Accepted!");
			client.Send(rep1, rep1.Length, SocketFlags.None);

			while (true)
			{
				var rec = client.Receive(data);
				if (rec == 0)
				{
					Console.WriteLine("Error!");
					break;
				}
				else
				{
					var dataReceive = Encoding.ASCII.GetString(data, 0, rec);
					Console.WriteLine("Client : " + dataReceive);

					var temp = dataReceive.Split(' ');

					if(temp[0] == "xem")
					{
						string s = "";
						foreach (var item in mlist) s += item.toString+"\n";
						var rep = Encoding.ASCII.GetBytes(s);
						client.Send(rep, rep.Length, SocketFlags.None);
					}
					else if (temp[0] == "sua")
					{
						if (!ok)
						{
							string s = "begin edit";
							var rep = Encoding.ASCII.GetBytes(s);
							client.Send(rep, rep.Length, SocketFlags.None);
							ok = true;
						}
					}else if (ok)
					{
						SV sv = new SV() { ID = temp[0], Name = temp[1], DTB = temp[2] };
						string s = "";
						var edit = mlist.SingleOrDefault(t => t.ID == sv.ID);
						if (edit == null) s = "Khong tim thay";
						else
						{
							edit.Name = sv.Name;
							edit.DTB = sv.DTB;
							s = sv.toString;
						}
						var rep = Encoding.ASCII.GetBytes(s);
						client.Send(rep, rep.Length, SocketFlags.None);
						ok = false;
					}
					else if (temp[0] == "xoa")
					{
						string s = "Xoa thanh cong";
						bool check = false;
						int index = 0;
						foreach (var item in mlist)
						{
							if(item.ID == temp[1])
							{
								check = true;
								break; 
							}
							index++;
						}
						if (!check) s = "Khong tim thay";
						else mlist.RemoveAt(index);
						var rep = Encoding.ASCII.GetBytes(s);
						client.Send(rep, rep.Length, SocketFlags.None);
					}
				}

			}
			server.Close();
			Console.ReadKey();
		}
	}
}
