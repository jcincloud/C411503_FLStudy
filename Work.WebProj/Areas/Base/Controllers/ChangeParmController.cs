using DotWeb.Controller;
using Microsoft.AspNet.Identity;
using ProcCore.Business.DB0;
using ProcCore.Business.LogicConect;
using ProcCore.HandleResult;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;

namespace DotWeb.Areas.Sys_Base.Controllers
{
    public class ChangeParmController : BaseController
    {
        public ActionResult Main()
        {
            ActionRun();
            return View();
        }
        public ActionResult ChangePassword()
        {
            ActionRun();
            return View();
        }
        public string aj_init()
        {
            parm r;
            var open = openLogic();
            using (db0 = getDB0())
            {
                //公司簡介修改
                string receiveMails = string.Empty;
                if (System.Globalization.CultureInfo.CurrentCulture.Name == "zh-CN")
                {
                    receiveMails = (string)open.getParmValue(ParmDefine.receiveMails_cn);
                }
                else
                {
                    receiveMails = (string)open.getParmValue(ParmDefine.receiveMails);
                }

                r = new parm()
                {
                    receiveMails = receiveMails,
                };
            }

            return defJSON(r);
        }
        //[ValidateInput(false)]
        public string aj_MasterUpdate(parm md)
        {

            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                var open = openLogic();
                using (db0 = getDB0())
                {

                    if (System.Globalization.CultureInfo.CurrentCulture.Name == "zh-CN")
                    {
                        open.setParmValue(ParmDefine.receiveMails_cn, md.receiveMails);
                    }
                    else
                    {
                        open.setParmValue(ParmDefine.receiveMails, md.receiveMails);
                    }

                }
                rAjaxResult.result = true;
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
            }
            return defJSON(rAjaxResult);
        }
        [HttpPost]
        public async Task<string> aj_MasterPasswordUpdate(ManageUserViewModel md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), md.OldPassword, md.NewPassword);

                    if (result.Succeeded)
                    {
                        rAjaxResult.result = true;
                    }
                    else
                    {
                        rAjaxResult.message = String.Join(":", result.Errors);
                        rAjaxResult.result = false;
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
                }
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
            }

            return defJSON(rAjaxResult);

        }
    }
}
