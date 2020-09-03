var __bodyheight;
var __isloading = true;

$(function () {
    resetframe();
});

window.onload = function () {
    if (top != this) { //判断页面是否在iframe中
        if (__bodyheight != $("body").height())
            resetframe();
    }
}

function call_ajax(option) {

    if (option.isshowloading == undefined)
        option.isshowloading = true;

    if (__isloading && option.isshowloading)
        showloading("open");

    option = $.extend({}, option);
    $.ajax({
        url: option.url ? option.url : location.href,
        type: (option.method == 'get' ? 'get' : 'post'),
        data: option.data,
        check_session: option.check_session,
        async: option.async == false ? false : true,
        beforeSend: function (xhr, settings) {
            if (typeof (option.beforeSend) == 'function')
                option.beforeSend(xhr, settings);
        },
        success: function (r_data, status, xhr, settings) {
            if (typeof (option.success) == 'function') {
                var aaData = strToJson(r_data);
                if (aaData.sysError) { // 如果 sysError 有值 则为框架提供异常信息
                    if (typeof option.syserror === "function") {
                        option.syserror(aaData);
                    } else {
                        if (aaData.Msg)
                            alert(aaData.Msg);
                        if (aaData.Skip) // 服务器基类返回值 用于校验 校验服务状态 用户失效
                            top.document.location.href = aaData.Skip;
                    }
                    return;
                } else {


                    option.success(r_data, status, xhr);
                }
            }
            //重置显示高度
            resetframe();

            if (__isloading && option.isshowloading)
                showloading("close");
        },
        error: function (xhr, status, thrown) {

            if (typeof (option.error) == 'function')
                option.error(xhr, status, thrown);

            if (__isloading && option.isshowloading)
                showloading("close");
        }
    });
}

//需要 ~/js/UI/jquery.form.js 支持
function call_form(option) {

    if (option.isshowloading == undefined)
        option.isshowloading = true;

    if (__isloading && option.isshowloading)
        showloading("open");

    var _option = {
        forceSync: option.forceSync ? option.forceSync : true,    //强制同步
        async: option.async ? option.async : false,
        data: option.data,
        success: function (data) {
            try {
                var aaData = strToJson(data);
                if (aaData.sysError) { // 如果 sysError 有值 则为框架提供异常信息
                    if (aaData.Msg)
                        alert(aaData.Msg);
                    if (aaData.Skip) // 服务器基类返回值 用于校验 校验服务状态 用户失效
                        top.document.location.href = aaData.Skip;
                    return;

                }
            } catch (e) {
                //隐藏此异常
            }

            if (__isloading && option.isshowloading)
                showloading("close");

            option.success(data);
        }
    };

    $("#" + option.formid).ajaxSubmit(_option);
}

//重置显示高度
function resetframe() {
    if (top != this) { //判断页面是否在iframe中
        if (typeof (parent.setframeheight) == 'function') {
            __bodyheight = $("body").height();

            parent.setframeheight(__bodyheight);
        }
    }
}

function strToJson(obj) {
    try {
        if (typeof (obj) == "string") {
            var json = eval('(' + obj + ')');
            return json;
        }
        return obj;
    } catch (e) {
        return [];
    }
}

/// 加载等待遮盖层
/// action      执行动作 取值范围 open close
/// identity    唯一标识字段
function showloading(action) {
    if ($("div[class='loadingbox']").length <= 0) {
        var html = '';
        html += '<div class="loadingbox">';
        html += '    <div class="loadingback"></div>';
        html += '    <section>';
        html += '        <div class="sk-double-bounce">';
        html += '            <div class="sk-child sk-double-bounce-1"></div>';
        html += '            <div class="sk-child sk-double-bounce-2"></div>';
        html += '        </div>';
        html += '    </section>';
        html += '</div>';
        $("body").append(html);
    }

    if (action === 'close') {
        $(".loadingbox").css({ "display": "none" });
    }
    else {
        $(".loadingbox").css({ "display": "block" });
    }

}

/// ****************************
/// 获取当前网页的url根目录  
/// http://location:8080/Pages/Login.aspx => http://location:8080 无斜线
function getRootPath() {
    var pathName = window.location.pathname.substring(1);
    var webName = pathName == '' ? '' : pathName.substring(0, pathName.indexOf('/'));
    //return window.location.protocol + '//' + window.location.host + '/'+ webName + '/';
    var firstRoot = "APP,MANAGER";
    if (firstRoot.indexOf(webName.toLocaleUpperCase()) >= 0)
        webName = "";
    else
        webName = "/" + webName;
    return window.location.protocol + "//" + window.location.host + webName;
}
//获取form表单数据  返回string 类型json数据
(function ($) {
    $.fn.serializeJson = function () {
        var serializeObj = {};
        var array = this.serializeArray();
        var str = this.serialize();
        $(array).each(function () {
            if (serializeObj[this.name]) {
                if ($.isArray(serializeObj[this.name])) {
                    serializeObj[this.name].push(this.value);
                } else {
                    serializeObj[this.name] = [serializeObj[this.name], this.value];
                }
            } else {
                serializeObj[this.name] = this.value;
            }
        });
        return serializeObj;
    };
})(jQuery);



