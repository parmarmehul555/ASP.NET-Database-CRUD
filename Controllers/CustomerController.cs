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
            SqlCommand cmd = new SqlCommand();
            connection.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_Cus_SelectAll";
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable customer = new DataTable();
            customer.Load(reader);
            return View(customer);
        }
        public IActionResult customerAddEdit()
        {
            return View();
        }
    }
}
