using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EIPM_ECommerce_Website.Models;

namespace EIPM_ECommerce_Website.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Failed()
        {
            return View();
        }

        public ActionResult Buy(int Id)
        {
            EIPMDBEntities DB = new EIPMDBEntities();
                if (Session["Checkout"] == null)
                {
                    List<Checkout> checkout = new List<Checkout>();
                    checkout.Add(new Checkout { Product = DB.ProductTables.Find(Id), Quantity = 1 });
                    Session["Checkout"] = checkout;
                }
                else
                {
                    List<Checkout> checkout = (List<Checkout>)Session["Checkout"];
                    int index = isExist(Id);
                    if (index != -1)
                    {
                        checkout[index].Quantity++;
                    }
                    else
                    {
                        checkout.Add(new Checkout { Product = DB.ProductTables.Find(Id), Quantity = 1 });
                    }
                    Session["Checkout"] = checkout;
                }
            
            return RedirectToAction("Index");
        }

        public ActionResult Remove(int Id)
        {
            List<Checkout> checkout = (List<Checkout>)Session["Checkout"];
            int index = isExist(Id);
            checkout.RemoveAt(index);
            Session["Checkout"] = checkout;
            return RedirectToAction("Index");
        }

        private int isExist(int Id)
        {
            List<Checkout> checkout = (List<Checkout>)Session["Checkout"];
            for (int i = 0; i < checkout.Count; i++)
                if (checkout[i].Product.Id.Equals(Id))
                    return i;
            return -1;
        }


    }
}