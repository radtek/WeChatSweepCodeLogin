/*
    * 文件说明: BootStrap Table 操作函数库
    * 作者:     Aviv
    * 更新时间: 2017-08-01              
    * 版本号:   1.0.0.5
    * 说明
		所需JS文件支持
			~/js/jQuery/jquery-1.12.3.js 
			~/js/app.js 
        所需CSS文件支持
            ~/js/bootstrap/css/bootstrap.min.css
            ~/js/bootstrap-table/bootstrap-table.css
*/


if (typeof Table !== 'object') {
    Table = {};
}




/*
    * 函数说明: 加载BootStrap Table 
    * 参数说明: 
    *           参数名称    类型          说明
    *           table       object        BootStrap Table 控件  
    *           options     json          BootStrap Table 设置属性
*/
Table.loadTable = function (table, options) {
    options.table = table;

    var rootPath = getRootPath();

    var scripts = [
            rootPath + '/js/bootstrap-table/bootstrap-table.js',
            //rootPath + '/js/bootstrap-table/extensions/export/bootstrap-table-export.js',
            rootPath + '/js/bootstrap-table/master/tableExport.js',
            rootPath + '/js/bootstrap-table/locale/bootstrap-table-zh-CN.min.js',
            rootPath + '/js/bootstrap-table/master/bootstrap-editable.js'
    ],
        eachSeries = function (arr, iterator, callback) {
            callback = callback || function () { };
            if (!arr.length) {
                return callback(options);
            }
            var completed = 0;
            var iterate = function () {
                iterator(arr[completed], function (err) {
                    if (err) {
                        callback(err);
                        callback = function () { };
                    }
                    else {
                        completed += 1;
                        if (completed >= arr.length) {
                            callback(options);
                        }
                        else {
                            iterate();
                        }
                    }
                });
            };
            iterate();
        };

    eachSeries(scripts, Table.getScript, Table.initTable);
};

/*
    * 函数说明: 重新加载BootStrap Table 
    * 参数说明: 
    *           参数名称    类型          说明
    *           table       object        BootStrap Table 控件  
    *           options     json          BootStrap Table 设置属性
*/
Table.reloadTable = function (table, options) {
    var _table = $(table);
    _table.bootstrapTable('refresh', options);
};


/*
    * 函数说明: 初始化BootStrap Table 不需要单独调用
    * 参数说明: 
    *           参数名称    类型          说明 
    *           options     json          BootStrap Table 设置属性
    * 返回值:  无 
*/
Table.initTable = function (options) {
    var $table = options.table, selections = [];


    options.locale = "zh-CN";//中文支持
    options.striped = options.data ? options.data : true; //行间变色
    options.data = options.data ? options.data : [],
    options.toolbar = options.toolbar ? options.toolbar : "#toolbar";
    options.search = options.search ? options.search : false;
    options.showRefresh = options.showRefresh ? options.showRefresh : false;
    options.showToggle = options.showToggle ? options.showToggle : false;
    options.showColumns = options.showColumns ? options.showColumns : false;
    options.showExport = options.showExport ? options.showExport : false;
    options.detailView = options.detailView ? options.detailView : false;
    options.minimumCountColumns = options.minimumCountColumns ? options.minimumCountColumns : 2;
    options.showPaginationSwitch = options.showpaginationswitch ? options.showpaginationswitch : false;
    options.pagination = options.pagination ? options.pagination : false;
    options.idField = options.idField ? options.idField : null;
    options.pageSize = options.pageSize ? options.pageSize : 50;
    options.pageList = options.pageList ? options.pageList : "[20, 50, 100]";
    options.showFooter = options.showFooter ? options.showFooter : false;
    options.sidePagination = options.sidePagination ? options.sidePagination : "client";
    options.detailFormatter = options.detailFormatter ? options.detailFormatter : function (index, row) {
        var html = [];
        $.each(row, function (key, value) {
            html.push('<p><b>' + key + '</b> ' + value + '</p>');
        });
        return html.join('');
    };
    options.responseHandler = options.responseHandler ? options.responseHandler : function (res) {
        $.each(res.rows, function (i, row) {
            row.state = $.inArray(row.id, selections) !== -1;
        });
        return res;
    };
    options.height = options.height ? options.height : 0;
    options.columns = options.columns ? options.columns : [];
    options.undefinedText = options.undefinedText ? options.undefinedText : "";

    //处理POST 提交数据服务器接收不到数据的问题
    if (options.method === 'post')
        options.contentType = "application/x-www-form-urlencoded";
    else
        options.contentType = options.contentType ? options.contentType : "application/json";


    //特殊处理执行成功函数 用于处理服务器返回错误信息
    options.success = function (res, that, silent) {
        if (res.sysError) { // 如果 sysError 有值 则为框架提供异常信息
            if (res.Msg)
                alert(res.Msg);
            if (res.Skip) // 服务器基类返回值 用于校验 校验服务状态 用户失效
                top.document.location.href = res.Skip;
            return false;
        }

        if (res.Result === "0") {
            that.load(res.Data);
            that.trigger('load-success', res);
            if (!silent) that.$tableLoading.hide();

            return true;
        }
        else {
            alert(res.errMsg)
            return false;
        }
    }

    // 2016-07-12 Aviv更新 处理调用  Table.loadTable方法 数据不刷新的问题
    var _op = $table.bootstrapTable('getOptions');

    if (_op.columns)
        $table.bootstrapTable('refresh', options);
    else
        $table.bootstrapTable(options);



    // sometimes footer render error.
    setTimeout(function () {
        $table.bootstrapTable('resetView');
    }, 200);
    $table.on('page-change.bs.table', function (number, size) {

    });
    $table.on('check.bs.table uncheck.bs.table ' +
            'check-all.bs.table uncheck-all.bs.table', function () {
                /* 2016-07-10 Aviv 注释此代码 因 $remove 函数返回 null
                $remove.prop('disabled', !$table.bootstrapTable('getSelections').length);
                */

                // save your data, here just save the current page
                selections = Table.getIdSelections($table);
                // push or splice the selections if you want to save all data selections
            });
    $table.on('expand-row.bs.table', function (e, index, row, $detail) {
        //if (index % 2 == 1) {
        //    $detail.html('Loading from ajax request...');
        //    $.get('LICENSE', function (res) {
        //        $detail.html(res.replace(/\n/g, '<br>'));
        //    });
        //}
    });
    $table.on('all.bs.table', function (e, name, args) {
        //console.log(name, args);
    });

    $(window).resize(function () {
        //$table.bootstrapTable('resetView', {
        //    height: getHeight()
        //});
    });

    //searchColumns 判断检索条件json是否符合规范
    if (options.searchColumns ) {
        $.each(options.searchColumns, function (i, searchColumns) {
            if (!searchColumns.field || !searchColumns.datatype || !searchColumns.ext || !searchColumns.title) {
                alert("searchColumns列第" + (i + 1) + "段json格式错误。");
            }

        });
    }
}


