using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web_app_MVC.Models
{
    public class UserModel
    {
     
        public int UserID { get; set; }
        [Required (ErrorMessage ="User name is required")]
        public string UserName  { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
        [Required]
        [Phone]
        public string MobileNo {  get; set; }
        [Required]
        [StringLength(1000)]
        public string Address { get; set; }
        [Required]
        public bool IsActive { get; set; }


    }
}
