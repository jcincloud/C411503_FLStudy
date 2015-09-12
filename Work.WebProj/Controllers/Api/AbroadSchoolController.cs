using DotWeb.Helpers;
using ProcCore.Business.DB0;
using ProcCore.HandleResult;
using ProcCore.WebCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DotWeb.Api
{
    public class AbroadSchoolController : ajaxApi<AbroadSchool, q_AbroadSchbool>
    {
        public async Task<IHttpActionResult> Get(int id)
        {
            using (db0 = getDB0())
            {
                item = await db0.AbroadSchool.FindAsync(id);
                r = new ResultInfo<AbroadSchool>() { data = item };
            }

            return Ok(r);
        }
        public IHttpActionResult Get([FromUri]q_AbroadSchbool q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var items = (from x in db0.AbroadSchool
                             orderby x.i_Lang descending, x.sort descending
                             where x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name
                             select new m_AbroadSchool()
                             {
                                 abroad_school_id = x.abroad_school_id,
                                 school_title = x.school_title,
                                 country_category = x.country_category,
                                 operation_category = x.operation_category,
                                 category = x.category,//區分 世界大學資料庫:1,各國語言學校資料:2
                                 sort = x.sort,
                                 i_Hide = x.i_Hide,
                                 i_Lang = x.i_Lang
                             });

                if (q.category != null)//國家
                {
                    items = items.Where(x => x.category == q.category);
                }
                if (q.title != null)
                {
                    items = items.Where(x => x.school_title.Contains(q.title));
                }

                if (q.country != null)//國家
                {
                    items = items.Where(x => x.country_category == q.country);
                }
                if (q.operation != null)//公/私立
                {
                    items = items.Where(x => x.operation_category == q.operation);
                }
                //if (q.i_Lang != null)
                //{
                //    items = items.Where(x => x.i_Lang == q.i_Lang);
                //}

                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                return Ok(new GridInfo<m_AbroadSchool>()
                {
                    rows = resultItems,
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                });
            }
            #endregion
        }
        public async Task<IHttpActionResult> Put([FromBody]AbroadSchool md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                item = await db0.AbroadSchool.FindAsync(md.abroad_school_id);

                item.school_title = md.school_title;
                item.list_intro = md.list_intro;
                item.content_intro = md.content_intro;
                item.school_features = md.school_features;
                item.school_info = md.school_info;
                item.school_apply = md.school_apply;
                item.country_category = md.country_category;
                item.operation_category = md.operation_category;
                item.sort = md.sort;
                item.i_Hide = md.i_Hide;
                //item.i_Lang = md.i_Lang;

                item.i_UpdateUserID = this.UserId;
                item.i_UpdateDateTime = DateTime.Now;
                item.i_UpdateDeptID = this.departmentId;
                await db0.SaveChangesAsync();
                rAjaxResult.result = true;
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.ToString();
            }
            finally
            {
                db0.Dispose();
            }
            return Ok(rAjaxResult);
        }
        public async Task<IHttpActionResult> Post([FromBody]AbroadSchool md)
        {
            md.abroad_school_id = GetNewId(ProcCore.Business.CodeTable.AbroadSchool);
            ResultInfo rAjaxResult = new ResultInfo();
            if (!ModelState.IsValid)
            {
                rAjaxResult.message = ModelStateErrorPack();
                rAjaxResult.result = false;
                return Ok(rAjaxResult);
            }

            try
            {
                #region working a
                db0 = getDB0();

                md.i_InsertUserID = this.UserId;
                md.i_InsertDateTime = DateTime.Now;
                md.i_InsertDeptID = this.departmentId;
                //md.i_Lang = "zh-TW";
                md.i_Lang = System.Globalization.CultureInfo.CurrentCulture.Name;

                db0.AbroadSchool.Add(md);
                await db0.SaveChangesAsync();

                rAjaxResult.result = true;
                rAjaxResult.id = md.abroad_school_id;
                return Ok(rAjaxResult);
                #endregion
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
                return Ok(rAjaxResult);
            }
            finally
            {
                db0.Dispose();
            }
        }
        public async Task<IHttpActionResult> Delete([FromUri]int[] ids)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                foreach (var id in ids)
                {
                    item = new AbroadSchool() { abroad_school_id = id };
                    db0.AbroadSchool.Attach(item);
                    db0.AbroadSchool.Remove(item);
                }


                await db0.SaveChangesAsync();

                rAjaxResult.result = true;
                return Ok(rAjaxResult);
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
                return Ok(rAjaxResult);
            }
            finally
            {
                db0.Dispose();
            }
        }
    }
}
