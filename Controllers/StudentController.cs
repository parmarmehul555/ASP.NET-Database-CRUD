using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace web_app_MVC.Controllers
{
    public class StudentController : Controller
    {
        private IConfiguration _configuration;

        public StudentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            String connStr = _configuration.GetConnectionString("MyConnectionString");
            SqlConnection sqlDB = new SqlConnection(connStr);
            SqlCommand cmd = sqlDB.CreateCommand();
            sqlDB.Open();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "";
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable students = new DataTable();
            students.Load(reader);
            sqlDB.Close();
            return View(students);
        }
    }
}
