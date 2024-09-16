using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using web_app_MVC.Attributes;
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
        [CheckAccess]
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
        [CheckAccess]
        [HttpDelete]
        public IActionResult OrderDetailDelete(int OrderDetailID)
        {
            try
            {
                String connstr = _configuration.GetConnectionString("MyConnectionString");
                SqlConnection connection = new SqlConnection(connstr);
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_OrderDetail_Delete";
                cmd.Parameters.AddWithValue("OrderDetailID", OrderDetailID);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch(Exception ex)
            {
                TempData["Message"] = "Foreign key conflit error occured!";
            }
            return RedirectToAction("orderDetailList");
       }
        [CheckAccess]
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
        [CheckAccess]
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
        [CheckAccess]
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
        [CheckAccess]
        public IActionResult Save(OrderDetailModel modelOrderDetail)
        {
            String connstr = _configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(connstr);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            if (modelOrderDetail.OrderDetailID == null)
            {
                cmd.CommandText = "PR_OrderDetail_Insert";
            }
            else
            {
                cmd.CommandText = "PR_OrderDetail_Update";
                cmd.Parameters.AddWithValue("OrderDetailID", modelOrderDetail.OrderDetailID);
            }
            cmd.Parameters.AddWithValue("OrderID", modelOrderDetail.OrderID);
            cmd.Parameters.AddWithValue("ProductID", modelOrderDetail.ProductID);
            cmd.Parameters.AddWithValue("Quantity", modelOrderDetail.Quantity);
            cmd.Parameters.AddWithValue("Amount", modelOrderDetail.Amount);
            cmd.Parameters.AddWithValue("TotalAmount", modelOrderDetail.TotalAmount);
            cmd.Parameters.AddWithValue("UserID", modelOrderDetail.UserID);
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return RedirectToAction("orderDetailList");
        }
        [CheckAccess]
        public IActionResult orderDetailAddEdit(int? OrderDetailID)
        {
            List<UserDropdownModel> userDropdown = GetUserDropdowns();
            ViewBag.userDropdown = userDropdown;

            List<OrderDropdown> orderDropdown = GetOrderDropdowns();
            ViewBag.orderDropdown = orderDropdown;

            List<ProductDropdownModel> productDropdown = GetProductDropdowns();
            ViewBag.productDropdown = productDropdown;

            if(OrderDetailID != null)
            {
                String str = this._configuration.GetConnectionString("MyConnectionString");
                SqlConnection connection = new SqlConnection(str);
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_OrderDetail_SelectByID";
                cmd.Parameters.AddWithValue("OrderDetailID", OrderDetailID)
;                SqlDataReader reader = cmd.ExecuteReader();
                DataTable orderDetail = new DataTable();
                orderDetail.Load(reader);
                OrderDetailModel orm = new OrderDetailModel();
                foreach(DataRow row in orderDetail.Rows)
                {
                    orm.OrderDetailID = Convert.ToInt32(row["OrderDetailID"]);
                    orm.OrderID = Convert.ToInt32(row["OrderID"]);
                    orm.ProductID = Convert.ToInt32(row["ProductID"]);
                    orm.Quantity = Convert.ToInt32(row["Quantity"]);
                    orm.Amount= Convert.ToInt32(row["Amount"]);
                    orm.TotalAmount = Convert.ToInt32(row["TotalAmount"]);
                    orm.UserID = Convert.ToInt32(row["UserID"]);
                }
                return View(orm);
            }
            return View();
        }
    }
}