//获取地址栏参数
function GetQueryString(name) {
    if (!isNaN(name)) {
        var url = window.location.href;
        var ua = url.split('/');
        var pa = ua[ua.length - 1].split('-');
        var r = pa[name]
        if (r != null && typeof (r) != "undefined") return r; return null;
    } else {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }
}

//获取地址栏中文参数，拼接URL实例：A.aspx?id=' + id + "&name=" + encodeURI(name)
function GetQueryChineseString(name) {
    if (!isNaN(name)) {
        var url = window.location.href;
        var ua = url.split('/');
        var pa = ua[ua.length - 1].split('-');
        var r = pa[name]
        if (r != null && typeof (r) != "undefined") return r; return null;
    } else {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) {
            return decodeURI(r[2]);//解决中文乱码
        }
    }
}

//获取选中的CheckBox  的Value
function GetCheckedData(type) {
    var checkBox = document.getElementsByName("btSelectItem");
    var Count = 0;
    var ID = "";
    for (var i = 0; i < checkBox.length; i++) {
        if (checkBox[i].checked == true) {
            Count++;
            ID += checkBox[i].value + ",";
        }
    }
    if (Count <= 0) {
        return "-2";//没选
    }
    if (type == "update") {
        if (Count > 1) {
            return "-1";//选多了
        }
        else {
            ID = ID.substring(0, ID.length - 1);
            return ID;
        }
    }
    else {
        ID = ID.substring(0, ID.length - 1);
        return ID;
    }
}

//写cookies
function setCookie(name, value) {
    var Days = 30;
    var exp = new Date();
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
}


//读取Cookie
function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return unescape(arr[2]);
    else
        return null;
}

//删除Cookie
function delCookie(name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(name);
    if (cval != null)
        document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
}

///------金额转大写----
var digitUppercase = function (n) {
    var unit = "仟佰拾亿仟佰拾万仟佰拾元角分", str = "";
    n += "00";
    var p = n.indexOf('.');
    if (p >= 0)
        n = n.substring(0, p) + n.substr(p + 1, 2);
    unit = unit.substr(unit.length - n.length);
    for (var i = 0; i < n.length; i++)
        str += '零壹贰叁肆伍陆柒捌玖'.charAt(n.charAt(i)) + unit.charAt(i);
    return str.replace(/零(仟|佰|拾|角)/g, "零").replace(/(零)+/g, "零").replace(/零(万|亿|元)/g, "$1").replace(/(亿)万|壹(拾)/g, "$1$2").replace(/^元零?|零分/g, "").replace(/元$/g, "元整");
    //var fraction = ['角', '分'];
    //var digit = [
    //    '零', '壹', '贰', '叁', '肆',
    //    '伍', '陆', '柒', '捌', '玖'
    //];
    //var unit = [
    //    ['元', '万', '亿'],
    //    ['', '拾', '佰', '仟']
    //];
    //var head = n < 0 ? '欠' : '';
    //n = Math.abs(n);
    //var s = '';
    //for (var i = 0; i < fraction.length; i++) {
    //    s += (digit[Math.floor(n * 10 * Math.pow(10, i)) % 10] + fraction[i]).replace(/零./, '');
    //}
    ////s = s || '整';
    //n = Math.floor(n);
    //for (var i = 0; i < unit[0].length && n > 0; i++) {
    //    var p = '';
    //    for (var j = 0; j < unit[1].length && n > 0; j++) {
    //        p = digit[n % 10] + unit[1][j] + p;
    //        n = Math.floor(n / 10);
    //    }
    //    s = p.replace(/(零.)*零$/, '').replace(/^$/, '零') + unit[0][i] + s;
    //}
    //return head + s.replace(/(零.)*零元/, '元')
    //    .replace(/(零.)+/g, '零')
    //    .replace(/^整$/, '零元');
};

///------金额转大写----
var digitUppercaseGZ = function (n) {
    var fraction = ['角', '分'];
    var digit = [
        '零', '壹', '贰', '叁', '肆',
        '伍', '陆', '柒', '捌', '玖'
    ];
    var unit = [
        ['元', '万', '亿'],
        ['', '拾', '佰', '仟']
    ];
    var head = n < 0 ? '欠' : '';
    n = Math.abs(n);
    var s = '';
    for (var i = 0; i < fraction.length; i++) {
        s += (digit[Math.floor(n * 10 * Math.pow(10, i)) % 10] + fraction[i]).replace(/零./, '');
    }
    //s = s || '整';
    n = Math.floor(n);
    for (var i = 0; i < unit[0].length && n > 0; i++) {
        var p = '';
        for (var j = 0; j < unit[1].length && n > 0; j++) {
            p = digit[n % 10] + unit[1][j] + p;
            n = Math.floor(n / 10);
        }
        s = p.replace(/(零.)*零$/, '').replace(/^$/, '零') + unit[0][i] + s;
    }
    return head + s.replace(/(零.)*零元/, '元')
        .replace(/(零.)+/g, '零')
        .replace(/^整$/, '零元');
};

//-------------------


