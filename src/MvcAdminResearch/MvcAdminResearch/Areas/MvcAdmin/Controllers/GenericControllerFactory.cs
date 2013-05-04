using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using MvcAdminResearch.Helpers;

namespace MvcAdminResearch.Areas.MvcAdmin.Controllers
{
    public class GenericControllerFactory : IControllerFactory
    {
        public IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            object dataContextRes;
            requestContext.RouteData.Values.TryGetValue("dataContextType", out dataContextRes);
            var dataContextType = dataContextRes as Type;
            if (dataContextType == null)
            {
                throw new ArgumentException("dataContext is not passed to route data or is not of type Type");
            }

            var modelType = dataContextType.GetModelCollectionTypesDbContext().FirstOrDefault(t=>t.Name.Equals(controllerName, StringComparison.OrdinalIgnoreCase));
            if(modelType == null){
                throw new ArgumentException(string.Format("dynamic controller resolution: NO model type with name '{0}'", controllerName));
            }
            
            var controllerType = typeof(GenericController<,>).MakeGenericType(modelType, dataContextType);
            IController controller = (IController)Activator.CreateInstance(controllerType);
            return controller;
        }

        public System.Web.SessionState.SessionStateBehavior GetControllerSessionBehavior(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            return SessionStateBehavior.Default;
        }

        public void ReleaseController(IController controller)
        {
            throw new NotImplementedException();
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return true;
        }
    }
}