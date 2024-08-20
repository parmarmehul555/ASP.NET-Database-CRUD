using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using web_app_MVC.Models;

namespace web_app_MVC.Controllers
{
    public class BillsController : Controller
    {
        private IConfiguration _configuration;

        public BillsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Save(BillsModel billsModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("billAddEdit");
            }
            else { 
                return View("billAddEdit",billsModel);
            }
           
        }
        public IActionResult billList()
        {
            String str = this._configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(str);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_Bills_SelectAll";
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            connection.Close();
            return View(table);
        }

        public IActionResult BillDelete(int BillID)
        {
            try
            {
                String conn = _configuration.GetConnectionString("MyConnectionString");
                SqlConnection connection = new SqlConnection(conn);
                SqlCommand cmd = connection.CreateCommand();
                connection.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_Bills_Delete";
                cmd.Parameters.AddWithValue("BillID", BillID);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
            }
            return RedirectToAction("customerList");
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
        public IActionResult billAddEdit()
        {
            List<UserDropdownModel> userDropdown = GetUserDropdowns();
            ViewBag.userDropdown = userDropdown;

            List<OrderDropdown> orderDropdown = GetOrderDropdowns();
            ViewBag.orderDropdown = orderDropdown;
            return View();
        }
    }
}
