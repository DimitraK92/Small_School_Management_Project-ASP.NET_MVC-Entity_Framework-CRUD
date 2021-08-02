using System.Web;
using System.Web.Mvc;

namespace MVC_Assignment_June_2021
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
