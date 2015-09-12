using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using ProcCore.HandleResult;
using DotWeb.CommSetup;
using DotWeb.Controller;
namespace DotWeb.Controllers
{
    public class ContactUsController : WebFrontController
    {
        public ActionResult Index()
        {
            return View("ContactUs");
        }
        public ActionResult p2()
        {
            return View("ContactUs2");
        }
    }

}
