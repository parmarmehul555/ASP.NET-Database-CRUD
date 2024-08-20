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

        public IActionResult OrderDelete(int OrderID)
        {
            try
            {
                String connstr = _configuration.GetConnectionString("MyConnectionString");
               SqlConnection connection = new SqlConnection(connstr);
                SqlCommand command = connection.CreateCommand();
                connection.Open();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "PR_Order_Delete";
                command.Parameters.AddWithValue("OrderID", OrderID);
                command.ExecuteNonQuery();
            }catch(Exception ex)
            {
                TempData["Message"] = "Foreign key conflict error occured!";
            }
            return RedirectToAction("orderList");
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

        public List<CustomerDropdown> GetCustomerDropdowns()
        {
            String connstr = _configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(connstr);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_Customer_Dropdown";
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            List<CustomerDropdown> customerList = new List<CustomerDropdown>();
            foreach (DataRow user in table.Rows)
            {
                CustomerDropdown customerr = new CustomerDropdown();
                customerr.CustomerID = Convert.ToInt32(user["CustomerID"]);
                customerr.CustomerName = user["CustomerName"].ToString();
                customerList.Add(customerr);
            }
            connection.Close();
            return customerList;
        }
        public IActionResult orderAddEdit()
        {
            List<UserDropdownModel> userDropdown = GetUserDropdowns();
            ViewBag.userDropdown = userDropdown;

            List<CustomerDropdown> customerDropdown = GetCustomerDropdowns();
            ViewBag.customerDropdown = customerDropdown;
            return View();
        }
    }
}