///校验身份证号
function CheckIDNumber(idnum) {
    var filter = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
    if (filter.test(idnum)) return true;
    else {
        return false;
    }
}
//手机号校验 支持电信199、移动198、联通166号段校验
function CheckMobile(phone) {
    var filter = /^(13[0-9]|14[5-9]|15[012356789]|166|17[0-8]|18[0-9]|19[8-9])[0-9]{8}$/;
    if (filter.test(phone)) return true;
    else {
        return false;
    }
}
function CheckMail(mail) {
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (filter.test(mail)) return true;
    else {
        return false;
    }
}
//校验邮编
function CheckZipCode(zipcode) {
    var filter = /[1-9]\d{5}(?!\d)/;
    if (filter.test(zipcode)) return true;
    else {
        return false;
    }
}
//校验数字
function CheckNumber(num) {
    var filter = /^[0-9]+.?[0-9]*$/;
    if (filter.test(num)) return true;
    else {
        return false;
    }
}

function CheckTel(val) {
    var filter = /^(\(\d{3,4}\)|\d{3,4}-|\s)?\d{7,14}$/;
    if (filter.test(val)) {

        return true;
    } else
        return false;
}
//校验QQ
function CheckQQ(val) {
    var filter = /^[1-9][0-9]{4,14}$/;
    if (filter.test(val)) {
        return true;
    } else {
        return false;
    }
}


/*************校验申请号 begin**************/
function ValidateFormat() {
    this.valiShenqh = /^(((19[5-9]\d)|(20[0-4]\d))[123589](\d{7})[0-9xX])$|^((([5-9]\d)|(0[0-3]))(\d{6})[0-9xX])$/;
}
var ValidateFormat = new ValidateFormat();
function CheckSQH(shenqh) {
    var flag = true;
    if (shenqh == null || shenqh == "") {
        return false;
    }
    else {
        shenqh = shenqh.replace('.', '').trim();
        if (ValidateFormat.valiShenqh.test(shenqh)) {
            if (shenqh.length == 13) flag = _13shenqhCheckFlag(shenqh);
            else if (shenqh.length == 9) flag = _9shenqhCheckFlag(shenqh);
            else flag = false;
        } else {
            flag = false;
        }
    } return flag;
}
function _13shenqhCheckFlag(shenqh) {
    var sqh = shenqh.substring(0, 12);
    var vd1 = shenqh.substring(shenqh.length - 1);
    var vd2 = 0;
    for (var i = 0; i < sqh.length; i++) {
        var sindex = 1 + i; var snum = sqh.charAt(i);
        if (null == snum || "" == snum) {
            return false;
        } else {
            if (sindex < 9) {
                vd2 = vd2 + snum * (sindex + 1);
            } else {
                vd2 = vd2 + snum * (sindex - 7);
            }
        }
    }
    vd2 = vd2 % 11; vd2 = (vd2 == 10) ? 'X' : vd2;
    if (vd1 == vd2 || (vd2 == 'X' && vd1 == 'x')) {
        return true;
    } else {
        return false;
    }
}
function _9shenqhCheckFlag(shenqh) {
    var sum = 0; var sqharr = shenqh.split('');
    for (var i = 0; i < sqharr.length - 1; i++) {
        sum = sum + parseInt(sqharr[i]) * (i + 2);
    }
    var vd1 = shenqh.substring(shenqh.length - 1);
    var vd2 = sum % 11; vd2 = (vd2 == 10) ? 'X' : vd2;
    if (vd1 == vd2 || (vd2 == 'X' && vd1 == 'x'))
        return true;
    else {
        return false;
    }
}

/*************校验申请号 end**************/


//input框只能输入数字和一位小数点和小数点后面两位小数
function clearnonum(obj) {
    obj.value = obj.value.replace(/[^\d.]/g, "");  //清除“数字”和“.”以外的字符  
    obj.value = obj.value.replace(/\.{2,}/g, "."); //只保留第一个. 清除多余的  
    obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
    obj.value = obj.value.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3');//只能输入两个小数  
    if (obj.value.indexOf(".") < 0 && obj.value != "") {//以上已经过滤，此处控制的是如果没有小数点，首位不能为类似于 01、02的金额 
        obj.value = parseFloat(obj.value);
    }
}
//input框只能输入大于0的正整数
function onlynum(obj) {
    if (obj.value.length == 1) {
        obj.value = obj.value.replace(/[^1-9]/g, '');
    } else {
        if (obj.value.replace(/\D/g, '')) {
            obj.value = parseInt(obj.value.replace(/\D/g, ''), 10);
        } else {
            obj.value = obj.value.replace(/\D/g, '');
        }
    }
}
//input框输入大于等于0的正整数
function checknum(obj) {
    if (obj.value.length == 1) {
        obj.value = parseInt(obj.value.replace(/[^0-9]/g, ''), 10);
    } else {
        if (obj.value.replace(/\D/g, '')) {
            obj.value = parseInt(obj.value.replace(/\D/g, ''), 10);
        } else {
            obj.value = obj.value.replace(/\D/g, '');
        }
    }
}
//input框只能输入大于0的4位正整数
function onlyfournum(obj) {
    if (obj.value.length > 4) {
        obj.value = obj.value.substr(0, 4)
    }
    obj.value = obj.value.replace(/[^\d]/g, '');
}