/*
    * 函数说明: 获取BootStrap Table 选择的标识列数据
    * 参数说明: 
    *           参数名称    类型          说明 
    *           table       object        BootStrap Table 控件  
    * 返回值:  
    *           类型        说明
    *           array        选择的行对象数组
*/
Table.getIdSelections = function (table) {
    var _table = $(table);
    return _table.bootstrapTable('getSelections');
}


/*
    * 函数说明: 获取BootStrap Table 选择的标识列数据
    * 参数说明: 
    *           参数名称    类型          说明 
    *           table       object        BootStrap Table 控件  
    *           colname     string        字段名称 根据传入的字段名称获取数据  
    * 返回值:  
    *           类型        说明
    *           string      选择的ID 以,(英文/半角)号分割
*/
Table.getIdSelectionsString = function (table, colname) {
    var ids = Table.getIdSelections(table);
    var _ids = "";
    for (var i = 0; i < ids.length; i++) {
        _ids += ids[i][colname];
        if (i < ids.length - 1)
            _ids += ",";
    }
    return _ids;
}


/*
    * 函数说明: 获取BootStrap Table 所有数据
    * 参数说明: 
    *           参数名称    类型          说明 
    *           table       object        BootStrap Table 控件  
    * 返回值:   
    *           类型        说明
    *           json        所有数据
*/
Table.getData = function (table) {
    var _table = $(table);
    return _table.bootstrapTable('getData');
}


/*
    * 函数说明: 加载JS文件
    * 参数说明: 
    *           参数名称    类型          说明 
    *           url         string        JS文件引用路径  
    *           callback    function      回调函数
    * 返回值:   
    *           类型        说明
    *           undefined   无
*/
Table.getScript = function (url, callback) {
    var head = document.getElementsByTagName('head')[0];
    var script = document.createElement('script');
    script.src = url;

    var done = false;
    // Attach handlers for all browsers
    script.onload = script.onreadystatechange = function () {
        if (!done && (!this.readyState ||
                this.readyState == 'loaded' || this.readyState == 'complete')) {
            done = true;
            if (callback)
                callback();

            // Handle memory leak in IE
            script.onload = script.onreadystatechange = null;
        }
    };

    head.appendChild(script);

    // We handle everything using the script element injection
    return undefined;
}