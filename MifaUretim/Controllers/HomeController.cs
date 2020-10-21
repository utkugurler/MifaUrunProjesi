using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MifaUretim.Models;
using System.Data.SqlClient;

namespace MifaUretim.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		ConnectDB connectDB = new ConnectDB();
		SqlConnection con;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
			
			
			var configuration = ConnectDB.GetConfiguration();
			con = new SqlConnection(configuration.GetSection("ConnectionStrings").GetSection("MvcUretimContext").Value);
			
		}

		public IActionResult GetUretimListe()
		{
			return View();
		}
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public IActionResult Duzenle()
		{
			return View();
		}
	}
}
