using CP.Core.Web;
using System;
using System.Threading;

/// <summary>
/// 常见问题
/// </summary>
public partial class App_Footer_Problem : SuperPage
{
    #region 页面加载 崔萌 2020-4-22 15:11:38
    /// <remarks>崔萌 2020-4-22 15:11:38</remarks>
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