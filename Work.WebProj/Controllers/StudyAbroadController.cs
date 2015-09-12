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
    public class StudyAbroadController : WebFrontController
    {
        public ActionResult Index()
        {
            return View("StudyB");
        }
        public ActionResult Content(int id)
        {
            AbroadSchool item = null;
            using (db0 = getDB0())
            {
                Boolean Exist = db0.AbroadSchool.Any(x => x.abroad_school_id == id && x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name & x.category == (int)AbroadSchoolCategory.World);
                if (!Exist)
                {
                    return Redirect("~/StudyAbroad");
                }
                else
                {
                    item = db0.AbroadSchool.Find(id);

                    item.photo_imgsrc = GetImgs(item.abroad_school_id, "Photo2", "AbroadSchool", "Photo", null);
                    item.imgsrc = GetImg(item.abroad_school_id, "Photo3", "AbroadSchool", "Photo");
                }
            }
            return View("StudyB_Content", item);
        }
        public string aj_Init()
        {
            using (var db0 = getDB0())
            {

                return defJSON(new
                {
                    options_country_category = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.Country && x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name).OrderByDescending(x => x.sort).Select(x => new option() { val = x.all_category_l2_id, Lname = x.l2_name })
                });
            }
        }
        public ActionResult p2()
        {
            return View("StudyB2");
        }
        public ActionResult p3()
        {
            return View("StudyB3");
        }
        public ActionResult p2_Content(int id)
        {
            TestInfo item = null;
            using (db0 = getDB0())
            {
                var options_category = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.TestInfo && x.i_Hide == false).OrderBy(x => x.sort).Select(x => new option() { val = x.all_category_l2_id, Lname = x.l2_name });
                Boolean Exist = db0.TestInfo.Any(x => x.test_info_id == id && x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name);
                if (!Exist)
                {
                    return Redirect("~/StudyAbroad");
                }
                else
                {
                    item = db0.TestInfo.Find(id);
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
                    item.imgsrc = GetImg(item.test_info_id, "Photo1", "TestInfo", "Photo");//內頁圖片
                }
            }
            return View("StudyB2_Content", item);
        }
    }

}
