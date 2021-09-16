using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EIPM_ECommerce_Website.Models;
using System.IO;
using System.Net;

namespace EIPM_ECommerce_Website.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Success(string paidAmount)
        {
            ViewData.Add("PaidAmount", paidAmount);
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

        public ActionResult AddQuantity(int Id)
        {
            List<Checkout> checkout = (List<Checkout>)Session["Checkout"];
            int index = isExist(Id);
            checkout[index].Quantity++;
            return RedirectToAction("Index");
        }

        public ActionResult MinusQuantity(int Id)
        {
            List<Checkout> checkout = (List<Checkout>)Session["Checkout"];
            int index = isExist(Id);
            if (checkout[index].Quantity == 1 )
            {
                checkout.RemoveAt(index);
                Session["Checkout"] = checkout;
                return RedirectToAction("Index");

            }
            else
            {
                checkout[index].Quantity--;
            }
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

        public ActionResult Payment(Decimal amount)
        {

            //Save Transaction Info into Database
            EIPMDBEntities DB = new EIPMDBEntities();
            TransactionTable transactionTable = new TransactionTable();
            transactionTable.Date = DateTime.Now;
            transactionTable.TAmount = amount;
            //transactionTable.TURL = link;
            DB.TransactionTables.Add(transactionTable);
            DB.SaveChanges();
            //Database

            string results = "";
            string link = "";

            //API Variables
            int APIPayAmount = int.Parse(((amount.ToString()).Replace(".", "")));
            string APICurrencyCode = "NZD";
            string APIEntityId = "71db68ad-a2e5-4f32-a771-688aa8ba0b9a";
            string APIPayMentContractId = "dbc21671-f4e7-4902-80b5-7b262a201408";
            string APIMerchant_ref = "EIPMOrderRef000" + transactionTable.TId;
            string returnURL= HttpContext.Request.Url.AbsoluteUri.Remove(HttpContext.Request.Url.AbsoluteUri.IndexOf(HttpContext.Request.Url.AbsolutePath));
            string APIReturnURL = returnURL + "/Checkout/PaymentResult/";
            string APIInteractionType="HPP";


            //Post Request to API
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://cst.test-gsc.vfims.com/oidc/checkout-service/v2/checkout");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Authorization", "Basic NDkwOGZkYjYtZTM1OS00NTNkLWJkYTYtY2E0NDY3NzExYThjOkZnaXBkZ29PUlVCWFlzRk9lRlB4TUZOeERVcVNtcmRSck1PTw==");
            using (StreamWriter streamWriter=new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json =
                    "{\n"
                    + "\t\"amount\" :" + APIPayAmount + ",\n"
                    + "\t\"currency_code\" : \"" + APICurrencyCode + "\" ,\n"
                    + "\t\"entity_id\" : \"" + APIEntityId + "\",\n"
                    + "\t\"configurations\": {\n"
                    + "\t\t\"card\": { \n"
                    + "\t\t\t\"payment_contract_id\": \"" + APIPayMentContractId + "\"\n"
                    + "\t\t} \n"
                    + "\t },\n"
                    + "\t\"merchant_reference\": \"" + APIMerchant_ref + "\",\n"
                    + "\t\"return_url\" : \"" + APIReturnURL + "\",\n"
                    + "\t\"interaction_type\": \"" + APIInteractionType + "\"\n"
                    + "}";
                streamWriter.Write(json);
            }
            try
            {
                // API causes an Exception to be thrown when there is a problem with the json format
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    results = streamReader.ReadToEnd().ToString();

                }
                httpResponse.Close();
            }
            catch (Exception exception)
            {
                
                string str = ((exception.Message.Replace(" ", "_")).Replace("(", "__")).Replace(")", "__");
                return Redirect(returnURL + "/Checkout/Failed?" + exception.Message);
            }

            string[] arrays = results.Split('\"');
            foreach(string getlink in arrays)
            {
                if(getlink.Contains("https://cst.checkout.vficloud.net/v2/checkout") == true)
                {
                    link = getlink;
                }
            }

            return Redirect(link);
        }

        public ActionResult PaymentResult()
        {
            string[] RetJson = HttpContext.Request.Url.AbsoluteUri.Split('?')[1].Split('&');
            string CheckoutID = RetJson[0].Replace("checkout_id=", "");
            string TransactionID = RetJson[1].Replace("transaction_id=", "");


            string results = "";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://cst.test-gsc.vfims.com/oidc/checkout-service/v2/checkout/" + CheckoutID);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", "Basic NDkwOGZkYjYtZTM1OS00NTNkLWJkYTYtY2E0NDY3NzExYThjOkZnaXBkZ29PUlVCWFlzRk9lRlB4TUZOeERVcVNtcmRSck1PTw==");

            WebResponse httpResponse = httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                results = streamReader.ReadToEnd().ToString();
            string[] result = results.Split(',');
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            
            string _URL = HttpContext.Request.Url.AbsoluteUri.Remove(HttpContext.Request.Url.AbsoluteUri.IndexOf(HttpContext.Request.Url.AbsolutePath));

            if (results.Contains("\"TRANSACTION_SUCCESS\""))
            {
                string amount = "";
                foreach (string re in result)
                {
                    if (re.Contains("\"amount\":"))
                    {
                        amount = re;
                    }
                }
                string[] array = amount.Split(':');
                amount = array[1];
                amount.Replace("\"", "");
                amount.Replace(",", "");
                amount = amount.Substring(0, (amount.Length - 2)) + "." + amount.Substring(amount.Length - 2);
                List<Checkout> checkout = (List<Checkout>)Session["Checkout"];
                checkout.Clear();
                return RedirectToAction("Success", "Checkout");
                //return RedirectToAction("Success", "Checkout", new { paidAmount = amount });
            }
            else
            {
                return RedirectToAction("Failed", "Checkout");
            }
        }
    }
}