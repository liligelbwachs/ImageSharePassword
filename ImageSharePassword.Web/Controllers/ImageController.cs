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
    public class ImageController : Controller
    {
        

        [HttpPost]
        public ActionResult Upload(Image image, int UserId, HttpPostedFileBase imageFile)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            imageFile.SaveAs(Path.Combine(Server.MapPath("/UploadedImages"), fileName));
            image.FileName = fileName;
            var db = new ImageDb(Properties.Settings.Default.ConStr);
            db.Add(image, UserId);
            return View(image);
        }

        public ActionResult ViewImage(int id)
        {
            var viewModel = new ViewImageViewModel();
            if (TempData["message"] != null)
            {
                viewModel.Message = (string)TempData["message"];
            }

            if (!HasPermissionToView(id))
            {
                viewModel.HasPermissionToView = false;
                viewModel.Image = new Image { Id = id };
            }
            else
            {
                viewModel.HasPermissionToView = true;
                var db = new ImageDb(Properties.Settings.Default.ConStr);
                db.IncrementViewCount(id);
                var image = db.GetById(id);
                if (image == null)
                {
                    return RedirectToAction("Index");
                }

                viewModel.Image = image;
            }

            return View(viewModel);
        }

        private bool HasPermissionToView(int id)
        {
            if (Session["allowedids"] == null)
            {
                return false;
            }

            var allowedIds = (List<int>)Session["allowedids"];
            return allowedIds.Contains(id);
        }

        [HttpPost]
        public ActionResult ViewImage(int id, string password)
        {
            
            var db = new ImageDb(Properties.Settings.Default.ConStr);
            var image = db.GetById(id);
            if (image == null)
            {
                return RedirectToAction("Index");
            }
            
            if (password != image.Password)
            {
                TempData["message"] = "Invalid password";
            }
            else
            {
                List<int> allowedIds;
                if (Session["allowedids"] == null)
                {
                    allowedIds = new List<int>();
                    Session["allowedids"] = allowedIds;
                }
                else
                {
                    allowedIds = (List<int>)Session["allowedids"];
                }

                allowedIds.Add(id);
            }

            return Redirect($"/image/viewimage?id={id}");
        }

        [Authorize]
        public ActionResult MyAccount(int id)
        {
            var db = new ImageDb(Properties.Settings.Default.ConStr);
            return View(db.AllImagesForUser(id));
        }
    }
}