using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcAdminResearch.Controllers
{
    public class RouteControllerFactory : IControllerFactory
    {
        private readonly DefaultControllerFactory Default = new DefaultControllerFactory();

        public IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            object controllerFactoryRes;
            requestContext.RouteData.Values.TryGetValue("controllerFactory", out controllerFactoryRes);
            var controllerFactory = controllerFactoryRes as IControllerFactory;
            
            var factoryToUse = (controllerFactory??Default);
            IController controller = factoryToUse.CreateController(requestContext, controllerName);
            return controller;
        }

        public System.Web.SessionState.SessionStateBehavior GetControllerSessionBehavior(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            object controllerFactoryRes;
            requestContext.RouteData.Values.TryGetValue("controllerFactory", out controllerFactoryRes);
            var controllerFactory = controllerFactoryRes as IControllerFactory;

            var sessionStateBehaviour = (controllerFactory ?? Default).GetControllerSessionBehavior(requestContext, controllerName);
            return sessionStateBehaviour;
        }

        public void ReleaseController(IController controller)
        {
            Default.ReleaseController(controller);
        }
    }
}