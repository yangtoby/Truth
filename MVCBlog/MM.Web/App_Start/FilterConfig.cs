using MM.Web.Infrastructure;
using System.Web;
using System.Web.Mvc;

namespace MM.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomExceptionAttribute());
            filters.Add(new MvcActionAttribute());
        }
    }
}
