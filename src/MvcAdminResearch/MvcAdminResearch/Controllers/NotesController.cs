using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcAdminResearch.Models;

namespace MvcAdminResearch.Controllers
{
    public class NotesController : Controller
    {
        private NotesappContext db = new NotesappContext();

        //
        // GET: /Notes/

        public ActionResult Index()
        {
            return View(db.Notes.ToList());
        }

        //
        // GET: /Notes/Details/5

        public ActionResult Details(int id = 0)
        {
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        //
        // GET: /Notes/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Notes/Create

        [HttpPost]
        public ActionResult Create(Note note)
        {
            if (ModelState.IsValid)
            {
                db.Notes.Add(note);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(note);
        }

        //
        // GET: /Notes/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        //
        // POST: /Notes/Edit/5

        [HttpPost]
        public ActionResult Edit(Note note)
        {
            if (ModelState.IsValid)
            {
                db.Entry(note).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(note);
        }

        //
        // GET: /Notes/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        //
        // POST: /Notes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = db.Notes.Find(id);
            db.Notes.Remove(note);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}