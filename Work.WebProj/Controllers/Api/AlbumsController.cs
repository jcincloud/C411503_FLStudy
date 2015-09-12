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
    public class AlbumsController : ajaxApi<Albums, q_Albums>
    {
        public async Task<IHttpActionResult> Get(int id)
        {
            using (db0 = getDB0())
            {
                item = await db0.Albums.FindAsync(id);
                r = new ResultInfo<Albums>() { data = item };
            }

            return Ok(r);
        }
        public IHttpActionResult Get([FromUri]q_Albums q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var items = (from x in db0.Albums
                             orderby x.i_Lang descending, x.sort descending
                             where x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name
                             select new m_Albums()
                             {
                                 albums_id = x.albums_id,
                                 title=x.title,
                                 url = x.url,
                                 category = x.category,
                                 year = x.year,
                                 sort = x.sort,
                                 i_Hide=x.i_Hide,
                                 i_Lang=x.i_Lang
                             });


                if (q.category != null)
                {
                    items = items.Where(x => x.category == q.category);
                }

                if (q.title != null)
                {
                    items = items.Where(x => x.title.Contains(q.title));
                }

                if (q.year != null)
                {
                    items = items.Where(x => x.year == q.year);
                }
                //if (q.i_Lang != null)
                //{
                //    items = items.Where(x => x.i_Lang == q.i_Lang);
                //}


                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                return Ok(new GridInfo<m_Albums>()
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
        public async Task<IHttpActionResult> Put([FromBody]Albums md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                item = await db0.Albums.FindAsync(md.albums_id);

                item.title = md.title;
                item.url = md.url;
                item.albums_content = md.albums_content;
                item.category = md.category;
                item.year = md.year;
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
        public async Task<IHttpActionResult> Post([FromBody]Albums md)
        {
            md.albums_id = GetNewId(ProcCore.Business.CodeTable.Albums);
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

                db0.Albums.Add(md);
                await db0.SaveChangesAsync();

                rAjaxResult.result = true;
                rAjaxResult.id = md.albums_id;
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
                    item = new Albums() { albums_id = id };
                    db0.Albums.Attach(item);
                    db0.Albums.Remove(item);
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
