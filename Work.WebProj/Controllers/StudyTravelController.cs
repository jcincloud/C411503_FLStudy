using System.Linq;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using ProcCore.HandleResult;
using DotWeb.CommSetup;
using DotWeb.Controller;
using ProcCore.Business.DB0;
namespace DotWeb.Controllers
{
    public class StudyTravelController : WebFrontController
    {
        public ActionResult Index()
        {
            return View("StudyA");
        }
        public ActionResult p1_content(int id)
        {
            StudyAbroad item = null;
            using (db0 = getDB0())
            {
                var options_vacation_category = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.Vacation && x.i_Hide == false).OrderBy(x => x.sort).Select(x => new option() { val = x.all_category_l2_id, Lname = x.l2_name });
                Boolean Exist = db0.StudyAbroad.Any(x => x.study_abroad_id == id && x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name);
                if (!Exist)
                {
                    return Redirect("~/StudyTravel");
                }
                else
                {
                    item = db0.StudyAbroad.Find(id);
                    foreach (var op in options_vacation_category)
                    {
                        if (op.val == item.vacation_category)
                            item.vacation_name = op.Lname;
                    }
                    item.photoB_imgsrc = GetImgs(item.study_abroad_id, "Photo2", "StudyAbroad", "Photo", "big");
                    item.photoS_imgsrc = GetImgs(item.study_abroad_id, "Photo2", "StudyAbroad", "Photo", "small");
                    item.benner_imgsrc = GetImgs(item.study_abroad_id, "Banner", "StudyAbroad", "Banner", null);
                    item.imgsrc = GetImg(item.study_abroad_id, "Photo1", "StudyAbroad", "Photo");
                }
            }
            return View("StudyA_Content", item);
        }
        public ActionResult p2()
        {
            return View("StudyA2");
        }
        public ActionResult p3()
        {
            IList<InfoDetail> items = null;
            using (db0 = getDB0())
            {
                items = db0.InfoDetail.Where(x => x.info_id == InfoDetailType.Info_p2 && x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name).OrderBy(x => x.sort).ToList();
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
            return View("StudyA3", items);
        }
        public ActionResult p4()
        {
            return View("StudyA4");
        }
        public ActionResult p5()
        {
            return View("StudyA5");
        }
        public ActionResult p6()
        {
            return View("StudyA6");
        }
        public ActionResult p6_content(int id)
        {
            AbroadSchool item = null;
            using (db0 = getDB0())
            {
                Boolean Exist = db0.AbroadSchool.Any(x => x.abroad_school_id == id && x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name & x.category == (int)AbroadSchoolCategory.Language);
                if (!Exist)
                {
                    return Redirect("~/StudyTravel");
                }
                else
                {
                    item = db0.AbroadSchool.Find(id);

                    item.photo_imgsrc = GetImgs(item.abroad_school_id, "Photo2", "AbroadSchool", "Photo", null);
                    item.imgsrc = GetImg(item.abroad_school_id, "Photo3", "AbroadSchool", "Photo");
                }
            }
            return View("StudyA6_Content",item);
        }

        #region ajax call section

        public string aj_Init()
        {
            using (var db0 = getDB0())
            {
                IList<option> options = new List<option>();
                option hot = new option() { val = 0, Lname = "Hot 行程" };//hot行程
                options.Add(hot);

                var other_options = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.Vacation_2 && x.i_Hide == false).OrderByDescending(x => x.sort).Select(x => new { x.all_category_l2_id, x.l2_name });

                foreach (var item in other_options)
                {
                    option o = new option();
                    o.val = item.all_category_l2_id;
                    o.Lname = item.l2_name;
                    options.Add(o);
                }
                option past = new option() { val = 1, Lname = "過往行程" };//過往行程
                options.Add(past);

                return defJSON(new
                {
                    options_vacation_category = options,
                    options_country_category = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.Country && x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name).OrderByDescending(x => x.sort).Select(x => new option() { val = x.all_category_l2_id, Lname = x.l2_name })
                });
            }
        }

        public string axContentInit()
        {
            using (var db0 = getDB0())
            {
                return defJSON(new
                {
                    options_vacation_category = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.Vacation && x.i_Hide == false).OrderBy(x => x.sort).Select(x => new option() { val = x.all_category_l2_id, Lname = x.l2_name })
                });
            }
        }


        public string sendMail(EmailContact md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                md.vaild = md.vaild.ToUpper();//轉大寫
                if (Session["ContactUs"].ToString() != md.vaild)
                {
                    rAjaxResult.result = false;
                    rAjaxResult.message = "驗證碼不正確";
                    return defJSON(rAjaxResult);
                };

                using (db0 = getDB0())
                {
                    #region 信件發送
                    md.category = 2;//參加遊學說明會
                    string Body = getMailBody("Email", md);//套用信件版面
                    Boolean mail;
                    mail = Mail_Send(md.email, //寄信人
                                    openLogic().getReceiveMails(), //收信人
                                    CommWebSetup.MailTitle1, //信件標題
                                    Body, //信件內容
                                    true); //是否為html格式
                    if (mail == false)
                    {
                        rAjaxResult.result = false;
                        rAjaxResult.message = "送信失敗，請換其他信箱再重新填寫！謝謝！";
                        return defJSON(rAjaxResult);
                    }
                    #endregion


                    #region 將信件內容寫進資料庫
                    md.email_contact_id = GetNewId(ProcCore.Business.CodeTable.EmailContact);
                    md.i_InsertDateTime = DateTime.Now;
                    md.i_Lang = System.Globalization.CultureInfo.CurrentCulture.Name;
                    db0.EmailContact.Add(md);
                    db0.SaveChanges();
                    #endregion
                }
                rAjaxResult.result = true;

            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
            }
            return defJSON(rAjaxResult);
        }
        #endregion

    }


}
