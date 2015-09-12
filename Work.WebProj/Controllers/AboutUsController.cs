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
    public class AboutUsController : WebFrontController
    {
        public ActionResult Index()
        {
            return View("AboutUs");
        }
        public ActionResult p2()
        {
            return View("AboutUs2");
        }
        public ActionResult p3()
        {
            IList<InfoDetail> items = null;
            using (db0 = getDB0())
            {
                items = db0.InfoDetail.Where(x => x.info_id == InfoDetailType.Info_p1 && x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name).OrderBy(x => x.sort).ToList();
                foreach (var item in items)
                {
                    if (item.stereotype == 2)
                    {//版型2的圖片
                        item.imgsrc = GetImg(item.info_detail_id, "Photo1", "InfoDetail", "Photo");//內頁圖片
                    }
                    else if (item.stereotype == 3)
                    {//版型3的圖片
                        item.imgsrc = GetImg(item.info_detail_id, "Photo2", "InfoDetail", "Photo");//內頁圖片                    
                    }
                }

            }
            return View("AboutUs3", items);
        }
    }

}
