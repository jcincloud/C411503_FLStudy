using Microsoft.Reporting.WebForms;
using ProcCore.Business.DB0;
using ProcCore.Business.LogicConect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace DotWeb.Areas.Base.Controllers
{
    public class ReportChartController : BaseController
    {
        public ActionResult OpenMonthAverage(q_Month_Average q)
        {
            db0 = getDB0();

            var getUserIdCK = Request.Cookies["user_id"];
            string getUserId = string.Empty;

            Apply_User getUserData = null;
            if (getUserIdCK == null)
            {
                getUserData = db0.Apply_User.Where(x => x.USERID == q.user_id).FirstOrDefault();
            }
            else
            {
                getUserId = getUserIdCK.Value;
                getUserData = db0.Apply_User.Where(x => x.USERID == getUserId).FirstOrDefault();
            }

            var getEquipmet = db0.Equipment.Find(q.equipment_id);
            MonthHeadInfo md = new MonthHeadInfo();
            md.Y = q.Y;
            md.user_id = q.user_id;
            md.apply_user_name = getUserData.USERNAME;
            md.equipment_id = q.equipment_id;
            md.equipment_sn = getEquipmet.equipment_sn;
            md.query_use_type = q.query_use_type;
            if (q.query_use_type == 1)
            {
                md.use_type_name = "煙氣出口溫度年平均值";
            }
            if (q.query_use_type == 2)
            {
                md.use_type_name = "爐氣含氧體積濃度年平均值";
            }
            return View(md);
        }
        public ActionResult OpenYearAverage(q_Year_Average q)
        {
            db0 = getDB0();

            var getUserIdCK = Request.Cookies["user_id"];
            string getUserId = string.Empty;

            Apply_User getUserData = null;
            if (getUserIdCK == null)
            {
                getUserData = db0.Apply_User.Where(x => x.USERID == q.user_id).FirstOrDefault();
            }
            else
            {
                getUserId = getUserIdCK.Value;
                getUserData = db0.Apply_User.Where(x => x.USERID == getUserId).FirstOrDefault();
            }

            var getEquipmet = db0.Equipment.Find(q.equipment_id);
            YearHeadInfo md = new YearHeadInfo();
            md.user_id = q.user_id;
            md.apply_user_name = getUserData.USERNAME;
            md.equipment_id = q.equipment_id;
            md.equipment_sn = getEquipmet.equipment_sn;
            md.query_use_type = q.query_use_type;
            if (q.query_use_type == 1)
            {
                md.use_type_name = "煙氣出口溫度年平均值";
            }
            if (q.query_use_type == 2)
            {
                md.use_type_name = "爐氣含氧體積濃度年平均值";
            }
            return View(md);
        }
        public string getMonthData(q_Month_Average q)
        {
            db0 = getDB0();

            var getUserIdCK = Request.Cookies["user_id"];
            string getUserId = string.Empty;

            Apply_User getUserData = null;
            if (getUserIdCK == null)
            {
                getUserData = db0.Apply_User.Where(x => x.USERID == q.user_id).FirstOrDefault();
            }
            else
            {
                getUserId = getUserIdCK.Value;
                getUserData = db0.Apply_User.Where(x => x.USERID == getUserId).FirstOrDefault();
            }

            var getMonthData = db0.Apply_MonthAverage.Where(x => x.equipment_id == q.equipment_id && x.USERID == getUserData.USERID && x.Y == q.Y).FirstOrDefault();
            IList<double> n = new List<double>();

            if (q.query_use_type == 1)
            {
                n.Add((double)getMonthData.temperature_01);
                n.Add((double)getMonthData.temperature_02);
                n.Add((double)getMonthData.temperature_03);
                n.Add((double)getMonthData.temperature_04);
                n.Add((double)getMonthData.temperature_05);
                n.Add((double)getMonthData.temperature_06);
                n.Add((double)getMonthData.temperature_07);
                n.Add((double)getMonthData.temperature_08);
                n.Add((double)getMonthData.temperature_09);
                n.Add((double)getMonthData.temperature_10);
                n.Add((double)getMonthData.temperature_11);
                n.Add((double)getMonthData.temperature_12);
            }
            if (q.query_use_type == 2)
            {
                n.Add((double)getMonthData.oxygen_concentration_01);
                n.Add((double)getMonthData.oxygen_concentration_02);
                n.Add((double)getMonthData.oxygen_concentration_03);
                n.Add((double)getMonthData.oxygen_concentration_04);
                n.Add((double)getMonthData.oxygen_concentration_05);
                n.Add((double)getMonthData.oxygen_concentration_06);
                n.Add((double)getMonthData.oxygen_concentration_07);
                n.Add((double)getMonthData.oxygen_concentration_08);
                n.Add((double)getMonthData.oxygen_concentration_09);
                n.Add((double)getMonthData.oxygen_concentration_10);
                n.Add((double)getMonthData.oxygen_concentration_11);
                n.Add((double)getMonthData.oxygen_concentration_12);
            }

            db0.Dispose();
            return defJSON(n.ToArray());
        }
        public string getYearTemperature(q_Month_Average q)
        {
            db0 = getDB0();

            string getUserId = this.UserId == null ? q.user_id : this.UserId;

            var getTemperatureData = db0.Apply_Detail.Where(x => x.equipment_id == q.equipment_id && x.USERID == getUserId).Select(x => x.avg_Y_gas_temperature).ToList();
            db0.Dispose();
            return defJSON(getTemperatureData);
        }
        public string getYeartConcentration(q_Month_Average q)
        {
            db0 = getDB0();

            string getUserId = this.UserId == null ? q.user_id : this.UserId;

            var getConcentrationData = db0.Apply_Detail.Where(x => x.equipment_id == q.equipment_id && x.USERID == getUserId).Select(x => x.avg_Y_oxygen_concentration).ToList(); ;
            db0.Dispose();
            return defJSON(getConcentrationData);
        }

    }

    public class MonthHeadInfo
    {
        public string user_id { get; set; }
        public string apply_user_name { get; set; }
        public int equipment_id { get; set; }
        public string equipment_sn { get; set; }
        public int Y { get; set; }

        public int query_use_type { get; set; }
        public string use_type_name { get; set; }
    }
    public class YearHeadInfo
    {
        public string user_id { get; set; }
        public string apply_user_name { get; set; }
        public int equipment_id { get; set; }
        public string equipment_sn { get; set; }
        public int Y { get; set; }

        public int query_use_type { get; set; }
        public string use_type_name { get; set; }
    }
}