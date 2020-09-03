using CP.Common;
using CP.Core.Web;
using Newtonsoft.Json.Linq;
using System;

public partial class App_WeChat_CallBack : SuperPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WeChatRedirect();
    }

    #region 微信重定向 崔萌 2020-4-29 15:45:32
    /// <remarks>崔萌 2020-4-29 15:45:32</remarks>
    /// <summary>
    /// 微信重定向
    /// </summary>
    private void WeChatRedirect()
    {
        try
        {
            //1.获取参数
            //用户授权后，将会重定向到redirect_uri的网址上，并且带上code和state参数
            //若用户禁止授权后，则重定向后不会带上code参数，金辉带上state的参数
            string code = Request.Params["code"] == null ? string.Empty : Request.Params["code"];
            string state = Request.Params["state"];

            string sessionState = Common.Session["WeChatState"].ToString();
            if (sessionState.Equals(state) && !string.IsNullOrEmpty(code))
            {
                //2.通过code获取用户信息
                GetUserInfoByCode(code);
            }
            else
            {
                //针对csrf攻击（跨站请求伪造攻击）的处理
                Log4.Log4Info(string.Format(@"微信扫码登录后重定向信息：code:{0},state:{1},
                            sessionstate:{2}", code, state, sessionState));
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    #endregion

    #region 通过code获取用户信息 崔萌 2020-4-29 15:51:22
    /// <remarks>崔萌 2020-4-29 15:51:22</remarks>
    /// <summary>
    /// 通过code获取用户信息
    /// </summary>
    /// <param name="code">微信授权临时票据（code）</param>
    private void GetUserInfoByCode(string code)
    {
        //1.通过code获取access_token
        string json = GetAccessTokenByCode(code);

        if (!json.Contains("errcode"))
        {
            JObject jo = (JObject)JToken.Parse(json);
            string access = jo["access_token"].ToString();//获取接口调用凭证
            string openId = jo["openid"].ToString();//授权用户唯一标识

            //2.拿着openId去用户表关联用户
        }
        else
        {
            Log4.Log4Error(string.Format("通过code获取access_token异常，异常信息：{0}", json));
            throw new UIException("扫码登录异常,请稍后再试。");
        }
    } 
    #endregion

    #region 通过code获取access_token 崔萌 2020-4-30 09:26:01
    /// <remarks>崔萌 2020-4-30 09:26:01</remarks>
    /// <summary>
    /// 通过code获取access_token
    /// </summary>
    /// <param name="code">微信授权临时票据（code）</param>
    /// <returns>access_token信息</returns>
    private static string GetAccessTokenByCode(string code)
    {
        //1.组装获取access_token的链接
        string getTokenUrl = string.Format(@"https://api.weixin.qq.com/sns/oauth2/access_token?
                            appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                            sysConst.AppId, sysConst.AppSecret, code);

        //2.获取网页授权凭证
        return HttpHepler.GetRequest(getTokenUrl);
    }
    #endregion
}