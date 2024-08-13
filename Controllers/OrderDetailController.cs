using Microsoft.AspNetCore.Mvc;
using web_app_MVC.Models;

namespace web_app_MVC.Controllers
{
    public class OrderDetailController : Controller
    {
        List<OrderDetailModel> orders=new List<OrderDetailModel>()
        {
            new OrderDetailModel(){ OrderDetailID =1,OrderID =20,ProductID=2,Quantity=5,Amount=300,TotalAmount=2000,UserID=2},
            new OrderDetailModel(){ OrderDetailID =2,OrderID =12,ProductID=2,Quantity=5,Amount=200,TotalAmount=21000,UserID=1},
            new OrderDetailModel(){ OrderDetailID =3,OrderID =13,ProductID=2,Quantity=5,Amount=2400,TotalAmount=23000,UserID=3},
            new OrderDetailModel(){ OrderDetailID =4,OrderID =22,ProductID=2,Quantity=5,Amount=2300,TotalAmount=2000,UserID=4},
            new OrderDetailModel(){ OrderDetailID =5,OrderID =2,ProductID=2,Quantity=5,Amount=2200,TotalAmount=29000,UserID=5},
            new OrderDetailModel(){ OrderDetailID =6,OrderID =24,ProductID=2,Quantity=5,Amount=1200,TotalAmount=24000,UserID=6},

        };
        public IActionResult orderDetailList()
        {
            return View(orders);
        }
        public IActionResult orderDetailAddEdit()
        {
            return View();
        }
    }
}
