using System.Web;
using System.Web.Mvc;

namespace CareTakerCT
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            // Register the RequireHttpsAttribute as a global filter,
            // used to confirm that requests are received over HTTPS instead of HTTP, implement the application's security 
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
