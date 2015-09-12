using System.Web.Mvc;

namespace DotWeb.Areas.Sys_Active
{
    public class ActiveAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Active";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Active_default",
                "Active/{controller}/{action}/{id}",
                new { action = "Main", id = UrlParameter.Optional }
            );
        }
    }
}
