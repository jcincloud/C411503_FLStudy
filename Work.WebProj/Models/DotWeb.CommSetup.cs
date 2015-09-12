using ProcCore.WebCore;
using System;

namespace DotWeb.CommSetup
{
    public static class CommWebSetup
    {
        private static string GetKeyValue(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
        public static string AutoLoginUser
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DEV_USER"];
            }
        }
        public static string AutoLoginPassword
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DEV_PWD"];
            }
        }
        public static string WebCookiesId
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["WebCookiesId"];
            }
        }
        public static string ManageDefCTR
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ManageDefCTR"];
            }
        }
        public static string UserLoginSource
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UserLoginSource"];
            }
        }
        public static DateTime Expire
        {
            get
            {
                return DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["Expire"]);
            }
        }
        public static int MasterGridDefPageSize
        {
            get
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"]);
            }
        }
        public static string Cookie_UserName
        {
            get
            {
                return "Cookie_UserName";
            }
        }
        public static string Cookie_LastLogin
        {
            get
            {
                return "Cookie_LastLogin";
            }
        }
        public static string Cookie_DepartmentId
        {
            get
            {
                return "Cookie_DepartmentId";
            }
        }
        public static string Cookie_DepartmentName
        {
            get
            {
                return "Cookie_DepartmentName";
            }
        }
        public static string CacheVer
        {
            get
            {
                return GetKeyValue("CacheVer");
            }
        }
        public static int Limit_Max_Apply_Days
        {
            get
            {
                return int.Parse(GetKeyValue("limit_max_apply_days"));
            }
        }
        public static int Chart_Canvas_Width
        {
            get
            {
                return int.Parse(GetKeyValue("Chart_Canvas_Width"));
            }
        }
        public static int Chart_Canvas_Height
        {
            get
            {
                return int.Parse(GetKeyValue("Chart_Canvas_Height"));
            }
        }
        public static int Chart_Canvas_EdgeColor
        {
            get
            {
                return Convert.ToInt32(GetKeyValue("Chart_Canvas_EdgeColor"), 16);
            }
        }
        public static int Chart_Canvas_BgColor
        {
            get
            {
                return Convert.ToInt32(GetKeyValue("Chart_Canvas_BgColor"), 16);
            }
        }
        public static int Chart_Diagram_Width
        {
            get
            {
                return int.Parse(GetKeyValue("Chart_Diagram_Width"));
            }
        }
        public static int Chart_Diagram_Height
        {
            get
            {
                return int.Parse(GetKeyValue("Chart_Diagram_Height"));
            }
        }
        public static int Chart_Title_BgColor
        {
            get
            {
                return Convert.ToInt32(GetKeyValue("Chart_Title_BgColor"), 16);
            }
        }
        public static int Chart_Title_FontColor
        {
            get
            {
                return Convert.ToInt32(GetKeyValue("Chart_Title_FontColor"), 16);
            }
        }
        public static int Chart_Title_FontSize
        {
            get
            {
                return Convert.ToInt32(GetKeyValue("Chart_Title_FontSize"));
            }
        }
        public static int Chart_Title_Height
        {
            get
            {
                return Convert.ToInt32(GetKeyValue("Chart_Title_Height"));
            }
        }
        public static string Chart_Title_FontFamily
        {
            get
            {
                return GetKeyValue("Chart_Title_FontFamily");
            }
        }
        public static string MailServer
        {
            get
            {
                return GetKeyValue("MailServer");
            }
        }
        public static string MailTitle1
        {
            get
            {
                return GetKeyValue("MailTitle1");
            }
        }
        public static string MailTitle2
        {
            get
            {
                return GetKeyValue("MailTitle2");
            }
        }
        public static string[] MailToList
        {
            get
            {
                var s = GetKeyValue("MailToList");
                string[] r = s.Split(',');
                return r;
            }
        }

        public static string DB0_CodeString
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DB0"];
            }
        }
    }

    #region Image UpLoad Parma
    public static class ImageFileUpParm
    {
        public static ImageUpScope NewsBasicSingle
        {
            get
            {
                ImageUpScope imUp = new ImageUpScope() { keepOrigin = true, limitCount = 50, limitSize = 1024 * 1024 * 5 };
                imUp.Parm = new ImageSizeParm[] {
                    new ImageSizeParm(){folderName="Size1",width=700}
                };
                return imUp;
            }
        }
        public static ImageUpScope Photo
        {
            get
            {
                ImageUpScope imUp = new ImageUpScope() { keepOrigin = true, limitCount = 1, limitSize = 1024 * 1024 * 5 };
                imUp.Parm = new ImageSizeParm[] {
                    new ImageSizeParm(){width=400}
                };
                return imUp;
            }
        }
        public static ImageUpScope Photo_type2
        {
            get
            {
                ImageUpScope imUp = new ImageUpScope() { keepOrigin = true, limitCount = 1, limitSize = 1024 * 1024 * 5 };
                imUp.Parm = new ImageSizeParm[] {
                    new ImageSizeParm(){width=800}
                };
                return imUp;
            }
        }
        public static ImageUpScope PhotoCollection
        {
            get
            {
                ImageUpScope imUp = new ImageUpScope() { keepOrigin = true, limitCount = 20, limitSize = 1024 * 1024 * 5 };
                imUp.Parm = new ImageSizeParm[] {
                    new ImageSizeParm(){folderName="big",width=900},
                    new ImageSizeParm(){folderName="small", width=250}
                };
                return imUp;
            }
        }
        public static ImageUpScope PhotoCollectionB
        {
            get
            {
                ImageUpScope imUp = new ImageUpScope() { keepOrigin = true, limitCount = 20, limitSize = 1024 * 1024 * 5 };
                imUp.Parm = new ImageSizeParm[] {
                    new ImageSizeParm(){width=550}
                };
                return imUp;
            }
        }
        public static ImageUpScope BannerRotator
        {
            get
            {
                ImageUpScope imUp = new ImageUpScope() { keepOrigin = true, limitCount = 10, limitSize = 1024 * 1024 * 5 };
                imUp.Parm = new ImageSizeParm[] {
                    new ImageSizeParm(){width=1700}             
                };
                return imUp;
            }
        }
        public static ImageUpScope BannerIndex
        {
            get
            {
                ImageUpScope imUp = new ImageUpScope() { keepOrigin = true, limitCount = 1, limitSize = 1024 * 1024 * 5 };
                imUp.Parm = new ImageSizeParm[] {
                    new ImageSizeParm(){width=1700}             
                };
                return imUp;
            }
        }
        public static ImageUpScope Badge
        {
            get
            {
                ImageUpScope imUp = new ImageUpScope() { keepOrigin = true, limitCount = 1, limitSize = 1024 * 1024 * 5 };
                imUp.Parm = new ImageSizeParm[] {
                    new ImageSizeParm(){width=500}
                };
                return imUp;
            }
        }
    }
    public static class SysFileUpParm
    {
        public static FilesUpScope BaseLimit
        {
            get
            {
                FilesUpScope FiUp = new FilesUpScope() { limitCount = 5, limitSize = 1024 * 1024 * 256 };
                return FiUp;
            }
        }
    }
    #endregion
}