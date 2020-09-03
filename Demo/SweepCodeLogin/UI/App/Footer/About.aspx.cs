using CP.Core.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// 关于我们
/// </summary>
public partial class App_Footer_About : SuperPage
{
    #region 页面加载 崔萌 2020-4-22 15:28:49
    /// <remarks>崔萌 2020-4-22 15:28:49</remarks>
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
                default:
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
}