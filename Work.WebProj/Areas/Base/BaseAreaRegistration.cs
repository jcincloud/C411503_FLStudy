using System.Web.Mvc;

namespace DotWeb.Areas.Sys_Base
{
    public class BaseAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Base";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Base_default",
                "Base/{controller}/{action}/{id}",
                new { action = "Main", id = UrlParameter.Optional },
                new string[] { "DotWeb.Areas.Sys_Base.Controllers" }
            ).DataTokens["UseNamespaceFallback" ] = false;
        }
    }
}
