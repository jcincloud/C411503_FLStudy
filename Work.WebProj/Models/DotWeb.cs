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
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace DotWeb
{
    public class ReportData
    {
        public string ReportName { get; set; }
        public object[] Parms { get; set; }
        public object[] Data { get; set; }
    }
    public class PageImgShow
    {

        public String icon_path { get; set; }
        public String link_path { get; set; }
    }
    public class WebInfo
    {

    }
    public class StringResult : ViewResult
    {
        public String ToHtmlString { get; set; }
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (string.IsNullOrEmpty(this.ViewName))
            {
                this.ViewName = context.RouteData.GetRequiredString("action");
            }

            ViewEngineResult result = null;

            if (this.View == null)
            {
                result = this.FindView(context);
                this.View = result.View;
            }

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);

            ViewContext viewContext = new ViewContext(context, this.View, this.ViewData, this.TempData, writer);

            this.View.Render(viewContext, writer);

            writer.Flush();

            ToHtmlString = Encoding.UTF8.GetString(stream.ToArray());

            if (result != null)
                result.ViewEngine.ReleaseView(context, this.View);
        }
    }
    public class CReportInfo
    {
        public CReportInfo()
        {
            SubReportDataSource = new List<SubReportData>();
        }
        public static String ReportCompany = "";
        public String ReportFile { get; set; }
        public DataTable ReportData { get; set; }
        public List<SubReportData> SubReportDataSource { get; set; }

        public DataSet ReportMDData { get; set; }
        public Dictionary<String, Object> ReportParm { get; set; }
    }
    public class SubReportData
    {
        public string SubReportName { get; set; }
        public DataTable DataSource { get; set; }
    }
}