using DotWeb.CommSetup;
using DotWeb.Controller;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ProcCore;
using ProcCore.HandleResult;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace DotWeb.Areas.Sys_Base.Controllers
{
    public class LoginController : SourceController
    {
        private ApplicationUserManager _userManager;
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ActionResult Main()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            ViewData["username"] = "";
            ViewData["password"] = "";

#if DEBUG
            ViewData["username"] = CommWebSetup.AutoLoginUser;
            ViewData["password"] = CommWebSetup.AutoLoginPassword;
#endif

            removeCookie("user_id");
            removeCookie("user_name");
            removeCookie("user_login");

            return View("Index");
        }
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            if (code == null)
            {
                return View("Error");
            }

            ResetPasswordViewModel md = new ResetPasswordViewModel()
            {
                Code = code
            };

            return View(md);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<string> ajax_Login(LoginViewModel model)
        {
            var userManager = UserManager;

            LoginResult getLoginResult = new LoginResult();

            if (!ModelState.IsValid)
            {
                getLoginResult.result = false;
                getLoginResult.message = "資訊不完整";
                return defJSON(getLoginResult);
            }

            #region 驗證碼檢查程序
            if (string.IsNullOrEmpty(Session["CheckCode"].ToString()))
            {
                Session["CheckCode"] = Guid.NewGuid();
                getLoginResult.result = false;
                getLoginResult.message = Resources.Res.Log_Err_ImgValideNotEquel;
                return defJSON(getLoginResult);
            }

            getLoginResult.vildate = Session["CheckCode"].Equals(model.validate) ? true : false;
#if DEBUG
            getLoginResult.vildate = true;
#endif
            if (!getLoginResult.vildate)
            {
                Session["CheckCode"] = Guid.NewGuid(); //只要有錯先隨意產生唯一碼 以防暴力破解，新的CheckCode會在Validate產生。
                getLoginResult.result = false;
                getLoginResult.message = Resources.Res.Log_Err_ImgValideNotEquel;
                return defJSON(getLoginResult);
            }
            #endregion

            #region 帳密碼檢查
            var item = await userManager.FindAsync(model.account, model.password);
            if (item == null)
            {
                getLoginResult.result = false;
                getLoginResult.message = Resources.Res.Login_Err_Password;
                return defJSON(getLoginResult);
            }
            await SignInAsync(item, model.rememberme);
            getLoginResult.result = true;
            getLoginResult.url = Url.Content(CommWebSetup.ManageDefCTR);
            Response.Cookies.Add(new HttpCookie(CommWebSetup.Cookie_UserName, item.UserName));
            Response.Cookies.Add(new HttpCookie(CommWebSetup.Cookie_LastLogin, DateTime.Now.ToString()));
            #endregion

            //語系使用
            HttpCookie WebLang = Request.Cookies[CommWebSetup.WebCookiesId + ".Lang"];
            var account = model.account.ToLower();
            if (account == "manager2")
            {
                WebLang.Value = "zh-CN";
            }
            else
            {
                WebLang.Value = model.lang;
            }

            Response.Cookies.Add(WebLang);

            try
            {
                var db = getDB0();

                var item_department = await db.Department.FindAsync(item.department_id);

                Response.Cookies.Add(new HttpCookie(CommWebSetup.Cookie_DepartmentId, item.department_id.ToString()));
                Response.Cookies.Add(new HttpCookie(CommWebSetup.Cookie_DepartmentName, item_department.department_name));
                Response.Cookies.Add(new HttpCookie("user_login", Server.UrlEncode(EncryptString.desEncryptBase64("N"))));
                var item_lang = db.i_Lang
                    .Where(x => x.lang == WebLang.Value)
                    .Select(x => new { x.area })
                    .Single();

                ViewData["lang"] = item_lang.area;
                db.Dispose();
            }
            catch (Exception ex)
            {
                getLoginResult.result = false;
                getLoginResult.message = ex.Message;
                return defJSON(getLoginResult);
            }
            finally
            {
                //db0.Dispose();
            }
            return defJSON(getLoginResult);
        }
        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(
                new AuthenticationProperties() { IsPersistent = isPersistent },
                await user.GenerateUserIdentityAsync(UserManager));
        }
        [HttpPost]
        public String ajax_Lang()
        {
            using (var db = getDB0())
            {
                var langs = db.i_Lang.Where(x => x.isuse == true).OrderBy(x => x.sort);
                return defJSON(langs);
            }
        }
        public RedirectResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();

            string getLoginFlag = string.Empty;
            var getCookie = Request.Cookies["user_login"];
            getLoginFlag = getCookie == null ? "Y" :
                EncryptString.desDecryptBase64(Server.UrlDecode(getCookie.Value)); //Value:N

            removeCookie("user_id");
            removeCookie("user_name");
            removeCookie("user_login");

            //return Redirect("~/_SysAdm?t=" + DateTime.Now.Ticks);

            if (getLoginFlag == "Y")
                return Redirect("~");
            else
                return Redirect("~/_SysAdm?t=" + DateTime.Now.Ticks);

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<string> ajax_ForgotPassword(ForgotPasswordViewModel model)
        {
            ResultInfo rAjaxResult = new ResultInfo();

            try
            {
                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindByEmailAsync(model.Email);
                    //2014-5-20 Jerry 目前本系統不作Email驗證工作
                    //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                    if (user == null)
                        throw new Exception(Resources.Res.Login_Err_Password);

                    string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ResetPassword", "MNGLogin", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "重設密碼", "請按 <a href=\"" + callbackUrl + "\">這裏</a> 重設密碼");

                    rAjaxResult.result = true;
                }
                else
                {
                    List<string> errMessage = new List<string>();
                    foreach (ModelState modelState in ModelState.Values)
                        foreach (ModelError error in modelState.Errors)
                            errMessage.Add(error.ErrorMessage);

                    rAjaxResult.message = String.Join(":", errMessage);
                    rAjaxResult.result = false;
                }
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
            }

            return defJSON(rAjaxResult);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<string> ajax_ResetPassword(ResetPasswordViewModel model)
        {
            ResultInfo rAjaxResult = new ResultInfo();

            try
            {
                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        rAjaxResult.result = false;
                        rAjaxResult.message = Resources.Res.Log_Err_NoThisUser;
                        return defJSON(rAjaxResult);
                    }
                    IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                    if (result.Succeeded)
                    {
                        rAjaxResult.result = true;
                        return defJSON(rAjaxResult);
                    }
                    else
                    {
                        rAjaxResult.message = String.Join(":", result.Errors);
                        rAjaxResult.result = false;
                        return defJSON(rAjaxResult);
                    }
                }
                else
                {
                    List<string> errMessage = new List<string>();
                    foreach (ModelState modelState in ModelState.Values)
                        foreach (ModelError error in modelState.Errors)
                            errMessage.Add(error.ErrorMessage);

                    rAjaxResult.message = String.Join(":", errMessage);
                    rAjaxResult.result = false;
                    return defJSON(rAjaxResult);
                }
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
                return defJSON(rAjaxResult);
            }
        }
        private void removeCookie(string key)
        {
            var c = new HttpCookie(key);
            c.Values.Clear();
            c.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Set(c);
        }
        public class MemberLogin
        {
            public string act { get; set; }
            public string pwd { get; set; }
        }
        class LoginResult
        {
            public String title { get; set; }
            public Boolean vildate { get; set; }
            public Boolean result { get; set; }
            public String message { get; set; }
            public String url { get; set; }
        }
        public class LoginViewModel
        {
            [Required]
            public string account { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string password { get; set; }
            [Required]
            public string lang { get; set; }
            [Required]
            public string validate { get; set; }
            public bool rememberme { get; set; }
        }
    }
}