//上传控件图片预览
function setImagePreview(File, ImageID, ImageDivID) {
    var docObj = File;//文件上传控件的ID
    if (docObj.value.toString().indexOf(".jpg") > 0 || docObj.value.toString().indexOf(".png") > 0 || docObj.value.toString().indexOf(".bmp") > 0) {

    }
    else {
        alert("请上传正确的图片文件");
        return;
    }
    var imgObjPreview = document.getElementById(ImageID);//显示IMG的ID

    if (docObj.files && docObj.files[0]) {
        //imgObjPreview.style.display = 'block';
        imgObjPreview.style.width = '150px';
        imgObjPreview.style.height = '150px';
        imgObjPreview.src = window.URL.createObjectURL(docObj.files[0]);
    }
    else {
        docObj.select();
        var imgSrc = document.selection.createRange().text;
        var localImagId = document.getElementById(ImageDivID);//显示Image外层DIV的ID
        localImagId.style.width = "150px";
        localImagId.style.height = "150px";
        try {
            localImagId.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale)";
            localImagId.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgSrc;
        }
        catch (e) {
            alert("出错了！");
            return false;
        }
        imgObjPreview.style.display = 'none';
        document.selection.empty();
    }
    return true;
}

//上传控件图片预览
function setImagePreview1(File, ImageID, ImageDivID, width, height) {
    var docObj = File;//文件上传控件的ID
    if (docObj.value.toString().indexOf(".jpg") > 0 || docObj.value.toString().indexOf(".png") > 0 || docObj.value.toString().indexOf(".bmp") > 0) {

    }
    else {
        alert("请上传正确的图片文件");
        return;
    }

    if (!checkfilemaxsize(docObj, filemaxsize)) {
        alert("选择的文件太大了, 最大允许上传" + filemaxsize + "KB");
        return;
    }

    var imgObjPreview = document.getElementById(ImageID);//显示IMG的ID

    if (docObj.files && docObj.files[0]) {
        //imgObjPreview.style.display = 'block';
        imgObjPreview.style.width = width;
        imgObjPreview.style.height = height;
        imgObjPreview.src = window.URL.createObjectURL(docObj.files[0]);
    }
    else {
        docObj.select();
        var imgSrc = document.selection.createRange().text;
        var localImagId = document.getElementById(ImageDivID);//显示Image外层DIV的ID
        localImagId.style.width = width;
        localImagId.style.height = height;
        try {
            localImagId.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale)";
            localImagId.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgSrc;
        }
        catch (e) {
            alert("出错了！");
            return false;
        }
        imgObjPreview.style.display = 'none';
        document.selection.empty();
    }
    return true;
}


function downLoad(url) {
    //var iframe;
    //if ($("#_iframe_down_").length > 0)
    //    iframe = $("#_iframe_down_");
    //else
    //    iframe = $("<iframe id='_iframe_down_' style='display:none;'>");

    //iframe.attr("src", url.replace("~/", getRootPath() + "/"));
    //$("body").append(iframe);

    tab = window.open(url.replace("~/", getRootPath() + "/", "downTab"));
    //tab.document.execCommand("SaveAs")
    //document.all.downTab.removeNode(true)
}



function createDate(str) {
    try {
        str = str.replace(/-/g, '/');

        var date = new Date(str);
        return date;
    } catch (e) {
        return null;
    }
}

Date.prototype.Format = function (fmt) {

    var o = {
        "M+": this.getMonth() + 1, //月份           
        "d+": this.getDate(), //日           
        "h+": this.getHours() % 12 == 0 ? 12 : this.getHours() % 12, //小时           
        "H+": this.getHours(), //小时           
        "m+": this.getMinutes(), //分           
        "s+": this.getSeconds(), //秒           
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度           
        "S": this.getMilliseconds() //毫秒           
    };
    var week = {
        "0": "/u65e5",
        "1": "/u4e00",
        "2": "/u4e8c",
        "3": "/u4e09",
        "4": "/u56db",
        "5": "/u4e94",
        "6": "/u516d"
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    if (/(E+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? "/u661f/u671f" : "/u5468") : "") + week[this.getDay() + ""]);
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
}

/*
    * 函数说明: 序列化表格转换成JSON数据
    * 参数说明: 
    *           参数名称    类型          说明
    *           data        array         table所在表单form.serializeArray()
    *           colcount    int           一行内字段个数 
    * 返回值:
    *           类型          说明
    *           Json          数据序列化之后的Json数组        
*/
function serializeTable(data, colcount) {
    var arrcount = (data.length) / colcount;
    var json = [];
    for (var i = 0; i < arrcount; i++) {
        var item = {};
        for (var t = 0; t < colcount; t++) {
            item[data[i * colcount + t].name] = data[i * colcount + t].value;
        }
        json.push(item);
    }
    return json;
}

/*
    * 函数说明: 序列化table中带有role属性的tr标签中的input 的 name和value 转换成JSON数据
    * 参数说明: 
    *           参数名称    类型          说明
    *            id         string        table的id
    * 返回值:
    *           类型          说明
    *           Json          数据序列化之后的Json数组        
*/
function tableSerializeArray(id) {
    var json = [];
    $("#" + id + " tr").each(function (index, item) {
        if ($(item).attr("role")) {

            $(item).find("input").each(function (index, data) {
                json.push({ 'name': $(data).attr("name"), 'value': $(data).val() });
            });
            $(item).find("select").each(function (index, data) {
                json.push({ 'name': $(data).attr("name"), 'value': $(data).val() });
            });
        }
    });
    return json;
}

