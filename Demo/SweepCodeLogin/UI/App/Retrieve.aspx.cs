using CP.Common;
using CP.Core;
using CP.Core.Web;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class App_Retrieve : SuperPage
{
    #region 页面加载 崔萌 2019-12-17 11:21:48
    /// <remarks>崔萌 2019-12-17 11:21:48</remarks>
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
                case "M_GetVerificationCode":
                    SendVerificationCode();
                    break;
                case "M_Confirm":
                    ConfirmRetrievePassword();
                    break;
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

    #region 获取验证码 崔萌 2019-12-17 15:02:10
    /// <remarks>崔萌 2019-12-17 15:02:10</remarks>
    /// <summary>
    /// 获取验证码
    /// </summary>
    private void SendVerificationCode()
    {
        Dictionary<string, string> json = new Dictionary<string, string>();

        try
        {
            //1.校验参数
            string mobilePhone = Request.Params["mobilephone"];
            if (string.IsNullOrEmpty(mobilePhone))
            {
                throw new UIException("请输入手机号。");
            }
            if (!Common.CheckMobile(mobilePhone))
            {
                throw new UIException("手机号格式错误。");
            }

            //2.发送验证码
            UserCore core = new UserCore();
            core.RetrievePasswordPostSNS(mobilePhone);

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

    #region 确认找回密码 崔萌 2019-12-18 16:54:26
    /// <remarkse>崔萌 2019-12-18 16:54:26</remarkse>
    /// <summary>
    /// 确认找回密码
    /// </summary>
    private void ConfirmRetrievePassword()
    {
        Dictionary<string, string> json = new Dictionary<string, string>();

        try
        {
            #region 1.校验参数
            string phoneNumber = Request.Params["phoneNumber"];
            string verificationCode = Request.Params["verificationcode"];
            string password = Request.Params["newPassword"];
            string againPassword = Request.Params["againpassword"];

            if (string.IsNullOrEmpty(phoneNumber))
            {
                throw new UIException("请输入手机号。");
            }
            if (!Common.CheckMobile(phoneNumber))
            {
                throw new UIException("手机号格式错误。");
            }
            if (string.IsNullOrEmpty(verificationCode))
            {
                throw new UIException("请输入验证码。");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new UIException("密码必须在6-12位之间请重新输入。");
            }
            if (!password.Equals(againPassword))
            {
                throw new UIException("密码与再次输入密码不一致请重新输入。");
            }
            #endregion

            //2.找回密码
            UserCore core = new UserCore();
            core.ConfirmRetrievePassword(phoneNumber, verificationCode, password);

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
}