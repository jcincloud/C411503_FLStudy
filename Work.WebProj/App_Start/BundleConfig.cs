//using System.Web.Optimization;
//using System.Web.Optimization.React;

//namespace DotWeb.AppStart
//{
//    public class BundleConfig
//    {
//        public static void RegisterBundles(BundleCollection bundles)
//        {
//            #region JScript

//            string[] comm_angular_file = new string[] {
//                "~/ScriptsTSDef/dynScript/defData.js",
//                "~/Scripts/jquery/jquery-2.1.3.js",
//                "~/Scripts/angular/angular.js",
//                "~/Scripts/angular/angular-animate.js",
//                "~/Scripts/angular/angular-route.js",
//                "~/Scripts/angular/i18n/angular-locale_zh-tw.js",
//                "~/Scripts/angular-plugging/signalr-hub.js",
//                "~/Scripts/angular-plugging/toaster.js",
//                "~/Scripts/angular-plugging/ui-bootstrap-tpls-0.11.2.js",
//                "~/ScriptsTSDef/commfunc.js",
//                "~/ScriptsTSDef/commangular.js",
//                "~/Scripts/BrowerInfo.js",
//            };

//            string[] angular_fileupload = new string[] {
//                "~/ScriptsTSDef/dynScript/defData.js",
//                "~/Scripts/angular-plugging/angular-file-upload/angular-file-upload-html5-shim.js",
//                "~/Scripts/jquery/jquery-2.1.3.js",
//                "~/Scripts/angular/angular.js",
//                "~/Scripts/angular-plugging/angular-file-upload/angular-file-upload.js",
//                "~/Scripts/angular/angular-animate.js",
//                "~/Scripts/angular/angular-route.js",
//                "~/Scripts/angular/i18n/angular-locale_zh-tw.js",
//                "~/Scripts/angular-plugging/signalr-hub.js",
//                "~/Scripts/angular-plugging/toaster.js",
//                "~/Scripts/angular-plugging/ui-bootstrap-tpls-0.11.2.js",
//                "~/ScriptsTSDef/commfunc.js",
//                "~/ScriptsTSDef/commangular.js",
//                "~/Scripts/BrowerInfo.js"
//            };

//            string[] comm_react_file_manage = new string[] {
//                "~/ScriptsTSDef/dynScript/defData.js",
//                "~/Scripts/moment/moment.js",
//                "~/Scripts/moment/locale/zh-tw.js",
//                "~/Scripts/toastr.js",
//                "~/Scripts/jquery/jquery-2.1.3.js",
//                "~/Scripts/react/react-with-addons.js",
//                "~/ScriptsTSDef/commfunc.js",
//                "~/ScriptsJSX/comm.jsx",
//                "~/Scripts/BrowerInfo.js"
//            };


//            string[] comm_reac_file_web = new string[] {
//                "~/Scripts/codrops/classie.js",
//                "~/Scripts/codrops/modalEffects.js",
//                "~/ScriptsTSDef/dynScript/defData.js",
//                "~/Scripts/moment/moment.js",
//                "~/Scripts/moment/locale/zh-tw.js",
//                "~/Scripts/react/react-with-addons.js",
//                "~/ScriptsTSDef/commfunc.js",
//                "~/ScriptsJSX/comm.jsx",
//                "~/ScriptsTSDef/commwww.js",
//                "~/Scripts/BrowerInfo.js"
//            };

//            bundles.Add(new JsxBundle("~/loginRct")
//            .Include(comm_react_file_manage)
//            .Include("~/ScriptsJSX/login.jsx")
//            );

//            bundles.Add(new JsxBundle("~/imgSortCompRct")
//            .Include( "~/Scripts/Sortable.min.js")
//            .Include("~/Scripts/react-sortable-mixin.js")
//            .Include("~/ScriptsJSX/com_img_sort.jsx")
//            );


//            #region Sys_Base
//            bundles.Add(new ScriptBundle("~/login")
//            .Include(comm_angular_file)
//            .Include("~/ScriptsCtrl/login.js")
//            );
//            bundles.Add(new ScriptBundle("~/rolesCtr")
//                .Include(comm_angular_file)
//                .Include("~/ScriptsCtrl/rolesCtr")
//                );
//            bundles.Add(new ScriptBundle("~/usersCtr")
//                .Include(comm_angular_file)
//                .Include("~/ScriptsCtrl/usersCtr.js")
//                );

//            bundles.Add(new ScriptBundle("~/changepasswordCtr")
//                .Include(comm_angular_file)
//                .Include("~/ScriptsCtrl/users_changepassword.js")
//                );
//            #endregion
//            #region Sys_Active
//            bundles.Add(new JsxBundle("~/newsRct")
//            .Include(comm_react_file_manage)
//            .Include("~/Scripts/datetimepicker/bootstrap-datetimepicker.js")
//            .Include("~/Scripts/SimpleAjaxUploader.js")
//            .Include("~/ScriptsJSXM/news_dataM.jsx")
//            );

//            bundles.Add(new JsxBundle("~/activeRct")
//            .Include(comm_react_file_manage)
//            .Include("~/Scripts/datetimepicker/bootstrap-datetimepicker.js")
//            .Include("~/Scripts/SimpleAjaxUploader.js")
//            .Include("~/Scripts/react-sortable-mixin.js")
//            .Include("~/ScriptsJSX/com_img_sort.jsx")
//            .Include("~/ScriptsJSXM/activeM.jsx")
//            );

