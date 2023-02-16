using BookManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookManager.Controllers
{
    public class LendBookController : Controller
    {
        BookManagerEntities db = new BookManagerEntities();
        // GET: LendBook
        public ActionResult Index()
        {
            ViewBag.data = "lendbook";
            var q = db.Lendings.ToList();
            return View(q);
            
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.data = "lendbook";
          
            List<Book> BookList = db.Books.Where(x => x.BookStatusId != 2).ToList();
            ViewBag.BookList = new SelectList(BookList, "BookId", "BookName");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Lending ld)
        {
            ViewBag.data = "lendbook";
           
            List<Book> BookList = db.Books.Where(x=>x.BookStatusId!=2).ToList();
            ViewBag.BookList = new SelectList(BookList, "BookId", "BookName");
            var q = db.Books.Find(ld.BookId);
            if(q!=null)
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                q.BookStatusId = 2;
                db.Books.AddOrUpdate(q);
                db.Lendings.Add(ld);
                db.SaveChanges();
                return RedirectToAction("Index","LendBook");
            }
            else
            {
                return View();
            }
           
        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            ViewBag.data = "lendbook";
            List<Book> BookList = db.Books.ToList();
            ViewBag.BookList = new SelectList(BookList, "BookId", "BookName");
            var query = db.Lendings.Find(id);
            Session["bookId"] = query.BookId;

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
        public ActionResult Edit(Lending l)
        {
            ViewBag.data = "lendbook";
            List<Book> BookList = db.Books.ToList();
            ViewBag.BookList = new SelectList(BookList, "BookId", "BookName");

            try
            {
                int bookId = (int)Session["bookId"];
                var book = db.Books.Find(bookId);
                var b = db.Books.Where(x => x.BookId == l.BookId).FirstOrDefault();
                if (l.BookId==bookId && l.ReturnDate==null && book.BookStatusId == 2) 
                {
                    db.Configuration.ValidateOnSaveEnabled = false; 
                    db.Entry(l).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "LendBook");
                }
                else if (l.BookId == bookId && l.ReturnDate != null && book.BookStatusId == 2)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    book.BookStatusId = 1;
                    db.Entry(book).State=EntityState.Modified;
                    db.Entry(l).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "LendBook");
                }

                else if(l.BookId != bookId && (b.BookStatusId) ==1)
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    book.BookStatusId = 1;
                    b.BookStatusId = 2;
                    db.Entry(b).State = EntityState.Modified;
                    db.Entry(book).State = EntityState.Modified;
                 
                    db.Entry(l).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "LendBook");
                }

                else
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Entry(l).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "LendBook");
                }
               

              


            }
            catch (Exception ex)
            {
                TempData["msg"] = "Detail isn't updated!" + ex;

                return RedirectToAction("Edit", "LendBook");
            }
          
        }


        public ActionResult Delete(int?id)
        {
            ViewBag.data = "lendbook";
            try
            {
                var query = db.Lendings.Find(id);
                db.Lendings.Remove(query);
                db.SaveChanges();
                return RedirectToAction("Index", "LendBook");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error");
            }
        }
    }
}