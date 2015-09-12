using DotWeb.CommSetup;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using ProcCore;
using ProcCore.Business;
using ProcCore.Business.DB0;
using ProcCore.Business.LogicConect;
using ProcCore.HandleResult;
using ProcCore.NetExtension;
using ProcCore.WebCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace DotWeb.Controller
{
    #region 基底控制器
    public abstract class SourceController : System.Web.Mvc.Controller
    {
        //protected string IP;
        protected C41A0_FLStudyEntities db0;
        protected virtual string getRecMessage(string MsgId)
        {
            String r = Resources.Res.ResourceManager.GetString(MsgId);
            return String.IsNullOrEmpty(r) ? MsgId : r;
        }
        protected string defJSON(object o)
        {
            return JsonConvert.SerializeObject(o, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
        protected TransactionScope defAsyncScope()
        {
            return new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }
        protected virtual LogicCenter openLogic()
        {
            LogicCenter dbLogic = new LogicCenter(CommSetup.CommWebSetup.DB0_CodeString);

            dbLogic.IP = System.Web.HttpContext.Current.Request.UserHostAddress;

            return dbLogic;
        }
        protected string getNowLnag()
        {
            return System.Globalization.CultureInfo.CurrentCulture.Name;
        }
        protected static C41A0_FLStudyEntities getDB0()
        {
            LogicCenter.SetDB0EntityString(CommSetup.CommWebSetup.DB0_CodeString);
            return LogicCenter.getDB0;
        }
    }
    [Authorize]
    public abstract class BaseController : SourceController
    {
        protected string UserId; //指的是廠商登錄帳號
        protected string aspUserId;
        protected int departmentId;

        protected int defPageSize = 0;

        //訂義取得本次執行 Controller Area Action名稱
        protected string getController = string.Empty;
        protected string getArea = string.Empty;
        protected string getAction = string.Empty;

        //訂義檔案上傳路行樣板
        protected string upload_path_tpl_o = "~/_Upload/{0}/{1}/{2}/{3}/{4}";
        protected string upload_path_tpl_s = "~/_Upload/{0}/{1}/{2}/{3}";
        //訂義檔案刪除路徑樣板
        protected string delete_file_path_tpl = "~/_Upload/{0}/{1}/{2}";

        //系統認可圖片檔副檔名
        protected string[] imgExtDef = new string[] { ".jpg", ".jpeg", ".gif", ".png", ".bmp" };

        //protected Log.LogPlamInfo plamInfo = new Log.LogPlamInfo() { AllowWrite = true };
        private ApplicationUserManager _userManager;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var getUserIdCookie = Request.Cookies["user_id"];
            var getUserName = Request.Cookies["user_name"];
            UserId = getUserIdCookie == null ? null : getUserIdCookie.Value;

            ViewBag.UserId = UserId;
            ViewBag.UserName = getUserName == null ? "" : Server.UrlDecode(getUserName.Value);

            var aspnet_user_id = User.Identity.GetUserId();
            ApplicationUser aspnet_user = UserManager.FindById(aspnet_user_id);
            string asp_net_roles = aspnet_user.Roles.Select(x => x.RoleId).FirstOrDefault();
            var role = roleManager.FindById(asp_net_roles);
            ViewBag.RoleName = role.Name;
            ViewBag.Lang = System.Globalization.CultureInfo.CurrentCulture.Name;

            if (MvcSiteMapProvider.SiteMaps.Current.CurrentNode != null && MvcSiteMapProvider.SiteMaps.Current.CurrentNode.ParentNode != null)
            {
                ViewBag.Caption = MvcSiteMapProvider.SiteMaps.Current.CurrentNode.Title;
                ViewBag.MenuName = MvcSiteMapProvider.SiteMaps.Current.CurrentNode.ParentNode.Title;
            }
            else
            {
                ViewBag.Caption = Resources.Res.ViewbagCapton;
                ViewBag.MenuName = Resources.Res.ViewbagMenuName;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public RoleManager<IdentityRole> roleManager
        {
            get
            {
                ApplicationDbContext context = ApplicationDbContext.Create();
                return new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            }
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            this.aspUserId = User.Identity.GetUserId();
            this.departmentId = int.Parse(Request.Cookies[CommWebSetup.Cookie_DepartmentId].Value);

            Log.SetupBasePath = System.Web.HttpContext.Current.Server.MapPath("~\\_Code\\Log\\");
            Log.Enabled = true;

            //plamInfo.BroswerInfo = System.Web.HttpContext.Current.Request.Browser.Browser + "." + System.Web.HttpContext.Current.Request.Browser.Version;
            //plamInfo.IP = this.IP;

            //plamInfo.UnitId = departmentId;

            defPageSize = CommSetup.CommWebSetup.MasterGridDefPageSize;
            this.getController = ControllerContext.RouteData.Values["controller"].ToString();
            this.getArea = ControllerContext.RouteData.DataTokens["area"].ToString();
            this.getAction = ControllerContext.RouteData.Values["action"].ToString();
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            Log.WriteToFile();
        }
        protected void ActionRun()
        {
            ViewBag.area = this.getArea;
            ViewBag.controller = this.getController;
            using (db0 = getDB0())
            {
                ViewBag.Langs = db0.i_Lang.Where(x => x.isuse == true).OrderBy(x => x.sort).ToList();
            }
        }
        public int getNewId()
        {
            return getNewId(ProcCore.Business.CodeTable.Base);
        }
        public int getNewId(ProcCore.Business.CodeTable tab)
        {

            //using (TransactionScope tx = new TransactionScope())
            //{
            var db = getDB0();
            try
            {
                string tab_name = Enum.GetName(typeof(ProcCore.Business.CodeTable), tab);
                var items = db.i_IDX.Where(x => x.table_name == tab_name).FirstOrDefault();

                if (items == null)
                {
                    return 0;
                }
                else
                {
                    //var item = items.FirstOrDefault();
                    items.IDX++;
                    db.SaveChanges();
                    //tx.Complete();
                    return items.IDX;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            finally
            {
                db.Dispose();
            }
            //}
        }
        protected void handleFileSave(string file_name, int id, FilesUpScope fp, string file_kind, string category1, string category2)
        {
            Stream file_stream = Request.InputStream;
            BinaryReader binary_read = new BinaryReader(file_stream);
            string file_ext = System.IO.Path.GetExtension(file_name);

            #region IE file stream handle

            string[] IEOlderVer = new string[] { "6.0", "7.0", "8.0", "9.0" };
            System.Web.HttpPostedFile GetPostFile = null;
            if (Request.Browser.Browser == "IE" && IEOlderVer.Any(x => x == Request.Browser.Version))
            {
                System.Web.HttpFileCollection collectFiles = System.Web.HttpContext.Current.Request.Files;
                GetPostFile = collectFiles[0];
                if (!GetPostFile.FileName.Equals(""))
                {
                    binary_read = new BinaryReader(GetPostFile.InputStream);
                }
            }

            Byte[] fileContents = { };

            while (binary_read.BaseStream.Position < binary_read.BaseStream.Length - 1)
            {
                Byte[] buffer = new Byte[binary_read.BaseStream.Length - 1];
                int read_line = binary_read.Read(buffer, 0, buffer.Length);
                Byte[] dummy = fileContents.Concat(buffer).ToArray();
                fileContents = dummy;
                dummy = null;
            }
            #endregion

            string web_path_org = string.Format(upload_path_tpl_s, category1, category2, id, file_kind);
            string server_path_org = Server.MapPath(web_path_org);

            #region 檔案上傳前檢查
            if (fp.limitSize > 0)
                if (binary_read.BaseStream.Length > fp.limitSize)
                    throw new LogicError("Log_Err_FileSizeOver");

            if (fp.limitCount > 0 && Directory.Exists(server_path_org))
            {
                string[] Files = Directory.GetFiles(server_path_org);
                if (Files.Count() >= fp.limitCount)
                    throw new LogicError("Log_Err_FileCountOver");
            }

            if (fp.allowExtType != null)
                if (!fp.allowExtType.Contains(file_ext.ToLower()))
                    throw new LogicError("Log_Err_AllowFileType");

            if (fp.limitExtType != null)
                if (fp.limitExtType.Contains(file_ext))
                    throw new LogicError("Log_Err_LimitedFileType");
            #endregion

            #region 存檔區

            if (!System.IO.Directory.Exists(server_path_org)) { System.IO.Directory.CreateDirectory(server_path_org); }

            FileStream write_stream = new FileStream(server_path_org + "\\" + file_name, FileMode.Create);
            BinaryWriter binary_write = new BinaryWriter(write_stream);
            binary_write.Write(fileContents);

            file_stream.Close();
            write_stream.Close();
            binary_write.Close();

            #endregion
        }
        protected void handleImageSave(string file_name, int id, ImageUpScope fp, string file_kind, string category1, string category2)
        {
            BinaryReader binary_read = null;
            string file_ext = System.IO.Path.GetExtension(file_name); //取得副檔名
            string[] ie_older_ver = new string[] { "6.0", "7.0", "8.0", "9.0" };

            if (Request.Browser.Browser == "IE" && ie_older_ver.Any(x => x == Request.Browser.Version))
            {
                #region IE file stream handle
                HttpPostedFile get_post_file = System.Web.HttpContext.Current.Request.Files[0];
                if (!get_post_file.FileName.Equals(""))
                    binary_read = new BinaryReader(get_post_file.InputStream);
                #endregion
            }
            else
                binary_read = new BinaryReader(Request.InputStream);

            byte[] upload_file = binary_read.ReadBytes(System.Convert.ToInt32(binary_read.BaseStream.Length));

            string web_path_org = string.Format(upload_path_tpl_o, category1, category2, id, file_kind, "origin");
            string server_path_org = Server.MapPath(web_path_org);

            #region 檔案上傳前檢查
            if (fp.limitSize > 0) //檔案大小檢查
                if (binary_read.BaseStream.Length > fp.limitSize)
                    throw new LogicError("Log_Err_FileSizeOver");

            if (fp.limitCount > 0 && Directory.Exists(server_path_org))
            {
                string[] Files = Directory.GetFiles(server_path_org);
                if (Files.Count() >= fp.limitCount) //還沒存檔，因此Selet到等於的數量，再加上現在要存的檔案即算超過
                    throw new LogicError("Log_Err_FileCountOver");
            }

            if (fp.allowExtType != null)
                if (!fp.allowExtType.Contains(file_ext.ToLower()))
                    throw new LogicError("Log_Err_AllowFileType");

            if (fp.limitExtType != null)
                if (fp.limitExtType.Contains(file_ext))
                    throw new LogicError("Log_Err_LimitedFileType");
            #endregion
            #region 存檔區

            if (fp.keepOrigin)
            {
                //原始檔
                if (!System.IO.Directory.Exists(server_path_org)) { System.IO.Directory.CreateDirectory(server_path_org); }

                FileStream file_stream = new FileStream(server_path_org + "\\" + file_name, FileMode.Create);
                BinaryWriter binary_write = new BinaryWriter(file_stream);
                binary_write.Write(upload_file);

                file_stream.Close();
                binary_write.Close();
            }

            //後台管理的ICON小圖
            string web_path_icon = string.Format(upload_path_tpl_o, category1, category2, id, file_kind, "icon");
            string server_path_icon = Server.MapPath(web_path_icon);
            if (!System.IO.Directory.Exists(server_path_icon)) { System.IO.Directory.CreateDirectory(server_path_icon); }
            MemoryStream smr = resizeImage(upload_file, 0, 90);
            System.IO.File.WriteAllBytes(server_path_icon + "\\" + Path.GetFileName(file_name), smr.ToArray());
            smr.Dispose();

            //依據參數進行裁圖
            if (fp.Parm.Count() > 1)//存兩種以上圖片大小
            {
                foreach (ImageSizeParm imSize in fp.Parm)
                {
                    string web_path_parm = string.Format(upload_path_tpl_o, category1, category2, id, file_kind, imSize.folderName);
                    string server_path_parm = Server.MapPath(web_path_parm);
                    if (!System.IO.Directory.Exists(server_path_parm)) { System.IO.Directory.CreateDirectory(server_path_parm); }//找不到路徑
                    MemoryStream sm = resizeImage(upload_file, imSize.width, imSize.heigh);
                    System.IO.File.WriteAllBytes(server_path_parm + "\\" + Path.GetFileName(file_name), sm.ToArray());
                    sm.Dispose();
                }
            }
            else if (fp.Parm.Count() > 0)
            {//只存一種
                string web_path_parm = string.Format(upload_path_tpl_s, category1, category2, id, file_kind);
                string server_path_parm = Server.MapPath(web_path_parm);
                foreach (ImageSizeParm imSize in fp.Parm)
                {
                    MemoryStream sm = resizeImage(upload_file, imSize.width, imSize.heigh);
                    System.IO.File.WriteAllBytes(server_path_parm + "\\" + Path.GetFileName(file_name), sm.ToArray());
                    sm.Dispose();
                }

            }
            #endregion

            #region Handle Json Info
            string file_json_web_path = string.Format(upload_path_tpl_s, category1, category2, id, file_kind);
            string file_json_server_path = Server.MapPath(file_json_web_path) + "\\file.json";

            IList<JsonFileInfo> f = null;
            int sort = 0;
            if (System.IO.File.Exists(file_json_server_path))
            {
                var read_json = System.IO.File.ReadAllText(file_json_server_path);
                f = JsonConvert.DeserializeObject<IList<JsonFileInfo>>(read_json);
                if (f.Any(x => x.fileName == file_name))
                {
                    return;
                }

                sort = f.Count + 1;
            }
            else
            {
                f = new List<JsonFileInfo>();
                sort = 1;
            }



            f.Add(new JsonFileInfo()
            {
                fileName = file_name,
                sort = sort
            });

            var json_string = JsonConvert.SerializeObject(f);
            System.IO.File.WriteAllText(file_json_server_path, json_string, Encoding.UTF8);
            #endregion
        }
        protected MemoryStream resizeImage(Byte[] s, int new_width, int new_hight)
        {
            try
            {
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                Bitmap im = (Bitmap)tc.ConvertFrom(s);

                if (new_hight == 0)
                    new_hight = (im.Height * new_width) / im.Width;

                if (new_width == 0)
                    new_width = (im.Width * new_hight) / im.Height;

                if (im.Width < new_width)
                    new_width = im.Width;

                if (im.Height < new_hight)
                    new_hight = im.Height;

                EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
                EncoderParameters myEncoderParameter = new EncoderParameters(1);
                myEncoderParameter.Param[0] = qualityParam;

                ImageCodecInfo myImageCodecInfo = getEncoder(im.RawFormat);

                Bitmap ImgOutput = new Bitmap(im, new_width, new_hight);

                //ImgOutput.Save();
                MemoryStream ss = new MemoryStream();

                ImgOutput.Save(ss, myImageCodecInfo, myEncoderParameter);
                im.Dispose();
                return ss;
            }
            catch (Exception ex)
            {
                Log.Write("Image Handle Error:" + ex.Message);
                return null;
            }
            //ImgOutput.Dispose(); 
        }
        private ImageCodecInfo getEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        protected MemoryStream cropCenterImage(Byte[] s, int width, int heigh)
        {
            try
            {
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));

                Bitmap ImgSource = (Bitmap)tc.ConvertFrom(s);
                Bitmap ImgOutput = new Bitmap(width, heigh);

                int x = (ImgSource.Width - width) / 2;
                int y = (ImgSource.Height - heigh) / 2;
                Rectangle cropRect = new Rectangle(x, y, width, heigh);

                using (Graphics g = Graphics.FromImage(ImgOutput))
                {
                    g.DrawImage(ImgSource, new Rectangle() { Height = heigh, Width = width, X = 0, Y = 0 }, cropRect, GraphicsUnit.Pixel);
                }

                MemoryStream ss = new MemoryStream();
                ImgOutput.Save(ss, ImgSource.RawFormat);
                ImgSource.Dispose();
                return ss;
            }
            catch (Exception ex)
            {
                Log.Write("Image Handle Error:" + ex.Message);
                return null;
            }
            //ImgOutput.Dispose(); 
        }
        protected SerializeFile[] listImgFiles(int id, string file_kind, string category1, string category2)
        {
            string web_path_org = string.Format(upload_path_tpl_o, category1, category2, id, file_kind, "origin");
            string server_path_org = Server.MapPath(web_path_org);
            string web_path_icon = string.Format(upload_path_tpl_o, category1, category2, id, file_kind, "icon");

            List<SerializeFile> l_files = new List<SerializeFile>();

            string file_json_web_path = string.Format(upload_path_tpl_s, category1, category2, id, file_kind);
            string file_json_server_path = Server.MapPath(file_json_web_path) + "\\file.json";

            string web_path_s = string.Format(upload_path_tpl_s, category1, category2, id, file_kind, "origin");
            string server_path_s = Server.MapPath(web_path_s);

            if (System.IO.File.Exists(file_json_server_path))
            {
                var read_json = System.IO.File.ReadAllText(file_json_server_path);
                var get_file_json_object = JsonConvert.DeserializeObject<IList<JsonFileInfo>>(read_json).OrderBy(x => x.sort);
                foreach (var m in get_file_json_object)
                {
                    string get_file = server_path_org + "//" + m.fileName;
                    if (System.IO.File.Exists(get_file))
                    {
                        FileInfo file_info = new FileInfo(get_file);
                        SerializeFile file_object = new SerializeFile()
                        {
                            fileName = file_info.Name,
                            fileKind = file_kind,
                            iconPath = Url.Content(web_path_icon + "/" + file_info.Name),
                            originPath = Url.Content(web_path_org + "/" + file_info.Name),
                            size = file_info.Length,
                            isImage = true
                        };
                        l_files.Add(file_object);
                    }
                }
            }

            return l_files.ToArray();
        }
        protected SerializeFile[] listDocFiles(int id, string file_kind, string category1, string category2)
        {
            string tpl_folder_path = string.Empty;
            string server_path = string.Empty;

            tpl_folder_path = string.Format(upload_path_tpl_s, category1, category2, id, file_kind);
            server_path = Server.MapPath(tpl_folder_path);

            List<SerializeFile> ls_files = new List<SerializeFile>();

            if (Directory.Exists(server_path))
            {
                foreach (string fileString in Directory.GetFiles(server_path))
                {
                    FileInfo file_info = new FileInfo(fileString);

                    ls_files.Add(new SerializeFile()
                    {
                        fileName = file_info.Name,
                        fileKind = file_kind,
                        iconPath = Url.Content(tpl_folder_path + "/" + file_info.Name),
                        originPath = Url.Content(tpl_folder_path + "/" + file_info.Name),
                        size = file_info.Length,
                        isImage = false
                    });
                }
            }
            return ls_files.ToArray();
        }
        protected void DeleteSysFile(int Id, string fileskind, string filename, ImageUpScope im)
        {
            string SystemDelSysIdKind = "~/_Upload/{0}/{1}/{2}/{3}";
            string tpl_FolderPath = Server.MapPath(String.Format(SystemDelSysIdKind, getArea, getController, Id, fileskind));
            #region Delete Run
            if (Directory.Exists(tpl_FolderPath))
            {
                var folders = Directory.GetDirectories(tpl_FolderPath);
                foreach (var folder in folders)
                {
                    String herefile = folder + "\\" + filename;
                    if (System.IO.File.Exists(herefile))
                        System.IO.File.Delete(herefile);
                }
            }
            #endregion
        }
        protected void DeleteSysFile(int id, string file_kind, string file_name, ImageUpScope im, string category1, string category2)
        {
            string tpl_FolderPath = Server.MapPath(string.Format(upload_path_tpl_s, category1, category2, id, file_kind));

            string handle_delete_file = tpl_FolderPath + "/" + file_name;
            if (System.IO.File.Exists(handle_delete_file))
                System.IO.File.Delete(handle_delete_file);
            #region Delete Run
            if (Directory.Exists(tpl_FolderPath))
            {
                var folders = Directory.GetDirectories(tpl_FolderPath);
                foreach (var folder in folders)
                {
                    string herefile = folder + "\\" + file_name;
                    if (System.IO.File.Exists(herefile))
                        System.IO.File.Delete(herefile);
                }
            }
            #endregion

            #region Handle Json Info
            string file_json_web_path = string.Format(upload_path_tpl_s, category1, category2, id, file_kind);
            string file_json_server_path = Server.MapPath(file_json_web_path) + "\\file.json";

            IList<JsonFileInfo> get_file_json_object = null;
            if (System.IO.File.Exists(file_json_server_path))
            {
                var read_json = System.IO.File.ReadAllText(file_json_server_path);
                get_file_json_object = JsonConvert.DeserializeObject<IList<JsonFileInfo>>(read_json);
                var get_file_object = get_file_json_object.Where(x => x.fileName == file_name).FirstOrDefault();
                if (get_file_object != null)
                {
                    get_file_json_object.Remove(get_file_object);
                    int i = 1;
                    foreach (var file_object in get_file_json_object)
                    {
                        file_object.sort = i;
                        i++;
                    }
                    var json_string = JsonConvert.SerializeObject(get_file_json_object);
                    System.IO.File.WriteAllText(file_json_server_path, json_string, Encoding.UTF8);
                }
            }
            #endregion

        }
        protected void DeleteIdFiles(int Id)
        {
            String tpl_FolderPath = String.Empty;
            tpl_FolderPath = String.Format(delete_file_path_tpl, getArea, getController, Id);
            String Path = Server.MapPath(tpl_FolderPath);
            if (Directory.Exists(Path))
                Directory.Delete(Path, true);
        }
        public FileResult DownLoadFile(int Id, string FilesKind, string FileName)
        {
            String SearchPath = String.Format(upload_path_tpl_o + "\\" + FileName, getArea, getController, Id, FilesKind, "OriginFile");
            String DownFilePath = Server.MapPath(SearchPath);

            FileInfo fi = null;
            if (System.IO.File.Exists(DownFilePath))
                fi = new FileInfo(DownFilePath);

            return File(DownFilePath, "application/" + fi.Extension.Replace(".", ""), Url.Encode(fi.Name));
        }
        public string ImgSrc(string AreaName, string ContorllerName, int Id, string FilesKind, string ImageSize)
        {
            String ImgSizeString = ImageSize;
            String SearchPath = String.Format(upload_path_tpl_o, AreaName, ContorllerName, Id, FilesKind, ImgSizeString);
            String FolderPth = Server.MapPath(SearchPath);

            if (Directory.Exists(FolderPth))
            {
                String[] SFiles = Directory.GetFiles(FolderPth);

                if (SFiles.Length > 0)
                {
                    FileInfo f = new FileInfo(SFiles[0]);
                    return Url.Content(SearchPath) + "/" + f.Name;
                }
                else
                    return null;
            }
            else
                return null;
        }
        public PageImgShow[] ImgSrcApp(string AreaName, String ContorllerName, int Id, string FilesKind, string ImageSize)
        {
            String ImgSizeString = ImageSize;
            String ICON_Path = String.Format(upload_path_tpl_o, AreaName, ContorllerName, Id, FilesKind, "RepresentICON");
            String Link_Path = String.Format(upload_path_tpl_o, AreaName, ContorllerName, Id, FilesKind, ImgSizeString);

            String FolderPth = Server.MapPath(ICON_Path);

            if (Directory.Exists(FolderPth))
            {
                String[] SFiles = Directory.GetFiles(FolderPth);
                List<PageImgShow> web_path = new List<PageImgShow>();
                foreach (var file_path in SFiles)
                {
                    FileInfo f = new FileInfo(file_path);
                    web_path.Add(new PageImgShow()
                    {
                        icon_path = Url.Content(ICON_Path) + "/" + f.Name,
                        link_path = Url.Content(Link_Path) + "/" + f.Name
                    });
                }
                return web_path.ToArray();
            }
            else
                return null;
        }
        public RedirectResult SetLanguage(string L, string A)
        {
            HttpCookie WebLang = new HttpCookie(DotWeb.CommSetup.CommWebSetup.WebCookiesId + ".Lang", L);
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(WebLang.Value);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(WebLang.Value);
            ViewBag.Lang = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            Response.Cookies.Add(WebLang);
            return Redirect(A);
        }
        public string ModelStateErrorPack()
        {
            List<string> errMessage = new List<string>();
            foreach (ModelState modelState in ModelState.Values)
                foreach (ModelError error in modelState.Errors)
                    errMessage.Add(error.ErrorMessage);

            return string.Join(":", errMessage);
        }
        protected override LogicCenter openLogic()
        {
            var o = base.openLogic();
            o.AspUserID = aspUserId;
            o.DepartmentId = departmentId;
            o.Lang = getNowLnag();
            return o;
        }
        protected void rewriteJsonFile(int id, string file_kind, string category1, string category2, IList<JsonFileInfo> json_file_object)
        {
            string file_json_web_path = string.Format(upload_path_tpl_s, category1, category2, id, file_kind);
            string file_json_server_path = Server.MapPath(file_json_web_path) + "\\file.json";
            if (System.IO.File.Exists(file_json_server_path))
            {
                string json_string = JsonConvert.SerializeObject(json_file_object);
                System.IO.File.WriteAllText(file_json_server_path, json_string, Encoding.UTF8);
            }
        }
    }
    public abstract class WebFrontController : SourceController
    {
        protected int visitCount = 0;
        //protected Log.LogPlamInfo plamInfo = new Log.LogPlamInfo() { AllowWrite = true };
        //protected readonly string sessionShoppingString = "CestLaVie.Shopping";
        //protected readonly string sessionMemberLoginString = "CestLaVie.loginMail";
        private readonly string sysUpFilePathTpl = "~/_Code/SysUpFiles/{0}.{1}/{2}/{3}/{4}";
        protected WebInfo wi;

        protected WebFrontController()
        {
            ViewBag.NowHeadMenu = "";
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            //plamInfo.BroswerInfo = System.Web.HttpContext.Current.Request.Browser.Browser + "." + System.Web.HttpContext.Current.Request.Browser.Version;
            //plamInfo.IP = System.Web.HttpContext.Current.Request.UserHostAddress;
            //plamInfo.UserId = 0;
            //plamInfo.UnitId = 0;

            Log.SetupBasePath = System.Web.HttpContext.Current.Server.MapPath("~\\_Code\\Log\\");
            Log.Enabled = true;

            try
            {
                var db = getDB0();

                var Async = db.SaveChangesAsync();
                Async.Wait();

                ViewBag.VisitCount = visitCount;
                ViewBag.IsFirstPage = false; //是否為首頁，請在首頁的Action此值設為True
                ViewBag.Lang = System.Globalization.CultureInfo.CurrentCulture.Name;

                wi = new WebInfo();
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
            }
        }
        public int GetNewId()
        {
            return GetNewId(ProcCore.Business.CodeTable.Base);
        }
        public int GetNewId(ProcCore.Business.CodeTable tab)
        {
            using (var db = getDB0())
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    try
                    {
                        String tab_name = Enum.GetName(typeof(ProcCore.Business.CodeTable), tab);
                        var items = from x in db.i_IDX where x.table_name == tab_name select x;

                        if (items.Count() == 0)
                        {
                            return 0;
                        }
                        else
                        {
                            var item = items.FirstOrDefault();
                            item.IDX++;
                            db.SaveChanges();
                            tx.Complete();
                            return item.IDX;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return 0;
                    }
                }
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
        private SNObject GetSN(ProcCore.Business.SNType tab)
        {

            SNObject sn = new SNObject();

            using (var db = getDB0())
            {
                using (TransactionScope tx = new TransactionScope())
                {
                    try
                    {
                        String tab_name = Enum.GetName(typeof(ProcCore.Business.SNType), tab);
                        var items = db.i_SN.Single(x => x.sn_type == tab_name);

                        if (items.y == DateTime.Now.Year &&
                            items.m == DateTime.Now.Month &&
                            items.d == DateTime.Now.Day
                            )
                        {
                            int now_max = items.sn_max;
                            now_max++;
                            items.sn_max = now_max;
                        }
                        else
                        {
                            items.y = DateTime.Now.Year;
                            items.m = DateTime.Now.Month;
                            items.d = DateTime.Now.Day;
                            items.sn_max = 1;
                        }

                        db.SaveChanges();
                        tx.Complete();

                        sn.y = items.y;
                        sn.m = items.m;
                        sn.d = items.d;
                        sn.sn_max = items.sn_max;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return sn;
        }
        public string Get_Orders_SN()
        {
            String tpl = "SN{0}{1:00}{2:00}-{3:00}{4:00}";
            SNObject sn = GetSN(ProcCore.Business.SNType.Orders);
            return String.Format(tpl, sn.y.ToString().Right(2), sn.m, sn.d, sn.sn_max, (new Random()).Next(99));
        }
        public FileResult DownLoadFile(Int32 Id, String GetArea, String GetController, String FileName, String FilesKind)
        {
            if (FilesKind == null)
                FilesKind = "DocFiles";

            String SystemUpFilePathTpl = "~/_Code/SysUpFiles/{0}.{1}/{2}/{3}/{4}";
            String SearchPath = String.Format(SystemUpFilePathTpl + "\\" + FileName, GetArea, GetController, Id, FilesKind, "OriginFile");
            String DownFilePath = Server.MapPath(SearchPath);

            FileInfo fi = null;
            if (System.IO.File.Exists(DownFilePath))
            {
                fi = new FileInfo(DownFilePath);
            }
            return File(DownFilePath, "application/" + fi.Extension.Replace(".", ""), Url.Encode(fi.Name));
        }
        public String ImgSrc(String AreaName, String ContorllerName, Int32 Id, String FilesKind, Int32 ImageSizeTRype)
        {
            String ImgSizeString = "s_" + ImageSizeTRype;
            String SearchPath = String.Format(sysUpFilePathTpl, AreaName, ContorllerName, Id, FilesKind, ImgSizeString);
            String FolderPth = Server.MapPath(SearchPath);

            if (Directory.Exists(FolderPth))
            {
                String[] SFiles = Directory.GetFiles(FolderPth);

                if (SFiles.Length > 0)
                {
                    FileInfo f = new FileInfo(SFiles[0]);
                    return Url.Content(SearchPath) + "/" + f.Name;
                }
                else
                {
                    return Url.Content("~/Content/images/nopic.png");
                }

            }
            else
                return Url.Content("~/Content/images/nopic.png");
        }
        public string[] GetImgs(int id, string file_kind, string category1, string category2, string size)
        {
            string tpl_path = "~/_Upload/" + category1 + "/" + category2 + "/" + id + "/" + file_kind;
            string web_folder = Url.Content(tpl_path);
            if (size != null) { web_folder = Url.Content(tpl_path + "/" + size); }
            string server_folder = Server.MapPath(tpl_path);
            string file_json_server_path = server_folder + "//file.json";
            string[] imgs = { };
            if (System.IO.File.Exists(file_json_server_path))
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
                return imgs;
            }
        }
        public string GetImg(int id, string file_kind, string category1, string category2)
        {
            string tpl_path = "~/_Upload/" + category1 + "/" + category2 + "/" + id + "/" + file_kind;
            string img_folder = Server.MapPath(tpl_path);

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
        public FileResult AudioFile(String FilePath)
        {
            String S = Url.Content(FilePath);
            String DownFilePath = Server.MapPath(S);

            FileInfo fi = null;
            if (System.IO.File.Exists(DownFilePath))
                fi = new FileInfo(DownFilePath);

            return File(DownFilePath, "audio/mp3", Url.Encode(fi.Name));
        }
        public String GetSYSImage(Int32 Id, String GetArea, String GetController)
        {
            String SystemUpFilePathTpl = "~/_Code/SysUpFiles/{0}.{1}/{2}/{3}/{4}";
            String SearchPath = String.Format(SystemUpFilePathTpl, GetArea, GetController, Id, "DefaultKind", "OriginFile");
            String GetFolderPath = Server.MapPath(SearchPath);

            if (System.IO.Directory.Exists(GetFolderPath))
            {
                String fs = Directory.GetFiles(GetFolderPath).FirstOrDefault();
                FileInfo f = new FileInfo(fs);
                return SearchPath + "/" + f.Name;
            }
            else
            {
                return null;
            }
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            Log.WriteToFile();
        }
        public RedirectResult SetLanguage(String L, String A)
        {
            HttpCookie WebLang = new HttpCookie(DotWeb.CommSetup.CommWebSetup.WebCookiesId + ".Lang", L);
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(WebLang.Value);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(WebLang.Value);
            Response.Cookies.Add(WebLang);
            return Redirect(Url.Action(A));
        }
        protected override string getRecMessage(string MsgId)
        {
            return Resources.Res.ResourceManager.GetString(MsgId);
        }
        protected List<SelectListItem> MakeNumOptions(Int32 num, Boolean FirstIsBlank)
        {

            List<SelectListItem> r = new List<SelectListItem>();
            if (FirstIsBlank)
            {
                SelectListItem sItem = new SelectListItem();
                sItem.Value = "";
                sItem.Text = "";
                r.Add(sItem);
            }

            for (int n = 1; n <= num; n++)
            {
                SelectListItem s = new SelectListItem();
                s.Value = n.ToString();
                s.Text = n.ToString();
                r.Add(s);
            }
            return r;
        }

        protected void insertEmailContact(EmailContact md)
        {//新增信件內容致後台管理
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                using (var db = getDB0())
                {
                    md.email_contact_id = GetNewId(ProcCore.Business.CodeTable.EmailContact);
                    md.i_InsertDateTime = DateTime.Now;
                    md.i_Lang = "zh-TW";
                    db.EmailContact.Add(md);
                    db.SaveChangesAsync();
                }
                rAjaxResult.result = true;
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
            }
        }

        #region 寄信相關
        //將變數套用至信件版面
        public string getMailBody(string EmailView, object md)
        {
            ViewResult resultView = View(EmailView, md);

            StringResult sr = new StringResult();
            sr.ViewName = resultView.ViewName;
            sr.MasterName = resultView.MasterName;
            sr.ViewData = resultView.ViewData;
            sr.TempData = resultView.TempData;
            sr.ExecuteResult(this.ControllerContext);

            return sr.ToHtmlString;
        }

        public bool Mail_Send(string MailFrom, string[] MailTos, string MailSub, string MailBody, bool isBodyHtml)
        {
            try
            {
                //建立MailMessage物件
                MailMessage mms = new MailMessage();
                mms.From = new MailAddress(MailFrom);//寄件人
                mms.Subject = MailSub;//信件主旨
                mms.Body = MailBody;//信件內容
                mms.IsBodyHtml = isBodyHtml;//判斷是否採用html格式

                if (MailTos != null)//防呆
                {
                    foreach (var str in MailTos)
                    {
                        if (str != "")
                        {
                            var m = str.Split(':');
                            if (m.Length == 2)
                            {
                                mms.To.Add(new MailAddress(m[1], m[0]));
                            }
                            else if (m.Length == 1)
                            {
                                mms.To.Add(new MailAddress(m[0]));
                            }
                        }
                    }
                }//End if (MailTos !=null)//防呆

                //if (Bcc != null) //防呆
                //{
                //    for (int i = 0; i < Bcc.Length; i++)
                //    {
                //        if (!string.IsNullOrEmpty(Bcc[i].Trim()))
                //        {
                //            //加入信件的密件副本(們)address
                //            mms.Bcc.Add(new MailAddress(Bcc[i].Trim()));
                //        }

                //    }
                //}//End if (Ccs!=null) //防呆



                using (SmtpClient client = new SmtpClient(CommWebSetup.MailServer))//或公司、客戶的smtp_server

                    client.Send(mms);//寄出一封信

                //釋放每個附件，才不會Lock住
                if (mms.Attachments != null && mms.Attachments.Count > 0)
                {
                    for (int i = 0; i < mms.Attachments.Count; i++)
                    {
                        mms.Attachments[i].Dispose();
                        mms.Attachments[i] = null;
                    }
                }

                return true;//寄信成功
            }
            catch (Exception)
            {
                return false;//寄信失敗
            }
        }

        #endregion
    }
    #endregion

    #region 泛型控制器擴充
    public abstract class CtrlTSN<M, Q> : BaseController
        where M : new()
        where Q : QueryBase
    {
        protected ResultInfo<M> r;
        protected M item;
        public abstract string aj_Init();
        public abstract Task<string> aj_MasterDel(string[] sns);
        public abstract string aj_MasterSearch(Q sh);
        public abstract Task<string> aj_MasterInsert(M md);
        public abstract Task<string> aj_MasterUpdate(M md);
        public abstract Task<string> aj_MasterGet(string sn);
    }
    #endregion
    public class DocInfo
    {
        public String Name { get; set; }
        public int Sort { get; set; }
        public String Momo { get; set; }
        public String Link { get; set; }
    }
}