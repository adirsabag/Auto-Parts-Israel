using HW2.Dal;
using HW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HW2.Controllers
{
    public class OrderController : Controller
    {
        // orders list
        public OrderDal dalOr = new OrderDal();

        public ActionResult Index()
        {
            return View();
        }

        // completes the order and stores it in the database
        [HttpPost]
        [NoDirectAccess] // prevents direct access from browser url to specific actions (in FilterConfig.cs)
        public ActionResult Buy(Order or)
        {
            if(Session["User"] != null)
            {
                or.Username = Session["User"].ToString();
            }
            else
            {
                or.Username = "Guest";
            }
            
            if(or.SafetyDigits.Length != 3)
            {
                TempData["Digits"] = "<script>alert('Please enter 3 digits only');</script>";
                return RedirectToAction("OrderProduct", "Product");
            }

            dalOr.Orders.Add(or);
            dalOr.SaveChanges();

            return View();
        }

    }
}