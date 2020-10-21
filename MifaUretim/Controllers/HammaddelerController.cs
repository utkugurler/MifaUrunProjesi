using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MifaUretim.Models;

namespace MifaUretim.Controllers
{
    public class HammaddelerController : Controller
    {
        SqlConnection con;

        public HammaddelerController()
        {
            var configuration = ConnectDB.GetConfiguration();
            con = new SqlConnection(configuration.GetSection("ConnectionStrings").GetSection("MvcUretimContext").Value);
        }

        // GET: Hammaddeler
        public IActionResult Index()
        {
            ViewBag.hammaddeler = GetHammaddeList();
            return View();
        }

        public List<Hammaddeler> GetHammaddeList()
        {
            List<Hammaddeler> hammaddeler = new List<Hammaddeler>();
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();

            SqlCommand cmd = new SqlCommand($"select * from Hammaddeler", con);
            SqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                Hammaddeler hammaddeler1 = new Hammaddeler();
                hammaddeler1.Id = Convert.ToInt32(dataReader["id"]);
                hammaddeler1.Hammadde = dataReader["Hammadde"].ToString();
                hammaddeler1.Stok = Convert.ToInt32(dataReader["Stok"]);
                hammaddeler1.HammaddeKod = dataReader["HammaddeKod"].ToString();
                hammaddeler.Add(hammaddeler1);
            }
            dataReader.Close();
            con.Close();

            return hammaddeler;
        }

        [HttpGet]
        public IActionResult HammaddeDuzenle(int id)
		{
            Hammaddeler hammaddeler = new Hammaddeler();
            if(con.State != System.Data.ConnectionState.Open)
			{
                con.Open();
			}

            SqlCommand cmd = new SqlCommand($"select * from Hammaddeler where id = '{id}'", con);
            SqlDataReader dataReader = cmd.ExecuteReader();
			if (dataReader.Read())
			{
                hammaddeler.Id = Convert.ToInt32(dataReader["id"]);
                hammaddeler.Hammadde = dataReader["Hammadde"].ToString();
                hammaddeler.Stok = Convert.ToInt32(dataReader["Stok"]);
                hammaddeler.HammaddeKod = dataReader["HammaddeKod"].ToString();
            }
            ViewBag.hammaddeler = hammaddeler;

            con.Close();
            dataReader.Close();
            return View();
		}

        [HttpPost]
        public IActionResult HammaddeDuzenle(Hammaddeler hammaddeler)
		{
            if (hammaddeler.Hammadde == null)
			{

			}
			else
			{
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand($"update Hammaddeler set Hammadde='{hammaddeler.Hammadde}', Stok='{hammaddeler.Stok}', HammaddeKod='{hammaddeler.HammaddeKod}' where id = '{hammaddeler.Id}'", con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToAction("Index");
        }

        public IActionResult YeniHammadde()
		{
            return View();
		}

        [HttpPost]
        public IActionResult YeniHammadde(Hammaddeler hammaddeler)
        {
            if (con.State != System.Data.ConnectionState.Open)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand($"insert into Hammaddeler(Hammadde, Stok, HammaddeKod) values('{hammaddeler.Hammadde}', '{hammaddeler.Stok}', '{hammaddeler.HammaddeKod}')", con);
            cmd.ExecuteNonQuery();
            con.Close();
            return View();
        }

    }
}
