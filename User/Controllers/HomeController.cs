using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace User.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
       // [Authorize]
        public ActionResult Index()
        {
            return View(GetData("Index"));
        }

        private Dictionary<string, object> GetData(string actionName)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Action", actionName);
            dic.Add("用户", HttpContext.User.Identity.Name);
            dic.Add("是否身份验证通过", HttpContext.User.Identity.IsAuthenticated);
            dic.Add("身份验证类型", HttpContext.User.Identity.AuthenticationType);
            dic.Add("是否隶属于admin", HttpContext.User.IsInRole("admin"));
            return dic;
        }
    }
}