using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EIPM_ECommerce_Website.Models;

namespace EIPM_ECommerce_Website.Models
{
    public class Checkout
    {
        public ProductTable Product
        {
            get;
            set;
        }

        public int Quantity
        {
            get;
            set;
        }
    }
}