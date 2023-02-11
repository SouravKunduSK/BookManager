using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using BookManager.Models;

namespace BookManager.Controllers
{
    public class AuthorController : Controller
    {
        private BookManagerEntities db = new BookManagerEntities();

        // GET: Author

        
        public ActionResult Index()
        {
            ViewBag.data = "aut";

            var q = db.Authors.OrderBy(x => x.AuthorName).ToList();
            return View(q);
        }


        // GET: Author/Create
        public ActionResult Create()
        {
            ViewBag.data = "aut";
            return View();
        }

        // POST: Author/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AuthorId,AuthorName")] Author author)
        {
            ViewBag.data = "aut";
            if (ModelState.IsValid)
            {
                var ed = db.Authors.Where(x => x.AuthorName == author.AuthorName).SingleOrDefault();
                if (ed != null)
                {
                    TempData["msg"] = "Author Name has already been added! Try another...";
                    return View();
                }
                else
                {

                    db.Authors.Add(author);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
               
            }

            return View(author);

        }

        // GET: Author/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.data = "aut";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Author/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AuthorId,AuthorName")] Author author)
        {
            ViewBag.data = "aut";
            if (ModelState.IsValid)
            {
                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(author);
        }

      
        // POST: Author/Delete/5
        
        public ActionResult Delete(int?id)
        {
            ViewBag.data = "aut";
            try
            {
                Author author = db.Authors.Find(id);
                db.Authors.Remove(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return RedirectToAction("Error");
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
