using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
namespace web_app_MVC.DAL
{
    public class DB_Connection : DAL_Base
    {
        public SqlCommand Connect_DB(String CMD_Text,System.Data.CommandType CMD_Type)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connstr);
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = CMD_Text;
                cmd.CommandType = CMD_Type;
                return cmd;
            }
            catch(SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
