using CP.Common;
using CP.Core;
using CP.Core.Web;
using CP.Entiry;
using CP.EntityBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;

public partial class App_Portal : SuperPage
{
    #region  页面加载 崔萌 2019-12-31 14:17:44
    /// <remarks>崔萌 2019-12-31 14:17:44</remarks>
    /// <summary>
    /// 页面加载
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            base.Page_Load(sender, e);
            string action = base.Action;
            switch (action)
            {
                case "M_Load":
                    Load();
                    break;
            }
        }
        catch (ThreadAbortException ex)
        {
            // Response.End() 引发此异常. 无需处理.
        }
        catch (Exception ex)
        {
            base.Exception(ex);
        }
    }
    #endregion

    #region 加载数据 崔萌 2019-12-31 14:23:53
    /// <remarks>崔萌 2019-12-31 14:23:53</remarks>
    /// <summary>
    /// 加载数据
    /// </summary>
    protected void Load()
    {
        Dictionary<string, string> json = new Dictionary<string, string>();
        //try
        //{
        //    //1.获取时间
        //    DateTime now = DateTime.Now.Date;//当前日期

        //    //当前是本周第几天
        //    int weeknow = Convert.ToInt32(now.DayOfWeek);
        //    //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天
        //    weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
        //    int daydiff = (-1) * weeknow;
        //    DateTime startWeek = now.AddDays(daydiff);//本周周一
        //    DateTime startMonth = now.AddDays(1 - now.Day);//本月1日

        //    //2.组装查询语句
        //    StringBuilder sql = new StringBuilder();
        //    sql.AppendFormat(" SELECT COUNT(1) AS Number, 'panum' AS Name FROM {0} WHERE {1}={2} ",
        //        b_Transaction_PA.PhysicsTable, b_Transaction_PAAttr.TranState,
        //        Convert.ToInt32(TechTranState.Publish));//0专利交易库数量
        //    sql.Append(" UNION ALL ");
        //    sql.AppendFormat(" SELECT SUM(ISNULL({0}, 0)) AS Number, 'transactionamount' AS Name FROM {1} WHERE {2}<>3 ",
        //        b_Transaction_PAAttr.ExpePrice, b_Transaction_PA.PhysicsTable, b_Transaction_PAAttr.TranState);//1交易金额
        //    sql.Append(" UNION ALL ");
        //    sql.AppendFormat(" SELECT COUNT(1) AS Number, 'reqnum' AS Name FROM {0} WHERE {1}='{2}' ",
        //        b_Transaction_Req.PhysicsTable, b_Transaction_ReqAttr.IsDelete, IsDelete.N);//2技术需求数量
        //    sql.Append(" UNION ALL ");
        //    sql.AppendFormat(@" SELECT COUNT(1) AS Number, 'dealnum' AS Name FROM {0} a
        //                        LEFT JOIN {1} b ON a.{2} = b.{3} AND b.{4} = '{0}' WHERE b.State={5} ",
        //                     b_Transaction_PA.PhysicsTable, b_MyOrders.PhysicsTable, b_Transaction_PAAttr.ID,
        //                     b_MyOrdersAttr.FK_ID, b_MyOrdersAttr.SubmitType, Convert.ToInt32(OrderState.HasPay));//3成功案例
        //    sql.Append(" UNION ALL ");
        //    sql.AppendFormat(" SELECT COUNT(1) AS Number,'daynum' AS Name FROM {0} WHERE {1}=1 AND ({2} BETWEEN '{3} 00:00:00' AND '{3} 23:59:59') ",
        //        b_Transaction_PA.PhysicsTable, b_Transaction_PAAttr.TranState, b_Transaction_PAAttr.PublishDate, now.ToShortDateString());//4本日最新发布
        //    sql.Append(" UNION ALL ");
        //    sql.AppendFormat(" SELECT COUNT(1) AS Number,'weeknum' AS Name FROM {0} WHERE {1}=1 AND {2} >='{3}'  ",
        //        b_Transaction_PA.PhysicsTable, b_Transaction_PAAttr.TranState, b_Transaction_PAAttr.PublishDate, startWeek);//5本周最新发布
        //    sql.Append(" UNION ALL ");
        //    sql.AppendFormat(" SELECT COUNT(1) AS Number,'monthnum' AS Name FROM {0} WHERE {1}=1 AND {2} >= '{3}' ",
        //        b_Transaction_PA.PhysicsTable, b_Transaction_PAAttr.TranState, b_Transaction_PAAttr.PublishDate, startMonth);//6本月最新发布

        //    //3.查询
        //    DataTable dt = DBHelper.ExecuteDataTable(sysConst.ReadConnStrName, sql.ToString());

        //    //4.获取特价秒杀
        //    DataTable bargainData = QueryPABargainData();

        //    //5.查询成交动态
        //    DataTable transactionTrendsData = QueryTransactionTrends();

        //    json.Clear();
        //    json.Add("Result", "0");
        //    json.Add("Data", JSONHelper.DataTableToJson(dt));
        //    json.Add("BargainData", JSONHelper.DataTableToJson(bargainData));
        //    json.Add("TransactionTrendsData", JSONHelper.DataTableToJson(transactionTrendsData));
        //}
        //catch (UIException ex)
        //{
        //    json.Clear();
        //    json.Add("Result", "-99");
        //    json.Add("errMsg", ex.Message);
        //}
        //catch (Exception ex)
        //{
        //    base.Exception(ex);
        //}

        Response.Write(JSONHelper.DictionaryToJson(json));
        Response.End();
    }
    #endregion
}