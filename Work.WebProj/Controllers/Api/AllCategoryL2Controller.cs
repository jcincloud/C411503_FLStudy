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
    public class AllCategoryL2Controller : ajaxApi<All_Category_L2, q_All_Category_L2>
    {
        public async Task<IHttpActionResult> Get(int id)
        {
            using (db0 = getDB0())
            {
                item = await db0.All_Category_L2.FindAsync(id);
                r = new ResultInfo<All_Category_L2>() { data = item };
            }

            return Ok(r);
        }
        public IHttpActionResult Get([FromUri]q_All_Category_L2 q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var items = (from x in db0.All_Category_L2
                             orderby x.sort descending
                             where x.all_category_l1_id == q.l1_id && x.i_Lang == System.Globalization.CultureInfo.CurrentCulture.Name
                             select new m_All_Category_L2()
                             {
                                 all_category_l1_id = x.all_category_l1_id,
                                 all_category_l2_id = x.all_category_l2_id,
                                 l2_name = x.l2_name,
                                 memo = x.memo,
                                 sort = x.sort,
                                 i_Hide = x.i_Hide,
                                 i_Lang = x.i_Lang
                             });

                if (q.l1_id != null)
                {
                    items = items.Where(x => x.all_category_l1_id == q.l1_id);
                }


                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                return Ok(new GridInfo<m_All_Category_L2>()
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
        public async Task<IHttpActionResult> Put([FromBody]All_Category_L2 md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                item = await db0.All_Category_L2.FindAsync(md.all_category_l2_id);

                item.l2_name = md.l2_name;
                item.memo = md.memo;
                item.sort = md.sort;
                item.i_Hide = md.i_Hide;


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
        public async Task<IHttpActionResult> Post([FromBody]All_Category_L2 md)
        {
            //if (md.all_category_l1_id == CategoryType.Country)
            //{
            //    md.all_category_l2_id = GetNewId(ProcCore.Business.CodeTable.CountryCategroy);
            //}
            //else if (md.all_category_l1_id == CategoryType.Findout)
            //{
            //    md.all_category_l2_id = GetNewId(ProcCore.Business.CodeTable.EmailCategroy);
            //}
            //else if (md.all_category_l1_id == CategoryType.Feedback_year)
            //{
            //    md.all_category_l2_id = GetNewId(ProcCore.Business.CodeTable.YearCategroy);
            //}
            //else if (md.all_category_l1_id == CategoryType.Feedback_category)
            //{
            //    md.all_category_l2_id = GetNewId(ProcCore.Business.CodeTable.FeedbackCategroy);
            //}
            //else if (md.all_category_l1_id == CategoryType.Interest)
            //{
            //    md.all_category_l2_id = GetNewId(ProcCore.Business.CodeTable.Interest);
            //}

            md.all_category_l2_id = GetNewId(ProcCore.Business.CodeTable.All_Category_L2);

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

                db0.All_Category_L2.Add(md);
                await db0.SaveChangesAsync();

                rAjaxResult.result = true;
                rAjaxResult.id = md.all_category_l2_id;
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
                    bool abrodSchool_exist = db0.AbroadSchool.Any(x => x.country_category == id);
                    bool alubms_exist = db0.Albums.Any(x => x.category == id || x.year == id);
                    bool email_exist = db0.EmailContact.Any(x => x.how_findout == id);
                    bool feedback_exist = db0.Feedback.Any(x => x.category == id || x.year == id);
                    bool studyAbroad_exist = db0.StudyAbroad.Any(x => x.country_category == id);
                    bool findout_exist = db0.EmailContact.Any(x => x.how_findout == id);

                    if (abrodSchool_exist || alubms_exist || email_exist || feedback_exist || studyAbroad_exist || findout_exist)
                    {
                        rAjaxResult.result = false;
                        rAjaxResult.message = Resources.Res.Log_Err_Delete_CategoryExist;
                        return Ok(rAjaxResult);
                    }
                    else
                    {
                        item = new All_Category_L2() { all_category_l2_id = id };
                        db0.All_Category_L2.Attach(item);
                        db0.All_Category_L2.Remove(item);
                    }

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
