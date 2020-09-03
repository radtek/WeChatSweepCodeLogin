using CP.Common;
using CP.Core;
using CP.Core.Web;
using System;
using System.Collections.Generic;
using System.Threading;

public partial class App_Register : SuperPage
{
    #region 页面加载 崔萌 2019-12-18 08:53:00
    /// <remarkse>崔萌 2019-12-18 08:53:00</remarkse>
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
                    GetVerificationCode();
                    break;
                case "M_Register":
                    Register();
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

    #region 获取验证码 崔萌 2019-12-18 09:48:36
    /// <remarks>崔萌 2019-12-18 09:48:36</remarks>
    /// <summary>
    /// 获取验证码
    /// </summary>
    private void GetVerificationCode()
    {
        Dictionary<string, string> json = new Dictionary<string, string>();

        try
        {
            //1.校验参数
            string phonenumber = Request.Params["phonenumber"];
            if (string.IsNullOrEmpty(phonenumber))
            {
                throw new UIException("请输入手机号。");
            }
            if (!Common.CheckMobile(phonenumber))
            {
                throw new UIException("手机号格式错误。");
            }

            //2.发送验证码
            UserCore core = new UserCore();
            core.RegisterSendVerificationCode(phonenumber);

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

    #region 注册 崔萌 2019-12-18 15:00:37
    /// <remarks>崔萌 2019-12-18 15:00:37</remarks>
    /// <summary>
    /// 注册
    /// </summary>
    private void Register()
    {
        Dictionary<string, string> json = new Dictionary<string, string>();

        try
        {
            #region 1.校验参数
            string phoneNumber = Request.Params["phonenumber"];
            string verificationCode = Request.Params["verificationcode"];
            string password = Request.Params["password"];
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

            //2.注册
            UserCore core = new UserCore();
            core.Register(phoneNumber, verificationCode, password);

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