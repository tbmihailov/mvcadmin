using MvcAdminResearch.Areas.MvcAdmin.Controllers;
using MvcAdminResearch.Models;
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
                "MvcAdmin_panel_navmenu",
                "MvcAdmin/Panel/NavMenu",
                new
                {
                    controller = "Panel",//not used
                    action = "NavMenu",

                    controllerFactory = new CustomControllerFactory(),//custom controller factory
                    controllerType = typeof(PanelController<NotesappContext>)//custom controller
                }
            );

            context.MapRoute(
                "MvcAdmin_panel",
                "MvcAdmin/Panel/{action}/{id}",
                new
                {
                    controller = "Panel",//not used
                    action = "Dashboard",
                    id = UrlParameter.Optional,
                    controllerFactory = new CustomControllerFactory(),//custom controller factory
                    controllerType = typeof(PanelController<NotesappContext>)//custom controller
                }
            );

            context.MapRoute(
                "MvcAdmin_default",
                "MvcAdmin",
                new
                {
                    controller = "Panel",
                    action = "Dashboard",
                    id = UrlParameter.Optional,
                    controllerFactory = new CustomControllerFactory(),//custom controller factory
                    controllerType = typeof(PanelController<NotesappContext>)//custom controller
                }
            );

            context.MapRoute(
                "MvcAdmin_Generic",
                "MvcAdmin/m/{controller}/{action}/{id}",
                new
                {
                    controllerFactory = new GenericControllerFactory(),//custom controller factory2
                    dataContextType = typeof(MvcAdminResearch.Models.NotesappContext),//Data context type  
                    controller = "Note",
                    action = "Index",
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
