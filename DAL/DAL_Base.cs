namespace web_app_MVC.DAL
{
    public class DAL_Base
    {
        public string connstr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("MyConnectionString");
    }
}
