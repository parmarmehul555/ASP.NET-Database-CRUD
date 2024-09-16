using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using web_app_MVC.Models;
using web_app_MVC.DAL;
using web_app_MVC.Attributes;

namespace web_app_MVC.Controllers
{
    public class userController : Controller
    {
        private IConfiguration _configuration;

        public userController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [CheckAccess]
        #region List Users
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
        #endregion

        [CheckAccess]
        #region Delete User
        public IActionResult UserDelete(int UserID)
        {
            try
            {
                String str = _configuration.GetConnectionString("MyConnectionString");
                SqlConnection connection = new SqlConnection(str);
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_User_Delete";
                cmd.Parameters.AddWithValue("UserID", UserID);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Foreign key conflict error accured!";
            }
            return RedirectToAction("userList");
        }
        #endregion

        [CheckAccess]
        #region User Add Edit

        public IActionResult userAddEdit(int? UserID)
        {
            if (UserID != null)
            {
                String str = this._configuration.GetConnectionString("MyConnectionString");
                SqlConnection connection = new SqlConnection(str);
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_User_SelectByID";
                cmd.Parameters.AddWithValue("UserID", UserID);
                SqlDataReader reader = cmd.ExecuteReader();
                DataTable user = new DataTable();
                user.Load(reader);
                UserModel user1 = new UserModel();
                foreach (DataRow dr in user.Rows)
                {
                    user1.UserID = Convert.ToInt32(dr["UserID"]);
                    user1.UserName = dr["UserName"].ToString();
                    user1.Email = dr["Email"].ToString();
                    user1.Password = dr["Password"].ToString();
                    user1.Address = dr["Address"].ToString();
                    user1.MobileNo = dr["MobileNo"].ToString();
                    user1.IsActive = Convert.ToBoolean(dr["IsActive"]);
                }
                cmd.ExecuteNonQuery();
                connection.Close();
                return View("userAddEdit", user1);
            }
            return View();
        }
        #endregion

        [CheckAccess]
        #region Save User
        [HttpPost]
        public IActionResult Save(UserModel user)
        {
            if(user.Password == null)
            {
                ModelState.AddModelError("Password", "Password Field is Required");
            }
            if (user.UserID == null)
            {
                if (ModelState.IsValid)
                {
                    String str = _configuration.GetConnectionString("MyConnectionString");
                    SqlConnection connection = new SqlConnection(str);
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "PR_User_Insert";
                    cmd.Parameters.AddWithValue("UserName", user.UserName);
                    cmd.Parameters.AddWithValue("Email", user.Email);
                    cmd.Parameters.AddWithValue("Password", user.Password);
                    cmd.Parameters.AddWithValue("MobileNo", user.MobileNo);
                    cmd.Parameters.AddWithValue("Address", user.Address);
                    cmd.Parameters.AddWithValue("IsActive", user.IsActive);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    return RedirectToAction("userList");
                }
                return View("userAddEdit", user);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    String str = _configuration.GetConnectionString("MyConnectionString");
                    SqlConnection connection = new SqlConnection(str);
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "PR_User_Update";
                    cmd.Parameters.AddWithValue("UserID", user.UserID);
                    cmd.Parameters.AddWithValue("UserName", user.UserName);
                    cmd.Parameters.AddWithValue("Email", user.Email);
                    cmd.Parameters.AddWithValue("Password", user.Password);
                    cmd.Parameters.AddWithValue("MobileNo", user.MobileNo);
                    cmd.Parameters.AddWithValue("Address", user.Address);
                    cmd.Parameters.AddWithValue("IsActive", user.IsActive);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    return RedirectToAction("userList");
                }
                return View("userAddEdit", user);
            }
            return View("userAddEdit", user);
        }
        #endregion

        #region Login Page
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }
        #endregion

        #region Login User
        public IActionResult UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DB_Connection connection = new DB_Connection();
                    SqlCommand cmd =  connection.Connect_DB("PR_User_Login", CommandType.StoredProcedure);
                    cmd.Parameters.AddWithValue("UserName", userLoginModel.UserName);
                    cmd.Parameters.AddWithValue("Password", userLoginModel.Password);
                    SqlDataReader reader = cmd.ExecuteReader();
                    DataTable td = new DataTable();
                   td.Load(reader);
                    foreach(DataRow row in td.Rows)
                    {
                        HttpContext.Session.SetString("UserID", row["UserID"].ToString());
                        HttpContext.Session.SetString("UserName", row["UserName"].ToString());
                    }
                    return RedirectToAction("ProductList", "Product");
                }
            }catch(Exception ex)
            {
                TempData["LoginErrorMessage"] = ex.Message;
            }
            return View("Login");
        }
        #endregion

    }
}
