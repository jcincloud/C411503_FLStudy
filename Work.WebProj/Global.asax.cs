using DotWeb.CommSetup;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
//using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;

namespace DotWeb.AppStart
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string VarCookie = CommWebSetup.WebCookiesId;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            DisplayModeProvider.Instance.Modes.Insert(0, new DefaultDisplayMode("zh-CN")
            { //使用狀況條件為 Cookies值 或 QueryString["lang"]
                ContextCondition = (C =>
                (
                C.Request.Cookies[VarCookie + ".Lang"] != null &&
                C.Request.Cookies[VarCookie + ".Lang"].Value.Contains("zh-CN") &&
                string.IsNullOrEmpty(C.Request.QueryString["lang"])
                ) ||
                C.Request.QueryString["lang"] == "zh-CN"
               )
            });
        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            if (true) //如不需語系設定流程 設為false
            {
                #region language follow
                HttpCookie WebLang = Request.Cookies[VarCookie + ".Lang"];
                string set_lang = string.Empty;
                string fource_lang = string.Empty; //預設強制語系 
                string query_lang = string.Empty; //參數設定語系
                string[] allow_lang = new string[] { "zh-TW", "zh-CN" };

                fource_lang = allow_lang[0]; //不預強制語系 此行註解
                query_lang = Request.QueryString["lang"]; //參數切換語系 參數查詢列為高優先權

                if (!string.IsNullOrEmpty(query_lang) && allow_lang.Contains(query_lang))
                {
                    //網址參數切換語系
                    var n = System.Globalization.CultureInfo.CreateSpecificCulture(query_lang);
                    set_lang = n.Name;

                    if (WebLang == null)
                    {
                        WebLang = new HttpCookie(VarCookie + ".Lang", set_lang);
                        Response.Cookies.Add(WebLang);
                    }
                    else
                    {
                        WebLang.Value = set_lang;
                        Response.Cookies.Set(WebLang);
                    }
                }
                else if (WebLang == null)
                {
                    if (!string.IsNullOrEmpty(fource_lang)) //採用系統強制設定語系
                    {
                        var q = fource_lang;
                        var n = System.Globalization.CultureInfo.CreateSpecificCulture(q); //轉換完整 語系-國家 編碼
                        set_lang = n.Name;
                    }
                    else if (Request.UserLanguages != null && Request.UserLanguages.Length > 0) //使用瀏覽器提供的語系
                    {
                        var q = Request.UserLanguages[0];
                        var n = System.Globalization.CultureInfo.CreateSpecificCulture(q);//轉換完整 語系-國家 編碼

                        if (allow_lang.Contains(n.Name))
                            set_lang = n.Name;
                        else
                            set_lang = allow_lang[0];
                    }
                    else //提供其他系統直接進行Request但Request Header裡無Accept-Language參數
                    {
                        var n = System.Threading.Thread.CurrentThread.CurrentCulture;
                        if (allow_lang.Contains(n.Name))
                            set_lang = n.Name;
                        else
                            set_lang = allow_lang[0];
                    }
                    WebLang = new HttpCookie(VarCookie + ".Lang", set_lang);
                    Response.Cookies.Add(WebLang);
                }
                else
                {
                    if (!allow_lang.Contains(WebLang.Value))
                    {
                        set_lang = allow_lang[0];
                        WebLang.Value = set_lang;
                    }
                }

                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(WebLang.Value);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.Name);

                #endregion
            }
        }
    }
}
