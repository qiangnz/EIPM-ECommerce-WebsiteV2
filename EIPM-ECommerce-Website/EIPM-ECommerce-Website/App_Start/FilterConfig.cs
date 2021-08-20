using System.Web;
using System.Web.Mvc;

namespace EIPM_ECommerce_Website
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
