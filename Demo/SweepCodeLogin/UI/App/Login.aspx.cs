using CP.Common;
using CP.Core.Web;
using CP.Entiry.Web;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class App_Login : SuperPage
{
    #region  页面加载 崔萌 2019-12-17 10:22:40
    /// <remarks>崔萌 2019-12-17 10:22:40</remarks>
    /// <summary>
    /// 页面加载
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["dotype"] == "logout")
            {
                LoginOut();
            }

            switch (base.Action)
            {
                case "m_login":
                    Login();
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

    #region 登录 崔萌 2019-12-17 10:23:50
    /// <remarks>崔萌 2019-12-17 10:23:50</remarks>
    /// <summary>
    ///  登陆
    /// </summary>
    private void Login()
    {
        Dictionary<string, string> json = new Dictionary<string, string>();

        try
        {
            //1.校验账号和密码
            //string usercode = Request.Params["usercode"];
            //string password = Request.Params["password"];

            //if (string.IsNullOrEmpty(usercode))
            //{
            //    throw new UIException("请填写登陆账号。");
            //}
            //if (string.IsNullOrEmpty(password))
            //{
            //    throw new UIException("请填写账号密码。");
            //}

            ////2.登录
            //string errMsg = string.Empty;

            //bool islogin = new UserCore().Login(usercode, password, LoginType.Forepart, out errMsg);

            //if (!islogin)
            //{
            //    throw new UIException(errMsg);
            //}

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

    #region 登出 崔萌 2019-12-17 10:33:08
    /// <remarks>崔萌 2019-12-17 10:33:08</remarks>
    /// <summary>
    ///  登出
    /// </summary>
    private void LoginOut()
    {
        WebUser.LoginOut();
    }
    #endregion

    #region 获取随机数 崔萌 2020-4-29 15:37:14
    /// <summary>
    /// 防止csrf攻击（跨站请求伪造攻击）,设置为简单的随机数
    /// </summary>
    /// <returns></returns>
    public string GetState()
    {
        string state = Common.CreateGUID();
        Common.Session["WeChatState"] = state;

        return state;
    }
    #endregion
}