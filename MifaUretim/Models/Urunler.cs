using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MifaUretim.Models
{
	public class Urunler
	{
		private int id;
		private string urun;
		private int miktar;

		public int Id { get => id; set => id = value; }
		public string Urun { get => urun; set => urun = value; }
		public int Miktar { get => miktar; set => miktar = value; }
	}
}
