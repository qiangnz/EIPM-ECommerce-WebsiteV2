using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EIPM_ECommerce_Website.Models;


namespace EIPM_Project_UnitTest
{
    [TestClass]
    public class ModelUnitTest
    {
        [TestMethod]
        public void TestProductId()
        {
            var product = new ProductTable();
            product.Id = 099;
            Assert.AreEqual(099, product.Id);
        }
        [TestMethod]
        public void TestProductName()
        {
            var product = new ProductTable();
            product.Name = "Jacket";
            Assert.AreEqual("Jacket", product.Name);
        }
        [TestMethod]
        public void TestProductPrice()
        {
            var product = new ProductTable();
            product.Price =52.99M;
            Assert.AreEqual(52.99M, product.Price);
        }
        [TestMethod]
        public void TestProductImgRef()
        {
            var product = new ProductTable();
            product.Name = "Product099.jpg";
            Assert.AreEqual("Product099.jpg", product.Name);
        }
        [TestMethod]
        public void TestTransactionTId()
        {
            var transaction = new TransactionTable();
            transaction.TId = 098;
            Assert.AreEqual(098, transaction.TId);
        }
        [TestMethod]
        public void TestTransactionTAmount()
        {
            var transaction = new TransactionTable();
            transaction.TAmount = 66.99M;
            Assert.AreEqual(66.99M, transaction.TAmount);
        }
        [TestMethod]
        public void TestTransactionDate()
        {
            var transaction = new TransactionTable();
            DateTime d= new DateTime(2021, 08, 12, 11, 00, 00);
            transaction.Date = d;
            Assert.AreEqual(d, transaction.Date);
        }
        [TestMethod]
        public void TestTransactionTURL()
        {
            var transaction = new TransactionTable();
            transaction.TURL = "https://eipm/home/Index";
            Assert.AreEqual("https://eipm/home/Index", transaction.TURL);
        }
    }
}
