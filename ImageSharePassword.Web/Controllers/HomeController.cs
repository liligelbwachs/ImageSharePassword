using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageSharePassword.Data;
using ImageSharePassword.Web.Models;

namespace ImageSharePassword.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            var vm = new HomePageViewModel
            {
                IsAuthenticated = User.Identity.IsAuthenticated
            };

            if (User.Identity.IsAuthenticated)
            {
                var db = new AuthDb(Properties.Settings.Default.ConStr);
                var user = db.GetByEmail(User.Identity.Name);
                vm.Name = user.Name;
                vm.UserId = user.Id;
            }

            return View(vm);
        }

        
        [Authorize]
        public ActionResult AddPic(User user)
        {
            int id = user.Id;
            return View(id);
        }
    }
}