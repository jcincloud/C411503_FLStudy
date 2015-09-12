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
    public class AllCategoryL1Controller : ajaxApi<All_Category_L1, q_All_Category_L1>
    {
        public async Task<IHttpActionResult> Get(int id)
        {
            using (db0 = getDB0())
            {
                item = await db0.All_Category_L1.FindAsync(id);
                r = new ResultInfo<All_Category_L1>() { data = item };
            }

            return Ok(r);
        }
        public IHttpActionResult Get([FromUri]q_All_Category_L1 q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var items = (from x in db0.All_Category_L1
                             orderby x.all_category_l1_id
                             where x.i_Hide == false
                             select new m_All_Category_L1()
                             {
                                 all_category_l1_id = x.all_category_l1_id,
                                 l1_name = x.l1_name,
                                 memo = x.memo,
                                 sort = x.sort
                             });

                if (q.l1_id != null)
                {
                    items = items.Where(x => x.all_category_l1_id == q.l1_id);
                }

                if (q.title != null)
                {
                    items = items.Where(x => x.l1_name.Contains(q.title));
                }

                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();
                if (System.Globalization.CultureInfo.CurrentCulture.Name == "zh-CN")
                {
                    foreach (var i in resultItems)
                    {
                        i.l1_name = ProcCore.NetExtension.ExtensionString.ToSimplified(i.l1_name);
                        i.memo = ProcCore.NetExtension.ExtensionString.ToSimplified(i.memo);
                    }
                }

                return Ok(new GridInfo<m_All_Category_L1>()
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
        public async Task<IHttpActionResult> Put([FromBody]All_Category_L1 md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                item = await db0.All_Category_L1.FindAsync(md.all_category_l1_id);

                item.l1_name = md.l1_name;
                item.memo = md.memo;
                item.sort = md.sort;
                //item.i_Hide = md.i_Hide;


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
        public async Task<IHttpActionResult> Post([FromBody]All_Category_L1 md)
        {
            md.all_category_l1_id = GetNewId(ProcCore.Business.CodeTable.All_Category_L1);
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


                db0.All_Category_L1.Add(md);
                await db0.SaveChangesAsync();

                rAjaxResult.result = true;
                rAjaxResult.id = md.all_category_l1_id;
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
                    item = new All_Category_L1() { all_category_l1_id = id };
                    db0.All_Category_L1.Attach(item);
                    db0.All_Category_L1.Remove(item);
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
