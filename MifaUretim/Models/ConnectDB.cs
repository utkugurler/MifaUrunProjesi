using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MifaUretim.Models
{
	public class ConnectDB
	{
		SqlConnection con;
		public ConnectDB()
		{
			var configuration = GetConfiguration();
			con = new SqlConnection(configuration.GetSection("ConnectionStrings").GetSection("MvcUretimContext").Value);

		}
		public static IConfigurationRoot GetConfiguration()
		{
			var builder = new ConfigurationBuilder().SetBasePath(System.IO.Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json",
				 optional: true, reloadOnChange: true);
			return builder.Build();
		}
	}
}