//html 特殊字符替换显示
function formathtml(str) {
    var ret = str;
    ret = ret.replace(/&apos/g, "'");
    ret = ret.replace(/&quot;/g, "\"");

    return ret;
}


//判断select 控件中是否存在某个值的option
function selectoptionexist(c, value) {
    var isExist = false;
    var count = c.find('option').length;
    for (var i = 0; i < count; i++) {
        if (c.get(0).options[i].value == value) {
            isExist = true;
            break;
        }
    }
    return isExist;
}


// *****************************************
// 设置自定义表单中控件是否可用 
// *****************************************
function setcontrolDisabled(_formID, _disabled) {
    var i_l = $("#" + _formID).find(":input");
    for (var i = 0; i < i_l.length; i++) {
        if (_disabled)
            $(i_l[i]).attr("disabled", _disabled);
        else
            $(i_l[i]).removeAttr("disabled");
    }
    i_l = $("#" + _formID).find("select");
    for (var i = 0; i < i_l.length; i++) {
        if (_disabled)
            $(i_l[i]).attr("disabled", _disabled);
        else
            $(i_l[i]).removeAttr("disabled");
    }
    i_l = $("#" + _formID).find("textarea");
    for (var i = 0; i < i_l.length; i++) {
        if (_disabled)
            $(i_l[i]).attr("disabled", _disabled);
        else
            $(i_l[i]).removeAttr("disabled");
    }
}

// *****************************************
// 设置自定义表单中控件为只读 
// *****************************************
function setcontrolReadonly(_formID, _readonly) {
    var i_l = $("#" + _formID).find(":input");
    for (var i = 0; i < i_l.length; i++) {
        if (_readonly)
            $(i_l[i]).attr("readonly", _readonly);
        else
            $(i_l[i]).removeAttr("readonly");
    }
    i_l = $("#" + _formID).find("select");
    for (var i = 0; i < i_l.length; i++) {
        if (_readonly)
            $(i_l[i]).attr("readonly", _readonly);
        else
            $(i_l[i]).removeAttr("readonly");
    }
    i_l = $("#" + _formID).find("textarea");
    for (var i = 0; i < i_l.length; i++) {
        if (_readonly)
            $(i_l[i]).attr("readonly", _readonly);
        else
            $(i_l[i]).removeAttr("readonly");
    }
}






/****************************************************交互式通用函数*************************************************************/

/*
    绑定省市区
    type // province,city,district 
    option.provincCtrlID
    option.cityCtrlID
    option.districtCtrlID 
    option.success // type, data 
*/
function loadorganization(type, option) {
    var provinc = $("#" + option.provincCtrlID);
    var city = $("#" + option.cityCtrlID);
    var district = $("#" + option.districtCtrlID);
    var route = option.route;

    switch (type) {
        case "province":
            $("#" + option.provincCtrlID + " option").remove();
            $("#" + option.cityCtrlID + " option").remove();
            $("#" + option.districtCtrlID + " option").remove();
            if (!provinc.val()) {
                //provinc.append("<option  value=''>---请选择---</option>");
                city.append("<option  value=''>---请选择---</option>");
                district.append("<option  value=''>---请选择---</option>");
                //return;
            }
            break;
        case "city":
            $("#" + option.cityCtrlID + " option").remove();
            $("#" + option.districtCtrlID + " option").remove();

            if (!provinc.val()) {
                city.append("<option  value=''>---请选择---</option>");
                district.append("<option  value=''>---请选择---</option>");
                return;
            }
            break;
        case "district":
            $("#" + option.districtCtrlID + " option").remove();
            if (!city.val()) {
                district.append("<option  value=''>---请选择---</option>");
                return;
            }
            break;
    }

    call_ajax({
        url: getRootPath() + "/handler/upload.ashx",
        data: {
            action: "p_bandorganization",
            type: type,
            provinc: provinc.val(),
            city: city.val(),
            district: district.val()
        },
        async: option.async == false ? false : true,
        success: function (data) {
            var aaData = strToJson(data);
            if (aaData) {
                if (aaData.Result === "0") {
                    var _ctrl, _text, _val;

                    _text = aaData.textField;
                    _val = aaData.valField;

                    switch (type) {
                        case "province":
                            _ctrl = $("#" + option.provincCtrlID);
                            break;
                        case "city":
                            _ctrl = $("#" + option.cityCtrlID);
                            break;
                        case "district":
                            _ctrl = $("#" + option.districtCtrlID);
                            break;
                    }

                    _ctrl.append("<option  value=''>-请选择-</option>");

                    //此处两个判断仅科技政策使用
                    if (type == "province" && route == 'news') {
                        _ctrl.append("<option  value='0'>全国</option>");
                    }
                    if (type == "city" && route == 'news') {
                        _ctrl.append("<option  value='0'>全省</option>");
                    }

                    for (var i = 0; i < aaData.Data.rows.length; i++) {
                        var html = "<option  value='" + aaData.Data.rows[i][_val] + "'>" + aaData.Data.rows[i][_text] + "</option>";

                        _ctrl.append(html);
                    }

                    if (typeof (option.success) == 'function')
                        option.success(type, aaData);
                }
                else {
                    alert(aaData.errMsg);
                }
            }
        }
    });
}

