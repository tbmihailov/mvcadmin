using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcAdminResearch.Models;
using MvcAdminResearch.Helpers;
using MvcAdminResearch.Areas.MvcAdmin.Models;

namespace MvcAdminResearch.Areas.MvcAdmin.Controllers
{
    //[Authorize]
    public class GenericController<TModel, TContext> : Controller
        where TModel : class, new()
        where TContext : DbContext, new()
    {
        private TContext db = new TContext();
        DataContextHost host = new DataContextHost(typeof(TModel), typeof(TContext));
        //
        // GET: /Generic/

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            //Set model infos for nav menu for all actions
            SetModelInfosForNavMenuToViewBag();
        }

        public ActionResult Index()
        {
            return View(db.Set<TModel>().ToList());
        }

        //
        // GET: /Generic/Details/5

        public ActionResult Details(int id = 0)
        {
            var note = db.Set<TModel>().Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        //
        // GET: /Generic/Create

        public ActionResult Create()
        {
            FillRelatedModelsData();

            TModel item = new TModel();
            return View(item);
        }

        private void FillRelatedModelsData()
        {
            var fkProperties = host.ModelProperties.Where(p => p.IsForeignKey);
            foreach (var prop in fkProperties)
            {
                var relatedModel = host.RelatedProperties[prop.Name];
                ViewData[relatedModel.PropertyName] = new SelectList(db.Set(relatedModel.Type), relatedModel.PrimaryKey, relatedModel.DisplayPropertyName);
            }
        }

        //
        // POST: /Generic/Create

        [HttpPost]
        public ActionResult Create(TModel note)
        {
            if (ModelState.IsValid)
            {
                db.Set<TModel>().Add(note);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            FillRelatedModelsData();
            return View(note);
        }

        //
        // GET: /Generic/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var note = db.Set<TModel>().Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }

            FillRelatedModelsData();
            return View(note);
        }

        //
        // POST: /Generic/Edit/5

        [HttpPost]
        public ActionResult Edit(TModel note)
        {
            if (ModelState.IsValid)
            {
                db.Entry(note).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            FillRelatedModelsData();
            return View(note);
        }

        //
        // GET: /Generic/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var note = db.Set<TModel>().Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }

            return View(note);
        }

        //
        // POST: /Generic/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var note = db.Set<TModel>().Find(id);
            db.Set<TModel>().Remove(note);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Returns ModelInfos - Info about the models that are available in the undelying context
        /// </summary>
        /// <returns></returns>
        private List<ModelInfoDto> GetModelInfosForNavMenu()
        {
            var modelInfos = new List<ModelInfoDto>();
            string controller = ControllerContext.RouteData.Values["controller"] != null ? ControllerContext.RouteData.Values["controller"].ToString() : "";

            var contextModels = db.GetType().GetModelCollectionTypesDbContext();
            foreach (var model in contextModels)
            {
                string controllerName = model.Name;
                bool isSelected = controllerName == controller;
                var modelInfo = new ModelInfoDto
                {
                    ControllerName = controllerName,
                    ActionName = "Index",
                    DisplayName = controllerName,
                    IsSelected = isSelected
                };

                modelInfos.Add(modelInfo);
            }
            modelInfos = modelInfos.OrderBy(mi => mi.DisplayName).ToList();
            return modelInfos;
        }

        public void SetModelInfosForNavMenuToViewBag()
        {
            var modelInfos = GetModelInfosForNavMenu();
            ViewBag.ModelInfos = modelInfos;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }



}