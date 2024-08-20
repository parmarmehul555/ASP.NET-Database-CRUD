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
        public IActionResult productDelete(int productID)
        {
            try
            {
                String connStr = _configuration.GetConnectionString("MyConnectionString");
                SqlConnection connection = new SqlConnection(connStr);
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_Product_Delete";
                cmd.Parameters.AddWithValue("ProductID", productID);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Foreign Key conflict error occured!";
            }
            return RedirectToAction("productList");
        }

        public List<UserDropdownModel> GetUserDropdownModels()
        {
            String connStr = _configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(connStr);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_User_Dropdown";
            SqlDataReader reader = command.ExecuteReader();
            DataTable products = new DataTable();
            products.Load(reader);
            List<UserDropdownModel> userDropdown = new List<UserDropdownModel>();
            foreach(DataRow product in products.Rows)
            {
                UserDropdownModel user = new UserDropdownModel();
                user.UserID = Convert.ToInt32(product["UserID"]);
                user.UserName = product["UserName"].ToString();
                userDropdown.Add(user);
            }
            ViewBag.userDropdown = userDropdown;
            return userDropdown;
        }
        public IActionResult productAddEdit()
        {
            var userDropdown = GetUserDropdownModels();
            ViewBag.userDropdown = userDropdown;
            return View();
        }
    }
}
