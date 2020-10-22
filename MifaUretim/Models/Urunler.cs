using MifaUretim.Controllers;
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
		private List<UrunKodlari> urunKodlari;

		public int Id { get => id; set => id = value; }
		public string Urun { get => urun; set => urun = value; }
		public int Miktar { get => miktar; set => miktar = value; }
		public List<UrunKodlari> UrunKodlari { get => urunKodlari; set => urunKodlari = value; }
	}
}
