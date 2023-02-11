using BookManager.Models;
using System;
using System.Collections.Generic;
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
            return View();
        }

        // POST: AllBook/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AllBook/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AllBook/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: AllBook/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

       
    }
}
