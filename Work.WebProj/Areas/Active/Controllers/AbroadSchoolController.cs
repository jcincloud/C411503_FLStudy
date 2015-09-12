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
    public class AbroadSchoolController : BaseController
    {
        #region Action and function section
        public ActionResult Main()
        {
            ActionRun();
            return View();
        }
        public ActionResult Language()
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
                    options_country_category = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.Country & x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name).OrderByDescending(x => x.sort).Select(x => new option() { val = x.all_category_l2_id, Lname = x.l2_name }),
                    //lang_country = db0.All_Category_L2.Where(x => x.all_category_l1_id == CategoryType.Country).OrderByDescending(x => x.sort).GroupBy(x => x.i_Lang).Select(x => new { lang = x.Key, items = x.Select(y => new option() { val = y.all_category_l2_id, Lname = y.l2_name }) }),
                    tmp_info = (string)open.getParmValue(ParmDefine.Information),
                    tmp_apply = (string)open.getParmValue(ParmDefine.Apply)
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
                //內頁校徽 圖片
                if (filekind == "Photo3")
                    handleImageSave(filename, id, ImageFileUpParm.Badge, filekind, "AbroadSchool", "Photo");

                //代表 圖片
                if (filekind == "Photo1")
                    handleImageSave(filename, id, ImageFileUpParm.Photo, filekind, "AbroadSchool", "Photo");

                //照片集錦 圖片
                if (filekind == "Photo2")
                    handleImageSave(filename, id, ImageFileUpParm.PhotoCollectionB, filekind, "AbroadSchool", "Photo");

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

            if (filekind == "Photo3")
                r.files = listImgFiles(id, filekind, "AbroadSchool", "Photo");

            if (filekind == "Photo1")
                r.files = listImgFiles(id, filekind, "AbroadSchool", "Photo");

            if (filekind == "Photo2")
                r.files = listImgFiles(id, filekind, "AbroadSchool", "Photo");

            r.result = true;
            return defJSON(r);
        }

        [HttpPost]
        public string axFDelete(int id, string filekind, string filename)
        {
            ResultInfo r = new ResultInfo();

            if (filekind == "Photo3")
                DeleteSysFile(id, filekind, filename, ImageFileUpParm.Badge, "AbroadSchool", "Photo");

            if (filekind == "Photo1")
                DeleteSysFile(id, filekind, filename, ImageFileUpParm.Photo, "AbroadSchool", "Photo");

            if (filekind == "Photo2")
                DeleteSysFile(id, filekind, filename, ImageFileUpParm.PhotoCollectionB, "AbroadSchool", "Photo");

            r.result = true;
            return defJSON(r);
        }

        [HttpPost]
        public string axFSort(int id, string filekind, IList<JsonFileInfo> file_object)
        {
            ResultInfo r = new ResultInfo();
            if (filekind == "Photo3")
                rewriteJsonFile(id, filekind, "AbroadSchool", "Photo", file_object);

            if (filekind == "Photo1")
                rewriteJsonFile(id, filekind, "AbroadSchool", "Photo", file_object);

            if (filekind == "Photo2")
                rewriteJsonFile(id, filekind, "AbroadSchool", "Photo", file_object);

            r.result = true;
            return defJSON(r);
        }

        [HttpGet]
        public FileResult axFDown(int id, string filekind, string filename)//下載附件檔案內容用(與圖片上傳無關)
        {
            string path_tpl = string.Format(upload_path_tpl_o, "AbroadSchool", "Badge", id, filekind, filename);
            string server_path = Server.MapPath(path_tpl);
            FileInfo file_info = new FileInfo(server_path);
            FileStream file_stream = new FileStream(server_path, FileMode.Open, FileAccess.Read);
            string web_path = Url.Content(path_tpl);
            return File(file_stream, "application/*", file_info.Name);
        }
        #endregion
    }
}