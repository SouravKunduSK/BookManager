using BookManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace BookManager.Controllers
{
    public class CompletedBookController : Controller
    {
        BookManagerEntities db = new BookManagerEntities();
        // GET: CompletedBook
        public ActionResult Index()
        {
            ViewBag.data = "combook";
            var q = db.CompletedBooks.OrderByDescending(x=>x.EndDate).ToList();
            return View(q);
        }
        public ActionResult Completed()
        {
            
            //var ui = (int)Session["uid"];
            var q = db.Books.Where(x => x.ReadingStat.ReadingStatus == "Completed" /*&& x.UserId == ui*/).ToList();
            return View(q);
        }
        public ActionResult Detail(int? id)
        {
            ViewBag.data = "combook";
            //var ui = (int)Session["uid"];
            var q = db.Books.Find(id);
            return View(q);
        }
    }
}