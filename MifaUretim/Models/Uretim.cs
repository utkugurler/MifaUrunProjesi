using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MifaUretim.Models
{
	public class Uretim
	{
		private int urunId;
		private int urunQuantity;

		public int UrunId { get => urunId; set => urunId = value; }
		public int UrunQuantity { get => urunQuantity; set => urunQuantity = value; }
	}
}
