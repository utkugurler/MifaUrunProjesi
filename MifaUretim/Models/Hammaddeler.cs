using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MifaUretim.Models
{
	public class Hammaddeler
	{
		private int id;
		private string hammadde;
		private int stok;

		public int Id { get => id; set => id = value; }
		public string Hammadde { get => hammadde; set => hammadde = value; }
		public int Stok { get => stok; set => stok = value; }
	}
}
