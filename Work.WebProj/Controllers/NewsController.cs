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
    public class NewsController : WebFrontController
    {
        public ActionResult Index()
        {
            return View("News");
        }
        public ActionResult Content(int id)
        {
            News item = null;
            using (db0 = getDB0())
            {
                var options_category = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.News && x.i_Hide == false).OrderBy(x => x.sort).Select(x => new option() { val = x.all_category_l2_id, Lname = x.l2_name });
                Boolean Exist = db0.News.Any(x => x.news_id == id && x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name);
                if (!Exist)
                {
                    return Redirect("~/News");
                }
                else
                {
                    item = db0.News.Find(id);
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
                    item.imgsrc = GetImg(item.news_id, "Photo1", "News", "Photo");//內頁圖片
                }
            }
            return View("News_Content", item);
        }
        public string aj_Init()
        {
            using (var db0 = getDB0())
            {
                var options_category = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.News && x.i_Hide == false).OrderBy(x => x.sort).Select(x => new option() { val = x.all_category_l2_id, Lname = x.l2_name }).ToList();
                if (System.Globalization.CultureInfo.CurrentCulture.Name == "zh-CN")
                {
                    foreach (var i in options_category)
                    {
                        i.Lname = ProcCore.NetExtension.ExtensionString.ToSimplified(i.Lname);
                    }
                }
                return defJSON(new
                {
                    options_category = options_category
                });
            }
        }
    }

}
