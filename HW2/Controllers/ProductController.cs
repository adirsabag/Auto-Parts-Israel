using HW2.Dal;
using HW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HW2.Controllers
{
    public class ProductController : Controller
    {
        // products list
        public ProductDal dalPr = new ProductDal();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Shop()
        {
            return View();
        }

        public ActionResult ServiceParts()
        {
            return View();
        }

        public ActionResult Tuning()
        {
            return View();
        }

        public ActionResult Suspension()
        {
            return View();
        }

        public ActionResult Interior()
        {
            return View();
        }

        public ActionResult Bulb()
        {
            return View();
        }

        public ActionResult Brake()
        {
            return View();
        }

        // displays product's info
        [NoDirectAccess] // prevents direct access from browser url to specific actions (in FilterConfig.cs)
        public ActionResult ProductPage(int id)
        {
            // get the desired product from the database
            var p = dalPr.Products.Where(x => x.Id == id);
            string Pname = p.Select(x => x.Name).Single().ToString();
            string Pprice = p.Select(x => x.Price).Single().ToString();
            string Pstock = p.Select(x => x.Stock).Single().ToString();

            ViewBag.Pname = Pname;
            ViewBag.Pprice = Pprice;
            ViewBag.Pstock = Pstock;

            int stk = 0;
            stk = Convert.ToInt32(Pstock);

            // checks if the product is in stock
            if (stk == 0)
            {
                TempData["out"] = "<script>alert('Product out of stock');</script>";
                return RedirectToAction("Shop", "Product");
            }
            else
            {
                Session["IdNum"] = id;
                Session["CurrentStock"] = Pstock;

                return View();
            }
        }

        // function to decrease the stock by 1 after an order has completed
        [HttpPost]
        [NoDirectAccess]
        public ActionResult Dec()
        {
            int i = 0;
            int s = 0;
            i = Convert.ToInt32(Session["IdNum"]);
            s = Convert.ToInt32(Session["CurrentStock"]);

            using (dalPr)
            {
                var p1 = dalPr.Products.Where(x => x.Id == i).FirstOrDefault();
                p1.Stock = s - 1;
                dalPr.SaveChanges();
            }

            Session["IdNum"] = null;
            Session["CurrentStock"] = null;

            return RedirectToAction("Welcome", "Home");
        }

        [NoDirectAccess]
        public ActionResult OrderProduct()
        {
            return View();
        }

        // function to renew the products stock (admin privileges)
        [NoDirectAccess]
        public ActionResult RenewStock()
        {
            using (dalPr)
            {
                var prLst = dalPr.Products.Where(x => x.Stock < 10).ToList();
                prLst.ForEach(a => a.Stock = 10);
                dalPr.SaveChanges();
            }

            return RedirectToAction("Welcome", "Home");
        }

        [NoDirectAccess]
        public ActionResult ProductsTable()
        {
            return View(dalPr.Products.ToList());
        }
    }
}