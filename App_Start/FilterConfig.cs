using ExpenseApplication.Filters;
using System.Web;
using System.Web.Mvc;

namespace ExpenseApplication
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
      
        }
    }
}
