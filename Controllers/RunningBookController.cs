using BookManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookManager.Controllers
{
    public class RunningBookController : Controller
    {
        BookManagerEntities db = new BookManagerEntities();
        // GET: RunBook
        public ActionResult Index()
        {
            ViewBag.data = "runbook";
           // var ui = (int)Session["uid"];
            var q = db.Books.Where(x => x.ReadingStat.ReadingStatus == "Running" && x.StartDate!=null /*&& x.UserId == ui*/).ToList();

            return View(q);
        }
     
        public ActionResult Create()
        {
            ViewBag.data = "runbook";
            List<Book> BookList = db.Books.Where(x=>x.ReadingStatId==1 || x.ReadingStatId == 2).ToList();
            ViewBag.BookList = new SelectList(BookList, "BookId", "BookName");
   
                return View();
           
        }
        [HttpPost]
        public ActionResult Create(Book b)
        {
            ViewBag.data = "runbook";
            List<Book> BookList = db.Books.Where(x => x.ReadingStatId == 1 || x.ReadingStatId == 2).ToList();
            ViewBag.BookList = new SelectList(BookList, "BookId", "BookName");
            var book = db.Books.Where(x => x.BookId == b.BookId).FirstOrDefault();
            try
            {
                
                    
                    book.ReadingStatId = 4;
                    book.StartDate = b.StartDate;
                    //db.Entry(b).State = EntityState.Modified;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                
                
                return RedirectToAction("Index", "RunningBook");
            }
            catch
            {
                TempData["msg"] = "Detail isn't updated!";
                return RedirectToAction("Create", "RunningBook");
            }
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.data = "runbook";
            List<Book> BookList = db.Books.Where(x => x.ReadingStatId == 4 && x.BookId == id).ToList();
            ViewBag.BookList = new SelectList(BookList, "BookId", "BookName");
            List<ReadingStat> ReadList = db.ReadingStats.ToList();
            ViewBag.ReadList = new SelectList(ReadList, "ReadingStatId", "ReadingStatus");
            var query = db.Books.Where(m => m.BookId == id).ToList().SingleOrDefault();
            Session["bookId"] = id;
            if (query == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(query);
            }
        }

        [HttpPost]
        public ActionResult Edit(Book b)
        {
            ViewBag.data = "runbook";
            var bookId = (int)Session["bookId"];
            List<Book> BookList = db.Books.Where(x => x.ReadingStatId == 4 && x.BookId == bookId).ToList();
            ViewBag.BookList = new SelectList(BookList, "BookId", "BookName");
            List<ReadingStat> ReadList = db.ReadingStats.ToList();
            ViewBag.ReadList = new SelectList(ReadList, "ReadingStatId", "ReadingStatus");
            var book = db.Books.Where(x => x.BookId == b.BookId).FirstOrDefault();
            try
            {
                if(b.EndDate == null)
                {
                    book.StartDate = b.StartDate;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    return RedirectToAction("Index", "RunningBook");

                }
                else
                {
                    
                    if (b.EndDate!=null)
                    {
                        
                        book.StartDate = b.StartDate;
                        book.EndDate = b.EndDate;
                        book.ReadingStatId = 3;
                    }
                    var f = NotFound(book.BookId);
                    if (f)
                    {
                        CompletedBook completedBook = new CompletedBook();
                        completedBook.BookId = book.BookId;
                        db.CompletedBooks.Add(completedBook);
                    }
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    return RedirectToAction("Index", "CompletedBook");

                }
                
            }
            catch
            {
                TempData["msg"] = "Detail isn't updated!";
                return RedirectToAction("Edit", "RunningBook");
            }
        }
        [NonAction]
        public bool NotFound(int ?id)
        {
            var q = db.CompletedBooks.Where(x=>x.BookId==id).ToList();
            return (q == null);
        }
    }
}