function islogin() {
    var result = false;
    call_ajax({
        data: {
            action: "P_IsLogin"
        },
        async: false,
        success: function (data) {
            var aaData = strToJson(data);
            if (aaData) {
                if (aaData.Result === "0") {
                    if (aaData.IsLogin == "true")
                        result = true;
                }
                else {
                    alert(aaData.errMsg);
                }
            }
        }
    });
    return result;
}

//加载国家下拉  
//control  控件对象
function load_country(control) {

    call_ajax({
        url: getRootPath() + "/Handler/upload.ashx",
        data: {
            action: "p_loadcountry"
        },
        async: false,
        success: function (data) {
            var aaData = strToJson(data);
            if (aaData) {
                var data = aaData.Data.rows;
                var html = "";
                for (var i = 0; i < data.length; i++) {
                    html += "<option value='" + data[i]["Text"] + "'>" + data[i]["Text"] + "</option>";
                }
                control.append(html);
            }
            else {
                alert(aaData.errMsg);
            }
        }
    })


}


function loaddictionaries(type) {
    var json = [];
    call_ajax({
        url: getRootPath() + "/Handler/Handler.ashx",
        data: {
            action: "p_loaddictionaries",
            type: type
        },
        async: false,
        success: function (data) {
            var aaData = strToJson(data);
            if (aaData) {
                json = aaData.Data.rows;
            }
            else {
                alert(aaData.errMsg);
            }
        }
    });
    return json;
}


function loadselectbydictionaries(c, type) {
    var json = loaddictionaries(type);

    var html = "";
    for (var i = 0; i < json.length; i++) {
        html += "<option value='" + json[i]["Text"] + "'>" + json[i]["Text"] + "</option>";
    }
    c.append(html);
}

//获取运行参数值
function runparameter(name) {

    var value = "";
    call_ajax({
        url: getRootPath() + "/Handler/Handler.ashx",
        data: {
            action: "p_loadrunparameter",
            name: name
        },
        async: false,
        success: function (data) {
            var aaData = strToJson(data);
            if (aaData) {
                value = aaData.Value;
            }
            else {
                alert(aaData.errMsg);
            }
        }
    })
    return value;
}

//计算天数差
//开始天数 datestart
//结束天数 datestop
function getoverdays(datestart, datestop) {

    var s = datestart.getTime();
    var t = datestop.getTime();

    var total = (t - s) / 1000;
    var day = parseInt(total / (24 * 60 * 60));//计算整数天数
    return day;
}


//专利交易--专利推广--区域匹配省份列表
function loadpapromotionprocince(type) {
    call_ajax({
        url: getRootPath() + "/Handler/Handler.ashx",
        data: {
            action: "p_loadpapromotionprocince",
            type: type
        },
        success: function (data) {
            var aaData = strToJson(data);

            if (aaData) {
                if (aaData.Result == "0") {
                    //区域匹配--省份信息
                    var data = aaData.Data.rows;

                    $("#gs").html("");
                    var html = "";
                    for (var i = 0; i < data.length; i++) {
                        html += "<li onclick='chooseprovince(" + i + ")'>" + data[i]["ProvinceName"] + "</li>";
                    }
                    $("#gs").append(html);

                }
                else {
                    alert(aaData.errMsg);
                }

            }
        }
    });
}


function printpage() {

    $(".layout-side").addClass("close");//确保打印时左侧菜单为关闭状态
    $(".layout-main").addClass("full-page");//确保打印时左侧菜单为关闭状态
    $(".layout-side-arrow").addClass("close");//确保打印时左侧菜单为关闭状态
    $(".layout-side-arrow-icon").addClass("close");//确保打印时左侧菜单为关闭状态

    $("header[role='notprint']").hide();//隐藏页面header，此部分不需要打印
    $("div[role='notprint']").hide();//隐藏页面div，此部分不需要打印
    window.print(); //调用浏览器的打印功能打印指定区域
    $(".ls-member").click();//将页面还原
    $("div[role='notprint']").show();//将页面还原
    $("header[role='notprint']").show();//将页面还原
}

//图片预览
function imagepreview(url) {

    if (url) {
        var win = window.open(url);
        setTimeout(function () { win.document.title = '图片预览'; }, 5);
    }
}

//图片预览
function imagepreview_obj(obj) {
    var url = $(obj).attr("src");
    if (url) {
        var win = window.open(url);
        setTimeout(function () { win.document.title = '图片预览'; }, 5);
    }
}

function checkfilemaxsize(obj, maxsize) {

    var isIE = /msie/i.test(navigator.userAgent) && !window.opera;

    var fileSize = 0;
    if (isIE && !obj.files) {
        var filePath = $(obj).val();
        var fileSystem = new ActiveXObject("Scripting.FileSystemObject");
        var file = fileSystem.GetFile(filePath);
        fileSize = file.Size;
    } else {
        fileSize = obj.files[0].size;
    }

    if (fileSize > parseFloat(maxsize) * 1024) {
        return false;
    }

    return true;
}


