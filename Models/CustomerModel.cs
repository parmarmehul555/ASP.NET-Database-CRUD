using System.ComponentModel.DataAnnotations;

namespace web_app_MVC.Models
{
    public class CustomerModel
    {
        public int CustomerID { get; set; }
        [Required(ErrorMessage = "Customer name is not empty")]

        public string CustomerName { get; set; }

        public string HomeAddress { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string MobileNo { get; set; }
        public string GSTNO{ get; set; }
        public string CityName { get; set; }
        public string PinCode { get; set; }
        [Required(ErrorMessage ="Net Amount is not empty")]
        public string NetAmount { get; set; }
        [Required(ErrorMessage = "Please select one option")]

        public int UserID { get; set; }

    }
}
