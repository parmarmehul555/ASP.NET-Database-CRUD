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
        public IActionResult Save(CustomerModel customerModel)
        {
            if (ModelState.IsValid) {
                return RedirectToAction("customerAddEdit");
            }
            else
            {
                return View("customerAddEdit",customerModel);
            }
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
        public IActionResult customerAddEdit()
        {
            List<UserDropdownModel> userDropdown = GetUserDropdowns();
            ViewBag.userDropdown = userDropdown;
            return View();
        }
    }
}
