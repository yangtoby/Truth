using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using User.Models;
using Microsoft.AspNet.Identity.Owin;
using User.Infrastructure;
using Microsoft.AspNet.Identity;

namespace User.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }
        public ActionResult LogOut()
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                AppUser user = await UserManager.FindAsync(model.Name, model.Password);
                if(user == null)
                {
                    ModelState.AddModelError("", "无效的用户名和密码");
                }
                else
                {
                    var claimsIdentity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthManager.SignOut();
                    AuthManager.SignIn(new AuthenticationProperties { IsPersistent = false }, claimsIdentity);
                   // return Redirect(");
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }
        private IAuthenticationManager AuthManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private AppUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); }
        }
    }
}