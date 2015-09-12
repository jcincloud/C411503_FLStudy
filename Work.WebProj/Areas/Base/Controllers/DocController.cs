using DotWeb.Controller;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml;

namespace DotWeb.Areas.Sys_Base.Controllers
{
    public class DocController : BaseController
    {
        public RedirectResult Index()
        {
            return Redirect(Url.Action("ListGrid"));
        }

        public ActionResult ListGrid()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(Server.MapPath("~/_Code/SysDoc/Doc.xml"));
            XmlNodeList xmlE = xml.GetElementsByTagName("file");
            List<DocInfo> l = new List<DocInfo>();
            foreach (XmlElement f in xmlE)
            {
                String f_name = f.GetElementsByTagName("name").Item(0).FirstChild.Value;
                String f_sort = f.GetElementsByTagName("sort").Item(0).FirstChild.Value;
                String f_memo = f.GetElementsByTagName("memo").Item(0).FirstChild.Value;

                if (System.IO.File.Exists( Server.MapPath("~/_Code/SysDoc/" + f_name)))
                {
                    DocInfo d = new DocInfo()
                    {
                        Name = f_name, Sort = int.Parse(f_sort),Momo=f_memo,Link = Url.Content("~/_Code/SysDoc/" + f_name)
                    };
                    l.Add(d);
                }
            }
            return View("ListData", l.ToArray());
        }
    }
}
