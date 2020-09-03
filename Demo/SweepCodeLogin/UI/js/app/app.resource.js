/*
    * 文件说明: 模块资源 操作函数库
    * 作者:     Aviv
    * 更新时间: 2017-06-26     
    * 版本号:   1.0.0.2
	* 说明
		所需JS文件支持
			~/js/jQuery/jquery-1.12.3.js 
			~/js/app.js 
*/

if (typeof Resource !== 'object') {
	Resource = {};
}



Resource.loadResource = function (panel) {

	$panel = $(panel);

	call_ajax({
		url: location.href,
		data: {
			action: "P_LoadResource" 
		},
		success: function (data) {
			var aaData = strToJson(data);
			if (aaData) {
				if (aaData.Result === "0") { 
					$(aaData.Data.rows).each(function () {
					
						var r = this;
						var ctrl = "";
			 
						switch (r.Tag) {
						    case "a":
						        ctrl = Resource.getAScript(r);
						        break;
						    case "button":
						        ctrl = Resource.getButtonScript(r);
						        break;
						}
                        						
						if ($("span[role='span_btn_" + r.ButtonCode + "']").length > 0)
						    $("span[role='span_btn_" + r.ButtonCode + "']").append(ctrl);
						else {
						    $(panel).append(ctrl);

						}
					});
				}
				else {
					alert(aaData.errMsg);
				}
			}
		}
	});
}

Resource.getButtonScript = function (r) {
 
    var button = $("<button></button>");
    button.addClass("btn");
    if (r.ButtonColor)
        button.addClass("btn-" + r.ButtonColor);
    if (r.JsFunction)
    {
        var func = r.JsFunction.replace(/&apos;/g, "'").replace(/&quot;/g, "\"");
        button.attr("onclick", func);
    }
    if (r.Action)
        button.attr("action", r.Action);

    button.css("margin-left", "5px")

    var i = $("<i>");
    i.addClass("glyphicon");
    if (r.Icon)
        i.addClass(r.Icon);

    button.append(i);
    button.append(r.ButtonName);

    return button;
}

Resource.getAScript = function (r) {
    var a = $("<a href='javascript:void(0)' class='table-btn fl '></a>");
    if (r.ButtonColor)
        a.addClass(r.ButtonColor);


    if (r.JsFunction){
        var func = r.JsFunction.replace(/&apos;/g, "'").replace(/&quot;/g, "\""); 
        a.attr("onclick", func);
    }
    if (r.Action)
        a.attr("action", r.Action);
    a.html(r.ButtonName);

    return a;
}


Resource.submit = function (c, option) {
    $c = $(c);

    call_ajax({
        url: location.href,
        data: {
            action: $c.attr("action"),
            data: JSON.stringify(data)
        },
        success: function (data) {
            var aaData = strToJson(data);
            if (aaData) {
                if (aaData.Result === "0") { 
                    alert("操作成功");
                    if (typeof (option.success) == 'function'){
                        option.success(data);
                    }
                }
                else {
                    alert(aaData.errMsg);
                    if (typeof (option.success) == 'function')
                        option.error(data);
                    }   
                }
            }
        }); 
}
