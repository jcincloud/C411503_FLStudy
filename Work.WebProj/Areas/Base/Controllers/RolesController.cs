using DotWeb.Controller;
using DotWeb.Helpers;
using Microsoft.AspNet.Identity.EntityFramework;
using ProcCore.Business.DB0;
using ProcCore.HandleResult;
using ProcCore.WebCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DotWeb.Areas.Base.Controllers
{
    public class RolesController : BaseController
    {

        #region Action and function section
        public ActionResult Main()
        {
            ActionRun();
            return View();
        }
        #endregion

        #region ajax call section
        public string aj_Init()
        {
            //var db = Open_Logic().get_db0;
            return defJSON(
            new
            {
                //options_unit = db.UnitData.Where(x => x.i_Hide == false).Select(x => new { x.unit_id, x.unit_name, x.sort }).OrderBy(x => x.sort),
                //options_user_state = ngCodeToOption(CodeSheet.user_state.MakeCodes())
            }
            );
        }
        [HttpPost]
        public async Task<string> aj_MasterGet(string sn)
        {
            var item = await roleManager.FindByIdAsync(sn);
            var r = new ResultInfo<IdentityRole>() { data = item };
            return defJSON(r);
        }
        [HttpPost]
        public string aj_MasterSearch(q_AspNetRoles q)
        {
            var items = roleManager.Roles.ToList();

            return defJSON(new GridInfo<IdentityRole>()
            {
                rows = items,
                total = PageCount.TotalPage,
                page = PageCount.Page,
                records = PageCount.RecordCount,
                startcount = PageCount.StartCount,
                endcount = PageCount.EndCount
            });
        }
        [HttpPost]
        public async Task<string> aj_MasterInsert(AspNetRoles md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                if (!await roleManager.RoleExistsAsync(md.Name))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(md.Name));
                    if (result.Succeeded)
                    {
                        var item = await roleManager.FindByNameAsync(md.Name);
                        rAjaxResult.result = true;
                        rAjaxResult.aspnetid = item.Id;
                    }
                    else
                    {
                        rAjaxResult.result = false;
                        rAjaxResult.message = String.Join(":", result.Errors.ToArray());
                    }
                }
                else
                {
                    rAjaxResult.result = false;
                    rAjaxResult.message = "the role " + md.Name + " has exist!";
                }
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
            }
            return defJSON(rAjaxResult);
        }
        #endregion
    }
}