using BookManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookManager.Controllers
{

    public class WishBookController : Controller
    {
        BookManagerEntities db = new BookManagerEntities();
        // GET: WishBook
        public ActionResult Index()
        {
            ViewBag.data = "wishbook";
            var q = db.WishLists.ToList();
            return View(q);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.data = "wishbook";
            return View();
        }

        [HttpPost]
        public ActionResult Create(WishList wl, HttpPostedFileBase file)
        {
            ViewBag.data = "wishbook";
            if (file != null)
            {
                int width = 100;
                int height = 70;
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);

                fileName = fileName + Guid.NewGuid() + extension;
                var folder = Server.MapPath("~/Uploads/");
                string path = Path.Combine(folder, fileName);
                wl.Photo = "~/Uploads/" + fileName;
                Image image = Image.FromStream(file.InputStream, true, true);
                var newImage = new Bitmap(width, height);
                using (var a = Graphics.FromImage(newImage))
                {
                    a.DrawImage(image, 0, 0, width, height);
                    newImage.Save(path);
                }
                
            }
            db.Configuration.ValidateOnSaveEnabled = false;
            db.WishLists.Add(wl);
            db.SaveChanges();
            
            return RedirectToAction("Index", "WishBook");
        }


        [HttpGet]
        public ActionResult Edit(int?id)
        {
            ViewBag.data = "wishbook";
            var query = db.WishLists.Find(id);

            if (query == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (query.Photo != null)
                    TempData["image"] = query.Photo;
               
                return View(query);
            }

           
        }

        [HttpPost]
        public ActionResult Edit(WishList wl, HttpPostedFileBase file)
        {
            ViewBag.data = "wishbook";
            var im = TempData["image"];
            try
            {

                if (file != null)
                {
                    int width = 100;
                    int height = 70;
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);

                    fileName = fileName + Guid.NewGuid() + extension;
                    var folder = Server.MapPath("~/Uploads/");
                    string path = Path.Combine(folder, fileName);
                    wl.Photo = "~/Uploads/" + fileName;
                    Image image = Image.FromStream(file.InputStream, true, true);
                    var newImage = new Bitmap(width, height);
                    using (var a = Graphics.FromImage(newImage))
                    {
                        a.DrawImage(image, 0, 0, width, height);
                        newImage.Save(path);
                    }
                }
                else if (file == null && im != null)
                {

                    wl.Photo = TempData["image"].ToString();


                    

                }

             
                db.Configuration.ValidateOnSaveEnabled = false;
                db.Entry(wl).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index", "AllBook");


            }
            catch (Exception ex)
            {
                TempData["msg"] = "Detail isn't updated!" + ex;

                return RedirectToAction("Edit", "WishBook");
            }
            
        }

        
        public ActionResult Delete(int?id)
        {
            ViewBag.data = "wishbook";

            try
            {
                var query = db.WishLists.Find(id);
                db.WishLists.Remove(query);
                db.SaveChanges();
                return RedirectToAction("Index", "WishBook");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error");
            }
        }
    }
}