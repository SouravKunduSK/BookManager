using BookManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookManager.Controllers
{
    public class RunningBookController : Controller
    {
        BookManagerEntities db = new BookManagerEntities();
        // GET: RunningBook
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(int id)
        {
            return View();
        }


        [HttpGet]
        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id)
        {
            return View();
        }


        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}