//页面控件数据校验 (通用)
//id form的id
function verifydata(id) {
    var ret = true;

    if (id) {
        id = "#" + id;
    } else {
        id = "";
    }

    $(id + " *[ismust='true']").each(function (index, element) {
        var desc = $(element).attr("desc");
        var row = $(element).attr("isrownum");

        if (typeof (desc) != "undefined") {
            if ($(element).val() == "" || $(element).val() == null || typeof ($(element).val()) == "undefined") {

                if (typeof (row) != "undefined") {
                    alert("第" + row + "行,请输入" + desc + "。");
                } else {
                    alert("请输入" + desc + "。");
                }
                ret = false;
                return false;
            }
        }
    });

    if (ret == false) return ret;

    $(id + " *[isnum='true']").each(function (index, element) {
        var desc = $(element).attr("desc");
        var row = $(element).attr("isrownum");

        if ($(element).val() != "") {
            if (isNaN($(element).val())) {
                if (typeof (row) != "undefined") {
                    alert("第" + row + "行," + desc + ",输入的值不是有效的数字格式。");
                } else {
                    alert("" + desc + ", 输入的值不是有效的数字格式。");
                }
                ret = false;
                return false;
            }
        }
    });

    if (ret == false) return ret;

    $(id + " *[isphone='true']").each(function (index, element) {
        var desc = $(element).attr("desc");
        var row = $(element).attr("isrownum");

        if ($(element).val() != "") {
            if (!CheckMobile($(element).val()) && !CheckTel($(element).val())) {

                if (typeof (row) != "undefined") {
                    alert("第" + row + "行," + desc + ",输入的值不是有效的电话格式。");
                } else {
                    alert("" + desc + ", 输入的值不是有效的电话格式。");
                }
                ret = false;
                return false;
            }
        }
    });

    if (ret == false) return ret;

    $(id + " *[ismobile='true']").each(function (index, element) {
        var desc = $(element).attr("desc");
        var row = $(element).attr("isrownum");

        if ($(element).val() != "") {
            if (!CheckMobile($(element).val())) {

                if (typeof (row) != "undefined") {
                    alert("第" + row + "行," + desc + ",输入的值不是有效的手机格式。");
                } else {
                    alert("" + desc + ", 输入的值不是有效的手机格式。");
                }
                ret = false;
                return false;
            }
        }
    });

    if (ret == false) return ret;

    $(id + " *[isemail='true']").each(function (index, element) {
        var desc = $(element).attr("desc");
        var row = $(element).attr("isrownum");

        if ($(element).val() != "") {
            if (!CheckMail($(element).val())) {
                if (typeof (row) != "undefined") {
                    alert("第" + row + "行," + desc + ",输入的值不是有效的E-mail格式。");
                } else {
                    alert("" + desc + ", 输入的值不是有效的E-mail格式。");
                }
                ret = false;
                return false;
            }
        }
    });

    if (ret == false) return ret;

    $(id + " *[issqh='true']").each(function (index, element) {
        var desc = $(element).attr("desc");
        var row = $(element).attr("isrownum");

        if ($(element).val() != "") {
            if (!CheckSQH($(element).val())) {
                if (typeof (row) != "undefined") {
                    alert("第" + row + "行," + desc + ",输入的值不是有效的申请号。");
                } else {
                    alert("" + desc + ", 输入的值不是有效的申请号。");
                }
                ret = false;
                return false;
            }
        }
    });
    if (ret == false) return ret;

    $(id + " *[ischeckspace='true']").each(function (index, element) {
        var desc = $(element).attr("desc");
        var row = $(element).attr("isrownum");

        if ($(element).val() != "") {
            if ($(element).val().indexOf(" ") >= 0) {
                if (typeof (row) != "undefined") {
                    alert("第" + row + "行," + desc + ",输入的值有空格。");
                } else {
                    alert("" + desc + ", 输入的值有空格。");
                }
                ret = false;
                return false;
            }
        }
    });
    if (ret == false) return ret;

    $(id + " *[maxlength]").each(function (index, element) {

        var desc = $(element).attr("desc");
        var maxlen = $(element).attr("maxlength");
        var row = $(element).attr("isrownum");

        if ($(element).val() && !isNaN(maxlen)) {
            var len = $(element).val().length;
            if (len > parseInt(maxlen)) {
                if (typeof (row) != "undefined") {
                    alert("第" + row + "行," + desc + "最多允许输入" + maxlen + "个字。");
                } else {
                    alert("" + desc + "最多允许输入" + maxlen + "个字。");
                }
                ret = false;
                return false;
            }
        }
    });

    return ret;
}


//下载上传的word、excel、zip附件（通用）
function downloadattachfile(path, fundingtype) {

    var form = $("<form>");
    form.attr('target', "_blank")
    form.attr('style', 'display:none');
    form.attr('method', 'post');
    form.attr('action', getRootPath() + "/app/member/funding/filecenter/download.aspx");

    var hiddnAction = $("<input type='hidden' name='action' value='M_Download'>");
    var hiddnType = $("<input type='hidden' name='fundingtype' value='" + fundingtype + "'>");
    var hiddnid = $("<input type='hidden' name='filepath' value='" + path + "'>");
    form.append(hiddnAction);
    form.append(hiddnType);
    form.append(hiddnid);

    $(document.body).append(form);

    form.submit();
}


