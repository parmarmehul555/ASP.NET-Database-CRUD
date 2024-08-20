using System.ComponentModel.DataAnnotations;

namespace web_app_MVC.Models
{
    public class ProductModel
    {
        public int? ProductID { get; set; }
        [Required(ErrorMessage ="Please Enter product name")]
        public string ProductName { get; set; }
        [Required(ErrorMessage ="ProductPrice is not Empty")]
        public int ProductPrice { get; set; }
        public string ProductCode { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        [Required(ErrorMessage = "please choose approproate option")]

        public int UserID { get; set; } 
    }

    public class UserDropdownModel
    {
        public int UserID { get; set; }
        public String UserName { get; set; }
    }

    public class ProductDropdownModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }
}
