using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SiemensDataApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SiemensDataApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> Logger;
        private IConfiguration Configuration;
 
        public HomeController(IConfiguration _configuration, ILogger<HomeController> logger)
        {
            Configuration = _configuration;

            Logger = logger;
        }

        public IActionResult Index()
        {
            string connString = this.Configuration.GetConnectionString("MyConnectionString");
            ViewBag.MyConnectionString = connString;

            DBResultModel model = new DBResultModel();

            if (!String.IsNullOrEmpty(connString))
            {
                try 
                { 
                    using (SqlConnection connection = new SqlConnection(connString))
                    {
                        Console.WriteLine("Query data example:");
                        Console.WriteLine("***********************************");

                        String sql = "SELECT Name, Collation_Name FROM sys.databases";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            connection.Open();
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    model.DBInfos.Add(
                                        new DBInfo() 
                                        {
                                            Name = reader.GetString(0),
                                            Collation_Name =  reader.GetString(1)
                                        }
                                    );
                                    //Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                                }
                            }
                        }                    
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            return View(model);
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
    }
}
