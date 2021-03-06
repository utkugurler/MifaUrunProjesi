﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using MifaUretim.Models;
using Newtonsoft.Json;

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

		public IActionResult UrunYarat()
		{
			// İlk önce hammaddeleri çekiyoruz
			if(con.State != System.Data.ConnectionState.Open)
			{
				con.Open();
			}

			List<Hammaddeler> hammaddelers = new List<Hammaddeler>();

			SqlCommand cmd = new SqlCommand("select Hammadde, id from Hammaddeler", con);
			SqlDataReader dataReader = cmd.ExecuteReader();
			
			while (dataReader.Read())
			{
				Hammaddeler hammaddeler = new Hammaddeler();
				hammaddeler.Hammadde = dataReader["Hammadde"].ToString();
				hammaddeler.Id = Convert.ToInt32(dataReader["id"]);
				hammaddelers.Add(hammaddeler);
			}
			dataReader.Close();
			con.Close();

			ViewBag.hammaddeler = hammaddelers;
			return View();
		}

		[HttpPost]
		public JsonResult UrunYarat(Urunler urunler)
		{
			if (con.State != System.Data.ConnectionState.Open)
			{
				con.Open();
			}

			// UrunKodlarını yeniden jsona çeviricez
			string jsonString = JsonConvert.SerializeObject(urunler.UrunKodlari);

			try
			{
				SqlCommand cmd = new SqlCommand($"insert into Urunler(Urun, Miktar, UrunKodlari) values('{urunler.Urun}', '0', '{jsonString}')", con);
				cmd.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				con.Close();
				return Json(new { success = false, responseText = "Ürün oluşturulurken bir hata meydana geldi!" });
			}


			con.Close();
			return Json(new { success = true, responseText = "Ürün başarılı bir şekilde oluşturuldu!" });
		}

		public IActionResult UrunSil(int id)
		{
			if (con.State != System.Data.ConnectionState.Open)
			{
				con.Open();
			}

			SqlCommand cmd = new SqlCommand($"delete from Urunler where id = '{id}'", con);
			cmd.ExecuteNonQuery();
			return RedirectToAction("Index");
		}

		public static Urunler urunler = new Urunler();
		static int urunId;

		[HttpGet]
		public IActionResult UrunDuzenle(int id)
		{
			urunId = id;
			if (con.State != System.Data.ConnectionState.Open)
			{
				con.Open();
			}

			SqlCommand cmd = new SqlCommand($"select * from Urunler where id = '{id}'", con);
			SqlDataReader dataReader = cmd.ExecuteReader();

			Urunler urunler = new Urunler();
			if (dataReader.Read())
			{
				urunler.Urun = dataReader["Urun"].ToString();
				urunler.Id = id;
				urunler.Miktar = Convert.ToInt32(dataReader["Miktar"]);
				urunler.UrunKodlari = urunler.UrunKodlari = JsonConvert.DeserializeObject<List<UrunKodlari>>(dataReader["UrunKodlari"].ToString());
			}
			dataReader.Close();

			int count = 0;
			// UrunKodlarindaki idleri isimlerle eşleştiricem
			foreach (var item in urunler.UrunKodlari)
			{
				SqlCommand sqlCommand = new SqlCommand($"select Hammadde, Stok from Hammaddeler where id = '{item.id}'", con);
				SqlDataReader dataReader1 = sqlCommand.ExecuteReader();
				if (dataReader1.Read())
				{
					urunler.UrunKodlari[count].hammaddeAdi = dataReader1["Hammadde"].ToString();
					urunler.UrunKodlari[count].envanterToplami = Convert.ToInt32(dataReader1["Stok"].ToString());
				}
				dataReader1.Close();
				count++;
			}

			List<Hammaddeler> hammaddeler = new List<Hammaddeler>();
			SqlCommand cmd2 = new SqlCommand($"select Hammadde, id from Hammaddeler", con);
			SqlDataReader sqlDataReader = cmd2.ExecuteReader();
			while (sqlDataReader.Read())
			{
				Hammaddeler hammaddeler1 = new Hammaddeler();
				hammaddeler1.Hammadde = sqlDataReader["Hammadde"].ToString();
				hammaddeler1.Id = Convert.ToInt32(sqlDataReader["id"]);
				hammaddeler.Add(hammaddeler1);
			}
			sqlDataReader.Close();
			con.Close();

			ViewBag.hammaddeler = hammaddeler;
			ViewBag.urunler = urunler;


			return View();
		}

		[HttpPost]
		public JsonResult UrunDuzenle(Urunler urunler)
		{
			if (con.State != System.Data.ConnectionState.Open)
			{
				con.Open();
			}

			// UrunKodlarını yeniden jsona çeviricez
			string jsonString = JsonConvert.SerializeObject(urunler.UrunKodlari);

			try
			{
				SqlCommand cmd = new SqlCommand($"update Urunler set Urun='{urunler.Urun}', UrunKodlari='{jsonString}' where id='{urunId}'", con);
				cmd.ExecuteNonQuery();
			}
			catch(Exception ex)
			{
				con.Close();
				return Json(new { success = false, responseText = "Ürün düzenlenirken bir hata meydana geldi!" });
			}
			con.Close();
			return Json(new { success = true, responseText = "Ürün başarılı bir şekilde düzenlendi!" });
		}

		[HttpGet]
		public IActionResult Uretim(int id)
		{
			urunId = id;
			// Urunler urunler = new Urunler();
			if(con.State != System.Data.ConnectionState.Open)
			{
				con.Open();
			}

			SqlCommand cmd = new SqlCommand($"select * from Urunler where id='{id}'", con);
			SqlDataReader dataReader = cmd.ExecuteReader();
			if (dataReader.Read())
			{
				urunler.Id = id;
				
				urunler.Urun = dataReader["Urun"].ToString();
				urunler.Miktar = Convert.ToInt32(dataReader["Miktar"]);
				urunler.UrunKodlari = JsonConvert.DeserializeObject<List<UrunKodlari>>(dataReader["UrunKodlari"].ToString());
			}
			dataReader.Close();

			int count = 0;
			// UrunKodlarindaki idleri isimlerle eşleştiricem
			foreach (var item in urunler.UrunKodlari)
			{
				SqlCommand sqlCommand = new SqlCommand($"select Hammadde, Stok from Hammaddeler where id = '{item.id}'", con);
				SqlDataReader dataReader1 = sqlCommand.ExecuteReader();
				if (dataReader1.Read())
				{
					urunler.UrunKodlari[count].hammaddeAdi = dataReader1["Hammadde"].ToString();
					urunler.UrunKodlari[count].envanterToplami = Convert.ToInt32(dataReader1["Stok"].ToString());
				}
				count++;
			}

			con.Close();

			//var urunKodlari = ;

			ViewBag.urunler = urunler;
			//ViewBag.UrunKodlari = urunKodlari;

			//foreach (var item in urunKodlari)
			//{
			//	int asd = item.id;
			//	int quantity = item.quantity;
			//}

			return View();
		}

		[HttpPost]
		public JsonResult Uretim(Uretim uretim, int urunQuantity)
		{
			// Viewbagden ürünlerin idlerinden dbde kontrol yapacak yeterlimi diye yeterliyse üretecek ve hammadden o miktar kadar kesecek

			if(con.State != System.Data.ConnectionState.Open)
			{
				con.Open();
			}

			bool flag = false;
			foreach (var item in urunler.UrunKodlari)
			{
				int q = item.quantity * urunQuantity;
				SqlCommand cmd = new SqlCommand($"select Stok from Hammaddeler where id = {item.id}", con);
				//var ex = cmd.ExecuteScalar();

				SqlDataReader dataReader = cmd.ExecuteReader();
				if (dataReader.Read())
				{
					if (q > Convert.ToInt32(dataReader["Stok"])) // Büyük geliyor olmaz
					{
						//return View("alert('fail')");
						flag = false;
						break;
					}
					else
					{
						flag = true;
					}
				}
				dataReader.Close();
			}

			if (flag == true)
			{
				foreach (var item in urunler.UrunKodlari)
				{
					int q = item.quantity * urunQuantity;
					SqlCommand cmd = new SqlCommand($"select Stok from Hammaddeler where id = {item.id}", con);
					//var ex = cmd.ExecuteScalar();

					SqlDataReader dataReader = cmd.ExecuteReader();
					if (dataReader.Read())
					{
						//envanterStok.Add(Convert.ToInt32(dataReader["Stok"]));
						

						SqlCommand cmd2 = new SqlCommand($"update Hammaddeler set Stok = Stok - {q} where id = '{item.id}'", con);
						cmd2.ExecuteNonQuery();
						
					}
					dataReader.Close();
				}

				SqlCommand cmd3 = new SqlCommand($"update Urunler set Miktar = Miktar + {urunQuantity} where id='{urunId}'", con);
				cmd3.ExecuteNonQuery();
				con.Close();
				return Json(new { success = true, responseText = "Üretim başarılı bir şekilde yapıldı!" });

			}
			else
			{
				// Yetmiyor!
				return Json(new { success = false, responseText = "Hammadden yetersiz!" });
				//ViewBag.Message = "Yeterli Hammadden Yok!";
				//return View();
			}
		}
	}

	public class UrunKodlari
	{
		public int id { get; set; }

		public int quantity { get; set; }

		public string hammaddeAdi { get; set; }

		public int envanterToplami { get; set; }
	}
}
