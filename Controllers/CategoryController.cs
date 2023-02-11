using BookManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookManager.Controllers
{
    public class CategoryController : Controller
    {
        BookManagerEntities db = new BookManagerEntities();
        // GET: Category
        public ActionResult Index()
        {
            ViewBag.data = "cat";
            var q = db.Categories.OrderBy(x => x.CategoryName).ToList();
            return View(q);
        }

        public ActionResult Create()
        {
            ViewBag.data = "cat";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category c)
        {
            ViewBag.data = "cat";
            try
            {

                var ed = db.Categories.Where(x => x.CategoryName == c.CategoryName).SingleOrDefault();
                if (ed != null)
                {
                    TempData["msg"] = "Category Name has already been added! Try another...";
                    return View();
                }
                else
                {

                    db.Categories.Add(c);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            catch (Exception ex)
            {
                TempData["msg"] = ex;
                return View();
            }


        }

        #region Edit
        public ActionResult Edit(int? id)
        {

            ViewBag.data = "cat";
            var query = db.Categories.Where(m => m.CategoryId == id).ToList().FirstOrDefault();
            return View(query);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category c)
        {
            ViewBag.data = "cat";
            try
            {

                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Category");
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex;
            }
            return RedirectToAction("Index", "Category");
        }

        public ActionResult Delete(int? id)
        {
            ViewBag.data = "cat";
            var query = db.Categories.SingleOrDefault(m => m.CategoryId == id);
            db.Categories.Remove(query);
            db.SaveChanges();
            return RedirectToAction("Index", "Category");
        }

        //Category Ends

        #endregion
    }
}