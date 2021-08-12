using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;
using EIPM_ECommerce_Website.Controllers;

namespace EIPM_Project_UnitTest
{
    [TestClass]
    public class HomeControllerUnitTest
    {
        [TestMethod]
        public void HomeController()
        {
            Assert.AreEqual("HomeController", "HomeController");
        }
        [TestMethod]
        public void Contact()
        {
            HomeController homeController = new HomeController();
            ViewResult viewResult = homeController.Contact() as ViewResult;
            Assert.IsNotNull(viewResult);
        }
        [TestMethod]
        public void About()
        {
            HomeController homeController = new HomeController();
            ViewResult viewResult = homeController.About() as ViewResult;
            Assert.IsNotNull(viewResult);
        }
    }
}
