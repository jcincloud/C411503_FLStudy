using ProcCore.Business.DB0;
using ProcCore.HandleResult;
using ProcCore.NetExtension;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace ProcCore.Business
{
    public enum CodeTable
    {
        Base, StudyAbroad, All_Category_L1, All_Category_L2, Info, InfoDeatil, EmailContact, News, Video, Banner, AbroadSchool, HelpfulInfo, Feedback, Albums, CountryCategroy, EmailCategroy, YearCategroy, FeedbackCategroy, TestInfo, Interest
    }
    public enum SNType
    {
        Orders, Product, Receiver, Import, Stock, Export, StockAdj
    }
    /// <summary>
    /// 客戶類別
    /// </summary>
    public enum CustomerType
    {
        Store = 1, //店家
        Straight = 2 //直客        
    }
    /// <summary>
    /// 客戶型態
    /// </summary>
    public enum CustomerKind
    {
        LS,
        BeerStore,
        Dancing,
        Bar,
        Cafe

    }

}
namespace ProcCore.Business.LogicConect
{
    #region Parm Section
    public enum ParmDefine
    {
        Open, Information, Apply, receiveMails, receiveMails_cn
    }
    #endregion

    public class LogicCenter
    {
        private static string db0_connectionstring;
        protected C41A0_FLStudyEntities db0;
        protected TransactionScope tx;
        private const string DatabaseName = "C41A0_FLStudy";
        public int DepartmentId { get; set; }
        public string Lang { get; set; }
        public string IP { get; set; }
        public string AspUserID { get; set; }
        public static string GetDB0EntityString(string configstring)
        {
            string[] DataConnectionInfo = configstring.Split(',');

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = DataConnectionInfo[0];
            builder.UserID = DataConnectionInfo[1];
            builder.Password = DataConnectionInfo[2];
            builder.InitialCatalog = DatabaseName;
            builder.IntegratedSecurity = false;
            builder.PersistSecurityInfo = false;

            EntityConnectionStringBuilder entBuilder = new EntityConnectionStringBuilder();
            entBuilder.Provider = "System.Data.SqlClient";
            entBuilder.ProviderConnectionString = builder.ConnectionString;
            entBuilder.Metadata = String.Format("res://{0}/{1}.csdl|res://{0}/{1}.ssdl|res://{0}/{1}.msl", "Proc.BusinessLogic", "DB0." + DatabaseName);
            return entBuilder.ConnectionString;
        }
        public static string GetDB0ConnectionString(string configstring)
        {
            string[] DataConnectionInfo = configstring.Split(',');

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = DataConnectionInfo[0];
            builder.UserID = DataConnectionInfo[1];
            builder.Password = DataConnectionInfo[2];
            builder.InitialCatalog = DatabaseName;
            builder.MultipleActiveResultSets = true;
            builder.IntegratedSecurity = false;

            return builder.ConnectionString;
        }
        public LogicCenter() { }
        public LogicCenter(string db0_configstring)
        {
            db0_connectionstring = LogicCenter.GetDB0EntityString(db0_configstring);
        }

        public static C41A0_FLStudyEntities getDB0
        {
            get
            {
                return new C41A0_FLStudyEntities(db0_connectionstring);
            }
        }
        public int GetNewId(ProcCore.Business.CodeTable tab)
        {
            int i = 0;

            using (var tx = new TransactionScope())
            {
                var get_id_db = getDB0;

                try
                {
                    string tab_name = Enum.GetName(typeof(ProcCore.Business.CodeTable), tab);
                    var item = get_id_db.i_IDX.Where(x => x.table_name == tab_name)
                        .ToList()
                        .FirstOrDefault();

                    if (item != null)
                    {
                        item.IDX++;
                        get_id_db.SaveChanges();
                        tx.Complete();
                        i = item.IDX;
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex.ToString());
                }
                finally
                {
                    get_id_db.Dispose();
                }
                return i;
            }
        }
        private SNObject GetSN(ProcCore.Business.SNType tab)
        {
            SNObject sn = new SNObject();

            using (var tx = new TransactionScope())
            {
                var get_sn_db = getDB0;
                try
                {
                    get_sn_db = getDB0;
                    string tab_name = Enum.GetName(typeof(ProcCore.Business.SNType), tab);
                    var items = get_sn_db.i_SN.Single(x => x.sn_type == tab_name);

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

                    get_sn_db.SaveChanges();
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
                finally
                {
                    get_sn_db.Dispose();
                }
            }
            return sn;
        }
        public string getReceiverSN() //
        {
            String tpl = "TY{0}{1:00}{2:00}-{3:00}";
            SNObject sn = GetSN(ProcCore.Business.SNType.Receiver);
            return string.Format(tpl, sn.y.ToString().Right(2), sn.m, sn.d, sn.sn_max);
        }
        public object getParmValue(ParmDefine ParmName)
        {
            db0 = getDB0;
            string getName = Enum.GetName(typeof(ParmDefine), ParmName);
            var item = db0.i_Parm.Where(x => x.ParmName == getName).FirstOrDefault();
            if (item != null)
            {
                if (item.ParmType == "S")
                {
                    return item.S;
                }
                if (item.ParmType == "I")
                {
                    return item.I;
                }
                if (item.ParmType == "F")
                {
                    return item.F;
                }
                if (item.ParmType == "D")
                {
                    return item.D;
                }
                if (item.ParmType == "B")
                {
                    return item.B;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public void setParmValue(ParmDefine ParmName, object value)
        {
            db0 = getDB0;
            string str = Enum.GetName(typeof(ParmDefine), ParmName);
            var item = db0.i_Parm.Where(x => x.ParmName == str).FirstOrDefault();
            if (item != null)
            {
                if (item.ParmType == "S")
                {
                    item.S = (string)value;
                }
                if (item.ParmType == "I")
                {
                    item.I = (int)value;
                }
                if (item.ParmType == "F")
                {
                    item.F = (decimal)value;
                }
                if (item.ParmType == "D")
                {
                    item.D = (DateTime)value;
                }
                if (item.ParmType == "B")
                {
                    item.B = (bool)value;
                }
            }
            db0.SaveChanges();
        }
        public string[] getReceiveMails()
        {
            var s = string.Empty;
            if (System.Globalization.CultureInfo.CurrentCulture.Name == "zh-CN")
            {
                 s = (string)getParmValue(ParmDefine.receiveMails_cn);
            }
            else {
                 s = (string)getParmValue(ParmDefine.receiveMails);
            }
            
            string[] r = s.Split(',');
            return r;
        }
        public static void SetDB0EntityString(string configstring)
        {
            db0_connectionstring = GetDB0EntityString(configstring);
        }
    }
    public class ReportCenter : LogicCenter
    {
        public ReportCenter() : base() { }
        public ReportCenter(string config_string)
            : base(config_string)
        {
            this.db0 = getDB0;
        }

    }
}