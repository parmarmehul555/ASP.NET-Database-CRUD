//using System.ComponentModel.DataAnnotations
using System.ComponentModel.DataAnnotations;

namespace web_app_MVC.Models
{
    public class OrderModel
    {
      
        public int? OrderID{ get; set; }
        public string OrderDate { get; set; }
        [Required(ErrorMessage = "please choose approproate option")]

        public int CustomerID { get; set; }
        [Required(ErrorMessage ="Enter payment mode" )]
        public string PaymentMode { get; set; }
        [Required(ErrorMessage ="please Enter Total amount")]
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        [Required(ErrorMessage = "please choose approproate option")]

        public int UserID { get; set; }
       
    }
}
