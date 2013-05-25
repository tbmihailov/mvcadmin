using MvcAdminResearch.Areas.MvcAdmin.Models;
using MvcAdminResearch.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Collections;

namespace MvcAdminResearch.Areas.MvcAdmin.Controllers
{
    /// <summary>
    /// Controller for MvcAdmin main panel
    /// </summary>
    [Authorize]
    public class PanelController<TContext> : Controller where TContext:DbContext, new()
    {
        Type _contextType;
        List<Type> _contextModels;
        TContext _context;
    
        public PanelController()
        {
            _context = new TContext();
            _contextType = _context.GetType();
            _contextModels = _contextType.GetModelCollectionTypesDbContext();
        }

        public ActionResult Dashboard()
        {
            var modelInfos = new List<ModelInfoDto>();
            
            //load records count
            foreach (var model in _contextModels)
            {
                var query = _context.Set(model).AsQueryable();
                dynamic dynamicQuery = query;
                int recordsCount = Queryable.Count(dynamicQuery);
                modelInfos.Add(new ModelInfoDto { 
                    ControllerName = model.Name,
                    DisplayName = model.Name,
                    RecordsCount = recordsCount,
                });
            }
            modelInfos = modelInfos.OrderBy(mi => mi.DisplayName).ToList();
            ViewBag.ModelInfos = modelInfos;

            return View();
        }
    }
}
