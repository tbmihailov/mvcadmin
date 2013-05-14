using MvcAdminResearch.Areas.MvcAdmin.Controllers;
using System.Web.Mvc;
using System.Web.Optimization;

namespace MvcAdminResearch.Areas.MvcAdmin
{
    public class MvcAdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MvcAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            RegisterRoutes(context);
            RegisterBundles();
        }

        private static void RegisterRoutes(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MvcAdmin_models",
                "MvcAdmin/Models/{controller}/{action}/{id}",
                new
                {
                    controllerFactory = new GenericControllerFactory(),//custom controller factory
                    dataContextType = typeof(MvcAdminResearch.Models.NotesappContext),//Data context type  
                    controller = "Note",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );

            context.MapRoute(
                "MvcAdmin_default",
                "MvcAdmin/{controller}/{action}/{id}",
                new
                {
                    controller = "Panel",
                    action = "Dashboard",
                    id = UrlParameter.Optional
                }
            );
        }

        private void RegisterBundles()
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }  
    }
}
