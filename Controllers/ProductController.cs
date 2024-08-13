using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using web_app_MVC.Models;

namespace web_app_MVC.Controllers
{
    public class ProductController : Controller
    {
        private IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult productList()
        {
            String connStr = _configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(connStr);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Product_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable products = new DataTable();
            products.Load(reader);
            connection.Close();
            return View(products);
        }
        public IActionResult productAddEdit()
        {
            return View();
        }
    }
}
