using DotWeb.Helpers;
using ProcCore.Business.DB0;
using ProcCore.HandleResult;
using ProcCore.WebCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DotWeb.Api
{
    public class StudyAbroadController : ajaxApi<StudyAbroad, q_StudyAbroad>
    {
        public async Task<IHttpActionResult> Get(int id)
        {
            using (db0 = getDB0())
            {
                item = await db0.StudyAbroad.FindAsync(id);
                r = new ResultInfo<StudyAbroad>() { data = item };
            }

            return Ok(r);
        }
        public IHttpActionResult Get([FromUri]q_StudyAbroad q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var items = (from x in db0.StudyAbroad
                             orderby x.i_Lang descending, x.study_abroad_id descending
                             where x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name
                             select new m_StudyAbroad()
                             {
                                 study_abroad_id = x.study_abroad_id,
                                 planning_name = x.planning_name,
                                 start_date = x.start_date,
                                 end_date = x.end_date,
                                 country_category = x.country_category,
                                 vacation_category = x.vacation_category,
                                 sort = x.sort,
                                 is_past = x.is_past,
                                 i_Lang = x.i_Lang
                             });
                if (q.category != null)
                {
                    if (q.category == 1)//後台各國遊學行程
                    {
                        items = items.Where(x => x.vacation_category != VacationType.Intensive);
                    }
                    else if (q.category == 2)//後台密集式體驗營
                    {
                        items = items.Where(x => x.vacation_category == VacationType.Intensive);
                    }
                }

                if (q.name != null)
                {
                    items = items.Where(x => x.planning_name.Contains(q.name));
                }

                if (q.vacation != null)
                {
                    items = items.Where(x => x.vacation_category == q.vacation);
                }

                if (q.country != null)
                {
                    items = items.Where(x => x.country_category == q.country);
                }
                if (q.is_past != null)
                {
                    items = items.Where(x => x.is_past == q.is_past);
                }
                //if (q.i_Lang != null)
                //{
                //    items = items.Where(x => x.i_Lang == q.i_Lang);
                //}

                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                return Ok(new GridInfo<m_StudyAbroad>()
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
        public async Task<IHttpActionResult> Put([FromBody]StudyAbroad md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                item = await db0.StudyAbroad.FindAsync(md.study_abroad_id);

                item.planning_name = md.planning_name;
                item.intro = md.intro;
                item.intro_titile = md.intro_titile;
                item.intro_content = md.intro_content;
                item.start_date = md.start_date;
                item.end_date = md.end_date;
                item.is_hot = md.is_hot;//是否為熱門行程
                item.vacation_category = md.vacation_category;
                item.country_category = md.country_category;
                item.youtube_iframe = md.youtube_iframe;
                item.curriculum = md.curriculum;
                item.lodging = md.lodging;
                item.activity = md.activity;
                item.sort = md.sort;
                item.i_Hide = md.i_Hide;//是否顯示於前台
                item.is_past = md.is_past;//是否為過往行程
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
        public async Task<IHttpActionResult> Post([FromBody]StudyAbroad md)
        {
            md.study_abroad_id = GetNewId(ProcCore.Business.CodeTable.StudyAbroad);
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


                db0.StudyAbroad.Add(md);
                await db0.SaveChangesAsync();

                rAjaxResult.result = true;
                rAjaxResult.id = md.study_abroad_id;
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
                    item = new StudyAbroad() { study_abroad_id = id };
                    db0.StudyAbroad.Attach(item);
                    db0.StudyAbroad.Remove(item);
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
