$(function () {
    /**************************************注册、登录 begin**********************************************/
    if (!window.regandlogin) {
        window.regandlogin = {};
    }

    //登录 崔萌 2019-12-17 14:36:29
    window.regandlogin.userlogin = function () {
        if ($("input[name='usercode']").val() === "") {
            alert("请填写登陆账号。");
            return;
        }
        if ($("input[name='password']").val() === "") {
            alert("请填写账号密码。");
            return;
        }

        call_ajax({
            url: location.href,
            data: {
                action: "m_login",
                usercode: $("input[name='usercode']").val(),
                password: $("input[name='password']").val()
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result === "0") {
                        location = "Portal";
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    /**************************************注册、登录 end**********************************************/

});