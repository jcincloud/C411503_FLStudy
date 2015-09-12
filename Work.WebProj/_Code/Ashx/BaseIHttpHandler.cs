using System;
using System.Web;



namespace DotWeb._Code
{
    public class BaseIHttpHandler : IHttpHandler
    {
        public virtual void ProcessRequest(HttpContext context)
        {
            if (context.Session["Id"] == null) { }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class vmJsonResult
    {
        public vmJsonResult() {
            message = "";
        }
        public Boolean result { get; set; }
        public String message { get; set; }
        public Object data { get; set; }
    }
}