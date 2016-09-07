using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
namespace ProcCore.Business.DB0
{
    public enum EditState
    {
        Insert = 0,
        Update = 1
    }
    public enum OperationCategory //區分海外留學學校  公/私立
    {
        Private=0,//私立
        Public=1//公立
    }
    public enum AbroadSchoolCategory //區分 世界大學資料庫:1,各國語言學校資料:2
    {
        World = 1,//世界大學資料庫
        Language = 2//各國語言學校資料
    }
    public partial class C41A0_FLStudyEntities : DbContext
    {
        public C41A0_FLStudyEntities(string connectionstring)
            : base(connectionstring)
        {
        }

        public override Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                Log.Write(ex.Message, ex.StackTrace);
                foreach (var err_Items in ex.EntityValidationErrors)
                {
                    foreach (var err_Item in err_Items.ValidationErrors)
                    {
                        Log.Write("欄位驗證錯誤", err_Item.PropertyName, err_Item.ErrorMessage);
                    }
                }

                throw ex;
            }
            catch (DbUpdateException ex)
            {
                Log.Write("DbUpdateException", ex.InnerException.Message);
                throw ex;
            }
            catch (EntityException ex)
            {
                Log.Write("EntityException", ex.Message);
                throw ex;
            }
            catch (UpdateException ex)
            {
                Log.Write("UpdateException", ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                Log.Write("Exception", ex.Message);
                throw ex;
            }
        }

    }

    #region Model Expand
    public partial class m_StudyAbroad : BaseEntityTable
    {
        public string imgsrc { get; set; } //list
        public EditState edit_state { get; set; }
    }
    public partial class m_AbroadSchool : BaseEntityTable
    {
        public string imgsrc { get; set; } //list
    }
    public partial class m_Video : BaseEntityTable
    {
        public string imgsrc { get; set; }
    }
    public partial class StudyAbroad : BaseEntityTable
    {
        public string[] benner_imgsrc { get; set; }
        public string[] photoS_imgsrc { get; set; }
        public string[] photoB_imgsrc { get; set; }
        public string imgsrc { get; set; } //list
        public string vacation_name { get; set; }
    }
    public partial class AbroadSchool : BaseEntityTable
    {
        public string[] photo_imgsrc { get; set; }
        public string imgsrc { get; set; }
    }
    public partial class Banner : BaseEntityTable
    {
        public string imgsrc { get; set; }
    }
    public partial class News : BaseEntityTable
    {
        public string imgsrc { get; set; }
        public string category_name { get; set; }
    }
    public partial class m_News : BaseEntityTable
    {
        public string imgsrc { get; set; }
    }
    public partial class TestInfo : BaseEntityTable
    {
        public string imgsrc { get; set; }
        public string category_name { get; set; }
    }
    public partial class m_TestInfo : BaseEntityTable
    {
        public string imgsrc { get; set; }
    }
    public partial class HelpfulInfo : BaseEntityTable
    {
        public string category_name { get; set; }
        public string imgsrc { get; set; }
    }
    public partial class m_HelpfulInfo : BaseEntityTable
    {
        public string imgsrc { get; set; }
    }
    public partial class EmailContact : BaseEntityTable
    {
        public string vaild { get; set; }
        public string findout_name { get; set; }
        public string interest_name { get; set; }
    }
    public partial class Feedback : BaseEntityTable
    {
        public string imgsrc { get; set; }
        public string[] photos_imgsrc { get; set; }
    }
    public partial class m_Feedback : BaseEntityTable
    {
        public string imgsrc { get; set; }
    }
    public partial class InfoDetail : BaseEntityTable
    {
        public string imgsrc { get; set; }
        public EditState edit_state { get; set; }
    }
    public partial class m_InfoDetail : BaseEntityTable
    {
        public EditState edit_state { get; set; }
    }
    public partial class Albums : BaseEntityTable
    {
        public string imgsrc { get; set; }
        public string[] photos_imgsrc { get; set; }
    }
    public partial class m_Albums : BaseEntityTable
    {
        public string imgsrc { get; set; }
    }
    public class option
    {
        public int val { get; set; }
        public string Lname { get; set; }
    }
    public class parm
    {
        public string receiveMails { get; set; }
    }

    /// <summary>
    /// 第一層分類的種類
    /// </summary>
    public static class CategoryType
    {
        public static int Vacation = 1;//(不可變動)
        public static int Country = 2;//國家(可變動)
        public static int Video = 3;//(不可變動)
        public static int Findout = 4;//如何得知富學的(可變動)
        public static int News = 5;//(不可變動)
        public static int HelpfulInfo = 6;//(不可變動)
        public static int Feedback_year = 7;//客戶反饋_年度(可變動)
        public static int Feedback_category = 8;//客戶反饋_分類(可變動)
        public static int TestInfo = 9;//(不可變動)
        public static int Interest = 11;//您對哪個行程感興趣?(可變動)
        public static int Vacation_2 = 12;//行程?(可變動)
    }
    public static class VacationType
    {
        public static int Winter = 1001;//寒假行程
        public static int Summer = 1002;//暑假行程
        public static int Intensive = 1003;//密集式體驗營
    }
    public static class VideoType
    {
        public static int AboutUS_p2 = 3001;//精彩影片分享
        public static int Share_p2 = 3002;//出國影片
        public static int Share_p3 = 3003;//成果發表影片
        public static int Share_p4 = 3004;//客戶形象影片
    }
    public static class HelpfulInfoType
    {
        public static int Info_p1 = 6001;//遊留學百科
        public static int Info_p2 = 6002;//遊留學新知
        public static int Info_p3 = 6003;//行前準備
    }
    public static class InfoDetailType
    {
        public static int Info_p1 = 1;//語言的重要性
        public static int Info_p2 = 2;//多元課程規劃
    }
    #endregion

    #region q_Model_Define
    public class q_AspNetRoles : QueryBase
    {
        public string Name { set; get; }

    }
    public class q_AspNetUsers : QueryBase
    {
        public string UserName { set; get; }

    }
    public class q_All_Category_L1 : QueryBase
    {
        public int? l1_id { get; set; }
        public string title { get; set; }
    }

    public class q_All_Category_L2 : QueryBase
    {
        public string l2_name { get; set; }
        public int? l1_id { get; set; }
        public int? l2_id { get; set; }

    }
    public class q_StudyAbroad : QueryBase
    {
        public string name { get; set; }
        public int? country { get; set; }
        public int? vacation { get; set; }
        public int? category { get; set; }
        public bool? is_past { get; set; }
    }
    public class q_Info : QueryBase
    {
        public int? category { get; set; }
        public string title { get; set; }
    }
    public class q_InfoDetail : QueryBase
    {
        public int main_id { get; set; }
    }
    public class q_EmailContact : QueryBase
    {
        public int? category { get; set; }
        public string name { get; set; }
    }
    public class q_News : QueryBase
    {
        public string title { get; set; }
        public int? category { get; set; }
    }
    public class q_TestInfo : QueryBase
    {
        public string title { get; set; }
        public int? category { get; set; }
    }
    public class q_HelpfulInfo : QueryBase
    {
        public string title { get; set; }
        public int? category { get; set; }
    }
    public class q_Video : QueryBase
    {
        public int? category { get; set; }
        public string title { get; set; }
    }
    public class q_Banner : QueryBase
    {
        public string name { get; set; }
    }
    public class q_AbroadSchbool : QueryBase
    {
        public string title { get; set; }
        public int? country { get; set; }
        public int? operation { get; set; }
        public int? category { get; set; }
    }
    public class q_Feedback : QueryBase
    {
        public string title { get; set; }
        public int? category { get; set; }
        public int? year { get; set; }
    }
    public class q_Albums : QueryBase
    {
        public string title { get; set; }
        public int? category { get; set; }
        public int? year { get; set; }
    }
    #endregion

    #region c_Model_Define
    public class c_AspNetRoles
    {
        public q_AspNetRoles q { get; set; }
        public AspNetRoles m { get; set; }
    }
    public partial class c_AspNetUsers
    {
        public q_AspNetUsers q { get; set; }
        public AspNetUsers m { get; set; }
    }


    #endregion
}
