<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="App_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>知聚网--专利转让,专利交易,专利运营，高校专利成果转化交易平台</title>
    <meta name="keywords" content="专利交易,专利转让,专利运营,专利查询,购买专利,专利出售,专利求购,专利交易网,专利转让网,商标转让网,中国专利转让网" />
    <meta name="description" content="知聚网是一站式知识产权交易服务,专利转让,专利交易,专利运营，高校专利成果转化交易平台,提供专利出售转让服务,专注专利买卖,专利变更,专利转让手续办理,专利技术推广,专利价值评估,具有优秀专业顾问团队为企业个人提供专利技术交易高端服务。" />
    <link rel="shortcut icon" href="../img/favicon.ico" />
    <meta name="renderer" content="webkit" />
    <meta name="force-rendering" content="webkit" />

    <link rel="stylesheet" type="text/css" href="css/fonts/fontawesome/css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="css/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="app/css/base.css" />

    <script type="text/javascript" src="../js/jquery/jquery-1.12.3.js"></script>
    <script type="text/javascript" src="../js/app/app.bootstrap.tabs.js"></script>
    <script type="text/javascript" src="../js/app.js"></script>
    <script type="text/javascript" src="app/js/base.js"></script>
    <script type="text/javascript" src="http://res.wx.qq.com/connect/zh_CN/htmledition/js/wxLogin.js"></script>
    <script type="text/javascript">
        $(function () {
            var redirect_uri = encodeURI(getRootPath() + "/WeChat/CallBack.aspx");
            var state =<%=GetState() %>;

            //将微信登录二维码内嵌到自己页面中，用户使用微信扫码授权后通过JS将code返回给网站
            var obj = new WxLogin({
                self_redirect: false,
                id: "qrcode",
                appid: "",//应用唯一标识
                scope: "snsapi_login,snsapi_userinfo",//应用授权作用域，拥有多个作用域用逗号（,）分隔，网页应用目前仅填写snsapi_login即
                redirect_uri: redirect_uri,
                state: state,
                style: "",
                href: ""
            });
        })
    </script>
</head>
<body>
    <div class="loginbox">
        <div class="loginheader box">
            <a class="fl logobox" href="Portal">
                <img src="../img/logo-z-w.png" />
            </a>
            <ul class="fr headernav">
                <li><a href="login">登录</a></li>
                <li><a href="register">注册</a></li>
            </ul>
        </div>
        <img class="loginimg" src="../img/loginimg02.png" />
        <div class="l-box login" id="qrcode">
            <%--<h3>登录</h3>
            <ul>
                <li>
                    <label><i class="fa fa-user"></i></label>
                    <input type="text" name="usercode" placeholder="请输入……" />
                </li>
                <li>
                    <label><i class="fa fa-lock"></i></label>
                    <input type="password" name="password" placeholder="请输入……" />
                </li>
                <li>
                    <a class="loginbtn btn-yellow" role="login" href="javascript:void(0);">登录</a>
                </li>
                <li>
                    <input type="checkbox" id="remember" class="check" style="margin: 0 6px 0 0;" />
                    记住账号
                </li>
                <li>
                    <a class="fl" href="retrieve">找回密码</a>
                    <a class="fr color-red" href="register">免费注册&gt;</a>
                </li>
            </ul>--%>
        </div>

        <div class="login-footer">
            <span>Copyright© 知聚网交易平台</span>
            <span><%=CP.Common.sysConst.Portal_IPC_BeiAnHao %></span>
            <span>技术支持：<a class="color-yellow" href="http://www.guoziw.cn" target="_blank">青岛果子科技服务平台</a></span>
        </div>
    </div>

    <script type="text/javascript" src="app/js/common.js?version=<%=CP.Common.sysConst.Sys_Version_Code %>"></script>
    <script>
        var checkstate = "<%= CP.Entiry.Web.WebUser.CheckState%>";

        $(function () {
            var keydown = function (e) {
                if (e.keyCode == 13) {
                    window.regandlogin.userlogin();
                }
            }
            $("input[name='usercode']").keydown(keydown);
            $("input[name='password']").keydown(keydown);

            //点击登录
            //$("a[role='login']").on("click", function () {
            //    window.regandlogin.userlogin();
            //});

            //处理记住密码
            //var username = getCookie("faccount");
            //if (username) {
            //    $("input[name='usercode']").val(username);
            //    $("#remember").attr("checked", "checked");
            //}
        })
    </script>
</body>
</html>
