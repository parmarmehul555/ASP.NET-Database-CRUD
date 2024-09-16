using System.ComponentModel.DataAnnotations;

namespace web_app_MVC.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Please Enter User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }
    }
}
