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
    public class InfoController : ajaxApi<Info, q_Info>
    {
        public async Task<IHttpActionResult> Get(int id)
        {
            using (db0 = getDB0())
            {
                item = await db0.Info.FindAsync(id);
                r = new ResultInfo<Info>() { data = item };
            }

            return Ok(r);
        }
        public IHttpActionResult Get([FromUri]q_Info q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var items = (from x in db0.Info
                             orderby x.info_id
                             select new m_Info()
                             {
                                 info_id = x.info_id,
                                 info_title = x.info_title,
                                 memo = x.memo,
                                 sort = x.sort,
                                 i_Hide = x.i_Hide
                             });

                if (q.title != null)
                {
                    items = items.Where(x => x.info_title.Contains(q.title));
                }


                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                return Ok(new GridInfo<m_Info>()
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
        public async Task<IHttpActionResult> Put([FromBody]Info md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                item = await db0.Info.FindAsync(md.info_id);
                string lang = string.Empty;
                if (md.InfoDetail.Count() > 0)
                    lang = md.InfoDetail.FirstOrDefault().i_Lang;

                var details = item.InfoDetail;

                foreach (var detail in details.Where(x => x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name))
                {
                    var md_detail = md.InfoDetail.First(x => x.info_detail_id == detail.info_detail_id);
                    detail.detail_title = md_detail.detail_title;
                    detail.sort = md_detail.sort;
                    detail.detail_content = md_detail.detail_content;
                    detail.i_Hide = md_detail.i_Hide;
                }

                var add_detail = md.InfoDetail.Where(x => x.edit_state == EditState.Insert);
                foreach (var detail in add_detail)
                {
                    detail.i_InsertUserID = this.UserId;
                    detail.i_InsertDateTime = DateTime.Now;
                    detail.i_InsertDeptID = this.departmentId;
                    detail.i_Lang = System.Globalization.CultureInfo.CurrentCulture.Name;
                    //detail.i_Lang = "zh-TW";
                    details.Add(detail);
                }



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
        public async Task<IHttpActionResult> Post([FromBody]Info md)
        {
            md.info_id = GetNewId(ProcCore.Business.CodeTable.Info);
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
                md.i_Lang = "zh-TW";

                db0.Info.Add(md);
                await db0.SaveChangesAsync();

                rAjaxResult.result = true;
                rAjaxResult.id = md.info_id;
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
                    item = new Info() { info_id = id };
                    db0.Info.Attach(item);
                    db0.Info.Remove(item);
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
