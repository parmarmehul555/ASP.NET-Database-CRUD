using System.ComponentModel.DataAnnotations;

namespace web_app_MVC.Models
{
    public class OrderDetailModel
    {
       
        public int? OrderDetailID { get; set; }

        [Required(ErrorMessage = "Please choose an appropriate option")]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "Please choose an appropriate option")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Please enter the quantity")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please enter the amount")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Please enter the total amount")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Please choose an appropriate option")]
        public int UserID { get; set; }
    }
}
