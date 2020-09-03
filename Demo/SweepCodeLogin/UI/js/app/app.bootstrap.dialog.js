/*
    * 文件说明: 弹出框 操作函数库
    * 作者:     Aviv
    * 更新时间: 2016-10-24              
    * 版本号:   1.0.0.2
	* 说明
		所需JS文件支持
			~/js/jQuery/jquery-1.12.3.js  
            ~/js/bootstrap/js/bootstrap.min.js 
        所需CSS文件支持
            ~/js/bootstrap/css/bootstrap.min.css 
*/



if (typeof Dialog !== 'object') {
	Dialog = {};
}

Dialog.show = function (option) {

	//检查是否存在已有的对话框
	$("#_Modal_").remove();
	var width = $("body").width() * 0.75;
	var height = ($("body").height() / 2) < 400 ? 400 : $("body").height() / 2;

	var modal = $('<div class="modal fade" id="_Modal_" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">');
	var modal_dialog = $('<div class="modal-dialog">').css({ width: width });

	if (option.top) 
	    modal_dialog.css({ top: option.top });
	if(option.left)
	    modal_dialog.css({ left: option.left });

	var modal_content = $('<div class="modal-content">');
	
	var modal_header = $('<div class="modal-header">');
	modal_header.append($('<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>'));
	modal_header.append($('<h4 class="modal-title" id="myModalLabel">' + option.title + '</h4>'));
 

	var modal_body = $('<div class="modal-body">');

	var ifarme = null;
	if (option.url) {
	    ifarme = $('<iframe id="_Modal_ifarme_" high="' + height + '" style="width:100%;border:0;" src="' + option.url.replace("~/", getRootPath() + "/") + '">');
	    ifarme.css({ height: height });

	    modal_body.append(ifarme);
	} else if (option.content) {
		modal_body.append(option.content);
	}

	var modal_footer = $('<div class="modal-footer">');
	modal_footer.append($(' <button type="button" class="btn btn-primary"> 确定 </button>').on("click", function (e) { Dialog.choose(ifarme, option.choose) }));
	modal_footer.append($(' <button type="button" class="btn btn-default" data-dismiss="modal"> 关闭 </button>'));

	modal_content.append(modal_header);
	modal_content.append(modal_body);
	modal_content.append(modal_footer);

	modal_dialog.append(modal_content);
	modal.append(modal_dialog); 
	$("body").append(modal);

	$("#_Modal_").modal('show');
}

Dialog.choose = function (ifarme, func)
{
	if (ifarme) { 
		var arr = $(ifarme)[0].contentWindow.returnChoose();

		if (typeof (func) == 'function') {
			var r = func(arr);

			if (r) {
				$("#_Modal_").modal('hide');
			}
		}
	}
}