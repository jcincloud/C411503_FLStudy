using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using System.Linq.Expressions;
using System.Resources;
using System.Runtime.Caching;
using System.Collections;
using System.Web.WebPages;

namespace DotWeb.Helpers
{
    public static class LocalizationHelpe
    {
        public static String Lang(this HtmlHelper htmlHelper, String key)
        {
            return Lang(htmlHelper.ViewDataContainer as WebViewPage, key);
        }

        public static String Lang<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e)
        where TModel : class
        {
            var n = ExpressionHelper.GetExpressionText(e);
            String m = n.Split('.').LastOrDefault();
            return Lang(h.ViewDataContainer as WebViewPage, m);
        }

        public static String Lang<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e, String prefx)
        where TModel : class
        {
            var n = ExpressionHelper.GetExpressionText(e);
            String m = n.Split('.').LastOrDefault();
            var i = prefx + m;
            return Lang(h.ViewDataContainer as WebViewPage, i);
        }

        private static IEnumerable<DictionaryEntry> GetResx(String LocalResourcePath)
        {
            //System.Resources.
            ObjectCache cache = MemoryCache.Default;
            IEnumerable<DictionaryEntry> resxs = null;

            if (cache.Contains(LocalResourcePath))
                resxs = cache.GetCacheItem(LocalResourcePath).Value as IEnumerable<DictionaryEntry>;
            else
            {
                if (File.Exists(LocalResourcePath))
                {
                    resxs = new ResXResourceReader(LocalResourcePath).Cast<DictionaryEntry>();
                    cache.Add(LocalResourcePath, resxs, new CacheItemPolicy() { Priority = CacheItemPriority.NotRemovable });
                }
            }
            return resxs;
        }
        public static String Lang(this WebPageBase page, String key)
        {
            var pagePath = page.VirtualPath;
            var pageName = pagePath.Substring(pagePath.LastIndexOf('/'), pagePath.Length - pagePath.LastIndexOf('/')).TrimStart('/');
            var filePath = page.Server.MapPath(pagePath.Substring(0, pagePath.LastIndexOf('/') + 1)) + "App_LocalResources";

            String lang = System.Globalization.CultureInfo.CurrentCulture.Name;
            String resxKey = String.Empty;
            String def_resKey = String.Format(@"{0}\{1}.resx", filePath, pageName);
            String lng_resKey = String.Format(@"{0}\{1}.{2}.resx", filePath, pageName, lang);

            resxKey = File.Exists(lng_resKey) ? lng_resKey : def_resKey;
            IEnumerable<DictionaryEntry> resxs = GetResx(resxKey);
            if (resxs != null)
                return (String)resxs.FirstOrDefault<DictionaryEntry>(x => x.Key.ToString() == key).Value;
            else
                return "";
        }
    }
    public static class CommVar
    {
        public static String ngSH(this HtmlHelper htmlHelper)
        {
            return "sd";
        }
        public static String ngGD(this HtmlHelper htmlHelper)
        {
            return "gd";
        }
        /// <summary>
        /// 明細Grid變數
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static String ngGT(this HtmlHelper htmlHelper)
        {
            return "subgd";
        }
        /// <summary>
        /// 編輯欄位變數
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>

        public static String ngFD(this HtmlHelper htmlHelper)
        {
            return "fd";
        }
    }
    public static class Pop
    {

        public static PopWindow popWindow(this HtmlHelper htmlHelper, String ng_show, String ng_close, String title, String ID, String size)
        {
            PopWindow w = new PopWindow(htmlHelper.ViewContext, ng_show, ng_close, title, ID, size);
            return w;
        }
    }
    public class PopWindow : IDisposable
    {
        private Boolean disposed;
        private ViewContext _vwContext;
        public PopWindow(ViewContext VCText, String ng_show, String ng_close, String title, String ID, String size)
        {
            String tpl = String.Format("<div class=\"modal fade\" id=\"" + ID + "\"><div class=\"modal-dialog " + size + "\"><div class=\"modal-content\"><div class=\"modal-header\"><button type=\"button\" class=\"close\" data-dismiss=\"modal\" ng-click=\"" + ng_close + "\"><span aria-hidden=\"true\">&times;</span><span class=\"sr-only\">" + Resources.Res.Info_Close_Layer + "</span></button><h4 class=\"modal-title\">" + title + Resources.Res.Info_Edit + "</h4></div>", ng_show);
            _vwContext = VCText;
            _vwContext.Writer.Write(tpl);
        }

        public void Dispose()
        {
            _vwContext.Writer.Write("</div></div></div>");
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {

                }
                disposed = true;
            }
        }
    }
    public static class ModelName
    {

        public static MvcHtmlString gd<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e) where TModel : class
        {
            string n = ExpressionHelper.GetExpressionText(e);
            string m = n.Split('.').LastOrDefault();
            return MvcHtmlString.Create("gd." + m);
        }
        public static MvcHtmlString fd<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e) where TModel : class
        {
            string n = ExpressionHelper.GetExpressionText(e);
            string m = n.Split('.').LastOrDefault();
            return MvcHtmlString.Create("fd." + m);
        }
        public static MvcHtmlString sd<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e) where TModel : class
        {
            string n = ExpressionHelper.GetExpressionText(e);
            string m = n.Split('.').LastOrDefault();
            return MvcHtmlString.Create("sd." + m);
        }
        public static MvcHtmlString fds<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e) where TModel : class
        {
            string n = ExpressionHelper.GetExpressionText(e);
            string m = n.Split('.').LastOrDefault();
            return MvcHtmlString.Create("fds." + m);
        }

        public static MvcHtmlString ngName<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e, String Prefix) where TModel : class
        {
            String n = ExpressionHelper.GetExpressionText(e);
            String m = n.Split('.').LastOrDefault();

            if (String.IsNullOrEmpty(Prefix))
                return MvcHtmlString.Create(m);
            else
                return MvcHtmlString.Create(Prefix + "." + m);
        }
        public static MvcHtmlString ngName(this HtmlHelper h, String e, String Prefix)
        {
            String n = e;
            if (String.IsNullOrEmpty(Prefix))
                return MvcHtmlString.Create(n);
            else
                return MvcHtmlString.Create(Prefix + "." + n);
        }
    }
    public class GridInfo<T>
    {
        public int total;
        public int page;
        public int records;
        public int startcount;
        public int endcount;

        public IList<T> rows;
    }
    public class IncludePagerParm
    {
        public IncludePagerParm()
        {
            this.show_add = true;
            this.show_del = true;
            this.edit_form_id = "Edit";
        }
        public bool show_add { get; set; }
        public bool show_del { get; set; }
        public string edit_form_id { get; set; }
    }
}