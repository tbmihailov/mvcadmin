using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace MvcAdminResearch.Areas.MvcAdmin.Controllers
{
    /// <summary>
    /// Activates Controller that is passed to "customController" object in Route data
    /// example:
    /// </summary>
    public class CustomControllerFactory : IControllerFactory
    {
        public IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            object controllerTypeObj;
            requestContext.RouteData.Values.TryGetValue("controllerType", out controllerTypeObj);
            Type controllerType = controllerTypeObj as Type;
            if (controllerType == null || !controllerType.GetInterfaces().Contains(typeof(IController)))
            {
                throw new ArgumentException("controllerType is not passed to route data or is not type of IController");
            }

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