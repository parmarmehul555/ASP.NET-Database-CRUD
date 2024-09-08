using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace web_app_MVC.Models
{
    public class CustomerController : Controller
    {
        private IConfiguration _configuration;
        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public IActionResult customerList()
        {
            String connstr = _configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(connstr);
            SqlCommand cmd = connection.CreateCommand();
            connection.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_Customer_SelectAll";
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable customer = new DataTable();
            customer.Load(reader);
            return View(customer);
        }

        public IActionResult CustomerDelete(int CustomerID)
        {
            try
            {
                String conn = _configuration.GetConnectionString("MyConnectionString");
                SqlConnection connection = new SqlConnection(conn);
                SqlCommand cmd = connection.CreateCommand();
                connection.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_Customer_Delete";
                cmd.Parameters.AddWithValue("CustomerID", CustomerID);
                cmd.ExecuteNonQuery();
                connection.Close();
            }catch(Exception ex)
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
            foreach(DataRow user in table.Rows)
            {
                UserDropdownModel userr = new UserDropdownModel();
                userr.UserID = Convert.ToInt32(user["UserID"]);
                userr.UserName = user["UserName"].ToString();
                userList.Add(userr);
            }
            connection.Close();
            return userList;
        }

        [HttpPost]
        public IActionResult Save(CustomerModel modelCustomer)
        {
            String connstr = _configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(connstr);
            SqlCommand cmd = connection.CreateCommand();
            connection.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            if(modelCustomer.CustomerID == null)
            {
                cmd.CommandText = "PR_Customer_Insert";
            }
            else
            {
                cmd.CommandText = "PR_Customer_Update";
                cmd.Parameters.AddWithValue("CustomerID", modelCustomer.CustomerID);
            }
            cmd.Parameters.AddWithValue("CustomerName", modelCustomer.CustomerName);
            cmd.Parameters.AddWithValue("HomeAddress", modelCustomer.HomeAddress);
            cmd.Parameters.AddWithValue("Email", modelCustomer.Email);
            cmd.Parameters.AddWithValue("MobileNo", modelCustomer.MobileNo);
            cmd.Parameters.AddWithValue("GSTNo", modelCustomer.GSTNO);
            cmd.Parameters.AddWithValue("CityName", modelCustomer.CityName);
            cmd.Parameters.AddWithValue("PinCode", modelCustomer.PinCode);
            cmd.Parameters.AddWithValue("NetAmount", modelCustomer.NetAmount);
            cmd.Parameters.AddWithValue("UserID", modelCustomer.UserID);
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            return RedirectToAction("customerList");
        }
        public IActionResult customerAddEdit(int? CustomerID)
        {
            List<UserDropdownModel> userDropdown = GetUserDropdowns();
            ViewBag.userDropdown = userDropdown;
            if (CustomerID != null)
            {
                String connstr = _configuration.GetConnectionString("MyConnectionString");
                SqlConnection connection = new SqlConnection(connstr);
                SqlCommand cmd = connection.CreateCommand();
                connection.Open();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "PR_Customer_SelectByID";
                cmd.Parameters.AddWithValue("CustomerID", CustomerID);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable customer = new DataTable();
                customer.Load(reader);
                CustomerModel cm = new CustomerModel();
                foreach(DataRow dr in customer.Rows)
                {
                    cm.CustomerID = Convert.ToInt32(dr["CustomerID"]);
                    cm.CustomerName = dr["CustomerName"].ToString();
                    cm.HomeAddress = dr["HomeAddress"].ToString();
                    cm.Email = dr["Email"].ToString();
                    cm.MobileNo = dr["MobileNo"].ToString();
                    cm.GSTNO = dr["GSTNo"].ToString();
                    cm.CityName = dr["CityName"].ToString();
                    cm.PinCode= dr["PinCode"].ToString();
                    cm.NetAmount = dr["NetAmount"].ToString();
                    cm.UserID = Convert.ToInt32(dr["UserID"]);
                }
                return View(cm);
            }
            return View();
        }
    }
}
