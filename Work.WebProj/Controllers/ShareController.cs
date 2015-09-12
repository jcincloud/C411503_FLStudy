using System.Linq;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using DotWeb.Controller;
using ProcCore.Business.DB0;
namespace DotWeb.Controllers
{
    public class ShareController : WebFrontController
    {
        public ActionResult Index()
        {
            #region
            IList<m_Feedback> items = null;
            using (db0 = getDB0())
            {
                 items = db0.Feedback
                    .Where(x => x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name)
                    .OrderByDescending(x => x.sort)
                    .Select(x => new m_Feedback
                    {
                        feedback_id = x.feedback_id,
                        title = x.title,
                        feedback_sort = x.feedback_sort
                    }).ToList();
            }
            #endregion
            return View("Share",items);
        }
        public ActionResult p1_content(int id)
        {
            Feedback item = null;
            using (db0 = getDB0())
            {
                //var options_year = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.Feedback_year && x.i_Hide == false).OrderBy(x => x.sort).Select(x => new option() { val = x.all_category_l2_id, Lname = x.l2_name });
                //var options_category = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.Feedback_category && x.i_Hide == false).OrderBy(x => x.sort).Select(x => new option() { val = x.all_category_l2_id, Lname = x.l2_name });
                Boolean Exist = db0.Feedback.Any(x => x.feedback_id == id && x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name);
                if (!Exist)
                {
                    return Redirect("~/Share");
                }
                else
                {
                    item = db0.Feedback.Find(id);
                    //foreach (var op in options_category)
                    //{
                    //    if (op.val == item.category)
                    //        item.category_name = op.Lname;
                    //}
                    item.imgsrc = GetImg(item.feedback_id, "Photo1", "Feedback", "Photo");//內頁圖片
                    item.photos_imgsrc = GetImgs(item.feedback_id, "Photo2", "Feedback", "Photo", null);//內頁圖片
                }
            }
            return View("Share_Content", item);
        }
        public ActionResult p2()
        {
            return View("Share2");
        }
        public ActionResult p3()
        {
            return View("Share3");
        }
        public ActionResult p4()
        {
            return View("Share4");
        }
        public ActionResult p5()
        {
            return View("Share5");
        }
        public ActionResult p5_Content(int id)
        {
            Albums item = null;
            using (db0 = getDB0())
            {
                Boolean Exist = db0.Albums.Any(x => x.albums_id == id && x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name);
                if (!Exist)
                {
                    return Redirect("~/Share/p5");
                }
                else
                {
                    item = db0.Albums.Find(id);
                    item.photos_imgsrc = GetImgs(item.albums_id, "Photo2", "Albums", "Photo", null);//內頁圖片
                }
            }
            return View("Share5_Content", item);
        }

        public string aj_Init()
        {
            using (var db0 = getDB0())
            {
                return defJSON(new
                {
                    options_year = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.Feedback_year && x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name).OrderByDescending(x => x.sort).Select(x => new option() { val = x.all_category_l2_id, Lname = x.l2_name }),
                    options_category = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.Feedback_category && x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name).OrderByDescending(x => x.sort).Select(x => new option() { val = x.all_category_l2_id, Lname = x.l2_name })

                });
            }
        }
    }

}
