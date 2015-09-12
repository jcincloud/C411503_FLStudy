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
    public class InfoDetailController : ajaxApi<InfoDetail, q_InfoDetail>
    {
        public async Task<IHttpActionResult> Get(int id)
        {
            using (db0 = getDB0())
            {
                item = await db0.InfoDetail.FindAsync(id);
                r = new ResultInfo<InfoDetail>() { data = item };
            }

            return Ok(r);
        }
        public IHttpActionResult Get([FromUri]q_InfoDetail q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var items = (from x in db0.InfoDetail
                             orderby x.sort
                             where x.info_id==q.main_id && x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name
                             select new m_InfoDetail()
                             {
                                 info_id = x.info_id,
                                 info_detail_id=x.info_detail_id,
                                 detail_title = x.detail_title,
                                 detail_content = x.detail_content,
                                 sort = x.sort,
                                 i_Hide=x.i_Hide,
                                 stereotype=x.stereotype,
                                 i_Lang=x.i_Lang,
                                 edit_state = EditState.Update
                             });

                return Ok(items.ToList());
            }
            #endregion
        }
        public async Task<IHttpActionResult> Put([FromBody]InfoDetail md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                item = await db0.InfoDetail.FindAsync(md.info_detail_id);

                item.detail_title = md.detail_title;
                item.detail_content = md.detail_content;
                item.sort = md.sort;
                item.i_Hide = md.i_Hide;

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
        public async Task<IHttpActionResult> Post([FromBody]InfoDetail md)
        {
            md.info_detail_id = GetNewId(ProcCore.Business.CodeTable.InfoDeatil);
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

                db0.InfoDetail.Add(md);
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
                    item = new InfoDetail() { info_detail_id = id };
                    db0.InfoDetail.Attach(item);
                    db0.InfoDetail.Remove(item);
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
