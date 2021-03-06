﻿using System;
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
            }
            ViewBag.hammaddeler = hammaddeler;

            con.Close();
            dataReader.Close();
            return View();
		}

        [HttpPost]
        public JsonResult HammaddeDuzenle(Hammaddeler hammaddeler)
		{
            if (hammaddeler.Hammadde == null)
			{
                return Json(new { success = false, responseText = "Hammadde adı boş!" });
            }
            else
			{
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
				try
				{
                    SqlCommand cmd = new SqlCommand($"update Hammaddeler set Hammadde='{hammaddeler.Hammadde}', Stok='{hammaddeler.Stok}' where id = '{hammaddeler.Id}'", con);
                    cmd.ExecuteNonQuery();
                    return Json(new { success = true, responseText = "Hammadde başarılı bir şekilde oluşturuldu!" });
                }
                catch (Exception ex)
				{
                    con.Close();
                    return Json(new { success = false, responseText = "Hammadde düzenlenirken bir hata meydana geldi!" });
                }
            }
        }

        [HttpGet]
        public IActionResult HammaddeDelete(int id)
		{
            if(con.State != System.Data.ConnectionState.Open)
			{
                con.Open();
			}

            SqlCommand cmd = new SqlCommand($"delete from Hammaddeler where id = '{id}'" ,con);
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
		}

        public IActionResult YeniHammadde()
		{
            return View();
		}

        [HttpPost]
        public JsonResult YeniHammadde(Hammaddeler hammaddeler)
        {
            if (con.State != System.Data.ConnectionState.Open)
            {
                con.Open();
            }

			try
			{
                SqlCommand cmd = new SqlCommand($"insert into Hammaddeler(Hammadde, Stok) values('{hammaddeler.Hammadde}', '{hammaddeler.Stok}')", con);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
			{
                con.Close();
                return Json(new { success = false, responseText = "Hammadde oluşturulurken bir hata meydana geldi!" });
            }

            con.Close();
            return Json(new { success = true, responseText = "Hammadde başarılı bir şekilde oluşturuldu!" });
        }

    }
}
