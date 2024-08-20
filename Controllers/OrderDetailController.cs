using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using web_app_MVC.Models;

namespace web_app_MVC.Controllers
{
    public class OrderDetailController : Controller
    {
        private IConfiguration _configuration;
        public OrderDetailController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult orderDetailList()
        {
            String str = this._configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(str);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_OrderDetail_SelectAll";
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable orderDetails= new DataTable();
            orderDetails.Load(reader);
            return View(orderDetails);
        }

        public IActionResult OrderDetailDelete(int OrderDetailsID)
        {
            try
            {
                String connstr = _configuration.GetConnectionString("MyConnectionString");
                SqlConnection connection = new SqlConnection(connstr);
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_OrderDetail_Delete";
                cmd.Parameters.AddWithValue("OrderDetailID", OrderDetailsID);
                cmd.ExecuteNonQuery();
                connection.Close();
            }catch(Exception ex)
            {
                TempData["Message"] = "Foreign key conflit error occured!";
            }
            return RedirectToAction("orderDetailList");
        }

        public List<UserDropdownModel> GetUserDropdowns()
        {
            String connstr = _configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(connstr);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_User_Dropdown";
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            List<UserDropdownModel> userList = new List<UserDropdownModel>();
            foreach (DataRow user in table.Rows)
            {
                UserDropdownModel userr = new UserDropdownModel();
                userr.UserID = Convert.ToInt32(user["UserID"]);
                userr.UserName = user["UserName"].ToString();
                userList.Add(userr);
            }
            connection.Close();
            return userList;
        }

        public List<OrderDropdown> GetOrderDropdowns()
        {
            String connstr = _configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(connstr);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_Order_Dropdown";
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            List<OrderDropdown> orderList = new List<OrderDropdown>();
            foreach (DataRow user in table.Rows)
            {
                OrderDropdown orderr = new OrderDropdown();
                orderr.OrderID = Convert.ToInt32(user["OrderID"]);
                orderr.ShippingAddress = user["ShippingAddress"].ToString();
                orderList.Add(orderr);
            }
            connection.Close();
            return orderList;
        }

        public List<ProductDropdownModel> GetProductDropdowns()
        {
            String connstr = _configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(connstr);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_Product_Dropdown";
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            List<ProductDropdownModel> productList = new List<ProductDropdownModel>();
            foreach (DataRow product in table.Rows)
            {
                ProductDropdownModel productt = new ProductDropdownModel();
                productt.ProductID= Convert.ToInt32(product["ProductID"]);
                productt.ProductName = product["ProductName"].ToString();
                productList.Add(productt);
            }
            connection.Close();
            return productList;
        }
        public IActionResult orderDetailAddEdit()
        {
            List<UserDropdownModel> userDropdown = GetUserDropdowns();
            ViewBag.userDropdown = userDropdown;

            List<OrderDropdown> orderDropdown = GetOrderDropdowns();
            ViewBag.orderDropdown = orderDropdown;

            List<ProductDropdownModel> productDropdown = GetProductDropdowns();
            ViewBag.productDropdown = productDropdown;
            return View();
        }
    }
}
