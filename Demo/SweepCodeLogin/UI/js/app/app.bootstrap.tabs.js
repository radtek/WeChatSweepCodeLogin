/*
    * 文件说明: BootStrap Tabs 操作函数库
    * 作者:     Aviv
    * 更新时间: 2017-05-24            
    * 版本号:   1.0.0.3  
    * 说明
		所需JS文件支持
			~/js/jQuery/jquery-1.12.3.js 
            ~/js/bootstrap/js/bootstrap.min.js 
			~/js/app.js 
        所需CSS文件支持
            ~/js/bootstrap/css/bootstrap.min.css
*/

if (typeof Tabs !== 'object') {
    Tabs = {};
}

Tabs.addTabs = function (obj) {
    id = obj.id;

    $(".tab-item .active").removeClass("active");
     
    //创建新TAB的title
    var title = '<li role="presentation" id="tab_' + id + '"><a href="#tab_content_item_' + id + '" data-toggle="tab">' + obj.title;
    //是否允许关闭
    if (obj.close)  
        title += ' <i class="icon-cancel3" tabclose="' + id + '"></i>'; 
    title += '</a></li>';

    //是否指定TAB内容
    var content = '<div id="tab_content_item_' + id + '" class="tab-pane fade tab-content-item">'

    if (obj.content) {
        content +=  obj.content;
    } else {//没有内容，使用IFRAME打开链接 
        var _scrolling = obj.scrolling ? obj.scrolling : "auto";
        //2019-08-01 鄂尔多斯项目 修改
        //content += '<iframe src="' + obj.url + '" style="width: 100%; height: 100%;" frameborder="no" border="0" marginwidth="0" marginheight="0" scrolling="'+_scrolling+'" allowtransparency="yes"></iframe>';
        content += '<iframe id="tabiframe" src="' + obj.url + '" style="min-height:700px" height: 100%;" width="100%" border="0" marginwidth="0" marginheight="0" frameborder="0" scrolling="' + _scrolling + '" allowtransparency="yes" ></iframe>';
    }
    content += '</div>';
     

    //如果TAB不存在，创建一个新的TAB
    if (!$("#tab_content_item_" + id)[0]) {
        //加入TABS
        $(".tab-item").append(title); 
    } else {
        $(".tab-content div[id='tab_content_item_" + id + "']").remove();
    } 

    $(".tab-content").append(content);
     
    $('.tab-item a[href="#tab_content_item_' + id + '"]').tab('show');
};

Tabs.closeTab = function (id) {
    ////如果关闭的是当前激活的TAB，激活他的前一个TAB
    //if ($("li.active").attr('id') == "tab_" + id) {
    //    $("#tab_" + id).prev().addClass('active');
    //    $("#tab_content_item_" + id).prev().addClass('active');
    //}

    //关闭TAB
    $(".tab-item li[id='" + id + "']").remove();    //删除ID等于参数传入的Tabs
    $(".tab-item li[id='tab_" + id + "']").remove(); //删除ID等于通过 addTabs 函数添加的Tabs
    $(".tab-content div[id='tab_content_item_" + id + "']").remove();

    //返回值第一个标签
    $('.tab-item li:first a:first').tab('show');
};

$(function () {
    //mainHeight = $(document.body).height() - 45;
    //$('.main-left,.main-right').height(mainHeight);
    //$("[addtabs]").click(function () {
    //    addTabs({ id: $(this).attr("id"), title: $(this).attr('title'), close: true });
    //});

    //$(".tab-item").on("click", "[tabclose]", function (e) {
    //    id = $(this).attr("tabclose");
    //    closeTab(id);
    //});
   
    //给第一个标签设置点击样式
    $(".tabs .tab-item li:first").attr("class", "active");

    $(".tabs .tab-content").height($("body").height() - $(".tabs .tab-item").height() - 10);
});