using DotWeb.CommSetup;
using DotWeb.Controller;
using DotWeb.WebApp;
using ProcCore.Business;
using ProcCore.Business.DB0;
using ProcCore.Business.LogicConect;
using ProcCore.HandleResult;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace DotWeb.Areas.Sys_Active.Controllers
{
    public class CategoryController : BaseController
    {
        #region Action and function section
        public ActionResult Main()
        {
            ActionRun();
            return View();
        }
        #endregion

        #region ajax call section

        public string aj_Init()
        {
            var open = openLogic();
            using (var db0 = getDB0())
            {
                return defJSON(new
                {
                    //options_category = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.News && x.i_Hide == false).OrderBy(x => x.sort).Select(x => new option() { val=x.all_category_l2_id,Lname=x.l2_name })
                });
            }
        }
        [HttpPost]
        public string UpdateSort(IList<CategroySort> Data)
        {
            using (db0 = getDB0())
            {
                foreach (var q in Data)
                {
                    var item = db0.All_Category_L2.Find(q.id);
                    item.sort = q.sort;
                }
                db0.SaveChanges();
                var r = new ResultInfo() { result = true };
                return defJSON(r);
            }
        }
        #endregion
    }

    public class CategroySort
    {
        public int id { get; set; }
        public int sort { get; set; }
    }
}