//            bundles.Add(new JsxBundle("~/memberRct")
//            .Include(comm_react_file_manage)
//            .Include("~/Scripts/datetimepicker/bootstrap-datetimepicker.js")
//            .Include("~/Scripts/SimpleAjaxUploader.js")
//            .Include("~/Scripts/react-sortable-mixin.js")
//            //.Include("~/ScriptsJSX/com_img_sort.jsx")
//            .Include("~/ScriptsJSXM/memberM.jsx")
//            );

//            bundles.Add(new JsxBundle("~/memberUserRct")
//            .Include(comm_react_file_manage)
//            .Include("~/Scripts/datetimepicker/bootstrap-datetimepicker.js")
//            .Include("~/Scripts/SimpleAjaxUploader.js")
//            .Include("~/Scripts/react-sortable-mixin.js")
//            .Include("~/ScriptsJSXM/member_userM.jsx")
//            );
//            bundles.Add(new JsxBundle("~/memberprintRct")
//            .Include(comm_react_file_manage)
//            .Include("~/Scripts/datetimepicker/bootstrap-datetimepicker.js")
//            .Include("~/ScriptsJSXM/member_printM.jsx")
//            );

//            bundles.Add(new JsxBundle("~/otherunitRct")
//            .Include(comm_react_file_manage)
//            .Include("~/Scripts/datetimepicker/bootstrap-datetimepicker.js")
//            .Include("~/ScriptsJSXM/other_unitM.jsx")
//            );


//            bundles.Add(new JsxBundle("~/documentRct")
//            .Include(comm_react_file_manage)
//            .Include("~/Scripts/datetimepicker/bootstrap-datetimepicker.js")
//            .Include("~/Scripts/SimpleAjaxUploader.js")
//            .Include("~/ScriptsJSXM/documentM.jsx")
//            );

//            bundles.Add(new JsxBundle("~/documentUserRct")
//            .Include(comm_react_file_manage)
//            .Include("~/Scripts/datetimepicker/bootstrap-datetimepicker.js")
//            .Include("~/Scripts/SimpleAjaxUploader.js")
//            .Include("~/ScriptsJSXM/document_userM.jsx")
//            );

//            bundles.Add(new JsxBundle("~/bannerRct")
//            .Include(comm_react_file_manage)
//            .Include("~/Scripts/datetimepicker/bootstrap-datetimepicker.js")
//            .Include("~/Scripts/SimpleAjaxUploader.js")
//            .Include("~/ScriptsJSXM/bannerM.jsx")
//            );

//            bundles.Add(new JsxBundle("~/bannervideoRct")
//            .Include(comm_react_file_manage)
//            .Include("~/Scripts/datetimepicker/bootstrap-datetimepicker.js")
//            .Include("~/Scripts/SimpleAjaxUploader.js")
//            .Include("~/ScriptsJSXM/bannervideoM.jsx")
//            );
//            #endregion
//            #region Web WWW

//            bundles.Add(new JsxBundle("~/homeWWW")
//            .Include(comm_reac_file_web)
//            .Include("~/Scripts/jquery.flexslider-min.js")
//            .Include("~/ScriptsTSDef/index.js")
//            .Include("~/ScriptsJSX/homeW.jsx")
//            );

//            bundles.Add(new JsxBundle("~/news_listWWW")
//            .Include(comm_reac_file_web)
//            .Include("~/ScriptsJSX/news_listW.jsx")
//            );

//            bundles.Add(new JsxBundle("~/news_contentWWW")
//            .Include(comm_reac_file_web)
//            .Include("~/ScriptsJSX/news_contentW.jsx")
//            );

//            bundles.Add(new JsxBundle("~/actities_listWWW")
//            .Include(comm_reac_file_web)
//            .Include("~/ScriptsJSX/actities_listW.jsx")
//            );

//            bundles.Add(new JsxBundle("~/actities_contentWWW")
//            .Include(comm_reac_file_web)
//            .Include("~/ScriptsJSX/actities_contentW.jsx")
//            );

//            bundles.Add(new JsxBundle("~/member_listWWW")
//            .Include(comm_reac_file_web)
//            .Include("~/ScriptsJSX/member_listW.jsx")
//            );

//            bundles.Add(new JsxBundle("~/member_contentWWW")
//            .Include(comm_reac_file_web)
//            .Include("~/ScriptsJSX/member_contentW.jsx")
//            );

//            bundles.Add(new JsxBundle("~/business_listWWW")
//            .Include(comm_reac_file_web)
//            .Include("~/ScriptsJSX/business_listW.jsx")
//            );

//            bundles.Add(new JsxBundle("~/document_listWWW")
//            .Include(comm_reac_file_web)
//            .Include("~/ScriptsJSX/document_listW.jsx")
//            );

//            bundles.Add(new JsxBundle("~/document_contentWWW")
//            .Include(comm_reac_file_web)
//            .Include("~/ScriptsJSX/document_contentW.jsx")
//            );

//            bundles.Add(new JsxBundle("~/about_usWWW")
//            .Include(comm_reac_file_web)
//            .Include("~/Scripts/ctrl/aboutUSW.js")
//            );
//            #endregion
//            #endregion

//            #region CSS
//            bundles.Add(new StyleBundle("~/_Code/CSS/CSS")
//            .Include(
//            "~/_Code/CSS/css/page.css",
//            "~/_Code/CSS/toaster.css"
//            ));
//            #endregion
//            BundleTable.EnableOptimizations = false;
//        }
//    }
//}
