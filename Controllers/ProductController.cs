using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using web_app_MVC.Attributes;
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
        [CheckAccess]
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
        [CheckAccess]
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
        [CheckAccess]
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
        [CheckAccess]
        [HttpPost]
        public IActionResult Save(ProductModel modelProduct)
        {
            String connstr = _configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(connstr);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            if(modelProduct.ProductID == null)
            {
                cmd.CommandText = "PR_Product_Insert";
            }
            else
            {
                cmd.CommandText = "PR_Product_Update";
                cmd.Parameters.AddWithValue("ProductID", modelProduct.ProductID);
            }
            cmd.Parameters.AddWithValue("ProductName", modelProduct.ProductName);
            cmd.Parameters.AddWithValue("ProductPrice", modelProduct.ProductPrice);
            cmd.Parameters.AddWithValue("Description", modelProduct.Description);
            cmd.Parameters.AddWithValue("ProductCode", modelProduct.ProductCode);
            cmd.Parameters.AddWithValue("UserID", modelProduct.UserID);
            SqlDataReader reader = cmd.ExecuteReader();
            dt.Load(reader);
            return RedirectToAction("productList");
        }
        [CheckAccess]
        public IActionResult productAddEdit(int? ProductID)
        {
            var userDropdown = GetUserDropdownModels();
            ViewBag.userDropdown = userDropdown;
            if (ProductID != null)
            {
                String connStr = _configuration.GetConnectionString("MyConnectionString");
                SqlConnection connection = new SqlConnection(connStr);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Product_SelectByID";
                command.Parameters.AddWithValue("ProductID", ProductID);
                SqlDataReader reader = command.ExecuteReader();
                DataTable product = new DataTable();
                product.Load(reader);
                ProductModel ProductData = new ProductModel();
                foreach(DataRow dr in product.Rows)
                {
                    ProductData.ProductName = dr["ProductName"].ToString();
                    ProductData.Description = dr["Description"].ToString();
                    ProductData.ProductCode = dr["ProductCode"].ToString();
                    ProductData.ProductPrice = Convert.ToInt32(dr["ProductPrice"]);
                    ProductData.UserID = Convert.ToInt32(dr["UserID"]);
                }
                connection.Close();
                return View(ProductData);
            }
            return View();
        }
    }
}
