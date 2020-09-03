using CP.Common;
using CP.Core;
using CP.Core.Web;
using CP.Entiry;
using CP.Entiry.Web;
using CP.EntityBase;
using System;
using System.Collections.Generic;
using System.Data;

public partial class App_index : SuperMasterMask
{
    #region  页面加载 崔萌 2019-12-30 10:04:41
    /// <remarkse>崔萌 2019-12-30 10:04:41</remarkse>
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
                case "M_LoadInfo":
                    Load();
                    break;
            }
        }
        catch (System.Threading.ThreadAbortException ex)
        {
            // Response.End() 引发此异常. 无需处理.
        }
        catch (Exception ex)
        {
            base.Exception(ex);
        }
    }
    #endregion

    #region 加载数据 崔萌 2019-12-30 10:05:08
    /// <remarkse>崔萌 2019-12-30 10:05:08</remarkse>
    /// <summary>
    /// 加载数据
    /// </summary>
    protected void Load()
    {
        Dictionary<string, string> json = new Dictionary<string, string>();
        try
        {

            json.Clear();
            json.Add("Result", "0");
        }
        catch (UIException ex)
        {
            json.Clear();
            json.Add("Result", "-99");
            json.Add("errMsg", ex.Message);
        }
        catch (Exception ex)
        {
            base.Exception(ex);
        }

        Response.Write(JSONHelper.DictionaryToJson(json));
        Response.End();
    }
    #endregion

    #region 获取未读取的消息数量 崔萌 2020-4-4 15:12:14
    /// <remarks>崔萌 2020-4-4 15:12:14</remarks>
    /// <summary>
    /// 获取未读取的消息数量
    /// </summary>
    /// <returns>消息数量</returns>
    protected int GetNoReadMessageNumber()
    {
        try
        {
            string[] fields = new string[] {
                string.Format(" COUNT(1) AS Number ")
            };

            QueryParams param = new QueryParams();
            param.addWhere(sys_MessageAttr.FK_User_ID, "=", "@FK_User_ID", WebUser.ID);
            param.addAnd();
            param.addWhere(sys_MessageAttr.IsRead, "=", "@IsRead", Convert.ToInt32(MessageIsRead.UnRead));

            sys_Message message = new sys_Message();
            DataTable dt = message.QueryToDataTable(param, fields);

            return Convert.ToInt32(dt.Rows[0]["Number"]);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    #endregion
}