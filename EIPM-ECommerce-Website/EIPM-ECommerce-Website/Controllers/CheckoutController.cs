using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
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


        public ActionResult TestAPI(Decimal amount)
        { 
            string redirectPage = "/Cart/PurchaseStatus";


            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://cst.test-gsc.vfims.com/oidc/checkout-service/v2/checkout");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Authorization", "Basic ZTQ0ODFlYTItOTQ4OS00NTUyLTk1YjItNDI1ZDUzNjcwODU4OlZnWmlndU9rTGNidk1JWlRlcU9OWElBQUtleEVHTmh0YW9wZQ==");
            // ----------------------------------------------------------------------------------------------------------------------------------------
            string url = HttpContext.Request.Url.AbsoluteUri;
            //Debug.WriteLine("\nurl is: " + url);
            url = url.Remove(url.IndexOf(HttpContext.Request.Url.AbsolutePath));
            //Debug.WriteLine("url cutoff is: " + url);
            // -----------------------------------------------------------------------------------------------------------------
            // API POST
            string s = amount.ToString();
            s= s.Replace(".", "");
            int p = int.Parse(s);
            EIPMDBEntities db = new EIPMDBEntities();
            
            int f =3 ;
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json =
                    "{\n"
                        + "\t\"amount\" :" + p + ",\n"
                        + "\t\"currency_code\" : \"NZD\" ,\n"
                        + "\t\"entity_id\" : \"71db68ad-a2e5-4f32-a771-688aa8ba0b9a\",\n"
                        + "\t\"configurations\": {\n"
                            + "\t\t\"card\": { \n"
                                + "\t\t\t\"payment_contract_id\": \"dbc21671-f4e7-4902-80b5-7b262a201408\"\n"
                            + "\t\t} \n"
                        + "\t },\n"
                        + "\t\"merchant_reference\": \"TestRef00" + f.ToString() + "\",\n"
                        + "\t\"return_url\" : \"" + url + redirectPage +"\",\n"
                        + "\t\"interaction_type\": \"HPP\"\n"
                    + "}";
                //Debug.WriteLine("JSON REQUEST - POST (send): " + json);
                streamWriter.Write(json);
            }
            string results = "";

            try
            {
                // API causes an Exception to be thrown when there is a problem with the json format
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    results = result.ToString();
                    //Debug.WriteLine("JSON REQUEST - POST (reply): " + results);
                    //Debug.WriteLine(Url.ToString());
                }
            }
            catch (Exception ECE)
            {
                //Debug.WriteLine("Error Code exception caught, redirecting");
                //Debug.WriteLine("Error: " + ECE.Message + "\n\n" + ECE.StackTrace);
                //Debug.WriteLine(ECE);
                string str = ECE.Message.Replace(" ", "_");
                str = str.Replace("(", "__");
                str = str.Replace(")", "__");
                return RedirectToAction("Index", "Cart", new { Error = str });
            }

            string link = "https://www.google.com";
            string[] arrays = results.Split('\"');
            foreach (string e in arrays)
            {
                //Debug.WriteLine(e);
                if (e.Contains("https://cst.checkout.vficloud.net/v2/checkout") == true)
                {
                    link = e;
                }
            }
            TransactionTable tt = new TransactionTable();
            tt.TAmount = amount;
            tt.TURL = link;
            tt.Date = DateTime.Now;
            db.TransactionTables.Add(tt);
            db.SaveChanges();
            return Redirect(link);
        }


        // example function to GET the api request
        // you can use your own one if you want. I made this function to help you understand in case you get confused.
        public string PurchaseStatus()
        {

            // get the URL and store as 'url'
            string url = HttpContext.Request.Url.AbsoluteUri;

            // you will need to put the url for the API's GET  request here :
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("API GET URL"+"id from url");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", "Basic ZTQ0ODFlYTItOTQ4OS00NTUyLTk1YjItNDI1ZDUzNjcwODU4OlZnWmlndU9rTGNidk1JWlRlcU9OWElBQUtleEVHTmh0YW9wZQ==");
            // ----------------------------------------------------------------------------------------------------------------------------------------

            // This line will send the json
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                // example json request
                string json = "";
            }

            string results = "";

            // use this to retrieve the json
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                results = result.ToString();
            }
            int code = 200;
            
            // anlyse result here

            if(code==200)
            {
                // if success then go to success page
            }
            else
            {
                // if fail go to fail page...
            }
            return "This is a placeholder page";
        }
    }
}