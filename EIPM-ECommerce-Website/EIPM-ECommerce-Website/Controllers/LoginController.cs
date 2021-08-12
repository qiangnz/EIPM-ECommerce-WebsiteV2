using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EIPM_ECommerce_Website.Models;

namespace EIPM_ECommerce_Website.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            UserLogin login = new UserLogin();
            return View();
        }
        //[HttpPost]
        //public ActionResult Authenticate([Bind(Include = "userName,password")] UserLogin LoginDetails)
        //{
        //    // Read credentials from file
        //    string[] credentials = System.IO.File.ReadAllLines("login.txt");
        //    var credUsername = credentials[0];
        //    var credPassword = credentials[1];
        //    if (LoginDetails.username == credUsername && LoginDetails.password == credPassword)
        //    {
        //        return RedirectToAction("Index", "Checkout");
        //    }
        //    return View("Index");
        //}

        [HttpPost]
        public ActionResult Authenticate(UserLogin LoginDetails)
        {
            // Read credentials from file
            string[] credentials = System.IO.File.ReadAllLines("login.txt");
            var credUsername = credentials[0];
            var credPassword = credentials[1];
            if (LoginDetails.username == credUsername && LoginDetails.password == credPassword)
            {
                return View("Index", "Checkout");
            }
            return View("Index","Home");
        }

    }
}