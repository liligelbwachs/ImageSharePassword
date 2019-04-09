using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ImageSharePassword.Data;

namespace ImageSharePassword.Web.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User user, string password)
        {
            var db = new AuthDb(Properties.Settings.Default.ConStr);
            db.AddUser(user, password);
            return Redirect("/");
        }

        public ActionResult LogIn()
        {
            return View();
        }
        

        [HttpPost]
        public ActionResult LogIn(string email, string password)
        {
            var db = new AuthDb(Properties.Settings.Default.ConStr);
            var user = db.Login(email, password);
            if (user == null)
            {
                TempData["message"] = "Invalid login attempt";
                return Redirect("/account/login");
            }

            FormsAuthentication.SetAuthCookie(email, true);
            return Redirect("/");
        }
    }

}