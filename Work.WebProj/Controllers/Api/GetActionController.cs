using ProcCore.Business.DB0;
using ProcCore.HandleResult;
using ProcCore.WebCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace DotWeb.Api
{
    public class GetActionController : BaseApiController
    {
        public GetActionController()
        {

        }
        [HttpPost]
        public ResultInfo UpdateSort([FromBody]IList<CategroySort> Data)
        {//測試webapi 是否能接收IList<> 資料(測試不行)
            using (db0 = getDB0())
            {
                foreach (var q in Data)
                {
                    var item = db0.All_Category_L2.Find(q.id);
                    item.sort = q.sort;
                }
                db0.SaveChanges();
                var r = new ResultInfo() { result = true };
                return r;
            }
        }
        public IHttpActionResult GetNewsWWW(int? y, int? page, string keyword)
        {
            return null;
        }
        public IHttpActionResult GetStudyAbroadWWW(int? page, int? category, int? country)
        {//海外遊學list
            #region

            using (db0 = getDB0())
            {
                var items = db0.StudyAbroad
                    .Where(x => x.i_Hide == false && x.vacation_category != VacationType.Intensive & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name)
                    .OrderByDescending(x => x.sort)
                    .Select(x => new m_StudyAbroad
                    {
                        study_abroad_id = x.study_abroad_id,
                        start_date = x.start_date,
                        end_date = x.end_date,
                        planning_name = x.planning_name,
                        intro = x.intro,
                        is_hot = x.is_hot,
                        is_past = x.is_past,
                        country_category = x.country_category,
                        vacation_category = x.vacation_category
                    });

                if (category != null)
                {
                    if (category == 0)
                    { //hot 行程
                        items = items.Where(x => x.is_hot & !x.is_past);
                    }
                    else if (category == 1)
                    {//過往行程
                        items = items.Where(x => x.is_past);
                    }
                    else
                    {//其他行程
                        items = items.Where(x => x.vacation_category == category & !x.is_past);
                    }
                }
                if (country != null)
                {
                    items = items.Where(x => x.country_category == country);
                }

                var nowpage = page == null ? 1 : (int)page;
                int startRecord = PageCount.PageInfo(nowpage, 10, items.Count());
                var resultItems = items.Skip(startRecord).Take(10);

                var GridInfo = new
                {
                    rows = resultItems.ToList(),
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                };

                foreach (var item in GridInfo.rows)
                {
                    item.imgsrc = GetImg(item.study_abroad_id, "Photo1", "StudyAbroad", "Photo");//顯示列表圖
                }

                return Ok(GridInfo);
            }
            #endregion
        }

        public IHttpActionResult GetStudyAbroadContentWWW(int id)
        {//海外遊學content
            #region
            using (db0 = getDB0())
            {
                var item = db0.StudyAbroad.Find(id);
                item.photoB_imgsrc = GetImgs(item.study_abroad_id, "Photo2", "StudyAbroad", "Photo", "big");
                item.photoS_imgsrc = GetImgs(item.study_abroad_id, "Photo2", "StudyAbroad", "Photo", "small");
                return Ok(item);
            }
            #endregion
        }
        public IHttpActionResult GetStudyAbroadbBannerWWW(int id)
        {//海外遊學content
            #region
            using (db0 = getDB0())
            {
                var item = db0.StudyAbroad.Find(id);
                item.benner_imgsrc = GetImgs(item.study_abroad_id, "Banner", "StudyAbroad", "Banner", null);

                return Ok(item);
            }
            #endregion
        }
        public IHttpActionResult GetAoubtUSVideoWWW(int? page)
        {//精彩影片分享list
            #region

            using (db0 = getDB0())
            {
                var items = db0.Video
                    .Where(x => x.i_Hide == false & x.category == VideoType.AboutUS_p2 & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name)
                    .OrderByDescending(x => x.sort)
                    .Select(x => new m_Video
                    {
                        video_id = x.video_id,
                        video_title = x.video_title,
                        video_url = x.video_url
                    });

                var nowpage = page == null ? 1 : (int)page;
                int startRecord = PageCount.PageInfo(nowpage, 9, items.Count());
                var resultItems = items.Skip(startRecord).Take(9);

                var GridInfo = new
                {
                    rows = resultItems.ToList(),
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                };

                foreach (var item in GridInfo.rows)
                {
                    item.imgsrc = GetImg(item.video_id, "Photo1", "Video", "Photo");//顯示列表圖
                }

                return Ok(GridInfo);
            }
            #endregion
        }
        public IHttpActionResult GetShareP2VideoWWW(int? page)
        {//出國影片list
            #region

            using (db0 = getDB0())
            {
                var items = db0.Video
                    .Where(x => x.i_Hide == false & x.category == VideoType.Share_p2 & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name)
                    .OrderByDescending(x => x.sort)
                    .Select(x => new m_Video
                    {
                        video_id = x.video_id,
                        video_title = x.video_title,
                        video_url = x.video_url
                    });

                var nowpage = page == null ? 1 : (int)page;
                int startRecord = PageCount.PageInfo(nowpage, 9, items.Count());
                var resultItems = items.Skip(startRecord).Take(9);

                var GridInfo = new
                {
                    rows = resultItems.ToList(),
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                };

                foreach (var item in GridInfo.rows)
                {
                    item.imgsrc = GetImg(item.video_id, "Photo1", "Video", "Photo");//顯示列表圖
                }

                return Ok(GridInfo);
            }
            #endregion
        }
        public IHttpActionResult GetShareP3VideoWWW(int? page)
        {//成果發表影片list
            #region

            using (db0 = getDB0())
            {
                var items = db0.Video
                    .Where(x => x.i_Hide == false & x.category == VideoType.Share_p3 & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name)
                    .OrderByDescending(x => x.sort)
                    .Select(x => new m_Video
                    {
                        video_id = x.video_id,
                        video_title = x.video_title,
                        video_url = x.video_url
                    });

                var nowpage = page == null ? 1 : (int)page;
                int startRecord = PageCount.PageInfo(nowpage, 9, items.Count());
                var resultItems = items.Skip(startRecord).Take(9);

                var GridInfo = new
                {
                    rows = resultItems.ToList(),
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                };

                foreach (var item in GridInfo.rows)
                {
                    item.imgsrc = GetImg(item.video_id, "Photo1", "Video", "Photo");//顯示列表圖
                }

                return Ok(GridInfo);
            }
            #endregion
        }
        public IHttpActionResult GetShareP4VideoWWW(int? page)
        {//客戶形象影片list
            #region

            using (db0 = getDB0())
            {
                var items = db0.Video
                    .Where(x => x.i_Hide == false & x.category == VideoType.Share_p4 & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name)
                    .OrderByDescending(x => x.sort)
                    .Select(x => new m_Video
                    {
                        video_id = x.video_id,
                        video_title = x.video_title,
                        video_url = x.video_url
                    });

                var nowpage = page == null ? 1 : (int)page;
                int startRecord = PageCount.PageInfo(nowpage, 9, items.Count());
                var resultItems = items.Skip(startRecord).Take(9);

                var GridInfo = new
                {
                    rows = resultItems.ToList(),
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                };

                foreach (var item in GridInfo.rows)
                {
                    item.imgsrc = GetImg(item.video_id, "Photo1", "Video", "Photo");//顯示列表圖
                }

                return Ok(GridInfo);
            }
            #endregion
        }
        public IHttpActionResult GetStudyAbroadA4WWW(int? page, int? country)
        {//密集式體驗營list
            #region

            using (db0 = getDB0())
            {
                var items = db0.StudyAbroad
                    .Where(x => x.i_Hide == false && x.vacation_category == VacationType.Intensive & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name)
                    .OrderByDescending(x => x.sort)
                    .Select(x => new m_StudyAbroad
                    {
                        study_abroad_id = x.study_abroad_id,
                        start_date = x.start_date,
                        end_date = x.end_date,
                        planning_name = x.planning_name,
                        intro = x.intro,
                        is_hot = x.is_hot,
                        country_category = x.country_category,
                        vacation_category = x.vacation_category
                    });

                if (country != null)
                {
                    items = items.Where(x => x.country_category == country);
                }

                var nowpage = page == null ? 1 : (int)page;
                int startRecord = PageCount.PageInfo(nowpage, 10, items.Count());
                var resultItems = items.Skip(startRecord).Take(10);

                var GridInfo = new
                {
                    rows = resultItems.ToList(),
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                };

                foreach (var item in GridInfo.rows)
                {
                    item.imgsrc = GetImg(item.study_abroad_id, "Photo1", "StudyAbroad", "Photo");//顯示列表圖
                }

                return Ok(GridInfo);
            }
            #endregion
        }
        public IHttpActionResult GetAbroadSchoolWWW(int? page, int? country, int? operation, string title, int? category)
        {//海外遊學list
            #region

            using (db0 = getDB0())
            {
                var items = db0.AbroadSchool
                    .Where(x => x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name)
                    .OrderByDescending(x => x.sort)
                    .Select(x => new m_AbroadSchool
                    {
                        abroad_school_id = x.abroad_school_id,
                        school_title = x.school_title,
                        list_intro = x.list_intro,
                        country_category = x.country_category,
                        operation_category = x.operation_category,
                        category=x.category
                    });

                if (category != null)
                {
                    items = items.Where(x => x.category == category);
                }
                if (country != null)
                {
                    items = items.Where(x => x.country_category == country);
                }

                if (operation != null)
                {
                    items = items.Where(x => x.operation_category == operation);
                }
                if (title != null)
                {
                    items = items.Where(x => x.school_title.Contains(title));
                }

                var nowpage = page == null ? 1 : (int)page;
                int startRecord = PageCount.PageInfo(nowpage, 10, items.Count());
                var resultItems = items.Skip(startRecord).Take(10);

                var GridInfo = new
                {
                    rows = resultItems.ToList(),
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                };

                foreach (var item in GridInfo.rows)
                {
                    item.imgsrc = GetImg(item.abroad_school_id, "Photo1", "AbroadSchool", "Photo");//顯示列表圖
                }

                return Ok(GridInfo);
            }
            #endregion
        }
        public IHttpActionResult GetNewsWWW(int? page, int? category)
        {//最新消息 list
            #region

            using (db0 = getDB0())
            {
                var items = db0.News
                    .Where(x => x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name)
                    .OrderByDescending(x => x.date)
                    .Select(x => new m_News
                    {
                        news_id = x.news_id,
                        title = x.title,
                        date = x.date,
                        list_intro = x.list_intro,
                        category = x.category
                    });

                if (category != null)
                {
                    items = items.Where(x => x.category == category);
                }

                var nowpage = page == null ? 1 : (int)page;
                int startRecord = PageCount.PageInfo(nowpage, 10, items.Count());
                var resultItems = items.Skip(startRecord).Take(10);

                var GridInfo = new
                {
                    rows = resultItems.ToList(),
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                };

                foreach (var item in GridInfo.rows)
                {
                    item.imgsrc = GetImg(item.news_id, "Photo1", "News", "Photo");//顯示列表圖
                }

                return Ok(GridInfo);
            }
            #endregion
        }
        public IHttpActionResult GetInfoP1WWW(int? page)
        {//遊留學百科 list
            #region

            using (db0 = getDB0())
            {
                var items = db0.HelpfulInfo
                    .Where(x => x.i_Hide == false & x.category == HelpfulInfoType.Info_p1 & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name)
                    .OrderByDescending(x => x.sort)
                    .Select(x => new m_HelpfulInfo
                    {
                        helpful_info_id = x.helpful_info_id,
                        title = x.title,
                        list_intro = x.list_intro
                    });

                var nowpage = page == null ? 1 : (int)page;
                int startRecord = PageCount.PageInfo(nowpage, 10, items.Count());
                var resultItems = items.Skip(startRecord).Take(10);

                var GridInfo = new
                {
                    rows = resultItems.ToList(),
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                };

                foreach (var item in GridInfo.rows)
                {
                    item.imgsrc = GetImg(item.helpful_info_id, "Photo1", "HelpfulInfo", "Photo");//顯示列表圖
                }

                return Ok(GridInfo);
            }
            #endregion
        }
        public IHttpActionResult GetInfoP2WWW(int? page)
        {//遊留學新知 list
            #region

            using (db0 = getDB0())
            {
                var items = db0.HelpfulInfo
                    .Where(x => x.i_Hide == false & x.category == HelpfulInfoType.Info_p2 & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name)
                    .OrderByDescending(x => x.sort)
                    .Select(x => new m_HelpfulInfo
                    {
                        helpful_info_id = x.helpful_info_id,
                        title = x.title,
                        list_intro = x.list_intro
                    });

                var nowpage = page == null ? 1 : (int)page;
                int startRecord = PageCount.PageInfo(nowpage, 10, items.Count());
                var resultItems = items.Skip(startRecord).Take(10);

                var GridInfo = new
                {
                    rows = resultItems.ToList(),
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                };

                foreach (var item in GridInfo.rows)
                {
                    item.imgsrc = GetImg(item.helpful_info_id, "Photo1", "HelpfulInfo", "Photo");//顯示列表圖
                }

                return Ok(GridInfo);
            }
            #endregion
        }
        public IHttpActionResult GetInfoP3WWW(int? page)
        {//行前準備 list
            #region

            using (db0 = getDB0())
            {
                var items = db0.HelpfulInfo
                    .Where(x => x.i_Hide == false & x.category == HelpfulInfoType.Info_p3 & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name)
                    .OrderByDescending(x => x.sort)
                    .Select(x => new m_HelpfulInfo
                    {
                        helpful_info_id = x.helpful_info_id,
                        title = x.title,
                        list_intro = x.list_intro
                    });

                var nowpage = page == null ? 1 : (int)page;
                int startRecord = PageCount.PageInfo(nowpage, 10, items.Count());
                var resultItems = items.Skip(startRecord).Take(10);

                var GridInfo = new
                {
                    rows = resultItems.ToList(),
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                };

                foreach (var item in GridInfo.rows)
                {
                    item.imgsrc = GetImg(item.helpful_info_id, "Photo1", "HelpfulInfo", "Photo");//顯示列表圖
                }

                return Ok(GridInfo);
            }
            #endregion
        }
        public IHttpActionResult GetFeedbackWWW(int? page, int? category, int? year)
        {//心得分享 list
            #region

            using (db0 = getDB0())
            {
                var items = db0.Feedback
                    .Where(x => x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name)
                    .OrderByDescending(x => x.sort)
                    .Select(x => new m_Feedback
                    {
                        feedback_id = x.feedback_id,
                        title = x.title,
                        feedback_content = x.feedback_content,
                        feedback_sort = x.feedback_sort,
                        category = x.category,
                        year = x.year
                    });

                if (category != null)
                {
                    items = items.Where(x => x.category == category);
                }

                if (year != null)
                {
                    items = items.Where(x => x.year == year);
                }

                var nowpage = page == null ? 1 : (int)page;
                int startRecord = PageCount.PageInfo(nowpage, 9, items.Count());
                var resultItems = items.Skip(startRecord).Take(9);

                var GridInfo = new
                {
                    rows = resultItems.ToList(),
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                };

                foreach (var item in GridInfo.rows)
                {
                    item.imgsrc = GetImg(item.feedback_id, "Photo1", "Feedback", "Photo");//顯示列表圖
                    item.feedback_content = item.feedback_content == null ? "" : RemoveHTMLTag(item.feedback_content);
                }

                return Ok(GridInfo);
            }
            #endregion
        }
        public IHttpActionResult GetAlbumsWWW(int? page, int? category, int? year)
        {//相片回顧 list
            #region

            using (db0 = getDB0())
            {
                var items = db0.Albums
                    .Where(x => x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name)
                    .OrderByDescending(x => x.sort)
                    .Select(x => new m_Albums
                    {
                        albums_id = x.albums_id,
                        title = x.title,
                        category = x.category,
                        year = x.year
                    });

                if (category != null)
                {
                    items = items.Where(x => x.category == category);
                }

                if (year != null)
                {
                    items = items.Where(x => x.year == year);
                }

                var nowpage = page == null ? 1 : (int)page;
                int startRecord = PageCount.PageInfo(nowpage, 9, items.Count());
                var resultItems = items.Skip(startRecord).Take(9);

                var GridInfo = new
                {
                    rows = resultItems.ToList(),
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                };

                foreach (var item in GridInfo.rows)
                {
                    item.imgsrc = GetImg(item.albums_id, "Photo1", "Albums", "Photo");//顯示列表圖
                }

                return Ok(GridInfo);
            }
            #endregion
        }
        public IHttpActionResult GetTestInfoWWW(int? page, int? category)
        {//各學術/英文 能力測驗 list
            #region

            using (db0 = getDB0())
            {
                var items = db0.TestInfo
                    .Where(x => x.i_Hide == false & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name)
                    .OrderByDescending(x => x.sort)
                    .Select(x => new m_TestInfo
                    {
                        test_info_id = x.test_info_id,
                        title = x.title,
                        list_intro = x.list_intro,
                        category = x.category
                    });

                if (category != null)
                {
                    items = items.Where(x => x.category == category);
                }

                var nowpage = page == null ? 1 : (int)page;
                int startRecord = PageCount.PageInfo(nowpage, 10, items.Count());
                var resultItems = items.Skip(startRecord).Take(10);

                var GridInfo = new
                {
                    rows = resultItems.ToList(),
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                };

                foreach (var item in GridInfo.rows)
                {
                    item.imgsrc = GetImg(item.test_info_id, "Photo1", "TestInfo", "Photo");//顯示列表圖
                }

                return Ok(GridInfo);
            }
            #endregion
        }
        private string GetImg(int id, string file_kind, string category1, string category2)
        {
            string tpl_path = "~/_Upload/" + category1 + "/" + category2 + "/" + id + "/" + file_kind;
            string img_folder = HttpContext.Current.Server.MapPath(tpl_path);

            if (Directory.Exists(img_folder))
            {
                var get_files = Directory.EnumerateFiles(img_folder)
                    .Where(x => x.EndsWith("jpg") || x.EndsWith("jpeg") || x.EndsWith("png") || x.EndsWith("gif"))
                    .FirstOrDefault();

                if (get_files != null)
                {
                    FileInfo file_info = new FileInfo(get_files);
                    return Url.Content(tpl_path + "\\" + file_info.Name);
                }
                else
                {
                    return Url.Content("../../Content/images/no-pic.jpg");
                }
            }
            else
            {
                return Url.Content("../../Content/images/no-pic.jpg");
            }
        }
        private string[] GetImgs(int id, string file_kind, string category1, string category2, string size)
        {
            string tpl_path = "~/_Upload/" + category1 + "/" + category2 + "/" + id + "/" + file_kind;
            string web_folder = Url.Content(tpl_path);
            if (size != null) { web_folder = Url.Content(tpl_path + "/" + size); }
            string server_folder = HttpContext.Current.Server.MapPath(tpl_path);
            string file_json_server_path = server_folder + "//file.json";

            if (File.Exists(file_json_server_path))
            {
                var read_json = System.IO.File.ReadAllText(file_json_server_path);
                var f = JsonConvert.DeserializeObject<IList<JsonFileInfo>>(read_json).OrderBy(x => x.sort);
                IList<string> image_path = new List<string>();
                foreach (var fobj in f)
                {
                    image_path.Add(web_folder + "//" + fobj.fileName);
                }
                return image_path.ToArray();
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 移除html tag
        /// </summary>
        /// <param name="htmlSource"></param>
        /// <returns></returns>
        public static string RemoveHTMLTag(string htmlSource)
        {
            //移除  javascript code.
            //htmlSource = Regex.Replace(htmlSource, @"<script[\d\D]*?>[\d\D]*?</script>", String.Empty);

            //移除html tag.
            htmlSource = Regex.Replace(htmlSource, @"<[^>]*>", String.Empty);
            return htmlSource;
        }
        [HttpGet]
        public async Task<IHttpActionResult> ChangeLangVal()
        {
            db0 = getDB0();
            try
            {
                var items = db0.StudyAbroad.OrderBy(x => x.sort);
                foreach (var i in items)
                {
                    int category = 0;

                    switch (i.country_category)
                    {
                        case 2001:
                            category = 2009;
                            break;
                        case 2002:
                            category = 2016;
                            break;
                        case 2003:
                            category = 2010;
                            break;
                        case 2004:
                            category = 2011;
                            break;
                        case 2005:
                            category = 2012;
                            break;
                        case 2006:
                            category = 2013;
                            break;
                        case 2007:
                            category = 2014;
                            break;
                        case 2008:
                            category = 2015;
                            break;
                    }


                    if (!i.i_Hide)
                    {
                        var item = new StudyAbroad()
                        {
                            study_abroad_id = GetNewId(ProcCore.Business.CodeTable.StudyAbroad),
                            planning_name = ProcCore.NetExtension.ExtensionString.ToSimplified(i.planning_name),
                            intro = ProcCore.NetExtension.ExtensionString.ToSimplified(i.intro),
                            intro_titile = ProcCore.NetExtension.ExtensionString.ToSimplified(i.intro_titile),
                            intro_content = ProcCore.NetExtension.ExtensionString.ToSimplified(i.intro_content),
                            start_date = i.start_date,
                            end_date = i.end_date,
                            is_hot = i.is_hot,
                            vacation_category = i.vacation_category,
                            country_category = category,
                            youtube_iframe = i.youtube_iframe,
                            curriculum = ProcCore.NetExtension.ExtensionString.ToSimplified(i.curriculum),
                            lodging = ProcCore.NetExtension.ExtensionString.ToSimplified(i.lodging),
                            activity = ProcCore.NetExtension.ExtensionString.ToSimplified(i.activity),
                            sort = i.sort,
                            i_Hide = i.i_Hide,
                            i_InsertUserID = this.UserId,
                            i_InsertDateTime = DateTime.Now,
                            i_InsertDeptID = this.departmentId,
                            i_Lang = "zh-CN"
                        };
                        db0.StudyAbroad.Add(item);
                    }

                }

                await db0.SaveChangesAsync();

                return Ok(new { result = false });
            }
            catch (Exception ex)
            {
                return Ok(new { result = false, data = ex.Message });
            }
            finally
            {
                db0.Dispose();
            }
        }

    }
    class ccc
    {
        public int id { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string title { get; set; }
        public object intro { get; set; }
        public string imgsrc { get; set; }
    }
    public class CategroySort
    {
        public int id { get; set; }
        public int sort { get; set; }
    }
}
