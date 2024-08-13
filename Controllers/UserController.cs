using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using web_app_MVC.Models;
namespace web_app_MVC.Controllers
{
    public class userController : Controller
    {
        private IConfiguration _configuration;

        public userController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [Route("/")]
        public IActionResult userList()
        {
            String str = this._configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(str);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "PR_User_SelectAll";
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable users = new DataTable();
            users.Load(reader);
            return View(users);
        }

        public IActionResult UserDelete(int UserID)
        {
            String str = _configuration.GetConnectionString("MyConnectionString");
            SqlConnection connection = new SqlConnection(str);
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_User_Delete";
            cmd.Parameters.AddWithValue("UserID",UserID);
            cmd.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("userList");
        }
        public IActionResult userAddEdit()
        {
            return View();
        }
        public IActionResult Save(UserModel usermodel )
        {
            if (ModelState.IsValid) {
                return RedirectToAction("userAddEdit");

            }
            else
            {
                return View("userAddEdit",usermodel);
            }
        }
    }
}
