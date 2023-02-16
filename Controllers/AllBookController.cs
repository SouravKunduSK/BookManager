using BookManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BookManager.Controllers
{
    public class AllBookController : Controller
    {

        BookManagerEntities db = new BookManagerEntities();
        // GET: AllBook
        public ActionResult Index()
        {
            ViewBag.data = "allbook";
            //var ui = (int)Session["uid"];
            var q = db.Books.Where(x => x.BookStat.Status != "Buyable" && x.ReadingStat.ReadingStatus != null
                                    /*&& x.UserId == ui*/).ToList();
            return View(q);
           
        }


        // GET: AllBook/Create
        public ActionResult Create()
        {
            ViewBag.data = "allbook";
            //var ui = (int)Session["uid"];
            List<Category> CategoryList = db.Categories.OrderBy(x => x.CategoryName).ToList();
            ViewBag.CategoryList = new SelectList(CategoryList, "CategoryId", "CategoryName");
            List<Author> AuthorList = db.Authors.OrderBy(x => x.AuthorName).ToList();
            ViewBag.AuthorList = new SelectList(AuthorList, "AuthorId", "AuthorName");
            List<BookStat> SatList = db.BookStats.ToList();
            ViewBag.SatList = new SelectList(SatList, "BookStatusId", "Status");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Book b, HttpPostedFileBase file)
        {
            ViewBag.data = "allbook";
            //var ui = (int)Session["uid"];
            //var username = Session["username"].ToString();
            //Book book = new Book();
            List<Category> CategoryList = db.Categories.OrderBy(x => x.CategoryName).ToList();
            ViewBag.CategoryList = new SelectList(CategoryList, "CategoryID", "CategoryName");
            List<Author> AuthorList = db.Authors.OrderBy(x => x.AuthorName).ToList();
            ViewBag.AuthorList = new SelectList(AuthorList, "AuthorId", "AuthorName");
            List<BookStat> SatList = db.BookStats.ToList();
            ViewBag.SatList = new SelectList(SatList, "BookStatusId", "Status");
            b.ReadingStatId = 1;

            if(file!=null)
            {
                int width = 100;
                int height = 70;
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);

                fileName = fileName + Guid.NewGuid() + extension;
                var folder = Server.MapPath("~/Uploads/");
                string path = Path.Combine(folder, fileName);
                b.Photo = "~/Uploads/" + fileName;
                Image image = Image.FromStream(file.InputStream, true, true);
                var newImage = new Bitmap(width, height);
                using (var a = Graphics.FromImage(newImage))
                {
                    a.DrawImage(image, 0, 0, width, height);
                    newImage.Save(path);
                }
            }
            if (b.CategoryId == null || b.AuthorId == null || b.BookName == null || b.BookStatusId == null || b.BuyingDate == null)
            {
                TempData["msg"] = "Book isn't Added!" +
                   "You must fill all the '*' sections..";
                return RedirectToAction("Create", "Book");
            }

            db.Books.Add(b);
            db.SaveChanges();

            return RedirectToAction("Index", "AllBook");

        }



        // GET: AllBook/Edit/5
        public ActionResult Edit(int?id)
        {
            ViewBag.data = "allbook";
            //var ui = (int)Session["uid"];
            Session["bookid"] = id;
            List<Category> CategoryList = db.Categories.OrderBy(x => x.CategoryName).ToList();
            ViewBag.CategoryList = new SelectList(CategoryList, "CategoryId", "CategoryName");
            List<Author> AuthorList = db.Authors.OrderBy(x => x.AuthorName).ToList();
            ViewBag.AuthorList = new SelectList(AuthorList, "AuthorId", "AuthorName");
            List<BookStat> SatList = db.BookStats.ToList();
            ViewBag.SatList = new SelectList(SatList, "BookStatusId", "Status");
            List<ReadingStat> ReadList = db.ReadingStats.ToList();
            ViewBag.ReadList = new SelectList(ReadList, "ReadingStatId", "ReadingStatus");
            var query = db.Books.Where(m => m.BookId == id).ToList().SingleOrDefault();

            if (query == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (query.Photo != null)
                    TempData["image"] = query.Photo;
                if (query.BuyingDate != null)
                    Session["buydate"] = query.BuyingDate;
                return View(query);
            }
        }

        // POST: AllBook/Edit/5
        [HttpPost]
        public ActionResult Edit(Book b, HttpPostedFileBase file)
        {
            ViewBag.data = "allbook";

            //var username = Session["username"].ToString();
            var im = TempData["image"];
            //var ui = (int)Session["uid"];
            List<Category> CategoryList = db.Categories.OrderBy(x => x.CategoryName).ToList();
            ViewBag.CategoryList = new SelectList(CategoryList, "CategoryId", "CategoryName");
            List<Author> AuthorList = db.Authors.OrderBy(x => x.AuthorName).ToList();
            ViewBag.AuthorList = new SelectList(AuthorList, "AuthorId", "AuthorName");
            List<BookStat> SatList = db.BookStats.ToList();
            ViewBag.SatList = new SelectList(SatList, "BookStatusId", "Status");
            List<ReadingStat> ReadList = db.ReadingStats.ToList();
            ViewBag.ReadList = new SelectList(ReadList, "ReadingStatId", "ReadingStatus");
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
                    b.Photo = "~/Uploads/" + fileName;
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

                    b.Photo = TempData["image"].ToString();


                    //b.Photo = bookdetail.Photo;

                }

                if (b.BuyingDate == null)
                {
                    b.BuyingDate = (DateTime)Session["buydate"];
                }

                if (b.ReadingStatId == 4)
                {
                    b.StartDate = DateTime.Today;
                }
                
                db.Entry(b).State = EntityState.Modified;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                return RedirectToAction("Index", "AllBook");


            }
            catch (Exception ex)
            {
                TempData["msg"] = "Detail isn't updated!" + ex;

                return RedirectToAction("Edit", "AllBook");
            }
        }

        // GET: AllBook/Delete/5
        public ActionResult Delete(int?id)
        {
            ViewBag.data = "allbook";
            try
            {
                var query = db.Books.SingleOrDefault(m => m.BookId == id);
                db.Books.Remove(query);
                db.SaveChanges();
                return RedirectToAction("Index", "AllBook");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error");
            }
        }
      

 

    }
}
