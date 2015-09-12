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
using ProcCore.Business.DB0;
namespace DotWeb.Controllers
{
    public class InfoController : WebFrontController
    {
        public ActionResult Index()
        {
            return View("Info");
        }
        public ActionResult p1_content(int id)
        {
            HelpfulInfo item = null;
            using (db0 = getDB0())
            {
                var options_category = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.HelpfulInfo && x.i_Hide == false).OrderBy(x => x.sort).Select(x => new option() { val = x.all_category_l2_id, Lname = x.l2_name });
                Boolean Exist = db0.HelpfulInfo.Any(x => x.helpful_info_id == id && x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name);
                if (!Exist)
                {
                    return Redirect("~/Info");
                }
                else
                {
                    item = db0.HelpfulInfo.Find(id);
                    foreach (var op in options_category)
                    {
                        if (op.val == item.category)
                        {
                            if (System.Globalization.CultureInfo.CurrentCulture.Name == "zh-CN")
                            {
                                item.category_name = ProcCore.NetExtension.ExtensionString.ToSimplified(op.Lname);
                            }
                            else
                            {
                                item.category_name = op.Lname;
                            }
                        }
                    }
                    item.imgsrc = GetImg(item.helpful_info_id, "Photo1", "HelpfulInfo", "Photo");//內頁圖片
                }
            }
            return View("Info_Content", item);
        }
        public ActionResult p2()
        {
            return View("Info2");
        }
        public ActionResult p2_content()
        {
            return View("Info2_Content");
        }
        public ActionResult p3()
        {
            return View("Info3");
        }
        public ActionResult p3_content()
        {
            return View("Info3_Content");
        }
    }

}
