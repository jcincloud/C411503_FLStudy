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
    public class HelpfulInfoController : BaseController
    {
        #region Action and function section
        public ActionResult Main()
        {
            ActionRun();
            return View();
        }
        public ActionResult P1()
        {
            ActionRun();
            return View();
        }
        public ActionResult P2()
        {
            ActionRun();
            return View();
        }
        public ActionResult P3()
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
                var options_category = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.HelpfulInfo && x.i_Hide == false).OrderBy(x => x.sort).Select(x => new option() { val = x.all_category_l2_id, Lname = x.l2_name }).ToList();
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
        #endregion

        #region ajax file section
        [HttpPost]
        public string axFUpload(int id, string filekind, string filename)
        {
            UpFileInfo r = new UpFileInfo();
            #region
            try
            {
                //內頁 圖片
                if (filekind == "Photo2")
                    handleImageSave(filename, id, ImageFileUpParm.Photo, filekind, "HelpfulInfo", "Photo");

                //代表 圖片
                if (filekind == "Photo1")
                    handleImageSave(filename, id, ImageFileUpParm.Photo, filekind, "HelpfulInfo", "Photo");

                r.result = true;
                r.file_name = filename;
            }
            catch (LogicError ex)
            {
                r.result = false;
                r.message = getRecMessage(ex.Message);
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
            }
            #endregion
            return defJSON(r);
        }

        [HttpPost]
        public string axFList(int id, string filekind)
        {
            SerializeFileList r = new SerializeFileList();

            if (filekind == "Photo1")
                r.files = listImgFiles(id, filekind, "HelpfulInfo", "Photo");

            if (filekind == "Photo2")
                r.files = listImgFiles(id, filekind, "HelpfulInfo", "Photo");

            r.result = true;
            return defJSON(r);
        }

        [HttpPost]
        public string axFDelete(int id, string filekind, string filename)
        {
            ResultInfo r = new ResultInfo();

            if (filekind == "Photo2")
                DeleteSysFile(id, filekind, filename, ImageFileUpParm.Photo, "HelpfulInfo", "Photo");

            if (filekind == "Photo1")
                DeleteSysFile(id, filekind, filename, ImageFileUpParm.Photo, "HelpfulInfo", "Photo");

            r.result = true;
            return defJSON(r);
        }

        [HttpPost]
        public string axFSort(int id, string filekind, IList<JsonFileInfo> file_object)
        {
            ResultInfo r = new ResultInfo();

            if (filekind == "Photo1")
                rewriteJsonFile(id, filekind, "HelpfulInfo", "Photo", file_object);

            if (filekind == "Photo2")
                rewriteJsonFile(id, filekind, "HelpfulInfo", "Photo", file_object);

            r.result = true;
            return defJSON(r);
        }

        [HttpGet]
        public FileResult axFDown(int id, string filekind, string filename)//下載附件檔案內容用(與圖片上傳無關)
        {
            string path_tpl = string.Format(upload_path_tpl_o, "HelpfulInfo", "Photo", id, filekind, filename);
            string server_path = Server.MapPath(path_tpl);
            FileInfo file_info = new FileInfo(server_path);
            FileStream file_stream = new FileStream(server_path, FileMode.Open, FileAccess.Read);
            string web_path = Url.Content(path_tpl);
            return File(file_stream, "application/*", file_info.Name);
        }
        #endregion
    }
}