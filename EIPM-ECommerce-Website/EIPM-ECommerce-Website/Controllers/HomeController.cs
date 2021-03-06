using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EIPM_ECommerce_Website.Models;

namespace EIPM_ECommerce_Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            EIPMDBEntities DB = new EIPMDBEntities();
            var Product = from p in DB.ProductTables orderby p.Id select p;
            ViewBag.Items = Product;
            return View(Product);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public Task<ViewResult> GetData()
        {
            throw new NotImplementedException();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}