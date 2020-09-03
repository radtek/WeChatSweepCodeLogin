<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Retrieve.aspx.cs" Inherits="App_Retrieve" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>知聚网--专利转让,专利交易,专利运营，高校专利成果转化交易平台</title>
    <meta name="keywords" content="专利交易,专利转让,专利运营,专利查询,购买专利,专利出售,专利求购,专利交易网,专利转让网,商标转让网,中国专利转让网" />
    <meta name="description" content="知聚网是一站式知识产权交易服务,专利转让,专利交易,专利运营，高校专利成果转化交易平台,提供专利出售转让服务,专注专利买卖,专利变更,专利转让手续办理,专利技术推广,专利价值评估,具有优秀专业顾问团队为企业个人提供专利技术交易高端服务。" />
    <link rel="shortcut icon" href="../img/favicon.ico" />
    <meta name="renderer" content="webkit" />
    <meta name="force-rendering" content="webkit" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1" />
    <meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1" />

    <link rel="stylesheet" type="text/css" href="../css/fonts/fontawesome/css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="../css/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="app/css/base.css" />

    <script type="text/javascript" src="../js/jquery/jquery-1.12.3.js"></script>
    <script type="text/javascript" src="../js/app.js"></script>
    <script type="text/javascript" src="app/js/base.js"></script>
</head>
<body>
    <div class="loginbox">
        <div class="loginheader box">
            <a class="fl logobox" href="Portal">
                <img src="../img/logo-z-w.png" /></a>
            <ul class="fr headernav">
                <li><a href="login">登录</a></li>
                <li><a href="signup.html">注册</a></li>
            </ul>
        </div>
        <img class="loginimg" src="../img/loginimg02.png" />
        <div class="l-box signup">
            <h3>找回密码</h3>
            <ul>
                <li>
                    <label><i class="fa fa-mobile-phone"></i></label>
                    <input type="text" name="MobilePhone" maxlength="15" placeholder="请输入手机号" />
                </li>
                <li>
                    <label><i class="fa fa-shield"></i></label>
                    <input class="code fl" type="text" name="verificationcode" placeholder="请输入验证码" />
                    <a class="codebtn btn-blue fl" role="verificationcode" href="javascript:void(0);">获取验证码</a>
                </li>
                <li>
                    <label><i class="fa fa-lock"></i></label>
                    <input type="password" name="NewPassWord" placeholder="请输入新密码" />
                </li>
                <li>
                    <label><i class="fa fa-lock"></i></label>
                    <input type="password" name="AgainInput" placeholder="请再次输入新密码" />
                </li>

                <li>
                    <a class="loginbtn btn-yellow" role="confirm" href="javascript:void(0);">确认</a>
                </li>
                <li>
                    <a class="fr color-red" href="login">返回登录&gt;</a>
                </li>

            </ul>
        </div>

        <div class="login-footer">
            <span>Copyright© 知聚网交易平台</span>
            <span><%=CP.Common.sysConst.Portal_IPC_BeiAnHao %></span>
            <span>技术支持：<a class="color-yellow" href="http://www.guoziw.cn" target="_blank">青岛果子科技服务平台</a></span>
        </div>
    </div>

    <script type="text/javascript" src="app/js/common.js?version=<%=CP.Common.sysConst.Sys_Version_Code %>"></script>
    <script type="text/javascript">
        $(function () {
            var keydown = function (e) {
                if (e.keyCode == 13) {
                    window.regandlogin.retrievepassword();
                }
            }
            $("input[name='MobilePhone']").keydown(keydown);

            //点击获取验证码
            $("a[role='verificationcode']").on("click", function () {
                window.regandlogin.getverificationcode();
            });

            //点击确认
            $("a[role='confirm']").on("click", function () {
                window.regandlogin.retrievepassword();
            });
        })

        var countdown = 60;
        //倒计时
        function SetTime(obj) {
            if (countdown == 0) {
                $("a[role='verificationcode']").attr("disabled", false).css("pointer-events", "auto");
                $("a[role='verificationcode']").removeClass("btn-gray");
                $("a[role='verificationcode']").addClass("btn-blue");
                $("a[role='verificationcode']").text("获取验证码");
                countdown = 60;
                return;
            } else {
                $("a[role='verificationcode']").text("重新获取(" + countdown + ")");
                countdown--;
            }

            setTimeout(function () {
                SetTime(obj)
            }, 1000);
        }
    </script>
</body>
</html>
