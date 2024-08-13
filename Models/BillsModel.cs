using System.ComponentModel.DataAnnotations;

namespace web_app_MVC.Models
{
    public class BillsModel
    {
       
        public int? BillID { get; set; }
        [Required(ErrorMessage ="Bill number is not empty")]
        public string BillNumber { get; set; }
        [Required(ErrorMessage = "Bill date is not empty")]

        public string BillDate { get; set; }
        [Required(ErrorMessage = "Compulsary select one option")]
        public int OrderID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        [Required(ErrorMessage = "Compulsary select one option")]

        public int UserID { get; set; }




    }
}
