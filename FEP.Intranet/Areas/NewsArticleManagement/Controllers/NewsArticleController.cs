using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FEP.Helper;

namespace FEP.Intranet.Areas.NewsArticleManagement.Controllers
{
    public class NewsArticleController : FEPController
    {
        // GET: NewsArticleManagement/NewsArticle
        public ActionResult Index()
        {
            return View();
        }

        // GET: NewsArticleManagement/NewsArticle/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NewsArticleManagement/NewsArticle/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewsArticleManagement/NewsArticle/Create
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

        // GET: NewsArticleManagement/NewsArticle/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NewsArticleManagement/NewsArticle/Edit/5
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

        // GET: NewsArticleManagement/NewsArticle/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NewsArticleManagement/NewsArticle/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