function getpatype(pattype) {
    var patype = "";
    switch (pattype) {
        case "1":
            patype = "发明专利";
            break;
        case "2":
            patype = "实用新型";
            break;
        case "3":
            patype = "外观设计";
            break;
        case "8":
            patype = "PCT发明";
            break;
        case "9":
            patype = "PCT实用新型";
            break;
        default:

            break;
    }

    return patype;
}

//和系统时间比较
//true oldDate比系统时间大
//false oldDate比系统时间小
function conpareDate(oldDate) {
    var sysDate = new Date();//获取系统时间 
    var newDate = new Date(oldDate);//换成日期格式；

    if (newDate > sysDate) {
        return true;
    } else {
        return false;
    }
}

//跳转第三方连接
function jumpsanfangopen(type, name) {
    var url = "";
    switch (type) {
        case "qichacha":
            url = "https://www.qichacha.com/search?key=";
            break;
        case "tianyancha":
            url = "https://www.tianyancha.com/search?key=";
            break;
        case "qixinbao":
            url = "https://www.qixin.com/search?key=";
            break;
        case "baidu":
            url = "https://www.baidu.com/s?wd=";
            break;
        case "baiduxueshu":
            url = "http://xueshu.baidu.com/s?wd=";
            break;
        default:
    }
    window.open(url + name, "_blank");
}

//列表 搜索框 
function f1() {
    if (document.getElementById('searchbox-con').style.display == 'none') {
        document.getElementById('searchbox-con').style.display = 'block';
    } else {
        document.getElementById('searchbox-con').style.display = 'none';
    };
    if (document.getElementById('arrow').style.transform == 'rotate(180deg)') {
        document.getElementById('arrow').style.transform = 'rotate(0deg)';
    } else {
        document.getElementById('arrow').style.transform = 'rotate(180deg)';
    }
}
function f2() {
    if (document.getElementById('export').style.display == 'block') {
        document.getElementById('export').style.display = 'none';
    } else {
        document.getElementById('export').style.display = 'block';
    };
    if (document.getElementById('arrow').style.transform == 'rotate(180deg)') {
        document.getElementById('arrow').style.transform = 'rotate(0deg)';
    } else {
        document.getElementById('arrow').style.transform = 'rotate(180deg)';
    };
}

function s1() {
    if (document.getElementById('searchbox-con01').style.display == 'none') {
        document.getElementById('searchbox-con01').style.display = 'block';
    } else {
        document.getElementById('searchbox-con01').style.display = 'none';
    };
    if (document.getElementById('arrow01').style.transform == 'rotate(180deg)') {
        document.getElementById('arrow01').style.transform = 'rotate(0deg)';
    } else {
        document.getElementById('arrow01').style.transform = 'rotate(180deg)';
    };
}
function s2() {
    if (document.getElementById('searchbox-con02').style.display == 'none') {
        document.getElementById('searchbox-con02').style.display = 'block';
    } else {
        document.getElementById('searchbox-con02').style.display = 'none';
    };
    if (document.getElementById('arrow02').style.transform == 'rotate(180deg)') {
        document.getElementById('arrow02').style.transform = 'rotate(0deg)';
    } else {
        document.getElementById('arrow02').style.transform = 'rotate(180deg)';
    };
}
function s3() {
    if (document.getElementById('searchbox-con03').style.display == 'none') {
        document.getElementById('searchbox-con03').style.display = 'block';
    } else {
        document.getElementById('searchbox-con03').style.display = 'none';
    };
    if (document.getElementById('arrow03').style.transform == 'rotate(180deg)') {
        document.getElementById('arrow03').style.transform = 'rotate(0deg)';
    } else {
        document.getElementById('arrow03').style.transform = 'rotate(180deg)';
    };
}
function s4() {
    if (document.getElementById('searchbox-con04').style.display == 'none') {
        document.getElementById('searchbox-con04').style.display = 'block';
    } else {
        document.getElementById('searchbox-con04').style.display = 'none';
    };
    if (document.getElementById('arrow04').style.transform == 'rotate(180deg)') {
        document.getElementById('arrow04').style.transform = 'rotate(0deg)';
    } else {
        document.getElementById('arrow04').style.transform = 'rotate(180deg)';
    };
}
function s5() {
    if (document.getElementById('searchbox-con05').style.display == 'none') {
        document.getElementById('searchbox-con05').style.display = 'block';
    } else {
        document.getElementById('searchbox-con05').style.display = 'none';
    };
    if (document.getElementById('arrow05').style.transform == 'rotate(180deg)') {
        document.getElementById('arrow05').style.transform = 'rotate(0deg)';
    } else {
        document.getElementById('arrow05').style.transform = 'rotate(180deg)';
    };
}
function f2() {
    if (document.getElementById('export').style.display == 'block') {
        document.getElementById('export').style.display = 'none';
    } else {
        document.getElementById('export').style.display = 'block';
    };
    if (document.getElementById('arrow').style.transform == 'rotate(180deg)') {
        document.getElementById('arrow').style.transform = 'rotate(0deg)';
    } else {
        document.getElementById('arrow').style.transform = 'rotate(180deg)';
    };
}