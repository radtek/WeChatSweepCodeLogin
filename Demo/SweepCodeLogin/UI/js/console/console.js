/*
    [>> ] 命令开始符号
    [ <<] 命令结束符号
    [error: ] 错误提示内容
*/

$(document).ready(function () {
    //设置AJAX发送请求时,不显示加载框
    _isloading = false;

    $('.console').value = ">> ";

    //$('.console').textareafullscreen(); 
});

var isRun = false;

function keyup(obj) {
    try {
        if (!isRun) {
            //回车键
            if (event.keyCode == 13) {

                var str = obj.value; //获取文本域内容  
                var arr = str.split("  ");

                var command = arr[arr.length - 1].replace("\r", "").replace("\n", "");

                if (checkcommand(command, obj)) {
                    isRun = true;

                    sendcommand(command, obj);  // isRun 重置已更新到 sendcommand() 函数中
                }
                obj.value += "  ";
            }
                // 退格键
            else if (event.keyCode == 8) {
                var str = obj.value; //获取文本域内容  

                var astr = replaceEnter(str, "\n", "^");
                var bstr = replaceEnter(str, "\r", "^");
                if (astr.substr(-1) == "^" || bstr.substr(-1) == "^") {
                    event.returnValue = false;
                }

                var index = obj.value.lastIndexOf(" <<") + 1;

                if (obj.selectionStart < index) {
                    event.returnValue = false;
                }
            }
                //光标左移
            else if (event.keyCode == 37) {
                var str = obj.value; //获取文本域内容  

                var astr = replaceEnter(str, "\n", "^");
                var bstr = replaceEnter(str, "\r", "^");
                if (astr.substr(-1) == "^" || bstr.substr(-1) == "^") {
                    event.returnValue = false;
                }
            }
                //光标上移
            else if (event.keyCode == 38) {

                event.returnValue = false;
            }
                //光标下移
            else if (event.keyCode == 40) {

                event.returnValue = false;
            }
                //delete键
            else if (event.keyCode == 46) {

                event.returnValue = false;
            }
        }
        else {
            //如果正在执行命令 则不允许操作
            event.returnValue = false;
        }
    } catch (e) {
        isRun = false;
        alert("平台异常: " + e.value + "！");
    }
}

function checkcommand(_command, obj) {

    if (_command.substring(0, 2).toString() != ">>") {
        obj.value += "  \r\n<< error: 请输入命令开始符。" + "\t" + getDateTime();
        return false;
    }
    else if (_command.substring(2, 3).toString() != " ") {
        obj.value += "  \r\n<< error: 开始符与命令之间请使用空格分隔。" + "\t" + getDateTime();
        return false;
    }

    return true;
}

function sendcommand(_command, obj) {
    try {
        $("#table").text("");
        call_ajax({
            url: location.href,
            data: {
                action: "command",
                command: _command.replace(">> ", "")  //去掉命令开始符
            },
            async: true,
            success: function (data) {

                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == 0) {
                        obj.value += "<< " + aaData.Msg + "\t" + getDateTime() + "  \r\n";
                        
                        if (aaData.Data != "") {
                            var Data = aaData.Data.rows;
                            var html = "";
                            html += "<tr>";
                            for (var item in Data[0]) {
                                html += "<td style='font-weight:bold'>" + item + "</td>";
                            }
                            html += "</tr>";

                            for (var i = 0; i < Data.length; i++) {
                                html += "<tr>";
                                for (var item in Data[i]) {
                                    html += "<td>" + Data[i][item] + "</td>";
                                }
                                html += "</tr>";
                            }

                            $("#table").append(html);
                            showdetial();
                        }
                    }
                    else {
                        obj.value += "<< " + aaData.errMsg + "\t" + getDateTime() + "  \r\n";
                    }
                }
                isRun = false;
            },
            error: function (xhr, status, thrown) {
                isRun = false;
            }
        });
    } catch (e) {
        isRun = false;
        alert("平台异常: " + e.value + "！");
    }
}

//获取系统当前时间，返回YYYY/MM/DD 上午(下午)hh:mm:ss
function getDateTime() {
    var localTime = new Date();
    return localTime.toLocaleString();
}

function replaceEnter(str, reallyDo, replaceWith) {
    var e = new RegExp(reallyDo, "g");
    words = str.replace(e, replaceWith);
    return words;
}


function showdetial() {
    jQuery('#modal-6').modal('show', { backdrop: 'static' })
}