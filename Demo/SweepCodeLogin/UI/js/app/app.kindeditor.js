/*
    * 文件说明: KindEditor 操作函数库
    * 作者:     Aviv
    * 更新时间: 2018-06-14              
    * 版本号:   1.0.0.1
    * 说明
		所需JS文件支持
			~/js/jQuery/jquery-1.12.3.js 
			~/js/editor/kindeditor.js 
*/

if (typeof Editor !== 'object') {
    Editor = {};
}

/*
    * 函数说明: 初始化 KindEditor控件
    * 参数说明: 
    *           参数名称    类型          说明
    *           obj         object        textarea空间 (注意是空间非ID或Class类名) 
    *           options     json          KindEditor控件 设置属性
    *           upload      json          KindEditor控件 设置上传属性
*/
Editor.create = function (obj, option, upload) {
    option.width = option.width ? option.width : "100%";
    option.height = option.height ? option.height : "auto";
    option.resizeType = option.resizeType ? option.resizeType : 1;

    //true时显示网络图片标签，false时不显示。
    option.allowImageRemote = option.allowImageRemote ? option.allowImageRemote : false;

    option.allowImageUpload = option.allowImageUpload ? option.allowImageUpload : true;
    if (option.allowImageUpload) {
        if (!option.uploadJson) {
            alert("由于控件开启图片上传功能,则必须设置uploadJson参数");
            return;
        }
    }
    option.allowFileManager = option.allowFileManager ? option.allowFileManager : true;
    if (option.allowFileManager) {
        if (!option.fileManagerJson) {
            alert("由于控件开启文件空间功能,则必须设置fileManagerJson参数");
            return;
        }
    }
    option.resizeType = option.resizeType ? option.resizeType : 1;
    option.items = option.items ? option.items : [
          'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline',
          'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
          'insertunorderedlist', '|', 'emoticons', 'image', 'link', 'preview'];


    var editor = KindEditor.create(obj, {
        width: '100%',
        height: '350px',
        resizeType: 1,
        uploadJson: '../../../handler/upload.ashx?action=editorfile&type=' + GetQueryString("type") + "&modulartype=" + upload.img.modulartype,
        fileManagerJson: '../../../handler/upload.ashx?action=managerfile',
        allowFileManager: true,
        items: [
          'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline',
          'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
          'insertunorderedlist', '|', 'emoticons', 'image', 'link', 'preview'],
        afterCreate: function () {
            this.sync();
        },
        afterBlur: function () {
            this.sync();
        }
    });


    $(".upload-img").InitUploader({
        filesize: upload.img.filesize,
        sendurl: upload.img.sendurl,
        newstype: upload.img.newstype,
        modulartype: upload.img.modulartype,
        swf: upload.img.swf,
        filetypes: upload.img.filetypes
    });
    $(".upload-video").InitUploader({
        filesize: upload.video.filesize,
        sendurl: upload.video.sendurl,
        swf: upload.video.swf,
        filetypes: upload.video.filetypes
    });
    $(".upload-album").InitUploader({
        btntext: upload.album.btntext,
        multiple: upload.album.multiple,
        water: upload.album.water,
        thumbnail: upload.album.thumbnail,
        filesize: upload.album.filesize,
        sendurl: upload.album.sendurl,
        swf: upload.album.swf
    });

    return editor;
}


