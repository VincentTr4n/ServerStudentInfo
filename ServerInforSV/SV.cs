using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInforSV
{
	public class SV
	{
		public string ID { get; set; }
		public string Name { get; set; }
		public string DTB { get; set; }
		public string toString { get { return ID + "|" + Name + "|" + DTB; } }
	}
}
