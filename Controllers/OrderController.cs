using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using web_app_MVC.Models;
namespace web_app_MVC.Controllers
{
    public class OrderController : Controller
    {
        private IConfiguration _configuration;
        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult orderList()
        {
            String connStr = _configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(connStr);
            SqlCommand command = connection.CreateCommand();
			connection.Open();
			command.CommandType = System.Data.CommandType.StoredProcedure;
			command.CommandText = "PR_Order_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
			table.Load(reader);
            return View(table);
        }
        public IActionResult orderAddEdit()
        {
            return View();
        }
    }
}
