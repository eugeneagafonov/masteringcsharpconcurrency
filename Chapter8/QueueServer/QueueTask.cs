using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueServer
{
	public class QueueTask
	{
		public Guid UniqueId { get; set; }

		public string Status { get; set; }

		public string Content { get; set; }
	}
}
