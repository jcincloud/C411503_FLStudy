using DotWeb.Controller;
using DotWeb.Helpers;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Reporting.WebForms;
using ProcCore.Business.DB0;
using ProcCore.Business.LogicConect;
using ProcCore.HandleResult;
using ProcCore.WebCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DotWeb.Areas.Base.Controllers
{
    public class ReportToPdfController : BaseController
    {
        private byte[] ToPdfReport(ReportData rpt, List<ReportParameter> param)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;

            ReportViewer rptvw = new ReportViewer();
            rptvw.ProcessingMode = ProcessingMode.Local;
            ReportDataSource rds = new ReportDataSource("ReportDataSet", rpt.Data);

            rptvw.LocalReport.DataSources.Clear();
            rptvw.LocalReport.ReportPath = @"_Code\RPTFile\" + rpt.ReportName;
            rptvw.LocalReport.DataSources.Add(rds);
            foreach (var pa in param)
            {
                rptvw.LocalReport.SetParameters(pa);
            }
            rptvw.LocalReport.Refresh();

            byte[] bytes = rptvw.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            return bytes;
        }
    }
}