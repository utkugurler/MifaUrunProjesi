using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using MifaUretim.Models;

namespace MifaUretim.Controllers
{
	public class UrunlerController : Controller
	{
		SqlConnection con;
		public UrunlerController()
		{
			var configuration = ConnectDB.GetConfiguration();
			con = new SqlConnection(configuration.GetSection("ConnectionStrings").GetSection("MvcUretimContext").Value);
		}

		public IActionResult Index()
		{
			ViewBag.urunler = GetUrunlerList();
			return View();
		}

		public List<Urunler> GetUrunlerList()
		{
			List<Urunler> urunler = new List<Urunler>();
			if(con.State != System.Data.ConnectionState.Open)
			{
				con.Open();
			}

			SqlCommand cmd = new SqlCommand($"select * from Urunler", con);
			SqlDataReader dataReader = cmd.ExecuteReader();

			while (dataReader.Read())
			{
				Urunler urunler1 = new Urunler();
				urunler1.Id = Convert.ToInt32(dataReader["id"]);
				urunler1.Urun = dataReader["Urun"].ToString();
				urunler1.Miktar = Convert.ToInt32(dataReader["Miktar"]);
				urunler.Add(urunler1);
			}
			dataReader.Close();
			con.Close();

			return urunler;
		}
	}
}
