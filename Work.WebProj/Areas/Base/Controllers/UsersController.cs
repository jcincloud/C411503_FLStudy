using DotWeb.Controller;
using DotWeb.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProcCore.Business.DB0;
using ProcCore.HandleResult;
using ProcCore.NetExtension;
using ProcCore.WebCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DotWeb.Areas.Sys_Base.Controllers
{
    public class UsersController : CtrlTSN<ApplicationUser, q_AspNetUsers>
    {
        #region Action and function section
        public ActionResult Main()
        {
            ActionRun();
            return View(new c_AspNetUsers());
        }
        public ActionResult ChangePassword()
        {
            ActionRun();
            return View();
        }
        #endregion

        #region ajax call section
        public override String aj_Init()
        {
            List<RoleArray> listRole = new List<RoleArray>();
            var items = roleManager.Roles.Select(x => new { x.Name, x.Id }).ToList();
            foreach (var item in items)
            {
                listRole.Add(new RoleArray() { role_id = item.Id, role_name = item.Name, role_use = false });
            }


            return defJSON(
            new
            {
                checkbox_roles = listRole
            }
            );
        }
        [HttpPost]
        public override async Task<string> aj_MasterGet(string sn)
        {
            var item = await UserManager.FindByIdAsync(sn);
            ApplicationUser user = new ApplicationUser();

            user.Id = item.Id;
            user.UserName = item.UserName;
            user.Email = item.Email;
            List<RoleArray> mem_roles = new List<RoleArray>();

            foreach (var role in roleManager.Roles)
                if (item.Roles.Any(x => x.RoleId == role.Id))
                    mem_roles.Add(new RoleArray() { role_id = role.Id, role_use = true, role_name = role.Name });
                else
                    mem_roles.Add(new RoleArray() { role_id = role.Id, role_use = false, role_name = role.Name });

            user.role_array = mem_roles.ToArray();
            var r = new ResultInfo<ApplicationUser>() { data = user };

            return defJSON(r);
        }
        [HttpPost]
        public override String aj_MasterSearch(q_AspNetUsers q)
        {
            #region

            var items = UserManager.Users
                .OrderBy(x => x.UserName).AsEnumerable();

            int page = (q.page == null ? 1 : q.page.CInt());
            int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

            items = items.Skip(startRecord).Take(this.defPageSize);

            return defJSON(new GridInfo<ApplicationUser>()
            {
                rows = items.ToList(),
                total = PageCount.TotalPage,
                page = PageCount.Page,
                records = PageCount.RecordCount,
                startcount = PageCount.StartCount,
                endcount = PageCount.EndCount
            });
            #endregion
        }
        [HttpPost]
        public async override Task<string> aj_MasterUpdate(ApplicationUser md)
        {
            ResultInfo rAjaxResult = new ResultInfo();

            try
            {
                ApplicationUser item = await UserManager.FindByIdAsync(md.Id);

                item.UserName = md.UserName;
                item.Email = md.Email;
                foreach (var roleObj in md.role_array)
                {
                    if (roleObj.role_use)
                    {
                        if (!item.Roles.Any(x => x.RoleId == roleObj.role_id))
                            item.Roles.Add(new IdentityUserRole() { RoleId = roleObj.role_id });
                    }
                    else
                    {
                        var del_item = item.Roles.Where(x => x.RoleId == roleObj.role_id).FirstOrDefault();
                        if (del_item != null)
                            item.Roles.Remove(del_item);
                    }
                }

                var result = await UserManager.UpdateAsync(item);
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
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
            }
            return defJSON(rAjaxResult);
        }
        [HttpPost]
        public async override Task<string> aj_MasterInsert(ApplicationUser md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                var result = await UserManager.CreateAsync(md);

                if (result.Succeeded)
                {
                    item = await UserManager.FindByNameAsync(md.UserName);
                    foreach (var roleObj in md.role_array)
                        item.Roles.Add(new IdentityUserRole() { RoleId = roleObj.role_id });

                    result = await UserManager.UpdateAsync(item);

                    if (result.Succeeded)
                    {
                        rAjaxResult.result = true;
                    }
                    else
                    {
                        rAjaxResult.result = false;
                        rAjaxResult.message = String.Join(":", result.Errors);
                    }
                }
                else
                {
                    rAjaxResult.message = String.Join(":", result.Errors);
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
        public async Task<string> ajax_MasterInsert(RegisterViewModel md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser() { UserName = md.UserName, Email = md.Email };

                    var result = await UserManager.CreateAsync(user, md.Password);

                    if (result.Succeeded)
                    {
                        foreach (var roleObj in md.role_array)
                        {
                            if (roleObj.role_use)
                                user.Roles.Add(
                                    new IdentityUserRole()
                                    {
                                        UserId = user.Id,
                                        RoleId = roleObj.role_id
                                    });
                        }

                        result = await UserManager.UpdateAsync(user);
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
                    rAjaxResult.message = ModelStateErrorPack();
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
        public async override Task<string> aj_MasterDel(string[] sns)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                foreach (var sn in sns)
                {
                    var result = await UserManager.DeleteAsync(new ApplicationUser() { Id = sn });
                    if (!result.Succeeded)
                    {
                        throw new Exception(String.Join(":", result.Errors));
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
        #endregion

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