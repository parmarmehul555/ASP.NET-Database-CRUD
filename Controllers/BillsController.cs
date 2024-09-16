using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using web_app_MVC.Models;
using web_app_MVC.Attributes;

namespace web_app_MVC.Controllers
{
    public class BillsController : Controller
    {
        private IConfiguration _configuration;

        public BillsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [CheckAccess]
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
        [CheckAccess]
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
            return RedirectToAction("billList");
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
        public IActionResult Save(BillsModel billsModel)
        {
            if (ModelState.IsValid)
            {
                String connstr = _configuration.GetConnectionString("MyConnectionString");
                SqlConnection connection = new SqlConnection(connstr);
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                if (billsModel.BillID == null)
                {
                    cmd.CommandText = "PR_Bills_Insert";
                }
                else
                {
                    cmd.CommandText = "PR_Bills_Update";
                    cmd.Parameters.AddWithValue("BillID", billsModel.BillID);
                }
                cmd.Parameters.AddWithValue("BillNumber", billsModel.BillNumber);
                cmd.Parameters.AddWithValue("BillDate", billsModel.BillDate);
                cmd.Parameters.AddWithValue("OrderID", billsModel.OrderID);
                cmd.Parameters.AddWithValue("TotalAmount", billsModel.TotalAmount);
                cmd.Parameters.AddWithValue("Discount", billsModel.Discount);
                cmd.Parameters.AddWithValue("NetAmount", billsModel.NetAmount);
                cmd.Parameters.AddWithValue("UserID", billsModel.UserID);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return RedirectToAction("billList");
            }
            else
            {
                return View("billAddEdit", billsModel);
            }

        }
        [CheckAccess]
        public IActionResult billAddEdit(int? BillID)
        {
            List<UserDropdownModel> userDropdown = GetUserDropdowns();
            ViewBag.userDropdown = userDropdown;

            List<OrderDropdown> orderDropdown = GetOrderDropdowns();
            ViewBag.orderDropdown = orderDropdown;

            if(BillID != null)
            {
                String str = this._configuration.GetConnectionString("MyConnectionString");
                SqlConnection connection = new SqlConnection(str);
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_Bills_SelectByID";
                cmd.Parameters.AddWithValue("BillID", BillID);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                BillsModel bm = new BillsModel();
                foreach(DataRow row in table.Rows)
                {
                    bm.BillID = Convert.ToInt32(row["BillID"]);
                    bm.BillNumber = row["BillNumber"].ToString();
                    bm.BillDate = row["BillDate"].ToString();
                    bm.OrderID = Convert.ToInt32(row["OrderID"]);
                    bm.TotalAmount = Convert.ToDecimal(row["TotalAmount"]);
                    bm.Discount = Convert.ToDecimal(row["Discount"]);
                    bm.NetAmount = Convert.ToDecimal(row["NetAmount"]);
                    bm.UserID = Convert.ToInt32(row["UserID"]);
                }
                connection.Close();
                return View(bm);
            }
            return View();
        }
    }
}
