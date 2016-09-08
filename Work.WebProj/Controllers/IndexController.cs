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
    public class IndexController : WebFrontController
    {
        public ActionResult Index()
        {
            IndexInfo info = new IndexInfo();
            using (var db0 = getDB0())
            {
                #region Banner
                info.banner = db0.Banner.Where(x => x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name).OrderByDescending(x => x.sort).Take(5).ToList();

                foreach (var item in info.banner)
                {
                    item.imgsrc = GetImg(item.banner_id, "Banner", "Banner", "Banner");//顯示列表圖
                }
                #endregion
                #region 行程
                string lang = System.Globalization.CultureInfo.CurrentCulture.Name;
                info.hot = db0.StudyAbroad.Where(x => x.i_Hide == false & x.is_hot & !x.is_past & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name).OrderByDescending(x => x.sort).Take(6).ToList();

                info.vacations = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.Vacation_2 && x.i_Hide == false)
                                  .OrderByDescending(x => x.sort)
                                  .Take(2)
                                  .Select(x => new Vacation()
                                  {
                                      vacation_id = x.all_category_l2_id,
                                      vacation_name = x.l2_name,
                                      data = db0.StudyAbroad
                                                .Where(y => !y.i_Hide & !y.is_past & y.vacation_category == x.all_category_l2_id & y.i_Lang == lang)
                                                .OrderByDescending(y => y.sort).Take(6).ToList()
                                  }).ToList();

                foreach (var item in info.hot)
                {
                    item.imgsrc = GetImg(item.study_abroad_id, "Photo1", "StudyAbroad", "Photo");//顯示列表圖
                }
                foreach (var item in info.vacations)
                {
                    foreach (var i in item.data)
                    {
                        i.imgsrc = GetImg(i.study_abroad_id, "Photo1", "StudyAbroad", "Photo");//顯示列表圖
                    }
                }
                #endregion
                #region 最新消息
                info.news = db0.News.Where(x => x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name).OrderByDescending(x => x.date).Take(4).ToList();
                foreach (var item in info.news)
                {
                    item.imgsrc = GetImg(item.news_id, "Photo1", "News", "Photo");//顯示列表圖
                }
                #endregion
                #region 留學遊學資訊
                info.helpfulinfo = db0.HelpfulInfo.Where(x => x.i_Hide == false && x.show_index & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name).OrderByDescending(x => x.sort).Take(4).ToList();
                foreach (var item in info.helpfulinfo)
                {
                    item.imgsrc = GetImg(item.helpful_info_id, "Photo1", "HelpfulInfo", "Photo");//顯示列表圖
                }
                #endregion
                #region 心得分享
                info.feedback = db0.Feedback.Where(x => x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name).OrderByDescending(x => x.sort).Take(4).ToList();
                foreach (var item in info.feedback)
                {
                    item.imgsrc = GetImg(item.feedback_id, "Photo1", "Feedback", "Photo");//顯示列表圖
                    item.feedback_content = item.feedback_content == null ? "" : RemoveHTMLTag(item.feedback_content);
                }
                #endregion
                #region 精彩照片集
                info.albums = db0.Albums.Where(x => x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name).OrderByDescending(x => x.sort).Take(8).ToList();
                foreach (var item in info.albums)
                {
                    item.imgsrc = GetImg(item.albums_id, "Photo1", "Albums", "Photo");//顯示列表圖
                }
                #endregion
            }
            return View(info);
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
                    md.category = 1;//索取簡章
                    string Body = getMailBody("Email", md);//套用信件版面
                    Boolean mail;
                    mail = Mail_Send(md.email, //寄信人
                                   openLogic().getReceiveMails(), //收信人 CommWebSetup.MailToList
                                    CommWebSetup.MailTitle2, //信件標題
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
        public string aj_Init()
        {
            using (var db0 = getDB0())
            {
                return defJSON(new
                {
                    options_findout_category = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.Findout && x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name).OrderByDescending(x => x.sort).Select(x => new option() { val = x.all_category_l2_id, Lname = x.l2_name }),
                    options_interest_category = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.Interest && x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name).OrderByDescending(x => x.sort).Select(x => new option() { val = x.all_category_l2_id, Lname = x.l2_name })
                });
            }
        }
        public RedirectResult Login()
        {
            return Redirect("~/Base/Login");
        }
    }
    public class Vacation
    {
        public int vacation_id { get; set; }
        public string vacation_name { get; set; }
        public IList<StudyAbroad> data { get; set; }
    }
    public class IndexInfo
    {
        public IList<Banner> banner { get; set; }
        public IList<StudyAbroad> hot { get; set; }
        public IList<Vacation> vacations { get; set; }
        public IList<News> news { get; set; }
        public IList<HelpfulInfo> helpfulinfo { get; set; }
        public IList<Feedback> feedback { get; set; }
        public IList<Albums> albums { get; set; }
    }
}
