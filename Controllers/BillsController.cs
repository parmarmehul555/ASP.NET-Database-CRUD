using Microsoft.AspNetCore.Mvc;
using web_app_MVC.Models;

namespace web_app_MVC.Controllers
{
    public class BillsController : Controller
    {
        List<BillsModel> bills = new List<BillsModel>()
        {
            new BillsModel(){ BillID =1,BillNumber="123",BillDate="20-9-2004",OrderID=2,TotalAmount=2000,Discount =2,NetAmount=2400,UserID=2},
            new BillsModel(){ BillID =2,BillNumber="123",BillDate="20-9-2004",OrderID=2,TotalAmount=2000,Discount =2,NetAmount=2400,UserID=2},
            new BillsModel(){ BillID =3,BillNumber="123",BillDate="20-9-2004",OrderID=2,TotalAmount=2000,Discount =2,NetAmount=2400,UserID=2},
            new BillsModel(){ BillID =4,BillNumber="123",BillDate="20-9-2004",OrderID=2,TotalAmount=2000,Discount =2,NetAmount=2400,UserID=2},
            new BillsModel(){ BillID =5,BillNumber="123",BillDate="20-9-2004",OrderID=2,TotalAmount=2000,Discount =2,NetAmount=2400,UserID=2},
            new BillsModel(){ BillID =6,BillNumber="123",BillDate="20-9-2004",OrderID=2,TotalAmount=2000,Discount =2,NetAmount=2400,UserID=2},
            new BillsModel(){ BillID =7,BillNumber="123",BillDate="20-9-2004",OrderID=2,TotalAmount=2000,Discount =2,NetAmount=2400,UserID=2},

        };
        public IActionResult Save(BillsModel billsModel)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("billAddEdit");
            }
            else { 
                return View("billAddEdit",billsModel);
            }
           
        }
        public IActionResult billList()
        {
            return View(bills);
        }
        public IActionResult billAddEdit()
        {
            return View();
        }
    }
}
