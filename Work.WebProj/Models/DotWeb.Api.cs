using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using ProcCore;
using ProcCore.Business;
using ProcCore.Business.DB0;
using ProcCore.Business.LogicConect;
using ProcCore.HandleResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace DotWeb.Api
{
    public class BaseApiController : ApiController
    {
        protected int defPageSize = 10;
        protected string aspUserId;
        protected int departmentId;
        protected string UserId; //指的是廠商登錄帳號
        protected string LoginUserFlag = string.Empty;

        protected C41A0_FLStudyEntities db0;


        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            var getUserIdCookie = controllerContext.Request.Headers.GetCookies("user_id").SingleOrDefault();
            UserId = getUserIdCookie == null ? "" :
                EncryptString.desDecryptBase64(getUserIdCookie["user_id"].Value);

            var getUserNameCookie = controllerContext.Request.Headers.GetCookies("user_name").SingleOrDefault();
            var UserName = getUserNameCookie == null ? "" : getUserNameCookie["user_name"].Value;

            //N:管理端登錄 Y:用戶端登錄
            var getLoginUserFlag = controllerContext.Request.Headers.GetCookies("user_login").SingleOrDefault();
            LoginUserFlag = getLoginUserFlag == null ? "" :
                EncryptString.desDecryptBase64(getLoginUserFlag["user_login"].Value);

        }

        protected virtual string getRecMessage(string MsgId)
        {
            String r = Resources.Res.ResourceManager.GetString(MsgId);
            return String.IsNullOrEmpty(r) ? MsgId : r;
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
        protected string ModelStateErrorPack()
        {
            List<string> errMessage = new List<string>();
            foreach (var modelState in ModelState.Values)
                foreach (var error in modelState.Errors)
                    errMessage.Add(error.ErrorMessage);

            return string.Join(":", errMessage);
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
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
        protected int GetNewId()
        {
            return GetNewId(ProcCore.Business.CodeTable.Base);
        }
        protected int GetNewId(ProcCore.Business.CodeTable tab)
        {
            using (TransactionScope tx = new TransactionScope())
            {
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
                        items.IDX++;
                        db.SaveChanges();
                        tx.Complete();
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
            }
        }
    }

    #region 泛型控制器擴充

    [System.Web.Http.Authorize]
    public abstract class ajaxApi<M, Q> : BaseApiController
        where M : new()
        where Q : QueryBase
    {
        protected ResultInfo<M> r;
        protected ResultInfo<M[]> rs;
        protected M item;
    }

    [System.Web.Http.Authorize]
    public abstract class ajaxBaseApi : BaseApiController
    {

    }
    #endregion
}