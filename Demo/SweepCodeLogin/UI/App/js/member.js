$(function () {
    /*******************************会员中心 我的工作台 begin*********************************/
    if (!window.portal) {
        window.portal = {};
    }

    //加载数据
    window.portal.loadData = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadData'
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var userData = responseData.UserData;
                        var noPayOrderNumber = responseData.NoPayOrderNumber;
                        var paNumber = responseData.PANumber;
                        var transactionNumber = responseData.TransactionNumber;
                        var demandNumber = responseData.DemandNumber;
                        var expertNumber = responseData.ExpertNumber;
                        var messageData = responseData.MessageData.rows;

                        window.portal.loadUserInfo(userData);

                        var roleType = getCookie("roletype");
                        //判断当前系统
                        if (roleType == "1") {
                            $("#college").attr("style", "display:block;padding-left: 5px;padding-right: 0;");
                            $("#ordinary").attr("style", "display:none");

                            $("label[role='transactionnumber'").text(transactionNumber);

                            if (paNumber > 9999) {
                                $("label[role='panumber']").text("9999");
                            } else {
                                $("label[role='panumber']").text(paNumber);
                            }

                            $("#expertNumber").text(expertNumber);
                        } else {
                            $("#college").attr("style", "display:none");
                            $("#ordinary").attr("style", "display:block;padding-left: 5px; padding-right: 0;");

                            $("#noPayOrder").text(noPayOrderNumber);

                            if (paNumber > 999) {
                                $("#panumber").text("999+");
                            } else {
                                $("#panumber").text(paNumber);
                            }

                            $("#transactionnumber").text(transactionNumber);
                            $("#demandnumber").text(demandNumber);
                        }

                        window.portal.loadMessage(messageData);
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载用户信息
    window.portal.loadUserInfo = function (data) {
        if (data) {
            var roleType = getCookie("roletype");

            //判断当前系统
            if (roleType == "1") {
                $("#position").attr("style", "display:block;");
                $("#membergrade").attr("style", "display:none;");
                $("span[role='position']").text(data["Position"]);
            } else {
                $("#position").attr("style", "display:none;");
                $("#membergrade").attr("style", "display:block;");
            }

            switch (data["AccountType"]) {
                case "个人账户":
                    $("#username").text(data["UserName"]);
                    break;
                case "企业账户":
                    $("#username").text(data["Company"]);
                    break;
                case "高校院所":
                    $("#username").text(data["UniversitiesName"]);
                    break;
                case "其他事业单位":
                    $("#username").text(data["Company"]);
                    break;
            }
            //审核状态
            switch (data["CheckState"]) {
                case "0":
                    $("#checkstate").addClass("btn-blue").text("未认证");
                    $("#checkstate").attr('href', 'member-info-certification');
                    break;
                case "1":
                    $("#checkstate").addClass('btn-yellow').text("认证中");
                    break;
                case "2":
                    $("#checkstate").addClass("btn-green").text("认证成功");
                    break;
                case "3":
                    $("#checkstate").addClass('btn-red').text("认证失败");
                    $("#checkstate").attr('href', 'member-info-certification');
                    break;
                default:
                    break;
            }
            //头像
            if (data["ProfilePhotoPath"]) {
                $("#profilephotopath").attr("src", data["ProfilePhotoPath"].replace('~', getRootPath()));
            }
            //会员等级
            switch (data["VIPGrade"]) {
                case "0":
                    $("#grade").text("普通会员");
                    break;
                case "1":
                    $("#grade").text("VIP会员");
                    break;
                case "2":
                    $("#grade").text("VIPPlus会员");
                    break;
            }
            //积分
            if (data["Integrals"] != null) {
                $("#integrals").text(data["Integrals"]);
            }
        }
    }

    //加载消息
    window.portal.loadMessage = function (data) {
        if (data.length > 0) {
            var html = "";

            for (var i = 0; i < data.length; i++) {
                html += "<ul class='scrollbar mlist'>";
                html += "   <li style='border: none;'>";
                html += "       <a onclick=\"window.portal.readMessage(" + data[i]["ID"] + ",'" + data[i]["Url"] + "')\">";
                html += "           <i class='fa fa-exclamation-triangle fl'></i>";
                html += "           <div class='fl'>";
                html += "               <label class='fl'>【" + data[i]["Modular"] + "】</label>";
                html += "               <p class='fl m-txt'>" + data[i]["Title"] + data[i]["State"] + "</p>";
                html += "               <span class='fr m-time'>" + createDate(data[i]["CreateTime"]).Format("yyyy-MM-dd") + "</span>";
                html += "           </div>";
                html += "       </a>";
                html += "   </li>";
                html += "</ul>";
                html += "<a class='m-btn m-close' onclick='window.portal.closeMessageBox();' title='关闭'><i class='fa fa-close'></i></a>";
                html += "<a class='m-btn m-read' onclick='window.portal.allReadMessage();' title='全部标为已读'><i class='fa fa-envelope-open'></i></a>";
            }

            $("#messagelist").html("");
            $("#messagelist").append(html);
            $("#messagelist").show();
        }
    }

    //读取消息
    window.portal.readMessage = function (id, url) {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_ReadMessage',
                id: id
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        window.location.href = url;
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //全部读取消息
    window.portal.allReadMessage = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_AllReadMessage'
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        $(".messagebox").css({ "display": "none" });
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //关闭消息弹窗
    window.portal.closeMessageBox = function () {
        $(".messagebox").css({ "display": "none" });
    }

    //加载数量
    window.portal.loadNumber = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadNumber',
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var data = responseData.Data.rows;

                        //加载我的专利
                        window.portal.loadMyPatent(data);

                        var roleType = getCookie("roletype");
                        //判断当前系统
                        if (roleType == "1") {
                            //加载我的专利法律状态
                            window.portal.loadMyPatentLawState(data);
                        } else {
                            //加载我的订单
                            window.portal.loadMyOrder(data);
                        }

                        //加载发布量
                        window.portal.loadPublishCount(data, roleType);
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载我的专利
    window.portal.loadMyPatent = function (data) {
        if (data) {
            var inventionPACount = data[0]["InventionPACount"];
            var appearanceCount = data[0]["AppearanceCount"];
            var newTypeCount = data[0]["NewTypeCount"];

            //我的专利
            var myChart = echarts.init(document.getElementById('chart-pat'));
            var option = {
                color: ['#ff6264', '#00b19d', '#40bbea'],
                title: {
                    text: '我的专利',
                    x: 'left',
                    textStyle: {
                        fontSize: 16
                    }
                },
                series: [{
                    name: '专利类型(/件)',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '50%'],
                    label: {
                        normal: {
                            show: true
                        },
                        emphasis: {
                            show: true,
                            formatter: "{b}/件\n{c} ({d}%)",
                        }
                    },
                    lableLine: {
                        normal: {
                            show: false
                        },
                        emphasis: {
                            show: true
                        }
                    },
                    data: [{
                        value: inventionPACount,
                        name: '发明专利'
                    },
                    {
                        value: appearanceCount,
                        name: '外观设计'
                    },
                    {
                        value: newTypeCount,
                        name: '实用新型'
                    }
                    ],
                }]
            };

            myChart.setOption(option);
        }
    }

    //加载我的订单
    window.portal.loadMyOrder = function (data) {
        if (data) {
            var noPayOrderCount = data[0]["NoPayOrderCount"];
            var hasPayOrderCount = data[0]["HasPayOrderCount"];

            //我的订单
            $("#chart-order").html("");
            var myChart = echarts.init(document.getElementById('chart-order'));
            var option = {
                color: ['#ff6264', '#00b19d', '#40bbea'],
                title: {
                    text: '我的订单',
                    x: 'left',
                    textStyle: {
                        fontSize: 16
                    }
                },
                series: [{
                    name: '我的订单/件',
                    type: 'pie',
                    radius: ['35%', '50%'],
                    label: {
                        normal: {
                            show: true
                        },
                        emphasis: {
                            show: true,
                            formatter: "{b}\n{c} ({d}%)",
                            textStyle: {
                                fontWeight: 'bold'
                            }
                        }
                    },
                    lableLine: {
                        normal: {
                            show: false
                        },
                        emphasis: {
                            show: true
                        }
                    },
                    data: [
                        {
                            value: noPayOrderCount,
                            name: '待付款'
                        },
                        {
                            value: hasPayOrderCount
                            ,
                            name: '已付款'
                        },
                    ]
                }]

            };

            myChart.setOption(option);
        }
    }

    //加载我的专利法律状态
    window.portal.loadMyPatentLawState = function (data) {
        if (data) {
            var printFeeCount = data[0]["PrintFeeCount"];
            var maintainCount = data[0]["MaintainCount"];
            var lateFeeCount = data[0]["LateFeeCount"];

            $("#chart-order").html("");
            var myChart = echarts.init(document.getElementById('chart-order'));
            var option = {
                color: ['#ff6264', '#00b19d', '#40bbea', '#f5b91e', '#da70d6', '#ff7f50', '#d7e361', '#44a99f', '#ef598c', '#ffeb7b'],
                series: [{
                    name: '我的专利',
                    type: 'pie',
                    radius: ['35%', '50%'],
                    label: {
                        normal: {
                            show: true
                        },
                        emphasis: {
                            show: true,
                            formatter: "{b}\n{c} ({d}%)",
                            textStyle: {
                                fontWeight: 'bold'
                            }
                        }
                    },
                    lableLine: {
                        normal: {
                            show: false
                        },
                        emphasis: {
                            show: true
                        }
                    },
                    data: [
                        {
                            value: printFeeCount,
                            name: '等年登印费'
                        },
                        {
                            value: maintainCount,
                            name: '专利权维持'
                        },
                        {
                            value: lateFeeCount,
                            name: '等年费滞纳金'
                        },
                    ]
                }]
            };

            myChart.setOption(option);
        }
    }

    //加载发布量
    window.portal.loadPublishCount = function (data, type) {
        if (data) {
            var transactionPublishCount = data[0]['TransactionPublishCount'];
            if (!transactionPublishCount) {
                transactionPublishCount = "0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0";
            }
            var demandPublishCount = data[0]["DemandPublishCount"];
            if (!demandPublishCount) {
                demandPublishCount = "0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0";
            }
            var patentPublishCount = data[0]["PatentPublishCount"];
            if (!patentPublishCount) {
                patentPublishCount = "0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0";
            }

            var date = new Date;
            var year = date.getFullYear();

            $("#chart-trading").html("");
            if (type == "1") {
                var myChart = echarts.init(document.getElementById('chart-trading'));
                var option = {
                    color: ['#ff6264', '#00b19d', '#40bbea'],
                    title: {
                        text: year + '专利数据',
                        x: 'left',
                        textStyle: {
                            fontSize: 16
                        }
                    },
                    tooltip: {
                        trigger: 'axis',
                    },
                    legend: {
                        data: ['我的专利', '专利交易'],
                        x: 'right',
                    },
                    grid: {
                        left: '3%',
                        right: '4%',
                        bottom: '3%',
                        containLabel: true
                    },
                    xAxis: [{
                        type: 'category',
                        boundaryGap: false,
                        data: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月']

                    }],
                    yAxis: [{
                        type: 'value'
                    }],
                    series: [{
                        name: '我的专利',
                        type: 'line',
                        stack: '总量',
                        areaStyle: {},
                        data: patentPublishCount.split(',')
                    },
                    {
                        name: '专利交易',
                        type: 'line',
                        stack: '总量',
                        areaStyle: {},
                        data: transactionPublishCount.split(',')
                    }
                    ]
                };

                myChart.setOption(option);
            } else {
                var myChart = echarts.init(document.getElementById('chart-trading'));
                var option = {
                    color: ['#ff6264', '#00b19d', '#40bbea'],
                    title: {
                        text: year + "发布量/件",
                        x: 'left',
                        textStyle: {
                            fontSize: 16
                        }
                    },
                    tooltip: {
                        trigger: 'axis',
                    },
                    legend: {
                        data: ['专利交易', '专利需求'],
                        x: 'right',
                    },
                    grid: {
                        left: '3%',
                        right: '4%',
                        bottom: '3%',
                        containLabel: true
                    },
                    xAxis: [{
                        type: 'category',
                        boundaryGap: false,
                        data: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月']

                    }],
                    yAxis: [{
                        type: 'value'
                    }],
                    series: [{
                        name: '专利交易',
                        type: 'line',
                        stack: '总量',
                        areaStyle: {},
                        data: transactionPublishCount.split(',')
                    },
                    {
                        name: '专利需求',
                        type: 'line',
                        stack: '总量',
                        areaStyle: {},
                        data: demandPublishCount.split(',')
                    }
                    ]
                };

                myChart.setOption(option);
            }
        }

    }
    /*******************************会员中心 我的工作台  end *********************************/

    /*******************************会员中心 我的预留 begin*********************************/
    if (!window.reserve) {
        window.reserve = {};
    }

    //加载列表
    window.reserve.loadList = function () {
        var option = {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "M_LoadList";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "id",
            height: $(".memberbox").height(),
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [{
                title: '专利号',
                field: 'ApplyNo',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '专利名称',
                field: 'PAName',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '预留到期',
                field: 'RetainTime',
                datatype: 'datetime',
                ext: 'like'
            }],
            columns: [{
                title: '_ckb_',
                field: "",
                sortable: false,
                checkbox: true,
                width: '30',
                align: 'center',
                valign: 'middle'
            }, {
                title: '',
                field: 'ID',
                visible: false,
                align: 'center'
            }, {
                title: '专利号',
                field: 'ApplyNo',
                sortable: false,
                align: 'center',
                valign: 'middle',
            }, {
                title: '专利名称',
                field: 'PAName',
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        var html = "";
                        var paname = value;
                        var applyno = row.ApplyNo.replace('.', '');
                        if (value.length > 20) {
                            paname = value.substring(0, 20) + "...";
                        }
                        html += "<a class='color-blue' title='" + value + "' href='market-detail-" + row.FK_TranPA_ID + "-" + applyno + "' target='_blank'>" + paname + "</a>";
                        return html;
                    }

                    return "";
                }
            }, {
                title: '专利类型',
                field: 'PAType',
                sortable: false,
                align: 'center',
                valign: 'middle',
            }, {
                title: '法律状态',
                field: 'PAFLState',
                sortable: false,
                align: 'center',
                valign: 'middle',
            }, {
                title: '交易价格',
                field: 'TranPrice',
                sortable: false,
                align: 'center',
                valign: 'middle',
            }, {
                title: '预留价格',
                field: 'Price',
                sortable: false,
                align: 'center',
                valign: 'middle',
            }, {
                title: '预留时间',
                field: 'CreateTime',
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        return createDate(value).Format("yyyy-MM-dd HH:ss");
                    } else {
                        return "";
                    }
                }
            }, {
                title: '预留到期',
                field: 'RetainTime',
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        return createDate(value).Format("yyyy-MM-dd HH:ss");
                    } else {
                        return "";
                    }
                }
            }, {
                title: '操作',
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    var html = "<a class='tablebtn btn-blue' onclick='window.reserve.cancelReserve(" + row.ID + ");' href='javascript:void(0);'>取消预留</a>";
                    return html;
                }
            }]
        };

        Table.loadTable($("#reservetable"), option);
    }

    //取消预留
    window.reserve.cancelReserve = function (id) {
        call_ajax({
            url: window.location.href,
            data: {
                action: "M_CancelReserve",
                id: id
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        alert("取消成功。");
                        window.reserve.loadList();
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }
    /*******************************会员中心 我的预留  end *********************************/

    /*******************************会员中心 我的订单 begin*********************************/
    if (!window.order) {
        window.order = {};
    }

    //加载金额
    window.order.loadAmount = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: "M_LoadAmount"
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var hasPay = responseData.HasPay;
                        var noPay = responseData.NoPay;
                        var totalAmount = responseData.TotalAmount;
                        var countData = responseData.Count.rows;

                        $("#ordertotalamount").text(totalAmount);
                        $("#ordernopay").text(noPay);
                        $("#orderhaspay").text(hasPay);

                        for (var i = 0; i < countData.length; i++) {
                            switch (countData[i]["Type"]) {
                                case "All":
                                    $("#allorder").text("全部订单（" + countData[i]["Num"] + "）");
                                    break;
                                case "NoPay":
                                    $("#nopayorder").text("待付款（" + countData[i]["Num"] + "）");
                                    break;
                                case "HasPay":
                                    $("#haspayorder").text("已付款（" + countData[i]["Num"] + "）");
                                    break;
                                case "Cancel":
                                    $("#cancelorder").text("已取消（" + countData[i]["Num"] + "）");
                                    break;
                            }
                        }
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载订单列表
    window.order.loadOrderList = function (type) {
        var option = "";

        switch (type) {
            case "all":
                option = window.order.loadAllOption(type);
                break;
            default:
                option = window.order.loadNoAllOption(type);
                break;
        }

        $(".pagetablebox").html("");
        $(".pagetablebox").append("<table class='pagetable' id='ordertable'></table>");
        Table.loadTable($("#ordertable"), option);
    }

    //加载全部订单的option
    window.order.loadAllOption = function (type) {
        return {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "M_LoadList";
                params.type = type;
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "id",
            height: 550,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [{
                title: '订单信息',
                field: 'Title',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '订单时间',
                field: 'CreateTime',
                datatype: 'datetime',
                ext: 'like'
            }],
            columns: [{
                title: '',
                field: 'ID',
                visible: false,
                align: 'center'
            }, {
                title: '订单编号',
                field: "No",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '订单时间',
                field: "CreateTime",
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        return createDate(value).Format("yyyy-MM-dd");
                    } else {
                        return "";
                    }
                }
            }, {
                title: '订单信息',
                field: "Title",
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        return "<a class='color-blue' onclick=\"window.order.openDetailTab(" + row.ID + ",'" + type + "');\" href='javascript:void(0);'>" + value + "</a>";
                    } else {
                        return "";
                    }
                }
            }, {
                title: '费用合计/元',
                field: "Amount",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '订单状态',
                field: "State",
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        switch (value) {
                            case "1":
                                return "未付款";
                                break;
                            case "2":
                                return "已付款";
                                break;
                            case "3":
                                return "已取消";
                                break;
                            case "4":
                                return "<span style='color: red;'>正在审核</span>";
                                break;
                        }
                    } else {
                        return "";
                    }
                }
            }, {
                title: '操作',
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    var html = "";

                    switch (row.State) {
                        case "1":
                            html += "<a class='tablebtn btn-red' href='pay-detail-" + row.No + "' target='_blank'>立即付款</a>";

                            if (row.ContractID != null) {
                                html += "<a class='tablebtn btn-blue' onclick='window.order.download(" + row.ID + ");' href='javascript:void(0);'>合同</a>";
                            }

                            html += "<a class='tablebtn btn-Dred' onclick='window.order.cancel(" + row.ID + ");' href='javascript:void(0);'>取消</a>";
                            break;
                        case "2":
                            if (row.ContractID != null) {
                                html += "<a class='tablebtn btn-blue' onclick='window.order.download(" + row.ID + ");' href='javascript:void(0);'>合同</a>";
                            }
                            break;
                        case "3":
                        case "4":
                            break;
                    }

                    return html;
                }
            }]
        };
    }

    window.order.loadNoAllOption = function (type) {
        return {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "M_LoadList";
                params.type = type;
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "id",
            height: 550,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [{
                title: '订单信息',
                field: 'Title',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '订单时间',
                field: 'CreateTime',
                datatype: 'datetime',
                ext: 'like'
            }],
            columns: [{
                title: '',
                field: 'ID',
                visible: false,
                align: 'center'
            }, {
                title: '订单编号',
                field: "No",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '订单时间',
                field: "CreateTime",
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        return createDate(value).Format("yyyy-MM-dd");
                    } else {
                        return "";
                    }
                }
            }, {
                title: '订单信息',
                field: "Title",
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        return "<a class='color-blue' onclick=\"window.order.openDetailTab(" + row.ID + ",'" + type + "');\" href='javascript:void(0);'>" + value + "</a>";
                    } else {
                        return "";
                    }
                }
            }, {
                title: '费用合计/元',
                field: "Amount",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '操作',
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    var html = "";

                    switch (row.State) {
                        case "1":
                            html += "<a class='tablebtn btn-red' href='pay-detail-" + row.No + "' target='_blank'>立即付款</a>";

                            if (row.ContractID != null) {
                                html += "<a class='tablebtn btn-blue' onclick='window.order.download(" + row.ID + ");' href='javascript:void(0);'>合同</a>";
                            }

                            html += "<a class='tablebtn btn-Dred' onclick='window.order.cancel(" + row.ID + ");' href='javascript:void(0);'>取消</a>";
                            break;
                        case "2":
                            if (row.ContractID != null) {
                                html += "<a class='tablebtn btn-blue' onclick='window.order.download(" + row.ID + ");' href='javascript:void(0);'>合同</a>";
                            }
                            break;
                        case "3":
                        case "4":
                            break;
                    }

                    return html;
                }
            }]
        };
    }

    //下载合同
    window.order.download = function (id) {
        var url = window.location.href;

        //1.组装form表单
        var form = $("<form>");
        form.attr('style', 'display:none');
        form.attr('target', '');
        form.attr('method', 'post');
        form.attr('action', url);

        var hidden = $("<input type='hidden' name='action' value='M_Download'>");
        form.append(hidden);

        var idHidden = $("<input type='hidden' name='ID' value='" + id + "' />");
        form.append(idHidden);

        //2.form表单提交
        $('body').append(form);
        form.submit();
    }

    //取消
    window.order.cancel = function (id) {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_Cancel',
                id: id
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        window.order.loadAmount();
                        window.order.loadOrderList('all');
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //打开详情Tab
    window.order.openDetailTab = function (id, type) {
        Tabs.addTabs({
            id: "detailtab",
            title: "详情",
            url: "member-order-detail-" + id + "-" + type,
            close: true
        });
    }

    //关闭详情Tab
    window.order.closeDetailTab = function (type) {
        Tabs.closeTab("detailtab");
        $("#" + type).click();
    }

    //加载订单详情
    window.order.loadOrderDetail = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadData'
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var orderData = responseData.Data.rows;

                        for (var i = 0; i < orderData.length; i++) {
                            if (orderData[0]["SubmitType"] == "sys_MemberIntegrals") {
                                $("#integralorderno").text(orderData[i]["No"]);
                                $("#integralordercreatetime").text(createDate(orderData[i]["CreateTime"]).Format("yyyy-MM-dd"));

                                switch (orderData[i]["State"]) {
                                    case "1":
                                    case "4":
                                        $("#integralorderstate").text("待付款");
                                        break;
                                    case "2":
                                        $("#integralorderstate").text("已付款");
                                        break;
                                    case "3":
                                        $("#integralorderstate").text("已取消");
                                        break;
                                }

                                $("#integralordertitle").text(orderData[i]["Title"]);
                                $("#integralorderamount").text(orderData[i]["Amount"]);

                                $(".integral").show();
                            } else {
                                $("#orderno").text(orderData[i]["No"]);
                                $("#ordercreatetime").text(createDate(orderData[i]["CreateTime"]).Format("yyyy-MM-dd"));

                                switch (orderData[i]["State"]) {
                                    case "1":
                                    case "4":
                                        $("#orderstate").text("待付款");
                                        break;
                                    case "2":
                                        $("#orderstate").text("已付款");
                                        break;
                                    case "3":
                                        $("#orderstate").text("已取消");
                                        break;
                                }

                                var paID = orderData[i]["FK_ID"];
                                var applyNo = orderData[i]["ApplyNo"];
                                applyNo = applyNo.replace(".", "");
                                $("#painfo").append("<a class='color-blue' href='market-detail-" + paID + "-" + applyNo + "' target='_blank'>" + orderData[i]["PAName"] + "</a>")

                                $("#ordertitle").text(orderData[i]["Title"]);
                                $("#ordersuccessprice").text(orderData[i]["DaiLiFei"] == null ? "" : orderData[i]["DaiLiFei"]);
                                $("#orderofficialfee").text(orderData[i]["GuanFei"] == null ? "" : orderData[i]["GuanFei"]);

                                if (orderData[i]["IsTax"] == 'Y') {
                                    $("#yes").attr("checked", "checked");
                                } else {
                                    $("#no").attr("checked", "checked");
                                }

                                $("#orderamount").text(orderData[i]["Amount"]);
                                $("#orderremark").text(orderData[i]["Remark"] == null ? "" : orderData[i]["Remark"]);

                                $(".order").show();
                            }
                        }
                    }
                } else {
                    alert(responseData.errMsg);
                }
            }
        });
    }
    /*******************************会员中心 我的订单  end *********************************/
    /*******************************支付 begin*********************************/
    if (!window.payment) {
        window.payment = {};
    }

    //加载订单信息
    window.payment.loadOrderData = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadOrderData',
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var orderData = responseData.Data.rows;
                        var html = "";

                        $("span[role='count']").text(orderData.length);

                        for (var i = 0; i < orderData.length; i++) {
                            html += "<tr>";
                            html += "   <td>" + (i + 1) + "</td>";
                            html += "   <td>" + orderData[i]["No"] + "</td>";
                            html += "   <td>" + createDate(orderData[i]["CreateTime"]).Format("yyyy-MM-dd") + "</td>";
                            html += "   <td>" + orderData[i]["Title"] + "</td>";
                            html += "   <td>" + orderData[i]["Amount"] + "</td>";

                            switch (orderData[i]["State"]) {
                                case "1":
                                case "4":
                                    html += "   <td>未付款</td>";
                                    break;
                            }

                            //html += "   <td><a class='tablebtn btn-Dred' href='javascript:void(0);'><i class='fa fa-trash'></i></a></td>";
                            html += "</tr>";
                            html += "<tr>";
                            html += "   <td colspan='7'><span class='font18 color-red fr fblod'>金额：" + orderData[i]["Amount"] + "元</span> </td>";
                            html += "</tr>";
                        }

                        $("#orderlist").html("");
                        $("#orderlist").append(html);
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //支付
    window.payment.pay = function (type) {
        var action = "";

        switch (type) {
            case "alipay-company":
                action = "M_Alipay";
                break;
            case "unionpay-company":
                if (!formviliable(type)) {
                    return;
                }
                action = "M_OfflinePayment";
                break;
            default:
                alert("支付平台不正确，请重新选择");
                break;
        }

        call_ajax({
            url: location.href,
            data: {
                action: action,
                type: type,
                payman: $("input[role='" + type + "'][name='PayMan']").val(),
                paydate: $("input[role='" + type + "'][name='PayDate']").val()
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result === "0") {
                        var html = aaData.Form;

                        html = html.replace(/&lt;/g, "<");
                        html = html.replace(/&apos;/g, "'");
                        html = html.replace(/&gt;/g, ">");

                        $("body").appendChild(aaData.Form);
                    } else if (aaData.Result === "10") {
                        alert("确认支付成功。");
                        location = 'member-order-list';
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    /*******************************支付  end *********************************/

    /*******************************会员中心 我的专利 begin*********************************/
    if (!window.patent) {
        window.patent = {};
    }

    //加载列表
    window.patent.loadList = function () {
        var option = {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "M_LoadList";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "id",
            height: $(".memberbox").height(),
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [{
                title: '申请号/专利号',
                field: 'ApplyNo',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '专利名称',
                field: 'PAName',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '案件状态',
                field: 'LawState',
                datatype: 'select',
                ext: 'like',
                suboption: [{
                    title: '发明已下证',
                    value: '发明已下证'
                }, {
                    title: '发明未缴费',
                    value: '发明未缴费'
                }, {
                    title: '新型已下证',
                    value: '新型已下证'
                }, {
                    title: '新型未缴费',
                    value: '新型未缴费'
                }, {
                    title: '有权-授权',
                    value: '有权-授权'
                }, {
                    title: '专利权维持',
                    value: '专利权维持'
                }, {
                    title: '等年费滞纳金',
                    value: '等年费滞纳金'
                }, {
                    title: '授权未交费',
                    value: '授权未交费'
                }, {
                    title: '无权-避重放弃',
                    value: '无权-避重放弃'
                }, {
                    title: '无权-视为放弃',
                    value: '无权-视为放弃'
                }, {
                    title: '无权-终止',
                    value: '无权-终止'
                }, {
                    title: '审中-公开',
                    value: '审中-公开'
                }, {
                    title: '审中-实审',
                    value: '审中-实审'
                }]
            }, {
                title: '申请日',
                field: 'ApplyDate',
                datatype: 'datetime',
                ext: 'like'
            }],
            columns: [{
                title: '_ckb_',
                field: "",
                sortable: false,
                width: '30',
                checkbox: true,
                align: 'center',
                valign: 'middle'
            }, {
                title: '',
                field: 'ID',
                visible: false,
                align: 'center'
            }, {
                title: '申请号/专利号',
                field: 'ApplyNo',
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '专利名称',
                field: 'PAName',
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        if (value.length > 20) {
                            return "<a class='color-blue' onclick='PatentDetail(" + row.ID + ");' title='" + value + "' href='javascript:void(0);'>" + value.substring(0, 20) + "..." + "</a>";
                        } else {
                            return "<a class='color-blue' onclick='PatentDetail(" + row.ID + ");' title='" + value + "' href='javascript:void(0);'>" + value + "</a>";
                        }
                    } else {
                        return "";
                    }
                }
            }, {
                title: '专利类型',
                field: "PAType",
                sortable: false,
                align: 'center',
                valign: 'middle'

            }, {
                title: '申请人/权利人',
                field: "CurrentObligee",
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        if (value.length > 25) {
                            return value.substring(0, 25) + "...";
                        } else {
                            return value;
                        }
                    } else {
                        return "";
                    }
                }
            }, {
                title: '申请日',
                field: "ApplyDate",
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        return createDate(value).Format("yyyy-MM-dd");
                    } else {
                        return "";
                    }
                }
            }, {
                title: '案件状态',
                field: "LawState",
                sortable: false,
                align: 'center',
                valign: 'middle',
            }, {
                title: '操作',
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (row.PublishType == 'N') {
                        return "<a class='tablebtn btn-blue' onclick='DisplayPublishTransaction(" + row.ID + ");' href='javascript:void(0);'>发布交易</a>";
                    } else {
                        return "<a class='tablebtn btn-yellow' href='javascript:void(0);'>已发布</a>";
                    }
                }
            }]
        };

        Table.loadTable($("#table"), option);
    }

    //发布交易
    window.patent.publishTransaction = function (id) {
        var tranPrice = $("#tranprice").val();
        var tranType = $("#trantype").val();
        var expePrice = $("#expeprice").val();

        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_PublishTransaction',
                paid: id,
                tranprice: tranPrice,
                trantype: tranType,
                expeprice: expePrice
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        alert("发布成功。");
                        $(".popbox").css({ "display": "none" });
                        $(".blackbox").css({ "display": "none" });
                        window.patent.loadList();
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //检索专利
    window.patent.searchPA = function (offset, page) {
        var searchParam = $("#searchcontents").val();
        var type = $("#tab-item li[class='active']").find("a").attr("id");

        if (searchParam == null || searchParam == "") {
            alert("请填写检索信息。");
            return;
        }

        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_SearchPA',
                offset: offset,
                limit: 50,
                type: type,
                searchParam: searchParam
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var paData = responseData.Data;
                        var userPAData = responseData.PAData.rows;
                        var limit = parseInt(responseData.Limit);
                        var count = responseData.AllCount;
                        var html = "";

                        $("#total").text(count);
                        $("#searchvalue").text(searchParam);
                        $("#recore").attr("style", "display: block");

                        for (var i = 0; i < paData.length; i++) {
                            var isEqual = false;
                            for (var j = 0; j < userPAData.length; j++) {
                                if (paData[i]["sqh"] == userPAData[j]["ApplyNo"]) {
                                    isEqual = true;
                                    break;
                                }
                            }

                            html += "<tr>";

                            if (isEqual) {
                                html += "   <td><input class='check' disabled='disabled' type='checkbox' name='' /></td>";
                            } else {
                                html += "   <td><input class='check noadd' type='checkbox' name='' value='" + paData[i]["sqh"] + "' /></td>";
                            }

                            html += "   <td>" + paData[i]["sqh"] + "</td>";
                            html += "   <td>" + paData[i]["title"] + "</td>";
                            html += "   <td>" + window.patent.paType(paData[i]["patType"]) + "</td>";
                            html += "   <td>" + paData[i]["applicantName"] + "</td>";
                            html += "   <td>" + paData[i]["appDate"] + "</td>";
                            html += "   <td>" + paData[i]["agencyName"] + "</td>";

                            if (isEqual) {
                                html += "   <td><a class='tablebtn btn-gray' href='javascript:void(0);'>已添加</a></td>";
                            } else {
                                html += "   <td><a class='tablebtn btn-blue' onclick=\"window.patent.addPatent(this,'" + paData[i]["sqh"] + "')\" href='javascript:void(0);'>添加</a></td>";
                            }

                            html += "</tr>";
                        }

                        $("#palist").html("");
                        $("#palist").append(html);

                        if (count <= limit) {
                            getStartedInitialization(page, 1);
                        } else {
                            if (count % responseData.Limit > 0) {
                                getStartedInitialization(page, ((count / limit) + 1));
                            } else {
                                getStartedInitialization(page, (count / limit));
                            }
                        }
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //转义专利类型
    window.patent.paType = function (type) {
        switch (type) {
            case "1":
                return "发明专利";
                break;
            case "2":
                return "实用新型";
                break;
            case "3":
                return "外观设计";
                break;
            case "8":
                return "PCT发明";
                break;
            case "9":
                return "PCT实用新型";
                break;
            default:
                break;
        }
    }

    //单个添加专利
    window.patent.addPatent = function (obj, applyno) {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_AddPatent',
                applyno: applyno
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        alert("添加成功。");
                        $(obj).removeClass("btn-blue").addClass("btn-gray");
                        $(obj).text("已添加");
                        $(obj).attr("onclick", "");
                        $(obj).parent().parent().find("input[type='checkbox']").attr("disabled", "disabled");
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //本页添加
    window.patent.addThisPage = function () {
        //1.获取当前页面未添加的专利
        var strApplyNo = "";
        $(".noadd").each(function () {
            strApplyNo += $(this).val() + ",";
        });

        //2.校验
        if (strApplyNo == "") {
            alert("本页专利已添加过，请勿重复添加。");
            return;
        }

        strApplyNo = strApplyNo.substring(0, strApplyNo.length - 1);

        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_BatchAddPatent',
                strapplyno: strApplyNo
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        window.patent.searchPA(1, 1);
                    } else {
                        alert(responseData.errMsg);
                        window.patent.searchPA(1, 1);
                    }
                }
            }
        });
    }

    //一键添加专利
    window.patent.oneClickAdd = function () {
        //1.获取勾选的专利
        var strApplyNo = "";
        $(".noadd").each(function () {
            if ($(this)[0].checked) {
                strApplyNo += $(this).val() + ",";
            }
        })

        strApplyNo = strApplyNo.substring(0, strApplyNo.length - 1);

        //2.校验
        if (strApplyNo == "") {
            alert("未选中需要添加的专利。");
            return;
        }

        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_BatchAddPatent',
                strapplyno: strApplyNo
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        window.patent.searchPA(1, 1);
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });

    }

    //检索专利信息
    window.patent.searchPAInfo = function () {
        var applyNo = $("input[name='applyno']").val();

        if (applyNo == "") {
            alert("请输入要检索的专利号。");
            return;
        }

        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_SearchPAInfo',
                applyNo: applyNo
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var paInfo = responseData.Data;

                        for (var item in paInfo[0]) {
                            switch (item) {
                                case "title":
                                    $("input[name='paname']").val(paInfo[0][item]);
                                    break;
                                case "appDate":
                                    $("input[name='applydate']").val(paInfo[0][item]);
                                    break;
                                case "applicantName":
                                    $("input[name='currentobligee']").val(paInfo[0][item].join(","));
                                    break;
                                case "patType":
                                    var paType = window.patent.paType(paInfo[0][item]);

                                    $("select[name='patype']").val(paType);
                                    break;
                                case "pubNumber":
                                    $("input[name='publicnumber']").val(paInfo[0][item]);
                                    break;
                                case "pubDate":
                                    $("input[name='publicdate']").val(paInfo[0][item]);
                                    $("input[name='authorizedate']").val(paInfo[0][item]);
                                    break;
                                case "mainIpc":
                                    $("input[name='mainipc']").val(paInfo[0][item]);
                                    break;
                                case "applicantNameBGQ":
                                    $("input[name='originalapplicant']").val(paInfo[0][item].join(','));
                                    break;
                                case "inventroNameBGQ":
                                    $("input[name='originalinventor']").val(paInfo[0][item].join(','));
                                    break;
                                case "inventroName":
                                    $("input[name='currentinventor']").val(paInfo[0][item].join(','));
                                    break;
                                case "address":
                                    $("input[name='currentaddress']").val(paInfo[0][item]);
                                    break;
                                case "curAgentName":
                                    $("input[name='agent']").val(paInfo[0][item]);
                                    break;
                                case "curAgencyName":
                                    $("input[name='agency']").val(paInfo[0][item]);
                                    break;
                                case "abs":
                                    $("textarea[name='abstracts']").text(paInfo[0][item]);
                                    break;
                                default:
                                    break
                            }
                        }

                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //确认
    window.patent.confirm = function () {
        //1.校验必填项
        if (!verifydata("manualadd")) {
            return;
        }

        var paType = $("select[name='patype']").val();
        if (paType == "") {
            alert("请选择专利类型。");
            return;
        }

        var lawState = $("select[name='lawstate']").val();
        if (lawState == "") {
            alert("请选择法律状态。");
            return;
        }

        call_form({
            formid: 'manualadd',
            url: window.location.href,
            data: {
                action: 'M_ManualAddConfirm'
            },
            forceSync: true,//强制同步
            async: false,
            success: function (data) {
                var responseData = strToJson(data);
                if (responseData.Result === "0") {
                    alert("保存成功。");
                    parent.closeTab("addtab");
                } else {
                    alert(responseData.errMsg);
                }
            }
        });
    }

    //编辑
    window.patent.edit = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadData'
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var paData = responseData.Data.rows;
                        var lawData = responseData.LawInfo;

                        window.patent.loadPAInfo(paData);

                        window.patent.loadLawInfo(lawData);
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载专利信息
    window.patent.loadPAInfo = function (data) {
        for (var i = 0; i < data.length; i++) {
            $("input[name='applyno']").val(data[i]["ApplyNo"]);

            if (data[i]["ApplyDate"]) {
                $("input[name='applydate']").val(createDate(data[i]["ApplyDate"]).Format("yyyy-MM-dd"));
            }

            $("input[name='paname']").val(data[i]["PAName"]);
            $("select[name='patype']").val(data[i]["PAType"]);
            $("select[name='lawstate']").val(data[i]["LawState"]);
            $("input[name='publicnumber']").val(data[i]["PublicNumber"]);

            if (data[i]["PublicDate"]) {
                $("input[name='publicdate']").val(createDate(data[i]["PublicDate"]).Format("yyyy-MM-dd"));
            }

            $("input[name='mainipc']").val(data[i]["MainIPC"]);

            if (data[i]["AuthorizeDate"]) {
                $("input[name='authorizedate']").val(createDate(data[i]["AuthorizeDate"]).Format("yyyy-MM-dd"));
            }

            $("input[name='originalapplicant']").val(data[i]["OriginalApplicant"]);
            $("input[name='currentobligee']").val(data[i]["CurrentObligee"]);
            $("input[name='originalinventor']").val(data[i]["OriginalInventor"]);
            $("input[name='currentinventor']").val(data[i]["CurrentInventor"]);
            $("input[name='originaladdress']").val(data[i]["OriginalAddress"]);
            $("input[name='currentaddress']").val(data[i]["CurrentAddress"]);
            $("input[name='agent']").val(data[i]["Agent"]);
            $("input[name='agency']").val(data[i]["Agency"]);
            $("textarea[name='abstracts']").text(data[i]["Abstracts"]);
            $("#publishdate").text(createDate(data[i]["PublishDate"]).Format("yyyy-MM-dd"));
        }
    }

    //加载法律信息
    window.patent.loadLawInfo = function (data) {
        if (data.length > 0) {
            for (var item in data[0]) {
                switch (item) {
                    case "legalList":
                        var legalList = data[0][item];
                        var html = "<tr class='fblod'>";
                        html += "   <td>法律状态公告日</td>";
                        html += "   <td>法律状态</td>";
                        html += "   <td>法律状态信息</td>";
                        html += "</tr>";

                        if (legalList.length > 0) {
                            $("#leagallist").empty();

                            for (var i = 0; i < legalList.length; i++) {
                                html += "<tr>";
                                html += "   <td>" + legalList[i]["prsDate"] + "</td>";
                                html += "   <td>" + legalList[i]["prsCode"] + "</td>";

                                html += " <td>";
                                var expl = legalList[i]["codeExpl"].split('\n');
                                if (expl.length > 0) {
                                    for (var j = 0; j < expl.length; j++) {
                                        html += "<p>" + expl[j] + "</p>";
                                    }
                                }

                                html += "</td>";
                                html += "</tr>";
                            }

                            $("#leagallist").append(html);
                        }
                        break;
                    default:
                }
            }
        }
    }

    //确认编辑
    window.patent.confirmEdit = function () {
        //1.校验必填项
        if (!verifydata("edit")) {
            return;
        }

        var paType = $("select[name='patype']").val();
        if (paType == "") {
            alert("请选择专利类型。");
            return;
        }

        var lawState = $("select[name='lawstate']").val();
        if (lawState == "") {
            alert("请选择法律状态。");
            return;
        }

        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_ConfirmEdit',
                applyno: $("input[name='applyno']").val(),
                applydate: $("input[name='applydate']").val(),
                paname: $("input[name='paname']").val(),
                patype: $("select[name='patype']").val(),
                lawstate: $("select[name='lawstate']").val(),
                publicnumber: $("input[name='publicnumber']").val(),
                publicdate: $("input[name='publicdate']").val(),
                mainipc: $("input[name='mainipc']").val(),
                authorizedate: $("input[name='authorizedate']").val(),
                originalapplicant: $("input[name='originalapplicant']").val(),
                currentobligee: $("input[name='currentobligee']").val(),
                originalinventor: $("input[name='originalinventor']").val(),
                currentinventor: $("input[name='currentinventor']").val(),
                originaladdress: $("input[name='originaladdress']").val(),
                currentaddress: $("input[name='currentaddress']").val(),
                agent: $("input[name='agent']").val(),
                agency: $("input[name='agency']").val(),
                abstracts: $("textarea[name='abstracts']").text(),
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        alert("保存成功。");
                        parent.closeTab("edittab");
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //删除
    window.patent.deletePA = function (id) {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_Delete',
                id: id
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        alert("删除成功。");
                        window.patent.loadList();
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //详细
    window.patent.detail = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadData'
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var paData = responseData.Data.rows;
                        var lawData = responseData.LawInfo;

                        window.patent.loadPADetailInfo(paData);

                        window.patent.loadLawInfo(lawData);
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载专利详情信息
    window.patent.loadPADetailInfo = function (data) {
        for (var i = 0; i < data.length; i++) {
            $("#applyno").text(data[i]["ApplyNo"]);

            if (data[i]["ApplyDate"]) {
                $("#applydate").text(createDate(data[i]["ApplyDate"]).Format("yyyy-MM-dd"));
            }

            $("#paname").text(data[i]["PAName"]);
            $("#patype").text(data[i]["PAType"]);
            $("#lawstate").text(data[i]["LawState"]);
            $("#publicnumber").text(data[i]["PublicNumber"]);

            if (data[i]["PublicDate"]) {
                $("#publicdate").text(createDate(data[i]["PublicDate"]).Format("yyyy-MM-dd"));
            }

            $("#mainipc").text(data[i]["MainIPC"]);

            if (data[i]["AuthorizeDate"]) {
                $("#authorizedate").text(createDate(data[i]["AuthorizeDate"]).Format("yyyy-MM-dd"));
            }

            $("#originalapplicant").text(data[i]["OriginalApplicant"]);
            $("#currentobligee").text(data[i]["CurrentObligee"]);
            $("#originalinventor").text(data[i]["OriginalInventor"]);
            $("#currentinventor").text(data[i]["CurrentInventor"]);
            $("#originaladdress").text(data[i]["OriginalAddress"] == null ? "" : data[i]["OriginalAddress"]);
            $("#currentaddress").text(data[i]["CurrentAddress"]);
            $("#agent").text(data[i]["Agent"]);
            $("#agency").text(data[i]["Agency"]);
            $("#abstracts").text(data[i]["Abstracts"]);
            $("#publishdate").text(createDate(data[i]["PublishDate"]).Format("yyyy-MM-dd"));
        }
    }

    //加载法律信息
    window.patent.loadLawStateInfo = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: "M_LoadLawStateInfo"
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        $(".integralbox").css({ "display": "block" });
                        $("#lawInfo").attr("style", "display:inline-block").siblings().attr("style", "display:none");
                        setTimeout(function () { $(".integralbox").hide(); }, 1000);

                        var paInfo = responseData.PAInfo;
                        var lawState = responseData.LawState;

                        if (paInfo.length > 0) {
                            $("#lawstateapplyno").text(paInfo[0]["sqh"]);
                            $("#lawstatepaname").text(paInfo[0]["title"]);
                            $("#lawstateinventroname").text(paInfo[0]["inventroName"]);
                        }

                        if (lawState) {
                            $("#newlawstate").text(lawState["status"]);
                            $("#lawstateapplicant").text(lawState["applicant"]);

                            var agentinfo = lawState["agentinfo"];
                            if (agentinfo.length > 0) {
                                var arr = agentinfo.split(':');

                                $("#lawstateagentname").text(arr[1]);
                                $("#lawstateagency").text(arr[0]);
                            }
                        }

                        $(".p-pagebox").css({ "display": "block" });
                        $(".blackbox").css({ "display": "block" });
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载费用信息
    window.patent.loadCostInfo = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: "M_LoadCostInfo"
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        $(".integralbox").css({ "display": "block" });
                        $("#feeInfo").attr("style", "display:inline-block").siblings().attr("style", "display:none");
                        setTimeout(function () { $(".integralbox").hide(); }, 1000);

                        var paData = responseData.PAData.rows;
                        var payableData = responseData.PayableData.rows;
                        var payedData = responseData.PayedData.rows;
                        var lateFeeData = responseData.LateFeeData.rows;

                        //1.加载基础信息
                        window.patent.loadCostPAData(paData);

                        //2.加载未缴费信息
                        window.patent.loadPayableData(payableData);

                        //3.加载滞纳金信息
                        window.patent.loadLateFeeData(lateFeeData);

                        //4.加载已缴费信息
                        window.patent.loadPayedData(payedData);

                        $(".modifybox").css({ "display": "block" });
                        $(".blackbox").css({ "display": "block" });
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载费用的专利信息
    window.patent.loadCostPAData = function (data) {
        if (data.length > 0) {
            $("#costapplyno").text(data[0]["ApplyNo"]);
            $("#costpaname").text(data[0]["PAName"]);
            $("#costlawstate").text(data[0]["PAFLState"]);

            switch (data[0]["PAType"]) {
                case "发明专利":
                case "PCT发明":
                    $("#costpatype").text("发");
                    $("#costpatype").addClass("btn-red");
                    break;
                case "PCT实用新型":
                case "实用新型":
                    $("#costpatype").text("实");
                    $("#costpatype").addClass("btn-green");
                    break;
                default:
                    $("#costpatype").text("外");
                    $("#costpatype").addClass("btn-blue");
                    break;
            }
        }
    }

    //加载未缴费信息
    window.patent.loadPayableData = function (data) {
        if (data.length > 0) {
            var html = "";

            for (var i = 0; i < data.length; i++) {
                html += "<tr>";
                html += "   <td>" + data[i]["feetype"] + "</td>";
                html += "   <td>¥" + data[i]["feenumber"] + "</td>";
                html += "   <td>" + data[i]["feedate"] + "</td>";
                html += "   <td><span class='color-red'>未缴费</span></td>";
                html += "</tr>";
            }

            $("#payablelist").html("");
            $("#payablelist").append(html);
        }
    }

    //加载滞纳金信息
    window.patent.loadLateFeeData = function (data) {
        if (data.length > 0) {
            var html = "";

            for (var i = 0; i < data.length; i++) {
                html += "<tr>";
                html += "   <td>" + data[i]["feedate"] + "</td>";
                html += "   <td>¥" + data[i]["feenumber"] + "</td>";
                html += "   <td><span class='color-red'>¥" + data[i]["overpaynumber"] + "</span></td>";
                html += "   <td>¥" + data[i]["sumfee"] + "</td>";
                html += "</tr>";
            }

            $("#latefeelist").html("");
            $("#latefeelist").append(html);
        }
    }

    //加载已缴费信息
    window.patent.loadPayedData = function (data) {
        if (data.length > 0) {
            var html = "";

            for (var i = 0; i < data.length; i++) {
                html += "<tr>";
                html += "   <td>" + data[i]["feetype"] + "</td>";
                html += "   <td>¥" + data[i]["feenumber"] + "</td>";
                html += "   <td>" + data[i]["feedate"] + "</td>";
                html += "   <td>" + data[i]["payername"] + "</td>";
                html += "   <td>" + data[i]["receiptno"] + "</td>";
                html += "</tr>";
            }

            $("#payedlist").html("");
            $("#payedlist").append(html);
        }
    }
    /*******************************会员中心 我的专利  end *********************************/

    /*******************************会员中心 我的交易 begin*********************************/
    if (!window.transaction) {
        window.transaction = {};
    }

    //加载列表
    window.transaction.loadList = function () {
        //加载列表
        var option = {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "M_LoadList";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "id",
            height: $(".memberbox").height(),
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [{
                title: '专利名称',
                field: 'PAName',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '专利号',
                field: 'ApplyNo',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '法律状态',
                field: 'PAFLState',
                datatype: 'select',
                ext: 'like',
                suboption: [{
                    title: '发明已下证',
                    value: '发明已下证'
                }, {
                    title: '发明未缴费',
                    value: '发明未缴费'
                }, {
                    title: '新型已下证',
                    value: '新型已下证'
                }, {
                    title: '新型未缴费',
                    value: '新型未缴费'
                }, {
                    title: '有权-授权',
                    value: '有权-授权'
                }, {
                    title: '专利权维持',
                    value: '专利权维持'
                }, {
                    title: '等年费滞纳金',
                    value: '等年费滞纳金'
                }, {
                    title: '授权未交费',
                    value: '授权未交费'
                }, {
                    title: '无权-避重放弃',
                    value: '无权-避重放弃'
                }, {
                    title: '无权-视为放弃',
                    value: '无权-视为放弃'
                }, {
                    title: '无权-终止',
                    value: '无权-终止'
                }, {
                    title: '审中-公开',
                    value: '审中-公开'
                }, {
                    title: '审中-实审',
                    value: '审中-实审'
                }]
            }, {
                title: '交易状态',
                field: 'TranState',
                datatype: 'select',
                ext: 'like',
                suboption: [{
                    title: '发布',
                    value: '1'
                }, {
                    title: '已交易',
                    value: '2'
                }, {
                    title: '已取消',
                    value: '3'
                }]
            }, {
                title: '发布日期',
                field: 'PublishDate',
                datatype: 'datetime',
                ext: 'like'
            }],
            columns: [
                {
                    title: '_ckb_',
                    field: "",
                    sortable: false,
                    width: '30',
                    checkbox: true,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '',
                    field: 'ID',
                    visible: false,
                    align: 'center'
                }, {
                    title: '专利名称',
                    field: 'PAName',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            if (value.length > 20) {
                                return "<a class='color-blue' onclick='openDetail(" + row.ID + ");' title='" + value + "' href='javascript:void(0);'>" + value.substring(0, 20) + "..." + "</a>";
                            } else {
                                return "<a class='color-blue' onclick='openDetail(" + row.ID + ");' title='" + value + "' href='javascript:void(0);'>" + value + "</a>";
                            }

                        }

                        return "";
                    }
                }, {
                    title: '专利号',
                    field: "ApplyNo",
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '专利类型',
                    field: "PAType",
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '法律状态',
                    field: "PAFLState",
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '期望价格/元',
                    field: "ExpePrice",
                    sortable: false,
                    align: 'center',
                    valign: 'middle'

                }, {
                    title: '交易状态',
                    field: "TranState",
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            switch (value) {
                                case "1":
                                    return "发布";
                                    break;
                                case "2":
                                    return "已交易";
                                    break;
                                case "3":
                                    return "已取消";
                                    break;
                                case "4":
                                    return "合同签订中";
                                    break;
                                default:
                                    break;
                            }
                        }

                        return "";
                    }
                }, {
                    title: '发布日期',
                    field: 'PublishDate',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            return createDate(value).Format('yyyy-MM-dd');
                        } else {
                            return '';
                        }
                    }
                }
            ]
        };

        Table.loadTable($("#table"), option);
    }

    //批量发布
    window.transaction.batchPublish = function () {
        var filePath = $("input[name='fileinp']").val();

        if (filePath == "") {
            alert("请上传(.xlsx)格式的文件。");
            return;
        }

        if (filePath.substring(filePath.lastIndexOf("\.") + 1, filePath.length) != "xlsx") {
            alert("文件类型错误，请上传(.xlsx)格式的文件。");
            return;
        }

        call_form({
            formid: "BatchPublish",
            forceSync: true,//强制同步
            async: false,
            success: function (data) {
                var responseData = strToJson(data);
                if (responseData.Result === "0") {
                    alert(responseData.errMsg);
                    $(".addbox").css({ "display": "none" });
                    $(".blackbox").css({ "display": "none" });
                    window.transaction.loadList();
                } else {
                    alert(responseData.errMsg);
                }
            }
        });
    }

    //删除
    window.transaction.delete = function () {
        var Id = Table.getIdSelections($("#table"));

        if (Id == "" || Id.length <= 0) {
            alert("没有选中任何数据。");
            return;
        }

        var ID = Table.getIdSelectionsString($("#table"), "ID");

        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_Delete',
                strid: ID
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        alert("删除成功。");
                        $(".popbox").css({ "display": "none" });
                        $(".blackbox").css({ "display": "none" });
                        window.transaction.loadList();
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //检索
    window.transaction.search = function () {
        var applyNo = $.trim($("input[name='ApplyNo']").val());

        //1.校验参数
        if (!applyNo) {
            alert("请输入专利号。");
            return;
        }
        if (!CheckSQH(applyNo)) {
            alert("输入的专利号格式不正确。");
            return;
        }

        //2.根据专利号获取数据
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_SearchPA',
                applyNO: applyNo
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        //1.获取数据
                        var data = responseData.Data.rows;

                        //2.绑定数据
                        if (data.length > 0) {
                            for (var item in data[0]) {
                                if (data[0][item]) {
                                    switch (item) {
                                        case 'title':
                                            $("input[name='PAName']").val(data[0][item]);
                                            break;
                                        case 'sqh':
                                            $("input[name='ApplyNo']").val(data[0][item]);
                                            break;
                                        case 'appDate':
                                            $("input[name='ShenQingRi']").val(data[0][item]);
                                            break;
                                        case 'pubDate':
                                            $("input[name='ShouQuanGongGaoRi']").val(data[0][item]);
                                            break;
                                        case 'abs':
                                            $("textarea[name='Description']").val(data[0][item]);
                                            break;
                                        case 'industryP':
                                            var o = $("select[name='IndustryP']").find("[value='" + data[0][item] + "']");
                                            if (o.length != 0) {
                                                o.attr("selected", true);
                                                window.transaction.bandIndustry($("select[name='IndustryP']"), 'seleTwo');
                                            }
                                            break;
                                        case 'industryC':
                                            var o = $("select[name='IndustryC']").find("[value='" + data[0][item] + "']");
                                            if (o.length != 0) {
                                                o.attr("selected", true);
                                            }
                                            break;
                                        case 'sqr':
                                            $("input[name='ShenQingRen']").val(data[0][item]);
                                            break;
                                        case 'patType':
                                            var patype = getpatype(data[0][item]);
                                            var o = $("select[name='paType']").find("[value='" + patype + "']");
                                            if (o.length === 0) {
                                                $("select[name='paType']").find("[value=0]").attr('selected', true);
                                            } else {
                                                o.attr("selected", true);
                                            }
                                            break;
                                        case 'mainIpc':
                                            $("input[name='IPC']").val(data[0][item]);
                                            break;
                                        case 'PAImage':
                                            $("#img_pa").attr('src', data[0][item].replace("~/", searchengine));
                                            $("input[name='IsUploadPAImage']").val('N');
                                            $("input[name='image']").val(data[0][item]);
                                            break;
                                    }
                                }
                            }
                        } else {
                            alert("没有获取到您输入的专利号对应的专利信息。");
                        }
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //绑定行业领域
    window.transaction.bandIndustry = function (c, id) {
        var parentID = '';

        if (c) {
            parentID = $(c).find("option:selected").attr("key");
        } else {
            parentID = "0";
        }

        $("#" + id + " option").remove();

        if (parentID != "") {
            call_ajax({
                url: getRootPath() + "/Handler/Handler.ashx",
                data: {
                    action: "p_loadIndustry",
                    pid: parentID
                },
                async: false,
                success: function (data) {
                    var responseData = strToJson(data);

                    if (responseData) {
                        if (responseData.Result === "0") {
                            var mainData = responseData.Data.rows;
                            $("#" + id).append("<option key='' value=''>-请选择-</option>");
                            for (var i = 0; i < mainData.length; i++) {
                                var html = "";
                                if (parentID != "0") {
                                    html += "<option BH='" + mainData[i]["Code"] + "' key='" + mainData[i]["ID"] + "' value='" + mainData[i]["Text"] + "'>" + mainData[i]["Text"] + "</option>";
                                } else {
                                    html += "<option BH='" + mainData[i]["Code"] + "' key='" + mainData[i]["ID"] + "' value='" + mainData[i]["Text"] + "'>" + mainData[i]["Text"] + "</option>";
                                }

                                $("#" + id).append(html);
                            }
                        } else {
                            alert(responseData.errMsg);
                        }
                    }
                }
            });
        }
    }

    //发布交易确认
    window.transaction.confirm = function () {
        //1.校验页面参数
        if (!verifydata()) {
            return;
        }

        var paType = $("select[name='paType']").val();
        if (paType == "") {
            alert("请选择专利类型。");
            return;
        }

        var PAFLState = $("select[name='PAFLState']").val();
        if (PAFLState == "") {
            alert("请选择法律状态。");
            return;
        }

        var industryP = $("select[name='IndustryP']").val();
        if (industryP == "") {
            alert("请选择行业领域大类。");
            return;
        }

        var industryC = $("select[name='IndustryC']").val();
        if (industryC == "") {
            alert("请选择行业领域小类。");
            return;
        }

        //2.处理输入的文本
        ProcessingTag();

        //3.提交
        call_form({
            formid: "form_pa",
            url: window.location.href,
            data: {
                action: "M_Confirm"
            },
            forceSync: true,//强制同步
            async: false,
            success: function (data) {
                var responseData = strToJson(data);
                if (responseData.Result === "0") {
                    alert("保存成功。");
                    $("a[role='cancel']").click();
                } else {
                    alert(responseData.errMsg);
                }
            }
        });
    }

    //加载编辑专利交易信息
    window.transaction.loadPAInfo = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadData'
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var paData = responseData.Data.rows;
                        var lawData = responseData.LawData;
                        window.transaction.loadPAData(paData);
                        window.transaction.loadLawData(lawData);
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载编辑专利信息
    window.transaction.loadPAData = function (data) {
        for (var i = 0; i < data.length; i++) {
            $("input[name='applyno']").val(data[i]["ApplyNo"]);

            if (data[i]["ShenQingRi"]) {
                $("input[name='applydate']").val(createDate(data[i]["ShenQingRi"]).Format("yyyy-MM-dd"));
            }

            $("input[name='paname']").val(data[i]["PAName"]);
            $("input[name='patype']").val(data[i]["PAType"]);
            $("input[name='applicant']").val(data[i]["ShenQingRen"]);
            $("input[name='lawstate']").val(data[i]["PAFLState"]);
            $("input[name='ipc']").val(data[i]["IPC"]);
            $("input[name='industryp']").val(data[i]["IndustryP"] + "-" + data[i]["IndustryC"]);

            if (data[i]["ShouQuanGongGaoRi"]) {
                $("input[name='publicdate']").val(createDate(data[i]["ShouQuanGongGaoRi"]).Format("yyyy-MM-dd"));
            }

            if (data[i]["YouXiaoQiXian"]) {
                $("input[name='term']").val(createDate(data[i]["YouXiaoQiXian"]).Format("yyyy-MM-dd"));
            }

            if (data[i]["HuiFuQi"] == "Y") {
                $("input[name='recoverytime']").attr("checked", "checked");
            }

            if (data[i]["PAImage"]) {
                var imgPath = data[i]["PAImage"].replace("~/", "").substring(0, 4);
                if (imgPath == "Data") {
                    $("#paimg").attr("src", getRootPath() + data[i]["PAImage"].replace("~/", "/"));
                } else {
                    $("#paimg").attr("src", data[i]["PAImage"].replace("~/", searchengine));
                }
            }

            $("textarea[name='abs']").val(data[i]["Description"]);
            $("input[name='keywords']").val(data[i]["KeyWords"]);
            $("select[name='tranprice']").val(data[i]["TranPrice"]);
            $("#tranprice").text(data[i]["TranPrice"]);
            $("select[name='trantype']").val(data[i]["TranType"]);
            $("#trantype").text(data[i]["TranType"]);
            $("input[name='expeprice']").val(data[i]["ExpePrice"]);

            if (data[i]["IsShowExpePrice"] == "Y") {
                $("input[name='isshowexpeprice']").val("Y");
                $("input[name='isshowexpeprice']").attr("checked", "checked");
            }

            $("input[name='remark']").val(data[i]["Remark"]);
            $("#publishdate").text(createDate(data[i]["PublishDate"]).Format("yyyy-MM-dd"));
        }
    }

    //加载编辑专利法律信息
    window.transaction.loadLawData = function (data) {
        if (data.length > 0) {
            for (var item in data[0]) {
                switch (item) {
                    case "legalList":
                        var legalList = data[0][item];
                        var html = "<tr class='fblod'>";
                        html += "   <td>法律状态公告日</td>";
                        html += "   <td>法律状态</td>";
                        html += "   <td>法律状态信息</td>";
                        html += "</tr>";

                        if (legalList.length > 0) {
                            $("#leagallist").empty();

                            for (var i = 0; i < legalList.length; i++) {
                                html += "<tr>";
                                html += "   <td>" + legalList[i]["prsDate"] + "</td>";
                                html += "   <td>" + legalList[i]["prsCode"] + "</td>";

                                html += " <td>";
                                var expl = legalList[i]["codeExpl"].split('\n');
                                if (expl.length > 0) {
                                    for (var j = 0; j < expl.length; j++) {
                                        html += "<p>" + expl[j] + "</p>";
                                    }
                                }

                                html += "</td>";
                                html += "</tr>";
                            }

                            $("#leagallist").append(html);
                        }
                        break;
                    default:
                }
            }
        }
    }

    //编辑专利信息确认
    window.transaction.editPAInfoConfirm = function (id) {
        if (!verifydata()) {
            return;
        }

        var keyWords = $("input[name='keywords']").val();
        var tranPrice = $("select[name='tranprice']").val();
        var tranType = $("select[name='trantype']").val();
        var expePrice = $("input[name='expeprice']").val();
        var isShowExpePrice = $("input[name='isshowexpeprice']").val();
        var remark = $("input[name='remark']").val();

        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_Confirm',
                keyWords: keyWords,
                tranPrice: tranPrice,
                tranType: tranType,
                expePrice: expePrice,
                isShowExpePrice: isShowExpePrice,
                remark: remark
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        alert("保存成功。");
                        $("a[role='cancel']").click();
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载法律信息
    window.patent.loadLawStateInfo = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: "M_LoadLawStateInfo"
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        $(".integralbox").css({ "display": "block" });
                        $("#lawInfo").attr("style", "display:inline-block").siblings().attr("style", "display:none");
                        setTimeout(function () { $(".integralbox").hide(); }, 1000);

                        var paInfo = responseData.PAInfo;
                        var lawState = responseData.LawState;

                        if (paInfo.length > 0) {
                            $("#lawstateapplyno").text(paInfo[0]["sqh"]);
                            $("#lawstatepaname").text(paInfo[0]["title"]);
                            $("#lawstateinventroname").text(paInfo[0]["inventroName"]);
                        }

                        if (lawState) {
                            $("#newlawstate").text(lawState["status"]);
                            $("#lawstateapplicant").text(lawState["applicant"]);

                            var agentinfo = lawState["agentinfo"];
                            if (agentinfo.length > 0) {
                                var arr = agentinfo.split(':');

                                $("#lawstateagentname").text(arr[1]);
                                $("#lawstateagency").text(arr[0]);
                            }
                        }

                        $(".p-pagebox").css({ "display": "block" });
                        $(".blackbox").css({ "display": "block" });
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载费用信息
    window.patent.loadCostInfo = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: "M_LoadCostInfo"
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        $(".integralbox").css({ "display": "block" });
                        $("#feeInfo").attr("style", "display:inline-block").siblings().attr("style", "display:none");
                        setTimeout(function () { $(".integralbox").hide(); }, 1000);

                        var paData = responseData.PAData.rows;
                        var payableData = responseData.PayableData.rows;
                        var payedData = responseData.PayedData.rows;
                        var lateFeeData = responseData.LateFeeData.rows;

                        //1.加载基础信息
                        window.patent.loadCostPAData(paData);

                        //2.加载未缴费信息
                        window.patent.loadPayableData(payableData);

                        //3.加载滞纳金信息
                        window.patent.loadLateFeeData(lateFeeData);

                        //4.加载已缴费信息
                        window.patent.loadPayedData(payedData);

                        $(".modifybox").css({ "display": "block" });
                        $(".blackbox").css({ "display": "block" });
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载费用的专利信息
    window.patent.loadCostPAData = function (data) {
        if (data.length > 0) {
            $("#costapplyno").text(data[0]["ApplyNo"]);
            $("#costpaname").text(data[0]["PAName"]);
            $("#costlawstate").text(data[0]["PAFLState"]);

            switch (data[0]["PAType"]) {
                case "发明专利":
                case "PCT发明":
                    $("#costpatype").text("发");
                    $("#costpatype").addClass("btn-red");
                    break;
                case "PCT实用新型":
                case "实用新型":
                    $("#costpatype").text("实");
                    $("#costpatype").addClass("btn-green");
                    break;
                default:
                    $("#costpatype").text("外");
                    $("#costpatype").addClass("btn-blue");
                    break;
            }
        }
    }

    //加载未缴费信息
    window.patent.loadPayableData = function (data) {
        if (data.length > 0) {
            var html = "";

            for (var i = 0; i < data.length; i++) {
                html += "<tr>";
                html += "   <td>" + data[i]["feetype"] + "</td>";
                html += "   <td>¥" + data[i]["feenumber"] + "</td>";
                html += "   <td>" + data[i]["feedate"] + "</td>";
                html += "   <td><span class='color-red'>未缴费</span></td>";
                html += "</tr>";
            }

            $("#payablelist").html("");
            $("#payablelist").append(html);
        }
    }

    //加载滞纳金信息
    window.patent.loadLateFeeData = function (data) {
        if (data.length > 0) {
            var html = "";

            for (var i = 0; i < data.length; i++) {
                html += "<tr>";
                html += "   <td>" + data[i]["feedate"] + "</td>";
                html += "   <td>¥" + data[i]["feenumber"] + "</td>";
                html += "   <td><span class='color-red'>¥" + data[i]["overpaynumber"] + "</span></td>";
                html += "   <td>¥" + data[i]["sumfee"] + "</td>";
                html += "</tr>";
            }

            $("#latefeelist").html("");
            $("#latefeelist").append(html);
        }
    }

    //加载已缴费信息
    window.patent.loadPayedData = function (data) {
        if (data.length > 0) {
            var html = "";

            for (var i = 0; i < data.length; i++) {
                html += "<tr>";
                html += "   <td>" + data[i]["feetype"] + "</td>";
                html += "   <td>¥" + data[i]["feenumber"] + "</td>";
                html += "   <td>" + data[i]["feedate"] + "</td>";
                html += "   <td>" + data[i]["payername"] + "</td>";
                html += "   <td>" + data[i]["receiptno"] + "</td>";
                html += "</tr>";
            }

            $("#payedlist").html("");
            $("#payedlist").append(html);
        }
    }
    /*******************************会员中心 我的交易  end *********************************/

    /*******************************会员中心 我的需求 begin*********************************/
    if (!window.demand) {
        window.demand = {};
    }

    //加载列表
    window.demand.loadList = function () {
        //加载列表
        var option = {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "M_LoadList";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "id",
            height: $(".memberbox").height(),
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [{
                title: '需求名称',
                field: 'SEQName',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '行业领域',
                field: 'IndustryP',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '交易方式',
                field: 'TranType',
                datatype: 'select',
                ext: 'like',
                suboption: [{
                    title: '转让',
                    value: '转让'
                }, {
                    title: '许可',
                    value: '许可'
                }, {
                    title: '合作入股',
                    value: '合作入股'
                }]
            }, {
                title: '发布日期',
                field: 'PublishDate',
                datatype: 'datetime',
                ext: 'like'
            }],
            columns: [
                {
                    title: '_ckb_',
                    field: "",
                    sortable: false,
                    width: '30',
                    checkbox: true,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '',
                    field: 'ID',
                    visible: false,
                    align: 'center'
                }, {
                    title: '需求名称',
                    field: 'SEQName',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            if (value.length > 20) {
                                return "<a class='color-blue' onclick='detail(" + row.ID + ");' href='javascript:void(0);' title='" + value + "'>" + value.substring(0, 20) + "..." + "</a>";
                            } else {
                                return "<a class='color-blue' onclick='detail(" + row.ID + ");' href='javascript:void(0);' title='" + value + "'>" + value + "</a>";
                            }
                        } else {
                            return "";
                        }
                    }
                }, {
                    title: '行业领域',
                    field: "IndustryP",
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            return value + "-" + row.IndustryC;
                        } else {
                            return "";
                        }
                    }
                }, {
                    title: '交易方式',
                    field: "TranType",
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '发布日期',
                    field: 'PublishDate',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            return createDate(value).Format('yyyy-MM-dd');
                        } else {
                            return '';
                        }
                    }
                }, {
                    title: '投资预算',
                    field: "Budget",
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '联系人',
                    field: "LinkMan",
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '联系方式',
                    field: "LinkMobile",
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }
            ]
        };

        Table.loadTable($("#table"), option);
    }

    //绑定行业领域
    window.demand.bandIndustry = function (c, id) {
        var parentID = '';

        if (c) {
            parentID = $(c).find("option:selected").attr("key");
        } else {
            parentID = "0";
        }

        $("#" + id + " option").remove();

        if (parentID != "") {
            call_ajax({
                url: getRootPath() + "/Handler/Handler.ashx",
                data: {
                    action: "p_loadIndustry",
                    pid: parentID
                },
                async: false,
                success: function (data) {
                    var responseData = strToJson(data);

                    if (responseData) {
                        if (responseData.Result === "0") {
                            var mainData = responseData.Data.rows;
                            $("#" + id).append("<option key='' value=''>-请选择-</option>");
                            for (var i = 0; i < mainData.length; i++) {
                                var html = "";
                                if (parentID != "0") {
                                    html += "<option BH='" + mainData[i]["Code"] + "' key='" + mainData[i]["ID"] + "' value='" + mainData[i]["Text"] + "'>" + mainData[i]["Text"] + "</option>";
                                } else {
                                    html += "<option BH='" + mainData[i]["Code"] + "' key='" + mainData[i]["ID"] + "' value='" + mainData[i]["Text"] + "'>" + mainData[i]["Text"] + "</option>";
                                }

                                $("#" + id).append(html);
                            }
                        } else {
                            alert(responseData.errMsg);
                        }
                    }
                }
            });
        }
    }

    //发布需求确认
    window.demand.publishDemandConfirm = function () {
        //1.校验必填项
        if (!verifydata("demandform")) {
            return;
        }

        var industryP = $("select[name='industryp']").val();
        if (industryP == "") {
            alert("请选择行业领域大类。");
            return;
        }

        var industryC = $("select[name='industryc']").val();
        if (industryC == "") {
            alert("请选择行业领域小类。");
            return;
        }

        var lawState = $("select[name='lawstate']").val();
        if (lawState == "") {
            alert("请选择法律状态。");
            return;
        }

        var tranType = $("select[name='trantype']").val();
        if (tranType == "") {
            alert("请选择交易方式。");
            return;
        }

        var budget = $("select[name='budget']").val();
        if (budget == "") {
            alert("请选择投资预算。");
            return;
        }

        //2.表单提交
        call_form({
            formid: 'demandform',
            url: window.location.href,
            data: {
                action: 'M_Confirm'
            },
            forceSync: true,//强制同步
            async: false,
            success: function (data) {
                var responseData = strToJson(data);
                if (responseData.Result === "0") {
                    alert("保存成功。");
                    $("a[role='cancel']").click();
                } else {
                    alert(responseData.errMsg);
                }
            }
        });
    }

    //加载编辑数据
    window.demand.loadEditData = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadEditData'
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var demandData = responseData.DemandData.rows;
                        var linkManData = responseData.LinkManData.rows;

                        window.demand.loadDemandInfo(demandData);
                        window.demand.loadLinkMan(linkManData);

                        //禁用输入框
                        $("input[name='seqname']").attr("disabled", "disabled");
                        $("select[name='industryp']").attr("disabled", "disabled");
                        $("select[name='industryc']").attr("disabled", "disabled");
                        $("select[name='lawstate']").attr("disabled", "disabled");
                        $("select[name='trantype']").attr("disabled", "disabled");
                        $("select[name='budget']").attr("disabled", "disabled");
                        $("input[name='expeprice']").attr("disabled", "disabled");
                        $("input[name='daterequest']").attr("disabled", "disabled");
                        $("textarea[name='description']").attr("disabled", "disabled");
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载编辑、详细页面的需求信息
    window.demand.loadDemandInfo = function (data) {
        for (var i = 0; i < data.length; i++) {
            $("input[name='seqname']").val(data[i]["SEQName"]);

            $("select[name='industryp']").append("<option value='" + data[i]["IndustryP"] + "'>" + data[i]["IndustryP"] + "</option>");
            $("select[name='industryp']").val(data[i]["IndustryP"]);

            $("select[name='industryc']").append("<option value='" + data[i]["IndustryC"] + "'>" + data[i]["IndustryC"] + "</option>");
            $("select[name='industryc']").val(data[i]["IndustryC"]);

            $("select[name='lawstate']").val(data[i]["PAFLState"]);
            $("select[name='trantype']").val(data[i]["TranType"]);
            $("select[name='budget']").val(data[i]["Budget"]);
            $("input[name='expeprice']").val(data[i]["ExpePrice"]);

            if (data[i]["DateRequest"]) {
                $("input[name='daterequest']").val(createDate(data[i]["DateRequest"]).Format("yyyy-MM-dd"));
            }

            $("textarea[name='remark']").val(data[i]["Remark"]);
            $("textarea[name='description']").val(data[i]["Description"]);
            $("#publishdate").text(createDate(data[i]["PublishDate"]).Format("yyyy-MM-dd"));
        }
    }

    //加载编辑、详细页面的联系人信息
    window.demand.loadLinkMan = function (data) {
        for (var i = 0; i < data.length; i++) {
            $("input[name='linkmanid']").val(data[i]["ID"]);
            $("input[name='linkman']").val(data[i]["LinkMan"]);
            $("input[name='companyname']").val(data[i]["CompanyName"]);
            $("input[name='linkmobile']").val(data[i]["LinkMobile"]);
            $("input[name='linkqq']").val(data[i]["LinkQQ"]);
            $("input[name='linkemail']").val(data[i]["LinkEmail"]);
        }
    }

    //删除
    window.demand.delete = function () {
        var Id = Table.getIdSelections($("#table"));

        if (Id == "" || Id.length <= 0) {
            alert("没有选中任何数据。");
            return;
        }

        var id = Table.getIdSelectionsString($("#table"), "ID");

        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_Delete',
                strid: id
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        window.demand.loadList();
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载详细数据
    window.demand.loadDetailData = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadData'
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var demandData = responseData.DemandData.rows;
                        var linkManData = responseData.LinkManData.rows;

                        window.demand.loadDemandInfo(demandData);
                        window.demand.loadLinkMan(linkManData);
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }
    /*******************************会员中心 我的需求  end *********************************/

    /*******************************会员中心 我的财务 begin*********************************/
    if (!window.finance) {
        window.finance = {};
    }

    //-----------财务明细 begin--------------//
    //加载财务明细数据
    window.finance.loadDetailsData = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadDetailsData'
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var payAmnout = responseData.Pay.rows;
                        $("#payamount").text(payAmnout[0]["Pay"] == null ? 0 : payAmnout[0]["Pay"]);
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载财务明细列表
    window.finance.loadDetailsList = function () {
        var option = {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "M_LoadList";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "id",
            height: $(".memberbox").height(),
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [{
                title: '订单信息',
                field: 'Title',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '开票状态',
                field: 'InvoiceState',
                datatype: 'select',
                ext: 'like',
                suboption: [{
                    title: '未开票',
                    value: '0'
                }, {
                    title: '申请中',
                    value: '1'
                }, {
                    title: '已开票',
                    value: '2'
                }]
            }, {
                title: '付款日期',
                field: 'PayDate',
                datatype: 'datetime',
                ext: 'like'
            }],
            columns: [
                {
                    title: '_ckb_',
                    field: "",
                    sortable: false,
                    width: '30',
                    checkbox: true,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '',
                    field: 'ID',
                    visible: false,
                    align: 'center'
                }, {
                    title: '订单编号',
                    field: 'No',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '订单信息',
                    field: "Title",
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            if (value.length > 25) {
                                return "<a href='javascript:void(0);' title='" + value + "'>" + value.substring(0, 25) + "..." + "</a>";
                            } else {
                                return "<a href='javascript:void(0);' title='" + value + "'>" + value + "</a>";
                            }
                        } else {
                            return "";
                        }
                    }
                }, {
                    title: '费用合计/元',
                    field: "Amount",
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '付款日期',
                    field: 'PayDate',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            return createDate(value).Format('yyyy-MM-dd');
                        } else {
                            return '';
                        }
                    }
                }, {
                    title: '开票状态',
                    field: "InvoiceState",
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            switch (value) {
                                case "0":
                                    return "未开票";
                                    break;
                                case "1":
                                    return '正在申请';
                                    break;
                                case "2":
                                    return '已开票';
                                    break;
                                case "3":
                                    return '已作废';
                                    break;
                                default:
                            }
                        } else {
                            return "";
                        }
                    }
                }, {
                    title: '操作',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        var html = "<a class='tablebtn btn-blue' href='javascript:void(0);' onclick='openDetail(" + row.ID + ");'>详情</a>";
                        html += "<a class='tablebtn btn-green' href='member-finance-receipt?id=" + row.ID + "&type=myfeedetail' target='_blank'>电子收据</a>";

                        return html;

                    }
                }
            ]
        };

        Table.loadTable($("#table"), option);
    }

    //加载详情数据
    window.finance.loadDetailData = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadData'
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var detailData = responseData.Data.rows;

                        if (detailData) {
                            for (var i = 0; i < detailData.length; i++) {
                                $("#no").text(detailData[i]["No"]);

                                if (detailData[i]["CreateTime"]) {
                                    $("#createtime").text(createDate(detailData[i]["CreateTime"]).Format("yyyy-MM-dd"));
                                }

                                $("#paname").text(detailData[i]["PAName"]);

                                var url = "market-detail-" + detailData[i]["FK_ID"] + "-" + detailData[i]["ApplyNo"].replace(".", "");
                                $("#paname").attr("href", url);

                                $("#title").text(detailData[i]["Title"]);
                                $("#transactionprice").text(detailData[i]["DaiLiFei"]);
                                $("#officialfee").text(detailData[i]["GuanFei"]);

                                if (detailData[i]["IsTax"] == "Y") {
                                    $("#tax").attr("checked", "checked");
                                } else {
                                    $("#notax").attr("checked", "checked");
                                }

                                $("#amount").text(detailData[i]["Amount"]);
                                $("#remark").text(detailData[i]["Remark"]);
                                $("#receiptaccount").text(detailData[i]["ReceiptAccount"] == null ? "" : detailData[i]["ReceiptAccount"]);
                                $("#payman").text(detailData[i]["PayMan"] == null ? "" : detailData[i]["PayMan"]);

                                if (detailData[i]["PayDate"]) {
                                    $("#paydate").text(createDate(detailData[i]["PayDate"]).Format("yyyy-MM-dd"));
                                }
                            }
                        }
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }
    //-----------财务明细  end --------------//

    //-----------发票管理 begin--------------//
    //加载发票管理可申请发票数据
    window.finance.loadApplyInvoiceData = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_loadApplyInvoiceData'
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var invoiceData = responseData.Data.rows;
                        if (invoiceData) {
                            $("#applyinvoicenumber").text(invoiceData[0]["Number"]);
                            $("#applyinvoiceamount").text(invoiceData[0]["Amount"] == null ? 0 : invoiceData[0]["Amount"]);
                        }
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载发票管理列表
    window.finance.loadInvoiceList = function (type) {
        var option = "";
        switch (type) {
            case "available":
                option = window.finance.loadAvailableList();
                break;
            case "application":
                option = window.finance.loadApplicationList();
                break;
            case "hasavailable":
                option = window.finance.loadHasAvailableList();
                break;
        }

        $(".pagetablebox").html("");
        $(".pagetablebox").append("<table class='pagetable' id='table'></table>");
        Table.loadTable($("#table"), option);
    }

    //加载可开具发票的订单列表
    window.finance.loadAvailableList = function () {
        return {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "M_LoadAvailableList";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "id",
            height: 400,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [{
                title: '订单信息',
                field: 'Title',
                datatype: 'string',
                ext: 'like'
            }],
            columns: [{
                title: '_ckb_',
                field: "",
                sortable: false,
                width: '30',
                checkbox: true,
                align: 'center',
                valign: 'middle'
            }, {
                title: '',
                field: 'ID',
                visible: false,
                align: 'center'
            }, {
                title: '订单编号',
                field: 'No',
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '订单信息',
                field: "Title",
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        if (value.length > 26) {
                            return "<a class='color-blue' href='javascript:void(0);' onclick='detail(" + row.ID + ");' title='" + value + "'>" + value.substring(0, 26) + "..." + "</a>";
                        } else {
                            return "<a class='color-blue' href='javascript:void(0);' onclick='detail(" + row.ID + ");' title='" + value + "'>" + value + "</a>";
                        }
                    } else {
                        return "";
                    }
                }
            }, {
                title: '费用合计/元',
                field: "Amount",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '订单状态',
                field: "State",
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        switch (value) {
                            case "1":
                                return '未付款';
                                break;
                            case "2":
                                return '已付款';
                                break;
                            case "3":
                                return '已取消';
                                break;
                            default:
                        }
                    } else {
                        return "";
                    }
                }
            }, {
                title: '付款日期',
                field: 'PayDate',
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        return createDate(value).Format('yyyy-MM-dd');
                    } else {
                        return '';
                    }
                }
            }, {
                title: '操作',
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    var html = "<a class='tablebtn btn-blue' href='javascript:void(0);' onclick='applyInvoice(" + row.ID + ");'>申请发票</a>";
                    html += "<a class='tablebtn btn-green' href='member-finance-receipt?id=" + row.ID + "&type=order' target='_blank'>电子收据</a>";

                    return html;
                }
            }]
        };
    }

    //加载申请中的订单列表
    window.finance.loadApplicationList = function () {
        return {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "M_LoadApplicationList";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "id",
            height: 400,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [{
                title: '订单信息',
                field: 'CaseName',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '发票人',
                field: 'InvoicePerson',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '开票时间',
                field: 'CreateTime',
                datatype: 'datetime',
                ext: 'like'
            }],
            columns: [{
                title: '_ckb_',
                field: "",
                sortable: false,
                width: '30',
                checkbox: true,
                align: 'center',
                valign: 'middle'
            }, {
                title: '',
                field: 'ID',
                visible: false,
                align: 'center'
            }, {
                title: '订单编号',
                field: 'OrderNo',
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '订单信息',
                field: "CaseName",
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        if (value.length > 26) {
                            return "<a class='color-blue' href='javascript:void(0);' onclick='detail(" + row.MyOrderID + ");' title='" + value + "'>" + value.substring(0, 26) + "..." + "</a>";
                        } else {
                            return "<a class='color-blue' href='javascript:void(0);' onclick='detail(" + row.MyOrderID + ");' title='" + value + "'>" + value + "</a>";
                        }
                    } else {
                        return "";
                    }
                }
            }, {
                title: '发票人',
                field: "InvoicePerson",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '费用合计/元',
                field: "ExpensesAmount",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '发票说明',
                field: "Remark",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '开票状态',
                field: "OpenInvoiceState",
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        switch (value) {
                            case "0":
                                return "未开票";
                                break;
                            case "1":
                                return '正在申请';
                                break;
                            case "2":
                                return '已开票';
                                break;
                            case "3":
                                return '已作废';
                                break;
                            default:
                                break;
                        }
                    } else {
                        return "";
                    }
                }
            }, {
                title: '开票时间',
                field: 'CreateTime',
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        return createDate(value).Format('yyyy-MM-dd');
                    } else {
                        return '';
                    }
                }
            }, {
                title: '操作',
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    return "<a class='tablebtn btn-green' href='member-finance-receipt?id=" + row.MyOrderID + "&type=order' target='_blank'>电子收据</a>";
                }
            }]
        };
    }

    //加载已开具的订单列表
    window.finance.loadHasAvailableList = function () {
        return {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "M_LoadHasAvailableList";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "id",
            height: 400,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [{
                title: '订单信息',
                field: 'CaseName',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '发票人',
                field: 'InvoicePerson',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '开票时间',
                field: 'CreateTime',
                datatype: 'datetime',
                ext: 'like'
            }],
            columns: [{
                title: '_ckb_',
                field: "",
                sortable: false,
                width: '30',
                checkbox: true,
                align: 'center',
                valign: 'middle'
            }, {
                title: '',
                field: 'ID',
                visible: false,
                align: 'center'
            }, {
                title: '订单编号',
                field: 'OrderNo',
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '订单信息',
                field: "CaseName",
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        if (value.length > 26) {
                            return "<a class='color-blue' href='javascript:void(0);' onclick='detail(" + row.OrderID + ");' title='" + value + "'>" + value.substring(0, 26) + "..." + "</a>";
                        } else {
                            return "<a class='color-blue' href='javascript:void(0);' onclick='detail(" + row.OrderID + ");' title='" + value + "'>" + value + "</a>";
                        }
                    } else {
                        return "";
                    }
                }
            }, {
                title: '发票人',
                field: "InvoicePerson",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '费用合计/元',
                field: "ExpensesAmount",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '发票说明',
                field: "Remark",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '开票状态',
                field: "InvoiceState",
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        switch (value) {
                            case "0":
                                return "未开票";
                                break;
                            case "1":
                                return '正在申请';
                                break;
                            case "2":
                                return '已开票';
                                break;
                            case "3":
                                return '已作废';
                                break;
                            default:
                                break;
                        }
                    } else {
                        return "";
                    }
                }
            }, {
                title: '开票时间',
                field: 'InvoiceRegTime',
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        return createDate(value).Format('yyyy-MM-dd');
                    } else {
                        return '';
                    }
                }
            }, {
                title: '操作',
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    return "<a class='tablebtn btn-green' href='member-finance-receipt?id=" + row.ID + "&type=invoice' target='_blank'>电子收据</a>";
                }
            }]
        };
    }

    //加载发票信息维护列表
    window.finance.loadMaintainList = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadMaintainList'
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var maintain = responseData.Data.rows;
                        var html = "";

                        for (var i = 0; i < maintain.length; i++) {
                            html += "<tr role='tr_" + (i + 1) + "'>";
                            html += "   <td>" + (i + 1) + "</td>";
                            html += "   <td><input type='text' ismust='true' desc='发票人' name='invoiceman' maxlength='100' value='" + maintain[i]["InvoiceMan"] + "' /></td>";
                            html += "   <td><input type='text' ismust='true' desc='税号' name='dutyparagraph' maxlength='50' value='" + maintain[i]["DutyParagraph"] + "' /></td>";
                            html += "   <td><input type='text' ismust='true' desc='地址' name='address' maxlength='500' value='" + maintain[i]["Address"] + "' /></td>";
                            html += "   <td><input type='text' ismust='true' isphone='true' desc='电话' name='telephone' maxlength='50' value='" + maintain[i]["Telephone"] + "' /></td>";
                            html += "   <td><input type='text' ismust='true' desc='开户行' name='bankaddress' maxlength='500' value='" + maintain[i]["BankAddress"] + "' /></td>";
                            html += "   <td><input type='text' ismust='true' desc='账号' name='bankaccount' maxlength='50' value='" + maintain[i]["BankAccount"] + "' /></td>";
                            html += "   <td><a class='tablebtn btn-Dred' role='removetr' href='javascript:void(0);'><i class='fa fa-trash'></i></a></td>";
                            html += "</tr>";
                        }

                        $("#maintainlist").html("");
                        $("#maintainlist").append(html);
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //添加发票信息维护行
    window.finance.addMaintainTr = function () {
        var tableid = "maintainlist";//获取tbody的id
        var role = $("#" + tableid).find("tr:last").attr("role");//获取tbody下最后一个tr的role     
        var rolenum = "";//role后缀数字
        if (!role) {//如果没有tr，给role赋初始值
            rolenum = "0";
        } else {
            rolenum = role.substring(role.lastIndexOf('_') + 1, role.length);//role后缀数字
            rolenum++;//数字加1
        }
        role = tableid.substring(tableid.lastIndexOf('_') + 1, tableid.length) + "_" + rolenum;//拼接全新role

        var html = "";
        html += "<tr role='" + role + "'>";
        html += "   <td>1</td>";
        html += "   <td><input type='text' ismust='true' desc='发票人' name='invoiceman' maxlength='100' value='' /></td>";
        html += "   <td><input type='text' ismust='true' desc='税号' name='dutyparagraph' maxlength='50' value='' /></td>";
        html += "   <td><input type='text' ismust='true' desc='地址' name='address' maxlength='500' value='' /></td>";
        html += "   <td><input type='text' ismust='true' isphone='true' desc='电话' name='telephone' maxlength='50' value='' /></td>";
        html += "   <td><input type='text' ismust='true' desc='开户行' name='bankaddress' maxlength='500' value='' /></td>";
        html += "   <td><input type='text' ismust='true' desc='账号' name='bankaccount' maxlength='50' value='' /></td>";
        html += "   <td><a class='tablebtn btn-Dred' role='removetr' href='javascript:void(0);'><i class='fa fa-trash'></i></a></td>";
        html += "</tr>";

        $("#" + tableid).append(html);
        $("#" + tableid).find("tr").each(function (i) {//寻找当前tbody下所有行数（tr标签）
            index = i + 1; //每行（tr）下第一列（td）显示的行数          
            $(this).find("td:first").text(index);//给td标签赋值 行数
            $(this).find("[ismust='true']").attr("isrownum", index);
        });
    }

    //删除发票信息维护行
    window.finance.removeMaintainTr = function (id, obj) {
        $("tr[role='" + $(obj).parents('tr:first').attr("role") + "']").remove();//删除行
        var index = 0;
        $("#" + id).find("tr").each(function (i) {//寻找当前tbody下所有行数（tr标签）
            index = i + 1; //每行（tr）下第一列（td）显示的行数          
            $(this).find("td:first").text(index);//给td标签赋值 行数
            $(this).find("[ismust='true']").attr("isrownum", index);
        });
    }

    //发票信息维护确认
    window.finance.maintainConfirm = function () {
        //1.校验
        var trCount = $("#maintainlist tr").length;
        if (trCount <= 0) {
            alert("请添加发票信息。");
            return;
        }

        if (!verifydata()) {
            return;
        }

        //2.提交
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_Confirm',
                maintaininfo: JSON.stringify(serializeTable(tableSerializeArray('maintainlist'), 6))
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        $("a[role='cancel']").click();
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载详情数据
    window.finance.loadInvoiceDetailData = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadData'
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var orderData = responseData.Data.rows;
                        var invoiceApplyData = responseData.InvoiceApplyData.rows;

                        //1.加载订单数据
                        for (var i = 0; i < orderData.length; i++) {
                            $("#no").text(orderData[i]["No"]);

                            if (orderData[i]["CreateTime"]) {
                                $("#createtime").text(createDate(orderData[i]["CreateTime"]).Format("yyyy-MM-dd"));
                            }

                            $("#title").text(orderData[i]["Title"]);

                            var amount = "¥" + orderData[i]["Amount"] + "（成交价格" + orderData[i]["DaiLiFei"] + "元，官费" + orderData[i]["GuanFei"] + "元）";
                            $("#amount").text(amount);

                            $("#receiptaccount").text(orderData[i]["ReceiptAccount"] == null ? "" : orderData[i]["ReceiptAccount"]);
                            $("#payman").text(orderData[i]["PayMan"] == null ? "" : orderData[i]["PayMan"]);

                            if (orderData[i]["PayDate"]) {
                                $("#paydate").text(createDate(orderData[i]["PayDate"]).Format("yyyy-MM-dd"));
                            }
                        }

                        //2.加载发票信息
                        if (invoiceApplyData.length > 0) {
                            $(".invoiceinfo").attr("style", "display:block");
                            for (var i = 0; i < invoiceApplyData.length; i++) {
                                $("#invoiceperson").text(invoiceApplyData[i]["InvoicePerson"]);
                                $("#dutyparagraph").text(invoiceApplyData[i]["ShuiHao"]);
                                $("#address").text(invoiceApplyData[i]["Address"]);
                                $("#telephone").text(invoiceApplyData[i]["Telephone"]);
                                $("#bankaddress").text(invoiceApplyData[i]["BankAddress"]);
                                $("#bankaccount").text(invoiceApplyData[i]["BankAccount"]);

                                switch (invoiceApplyData[i]["OpenInvoiceState"]) {
                                    case "0":
                                        $("#openinvoicestate").text("未开票");
                                        break;
                                    case "1":
                                        $("#openinvoicestate").text("正在申请");
                                        break;
                                    case "2":
                                        $("#openinvoicestate").text("已开票");
                                        break;
                                    case "3":
                                        $("#openinvoicestate").text("已作废");
                                        break;
                                    default:
                                        break;
                                }

                                $("#remark").text(invoiceApplyData[i]["Remark"]);
                            }
                        }
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载申请发票页面
    window.finance.loadApplyInvoice = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadData'
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var orderData = responseData.Data.rows;

                        for (var i = 0; i < orderData.length; i++) {
                            $("#no").text(orderData[i]["No"]);

                            if (orderData[i]["CreateTime"]) {
                                $("#createtime").text(createDate(orderData[i]["CreateTime"]).Format("yyyy-MM-dd"));
                            }

                            $("#title").text(orderData[i]["Title"]);

                            var amount = "¥" + orderData[i]["Amount"] + "（成交价格" + orderData[i]["DaiLiFei"] + "元，官费" + orderData[i]["GuanFei"] + "元）";
                            $("#amount").text(amount);
                        }
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载申请发票的发票信息列表
    window.finance.loadInvoiceInfoList = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadApplyMaintainList'
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var invoiceInfo = responseData.Data.rows;
                        var html = "";

                        for (var i = 0; i < invoiceInfo.length; i++) {
                            html += "<tr>";
                            html += "   <td><input id=" + invoiceInfo[i]["ID"] + " class='check' type='checkbox' /></td>";
                            html += "   <td>" + invoiceInfo[i]["InvoiceMan"] + "</td>";
                            html += "   <td>" + invoiceInfo[i]["DutyParagraph"] + "</td>";
                            html += "   <td>" + invoiceInfo[i]["Address"] + "</td>";
                            html += "   <td>" + invoiceInfo[i]["Telephone"] + "</td>";
                            html += "   <td>" + invoiceInfo[i]["BankAddress"] + "</td>";
                            html += "   <td>" + invoiceInfo[i]["BankAccount"] + "</td>";
                            html += "</tr>";
                        }

                        $("#invoiceinfolist").html(""
                        );
                        $("#invoiceinfolist").append(html);
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //通过主键获取开票信息
    window.finance.getInvoiceInfoById = function (id) {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_GetInvoiceInfo',
                infoid: id
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var infoData = responseData.Data.rows;

                        for (var i = 0; i < infoData.length; i++) {
                            $("input[name='invoiceman']").val(infoData[i]["InvoiceMan"]);
                            $("input[name='dutyparagraph']").val(infoData[i]["DutyParagraph"]);
                            $("input[name='address']").val(infoData[i]["Address"]);
                            $("input[name='telephone']").val(infoData[i]["Telephone"]);
                            $("input[name='bankaddress']").val(infoData[i]["BankAddress"]);
                            $("input[name='bankaccount']").val(infoData[i]["BankAccount"]);
                        }

                        $(".popbox").css({ "display": "none" });
                        $(".blackbox").css({ "display": "none" });
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //确认申请
    window.finance.confirmApply = function () {
        if (!verifydata()) {
            return;
        }

        call_form({
            formid: 'applyform',
            url: window.location.href,
            data: {
                action: 'M_ConfirmApply'
            },
            forceSync: true,//强制同步
            async: false,
            success: function (data) {
                var responseData = strToJson(data);
                if (responseData.Result === "0") {
                    alert("申请成功。");
                    $("a[role='cancel']").click();
                } else {
                    alert(responseData.errMsg);
                }
            }
        });
    }
    //-----------发票管理  end --------------//

    //加载电子发票信息
    window.finance.loadBillingInformation = function () {
        call_ajax({
            url: location.href,
            data: {
                action: "M_QueryAmount"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result === "0") {

                        var Amount = aaData.Data.rows[0].Amount;

                        if (Amount.indexOf(".") <= 0) {
                            Amount += ".00";
                        }
                        var AmoTxtArrey = Amount.split(".");

                        //金额  小写显示


                        //小数点前面金额
                        var qianArrey = AmoTxtArrey[0].substring(AmoTxtArrey[0].length - 4, AmoTxtArrey[0].length).split('');

                        if (AmoTxtArrey[0].length >= 5) {

                            var wan = AmoTxtArrey[0].substring(0, AmoTxtArrey[0].length - 4);
                            var wanTxt = digitUppercaseGZ(wan);

                            $(".wan").text(wanTxt.substring(0, wanTxt.length - 1));

                            $(".qian").text(transformation(qianArrey[0]));
                            $(".bai").text(transformation(qianArrey[1]));

                            $(".shi").text(transformation(qianArrey[2]));
                            $(".yuan").text(transformation(qianArrey[3]));

                        } else {

                            var qianlen = qianArrey.length;

                            switch (qianlen) {
                                case 4:
                                    $(".qian").text(transformation(qianArrey[0]));
                                    $(".bai").text(transformation(qianArrey[1]));
                                    $(".shi").text(transformation(qianArrey[2]));
                                    $(".yuan").text(transformation(qianArrey[3]));
                                    break;
                                case 3:
                                    $(".bai").text(transformation(qianArrey[0]));
                                    $(".shi").text(transformation(qianArrey[1]));
                                    $(".yuan").text(transformation(qianArrey[2]));
                                    break;
                                case 2:
                                    $(".shi").text(transformation(qianArrey[0]));
                                    $(".yuan").text(transformation(qianArrey[1]));
                                    break;
                                case 1:
                                    $(".yuan").text(transformation(qianArrey[0]));
                                    break;
                                default:
                                    $(".yuan").text("零");
                                    break;
                            }

                        }

                        //小数点后面金额
                        if (AmoTxtArrey[1].length >= 2) {

                            var HtxtArrey = AmoTxtArrey[1].split('');
                            $(".jiao").text(transformation(HtxtArrey[0]));
                            $(".fen").text(transformation(HtxtArrey[1]));

                            var zr = AmoTxtArrey[0];
                            zr += HtxtArrey[0];
                            zr += HtxtArrey[1];
                            var zrArrey = zr.toString().split('');
                            var zrIndex = 1;
                            for (var i = zrArrey.length - 1; i >= 0; i--) {
                                $(".am_" + zrIndex).text(zrArrey[i]);
                                zrIndex++;
                            }

                        } else {
                            var zr = AmoTxtArrey[0];

                            zr += "00";
                            var zrArrey = zr.toString().split('');
                            var zrIndex = 1;
                            for (var i = zrArrey.length - 1; i >= 0; i--) {
                                $(".am_" + zrIndex).text(zrArrey[i]);
                                zrIndex++;
                            }
                            $(".jiao").text("零");
                            $(".fen").text("零");
                        }

                        //赋值时间
                        var Date = aaData.Data.rows[0].CreateTime;
                        if (Date) {
                            var _date = createDate(Date);
                            var timeArrey = _date.Format("yyyy-MM-dd").split("-");
                            $("#Year").text(timeArrey[0]);
                            $("#Month").text(timeArrey[1]);
                            $("#Day").text(timeArrey[2]);
                        }

                        $("#No").text(aaData.Data.rows[0].No);
                        if (aaData.Data.rows[0].InvoicePerson)
                            $("#InvoicePerson").text(aaData.Data.rows[0].InvoicePerson);
                        if (aaData.Data.rows[0].Remark)
                            $("#Remark").text(aaData.Data.rows[0].Remark);
                        if (aaData.Data.rows[0].Title)
                            $("#Title").text(aaData.Data.rows[0].Title);


                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    /*******************************会员中心 我的财务  end *********************************/

    /*******************************会员中心 我的资料 begin*********************************/
    if (!window.memberinfo) {
        window.memberinfo = {};
    }

    //加载会员资料
    window.memberinfo.loadMemberInfo = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadMemberInfo'
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var userData = responseData.Data;

                        if (userData) {
                            $("#usercode").text(userData["UserCode"]);

                            if (userData["ProfilePhotoPath"]) {
                                $("#profilephoto").attr("src", userData["ProfilePhotoPath"].replace("~", getRootPath()));
                            }

                            switch (userData["CheckState"]) {
                                case "0":
                                    $("#checkstate").addClass("btn-blue").text("未认证");
                                    $("#checkstate").attr('href', 'member-info-certification');
                                    break;
                                case "1":
                                    $("#checkstate").addClass('btn-yellow').text("认证中");
                                    break;
                                case "2":
                                    $("#checkstate").addClass("btn-green").text("认证成功");
                                    break;
                                case "3":
                                    $("#checkstate").addClass('btn-red').text("认证失败");
                                    $("#checkstate").attr('href', 'member-info-certification');
                                    break;
                                default:
                                    break;
                            }

                            $("#integrals").text(userData["Integrals"]);

                            switch (userData["VIPGrade"]) {
                                case "0":
                                    $("#vipgrade").text("普通会员");
                                    break;
                                case "1":
                                    $("#vipgrade").text("VIP会员");
                                    break;
                                case "2":
                                    $("#vipgrade").text("VIPPlus会员");
                                    break;
                                default:
                                    break;
                            }

                            var type = '';
                            switch (userData["AccountType"]) {
                                case "个人账户":
                                    type = 'p';
                                    $("#radio1").attr('checked', 'checked');

                                    if (userData["IsDLJG"] == 1) {
                                        $("#p-agency").attr('checked', 'checked');
                                    }
                                    $("#p-name").val(userData["RealName"]);
                                    break;
                                case "企业账户":
                                    type = "c";
                                    $("#radio2").attr('checked', 'checked');

                                    if (userData["IsDLJG"] == 1) {
                                        $("#c-agency").attr('checked', 'checked');
                                    }

                                    $("#c-name").val(userData["Company"]);
                                    $("#c-contact").val(userData["LinkMan"]);
                                    $("#c-job").val(userData["Position"]);
                                    break;
                                case "高校院所":
                                    type = "co";
                                    $("#radio3").attr('checked', 'checked');
                                    $("#co-name").val(userData["UniversitiesName"]);
                                    $("#co-contact").val(userData["LinkMan"]);
                                    $("#co-job").val(userData["Position"]);
                                    break;
                                case "其他事业单位":
                                    type = "u";
                                    $("#radio4").attr('checked', 'checked');
                                    if (userData["IsDLJG"] == 1) {
                                        $("#u-agency").attr('checked', 'checked');
                                    }
                                    $("#u-name").val(userData["Company"]);
                                    $("#u-contact").val(userData["LinkMan"]);
                                    $("#u-job").val(userData["Position"]);
                                    break;
                                default:
                                    break;
                            }

                            $("#" + type + "-phone").val(userData["MobilePhone"]);
                            $("#" + type + "-qq").val(userData["QQ"]);
                            $("#" + type + "-email").val(userData["EMail"]);
                            $("#" + type + "-province").find('option:selected').text(userData["Province"]);
                            $("input[name='province']").val(userData["Province"]);
                            $("#" + type + "-city").find('option:selected').text(userData["City"]);
                            $("input[name='city']").val(userData["City"]);
                            $("#" + type + "-area").find('option:selected').text(userData["District"]);
                            $("input[name='district']").val(userData["District"]);
                            $("#" + type + "-add").val(userData["Address"]);
                            $("#c-introduce").text(userData["CompanyIntroduce"]);
                            $("#c-project").val(userData["ServiceProject"]);
                        }
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //编辑会员资料
    window.memberinfo.editMemberInfo = function () {
        var type = $("input[name='test-radio']:checked").attr('id');

        //修改资料 input的disable属性增删
        switch (type) {
            case "radio1"://个人
                document.getElementById("p-agency").disabled = "";
                document.getElementById("p-name").disabled = "";
                document.getElementById("p-phone").disabled = "";
                document.getElementById("p-province").disabled = "";
                document.getElementById("p-city").disabled = "";
                document.getElementById("p-area").disabled = "";
                document.getElementById("p-add").disabled = "";
                break;
            case "radio2"://企业
                document.getElementById("c-agency").disabled = "";
                document.getElementById("c-name").disabled = "";
                document.getElementById("c-contact").disabled = "";
                document.getElementById("c-job").disabled = "";
                document.getElementById("c-phone").disabled = "";
                document.getElementById("c-province").disabled = "";
                document.getElementById("c-city").disabled = "";
                document.getElementById("c-area").disabled = "";
                document.getElementById("c-add").disabled = "";
                break;
            case "radio3"://高校院所
                document.getElementById("co-name").disabled = "";
                document.getElementById("co-contact").disabled = "";
                document.getElementById("co-job").disabled = "";
                document.getElementById("co-phone").disabled = "";
                document.getElementById("co-province").disabled = "";
                document.getElementById("co-city").disabled = "";
                document.getElementById("co-area").disabled = "";
                document.getElementById("co-add").disabled = "";
                break;
            case "radio4"://其他事业单位
                document.getElementById("u-agency").disabled = "";
                document.getElementById("u-name").disabled = "";
                document.getElementById("u-contact").disabled = "";
                document.getElementById("u-job").disabled = "";
                document.getElementById("u-phone").disabled = "";
                document.getElementById("u-province").disabled = "";
                document.getElementById("u-city").disabled = "";
                document.getElementById("u-area").disabled = "";
                document.getElementById("u-add").disabled = "";
                break;
        }
    }

    //取消编辑会员资料
    window.memberinfo.cancelEditMemberInfo = function () {
        var type = $("input[name='test-radio']:checked").attr('id');

        switch (type) {
            case "radio1"://个人
                document.getElementById("p-agency").disabled = "true";
                document.getElementById("p-name").disabled = "true";
                document.getElementById("p-phone").disabled = "true";
                document.getElementById("p-province").disabled = "true";
                document.getElementById("p-city").disabled = "true";
                document.getElementById("p-add").disabled = "true";
                document.getElementById("p-area").disabled = "true";
                break;
            case "radio2"://企业
                document.getElementById("c-agency").disabled = "true";
                document.getElementById("c-name").disabled = "true";
                document.getElementById("c-contact").disabled = "true";
                document.getElementById("c-job").disabled = "true";
                document.getElementById("c-phone").disabled = "true";
                document.getElementById("c-province").disabled = "true";
                document.getElementById("c-city").disabled = "true";
                document.getElementById("c-add").disabled = "true";
                document.getElementById("c-area").disabled = "true";
                break;
            case "radio3"://高校院所
                document.getElementById("co-name").disabled = "true";
                document.getElementById("co-contact").disabled = "true";
                document.getElementById("co-job").disabled = "true";
                document.getElementById("co-phone").disabled = "true";
                document.getElementById("co-province").disabled = "true";
                document.getElementById("co-city").disabled = "true";
                document.getElementById("co-area").disabled = "true";
                document.getElementById("co-add").disabled = "true";
                break;
            case "radio4"://其他事业单位
                document.getElementById("u-agency").disabled = "true";
                document.getElementById("u-name").disabled = "true";
                document.getElementById("u-contact").disabled = "true";
                document.getElementById("u-job").disabled = "true";
                document.getElementById("u-phone").disabled = "true";
                document.getElementById("u-province").disabled = "true";
                document.getElementById("u-city").disabled = "true";
                document.getElementById("u-area").disabled = "true";
                document.getElementById("u-add").disabled = "true";
                break;
        }
    }

    //保存会员资料
    window.memberinfo.saveMemberInfo = function () {
        if (!verifydata()) {
            return;
        } else {
            call_form({
                formid: 'memberinfo',
                url: window.location.href,
                data: {
                    action: 'M_SaveInfo'
                },
                forceSync: true,//强制同步
                async: false,
                success: function (data) {
                    var responseData = strToJson(data);
                    if (responseData.Result === "0") {
                        alert("保存成功。");
                        window.location.href = "member-info-detail";
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            });
        }
    }

    //加载会员认证
    window.memberinfo.loadMemberCertification = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadData',
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var userData = responseData.UserData;
                        var authenticationData = responseData.AuthenticationData.rows;

                        if (authenticationData.length > 0) {
                            window.memberinfo.loadMemberCertificationInfo(authenticationData);
                        } else {
                            window.memberinfo.loadMemberCertificationUserInfo(userData);
                        }
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载会员认证-用户信息
    window.memberinfo.loadMemberCertificationUserInfo = function (data) {
        //个人
        $("#p-name").val(data["RealName"])
        $("#p-phone").val(data["MobilePhone"]);
        $("#p-qq").val(data["QQ"]);
        $("#p-email").val(data["EMail"]);
        $("#p-province").find('option:selected').text(data["Province"]);
        $("#p-city").find('option:selected').text(data["City"]);
        $("#p-area").find('option:selected').text(data["District"]);
        $("#p-add").val(data["Address"]);

        //企业
        $("#c-contact").val(data["LinkMan"]);
        $("#c-phone").val(data["MobilePhone"]);
        $("#c-qq").val(data["QQ"]);
        $("#c-email").val(data["EMail"]);
        $("#c-province").find('option:selected').text(data["Province"]);
        $("#c-city").find('option:selected').text(data["City"]);
        $("#c-area").find('option:selected').text(data["District"]);
        $("#c-add").val(data["Address"]);

        //高校院所
        $("#co-contact").val(data["LinkMan"]);
        $("#co-phone").val(data["MobilePhone"]);
        $("#co-qq").val(data["QQ"]);
        $("#co-email").val(data["EMail"]);
        $("#co-province").find('option:selected').text(data["Province"]);
        $("#co-city").find('option:selected').text(data["City"]);
        $("#co-area").find('option:selected').text(data["District"]);
        $("#co-add").val(data["Address"]);

        //其他事业单位
        $("#u-contact").val(data["LinkMan"]);
        $("#u-phone").val(data["MobilePhone"]);
        $("#u-qq").val(data["QQ"]);
        $("#u-email").val(data["EMail"]);
        $("#u-add").val(data["Address"]);
        $("#u-province").find('option:selected').text(data["Province"]);
        $("#u-city").find('option:selected').text(data["City"]);
        $("#u-area").find('option:selected').text(data["District"]);
        $("#u-add").val(data["Address"]);

        if (data["IsDLJG"] === "1") {
            $("#p-agency").attr("checked", "checked");
            $("#c-agency").attr("checked", "checked");
            $("#u-agency").attr("checked", "checked");
            $("input[name='intermediary']").val("1");
        }
        $("input[name='province']").val(data["Province"]);
        $("input[name='city']").val(data["City"]);
        $("input[name='district']").val(data["District"]);
    }

    //加载会员认证-认证信息
    window.memberinfo.loadMemberCertificationInfo = function (data) {
        for (var i = 0; i < data.length; i++) {
            var type = "";
            switch (data[i]["AccountType"]) {
                case "个人账户":
                    type = "p";
                    $("#radio1").attr("checked", "checked");
                    if (data[i]["IsDLJG"] === "1") {
                        $("#p-agency").attr("checked", "checked");
                        $("input[name='p_intermediary']").val("1");
                    }
                    $("#p-name").val(data[i]["RealName"]);
                    $("#p-number").val(data[i]["IDNumber"]);
                    $("#p_idcard_add").attr("src", data[i]["Phono"].replace("~", getRootPath()));
                    $("input[name='imgpath']").val(data[i]["Phono"]);
                    break;
                case "企业账户":
                    type = "c";
                    $("#radio2").attr("checked", "checked");
                    if (data[i]["IsDLJG"] === "1") {
                        $("#c-agency").attr("checked", "checked");
                        $("input[name='c_intermediary']").val("1");
                    }
                    $("#c-name").val(data[i]["Company"]);
                    $("#c-number").val(data[i]["AppayNumber"]);
                    $("#p_businesslicense_add").attr("src", data[i]["BusinessLicensePath"].replace("~", getRootPath()));
                    $("input[name='imgpath']").val(data[i]["BusinessLicensePath"]);
                    $("#c-contact").val(data[i]["LinkMan"]);
                    $("#c-job").val(data[i]["Position"]);
                    break;
                case "高校院所":
                    type = "co";
                    $("#radio3").attr("checked", "checked");
                    $("#co-name").val(data[i]["UniversitiesName"]);
                    $("#co-number").val(data[i]["AppayNumber"]);
                    $("#p_corporatecertificate_add").attr("src", data[i]["CorporateCertificatePath"].replace("~", getRootPath()));
                    $("input[name='imgpath']").val(data[i]["CorporateCertificatePath"]);
                    $("#co-job").val(data[i]["Position"]);
                    $("#co-contact").val(data[i]["LinkMan"]);
                    break;
                case "其他事业单位":
                    type = "u";
                    $("#radio4").attr("checked", "checked");
                    if (data[i]["IsDLJG"] === "1") {
                        $("#u-agency").attr("checked", "checked");
                        $("input[name='u_intermediary']").val("1");
                    }
                    $("#u-name").val(data[i]["Company"]);
                    $("#u-number").val(data[i]["AppayNumber"]);
                    $("#p_otherinstitution_add").attr("src", data[i]["CorporateCertificatePath"].replace("~", getRootPath()));
                    $("input[name='imgpath']").val(data[i]["CorporateCertificatePath"]);
                    $("#u-contact").val(data[i]["LinkMan"]);
                    $("#u-job").val(data[i]["Position"]);
                    break;
                default:
                    break;
            }

            $("#" + type + "-phone").val(data[i]["MobilePhone"]);
            $("#" + type + "-qq").val(data[i]["QQ"]);
            $("#" + type + "-email").val(data[i]["EMail"]);
            $("#" + type + "-province").find('option:selected').text(data[i]["Province"]);
            $("input[name='province']").val(data[i]["Province"]);
            $("#" + type + "-city").find('option:selected').text(data[i]["City"]);
            $("input[name='city']").val(data[i]["City"]);
            $("#" + type + "-area").find('option:selected').text(data[i]["District"]);
            $("input[name='district']").val(data[i]["District"]);
            $("#" + type + "-add").val(data[i]["Address"]);
        }
    }

    //提交认证
    window.memberinfo.confirmCertification = function () {
        var type = $("input[name='test-radio']:checked").attr('id');

        switch (type) {
            case "radio1":
                if (window.memberinfo.checkPersonalInfo() == false) {
                    return;
                }
                $("input[name='accounttype']").val("个人账户");
                break;
            case "radio2":
                if (window.memberinfo.checkCompanyInfo() == false) {
                    return;
                }
                $("input[name='accounttype']").val("企业账户");
                break;
            case "radio3":
                if (window.memberinfo.checkUniversitiesInfo() == false) {
                    return;
                }
                $("input[name='accounttype']").val("高校院所");
                break;
            case "radio4":
                if (window.memberinfo.checkOtherInstitutionInfo() == false) {
                    return;
                }
                $("input[name='accounttype']").val("其他事业单位");
                break;
            default:
                break;
        }

        call_form({
            formid: 'confrimform',
            url: window.location.href,
            data: {
                action: 'M_ConfirmCertification'
            },
            forceSync: true,//强制同步
            async: false,
            success: function (data) {
                var responseData = strToJson(data);
                if (responseData.Result === "0") {
                    alert("提交成功。");
                    window.location.href = "member-info-detail";
                } else {
                    alert(responseData.errMsg);
                }
            }
        });
    }

    //校验个人信息
    window.memberinfo.checkPersonalInfo = function () {
        if ($("#p-name").val() == "" || $("#p-name").val() == null || typeof ($("#p-name").val()) == "undefined") {
            alert("请输入姓名。");
            return false;
        }

        var idCard = $("#p-number").val();
        if (idCard == "" || idCard == null || typeof (idCard) == "undefined") {
            alert("请输入身份证号。");
            return false;
        } else {
            if (CheckIDNumber(idCard) == false) {
                alert("请输入正确的身份证号。");
                return false;
            }
        }

        var img = $("#p_idcard_add").attr('src');
        if (img == "../img/imgadd.png") {
            alert("请选择上传身份证件图片。");
            return false;
        }

        if ($("#p-qq").val() == "" || $("#p-qq").val() == null || typeof ($("#p-qq").val()) == "undefined") {
            alert("请输入QQ。");
            return false;
        }

        if ($("#p-email").val() == "" || $("#p-email").val() == null || typeof ($("#p-email").val()) == "undefined") {
            alert("请输入邮箱。");
            return false;
        }
        if (!CheckMail($("#p-email").val())) {
            alert("邮箱输入的值不是有效的E-mail格式。");
            return false;
        }

        var province = $("input[name='province']").val();
        if (province == "" || province == null || typeof (province) == "undefined") {
            alert("请选择省份。");
            return false;
        }
        var city = $("input[name='city']").val()
        if (city == "" || city == null || typeof (city) == "undefined") {
            alert("请选择市。");
            return false;
        }
        var area = $("input[name='district']").val();
        if (area == "" || area == null || typeof (area) == "undefined") {
            alert("请选择区。");
            return false;
        }
        if ($("#p-add").val() == "" || $("#p-add").val() == null || typeof ($("#p-add").val()) == "undefined") {
            alert("请输入地址。");
            return false;
        }

        return true;
    }

    //校验企业信息
    window.memberinfo.checkCompanyInfo = function () {
        if ($("#c-name").val() == "" || $("#c-name").val() == null || typeof ($("#c-name").val()) == "undefined") {
            alert("请输入公司名称。");
            return false;
        }

        var number = $("#c-number").val();
        if (number == "" || number == null || typeof (number) == "undefined") {
            alert("请输入统一社会信用代码。");
            return false;
        }

        var img = $("#p_businesslicense_add").attr('src');
        if (img == "../img/imgadd.png") {
            alert("请选择上传营业执照图片。");
            return false;
        }

        if ($("#c-qq").val() == "" || $("#c-qq").val() == null || typeof ($("#c-qq").val()) == "undefined") {
            alert("请输入QQ。");
            return false;
        }

        if ($("#c-email").val() == "" || $("#c-email").val() == null || typeof ($("#c-email").val()) == "undefined") {
            alert("请输入邮箱。");
            return false;
        }
        if (!CheckMail($("#c-email").val())) {
            alert("邮箱输入的值不是有效的E-mail格式。");
            return false;
        }

        var province = $("input[name='province']").val();
        if (province == "" || province == null || typeof (province) == "undefined") {
            alert("请选择省份。");
            return false;
        }
        var city = $("input[name='city']").val()
        if (city == "" || city == null || typeof (city) == "undefined") {
            alert("请选择市。");
            return false;
        }
        var area = $("input[name='district']").val();
        if (area == "" || area == null || typeof (area) == "undefined") {
            alert("请选择区。");
            return false;
        }
        if ($("#c-add").val() == "" || $("#c-add").val() == null || typeof ($("#c-add").val()) == "undefined") {
            alert("请输入地址。");
            return false;
        }

        return true;
    }

    //校验高校院所
    window.memberinfo.checkUniversitiesInfo = function () {
        if ($("#co-name").val() == "" || $("#co-name").val() == null || typeof ($("#co-name").val()) == "undefined") {
            alert("请输入院校名称。");
            return false;
        }

        var number = $("#co-number").val();
        if (number == "" || number == null || typeof (number) == "undefined") {
            alert("请输入统一社会信用代码。");
            return false;
        }

        var img = $("#p_corporatecertificate_add").attr('src');
        if (img == "../img/imgadd.png") {
            alert("请选择上传法人证书图片。");
            return false;
        }

        if ($("#co-job").val() == "" || $("#co-job").val() == null || typeof ($("#co-job").val()) == "undefined") {
            alert("请输入学院/部门。");
            return false;
        }

        if ($("#co-qq").val() == "" || $("#co-qq").val() == null || typeof ($("#co-qq").val()) == "undefined") {
            alert("请输入QQ。");
            return false;
        }

        if ($("#co-email").val() == "" || $("#co-email").val() == null || typeof ($("#co-email").val()) == "undefined") {
            alert("请输入邮箱。");
            return false;
        }
        if (!CheckMail($("#co-email").val())) {
            alert("邮箱输入的值不是有效的E-mail格式。");
            return false;
        }

        var province = $("input[name='province']").val();
        if (province == "" || province == null || typeof (province) == "undefined") {
            alert("请选择省份。");
            return false;
        }
        var city = $("input[name='city']").val()
        if (city == "" || city == null || typeof (city) == "undefined") {
            alert("请选择市。");
            return false;
        }
        var area = $("input[name='district']").val();
        if (area == "" || area == null || typeof (area) == "undefined") {
            alert("请选择区。");
            return false;
        }
        if ($("#co-add").val() == "" || $("#co-add").val() == null || typeof ($("#co-add").val()) == "undefined") {
            alert("请输入地址。");
            return false;
        }

        return true;
    }

    //校验其他事业单位
    window.memberinfo.checkOtherInstitutionInfo = function () {
        if ($("#u-name").val() == "" || $("#u-name").val() == null || typeof ($("#u-name").val()) == "undefined") {
            alert("请输入单位名称。");
            return false;
        }

        var number = $("#u-number").val();
        if (number == "" || number == null || typeof (number) == "undefined") {
            alert("请输入统一社会信用代码。");
            return false;
        }

        var img = $("#p_otherinstitution_add").attr('src');
        if (img == "../img/imgadd.png") {
            alert("请选择上传法人证书图片。");
            return false;
        }

        if ($("#u-qq").val() == "" || $("#u-qq").val() == null || typeof ($("#u-qq").val()) == "undefined") {
            alert("请输入QQ。");
            return false;
        }

        if ($("#u-email").val() == "" || $("#u-email").val() == null || typeof ($("#u-email").val()) == "undefined") {
            alert("请输入邮箱。");
            return false;
        }
        if (!CheckMail($("#u-email").val())) {
            alert("邮箱输入的值不是有效的E-mail格式。");
            return false;
        }

        var province = $("input[name='province']").val();
        if (province == "" || province == null || typeof (province) == "undefined") {
            alert("请选择省份。");
            return false;
        }

        var city = $("input[name='city']").val()
        if (city == "" || city == null || typeof (city) == "undefined") {
            alert("请选择市。");
            return false;
        }

        var area = $("input[name='district']").val();
        if (area == "" || area == null || typeof (area) == "undefined") {
            alert("请选择区。");
            return false;
        }

        if ($("#u-add").val() == "" || $("#u-add").val() == null || typeof ($("#u-add").val()) == "undefined") {
            alert("请输入地址。");
            return false;
        }

        return true;
    }

    //保存公司介绍
    window.memberinfo.saveCompanyIntroduce = function () {
        var companyIntroduce = $("#c-introduce")[0].value;

        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_SaveCompanyIntroduce',
                introduce: companyIntroduce
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        alert("保存成功。");
                        window.location.href = "member-info-detail";
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载积分统计
    window.memberinfo.loadIntegralsStatistics = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadIntegralsStatistics',
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var total = responseData.Total;
                        var consume = responseData.Consume;
                        var get = responseData.Get;

                        $("#total").text(total);
                        $("#consume").text(consume.substring(1));
                        $("#get").text(get);
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载积分记录
    window.memberinfo.loadIntegralsRecord = function (type) {
        var option = {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "M_LoadIntegralsRecord";
                params.type = type;
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "id",
            height: 650,
            searchShowType: "detail",
            columns: [{
                title: '',
                field: "_row_num_",
                sortable: false,
                align: 'center',
                width: '30',
                valign: 'middle'
            }, {
                title: '时间',
                field: 'CreateTime',
                sortable: false,
                width: '480',
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        return createDate(value).Format('yyyy-MM-dd HH:mm');
                    } else {
                        return '';
                    }
                }
            }, {
                title: '类型',
                field: "Source",
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        switch (value) {
                            case "1":
                                return "账户注册";
                                break;
                            case "2":
                                return "实名认证";
                                break;
                            case "3":
                                return "每日首次登录";
                                break;
                            case "4":
                                return "发布需求";
                                break;
                            case "5":
                                return "充值";
                                break;
                            case "6":
                                return "购买会员";
                                break;
                            case "7":
                                return "预留专利";
                                break;
                            case "8":
                                return "查询卖家信息";
                                break;
                            case "9":
                                return "费用信息";
                                break;
                            case "10":
                                return "法律状态";
                                break;

                        }
                    } else {
                        return "";
                    }
                }
            }, {
                title: '积分',
                field: "Integrals",
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        if (row.Type == 1) {
                            return "<span class='color-green font16 fblod'><i class='fa fa-diamond'></i>+" + value + "</span>";
                        } else {
                            return "<span class='color-red font16 fblod'><i class='fa fa-diamond'></i>" + value + "</span>";
                        }
                    } else {
                        return "";
                    }
                }
            }, {
                title: '备注',
                field: "",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }]
        };

        $(".pagetablebox").html("");
        $(".pagetablebox").append("<table class='pagetable' id='integralsrecordtable'></table>");
        Table.loadTable($("#integralsrecordtable"), option);
    }

    //加载安全设置
    window.memberinfo.loadSafeSetUp = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadSafeSetUp'
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var email = responseData.Email;
                        var mobilePhone = responseData.MobilePhone;
                        var qq = responseData.QQ;

                        var safeLevel = 25;
                        if (email == "true") {
                            safeLevel = safeLevel + 25;
                            $("span[role='haveemail']").siblings().eq(0).addClass("fa-check");
                            $(".haveemail").attr("style", "display:inline-block");
                        } else {
                            $("span[role='noeamil']").siblings().eq(0).addClass("fa-exclamation-circle");
                            $("span[role='noeamil']").parent().addClass("not");
                            $(".noeamil").attr("style", "display:inline-block");
                        }
                        if (mobilePhone == "true") {
                            safeLevel = safeLevel + 25;
                            $("span[role='havemobilephone']").siblings().eq(0).addClass("fa-check");
                            $(".havemobilephone").attr("style", "display:inline-block");
                        } else {
                            $("span[role='nomobilephone']").siblings().eq(0).addClass("fa-exclamation-circle");
                            $("span[role='nomobilephone']").parent().addClass("not");
                            $(".nomobilephone").attr("style", "display:inline-block");
                        }
                        if (qq == "true") {
                            safeLevel = safeLevel + 25;
                            $("span[role='haveqq']").siblings().eq(0).addClass("fa-check");
                            $(".haveqq").attr("style", "display:inline-block");
                        } else {
                            $("span[role='noqq']").siblings().eq(0).addClass("fa-exclamation-circle");
                            $("span[role='noqq']").parent().addClass("not");
                            $(".noqq").attr("style", "display:inline-block");
                        }

                        switch (safeLevel) {
                            case 25:
                                $("span[role='safe25']").attr("style", "display:inline-block");
                                break;
                            case 50:
                                $("span[role='safe50']").attr("style", "display:inline-block");
                                break;
                            case 75:
                                $("span[role='safe75']").attr("style", "display:inline-block");
                                break;
                            case 100:
                                $("span[role='safe100']").attr("style", "display:inline-block");
                                break;
                        }

                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //加载会员权益
    window.memberinfo.loadMemberIntegrals = function () {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadData'
            },
            async: false,
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var integralsData = responseData.Data.rows;
                        var name = "";
                        var title = "";
                        var choose = "";
                        var money = "";
                        var img = "";

                        for (var i = 0; i < integralsData.length; i++) {
                            var totalIntegrals = integralsData[i]["Integrals"];
                            if (integralsData[i]["GiveIntegrals"] != 0) {
                                totalIntegrals += "+" + integralsData[i]["GiveIntegrals"];
                            }

                            if (integralsData[i]["IsVIP"] == "Y") {
                                name += "<h3 class='RImember' style='display: none;' id='name" + integralsData[i]["ID"] + "'><i class='fa fa-user'></i>" + integralsData[i]["ServiceType"] + "</h3>";
                                money += "<span id='RImoney' style='display: none;'>" + (Number(integralsData[i]["Price"])) + "+ <label>送 <i class='fa fa-diamond'></i> " + integralsData[i]["GiveIntegrals"] + " 积分</label></span>";
                                img += "<div style='display: none;'><h3 class='RItitle'>会员权益</h3><img class='RIbox-img'  src='" + integralsData[i]["UploadExplain"].replace("~/", getRootPath() + "/") + "' /></div>";
                            } else {
                                name += "<h3 class='RImember' style='display: none;' id='name" + integralsData[i]["ID"] + "'><i class='fa fa-diamond'></i>" + totalIntegrals + "</h3>";
                                money += "<span id='RImoney' style='display: none;'>" + (Number(integralsData[i]["Price"])) + "</span>";
                                img += "<div style='display: none;'><img class='RIbox-img' id='img" + integralsData[i]["ID"] + "' src='" + integralsData[i]["UploadExplain"].replace("~/", getRootPath() + "/") + "' /></div>";
                            }

                            title += "<span id=" + integralsData[i]["ID"] + " style='display: none;'>" + integralsData[i]["ServiceType"] + "</span>";
                            choose += "<a href='javascript:void(0);' id=" + integralsData[i]["ID"] + "><i class='fa fa-check-square'></i>" + integralsData[i]["ServiceType"] + "</a>";
                        }

                        $("#RIname").html("");
                        $("#choose").html("");
                        $("#RItitle").html("");
                        $("#img").html("");
                        $("#RIname").append(name);
                        $("#choose").append(choose);
                        $("#money").append("" + money);
                        $("#RItitle").append(title);
                        $("#img").append(img);
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //修改密码
    window.memberinfo.modifyPassword = function () {
        var oldPassword = $("input[name='oldpassword']").val();
        var newPassword = $("input[name='newpassword']").val();
        var confirmNewPassword = $("input[name='confirmnewpassword']").val();

        if (!oldPassword) {
            alert("请输入原密码。");
            return;
        }
        if (!newPassword) {
            alert("请输入新密码。");
            return;
        }
        if (newPassword.length < 6 || newPassword.length > 12) {
            alert("密码必须在6-12位之间请重新输入");
            return;
        }
        if (!confirmNewPassword) {
            alert("请输入确认新密码。");
            return;
        }
        if (newPassword != confirmNewPassword) {
            alert("新密码与确认新密码不一致。");
            return;
        }

        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_ModifyPassword',
                oldpassword: oldPassword,
                newpassword: newPassword,
                confirmnewpassword: confirmNewPassword
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        $(".popbox").css({ "display": "none" });
                        $(".blackbox").css({ "display": "none" });
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //发送邮箱验证码 
    window.memberinfo.sendEmailCode = function () {
        var email = $("input[name='email']").val();
        if (!email) {
            alert("请输入邮箱。");
            return;
        }
        if (!CheckMail(email)) {
            alert("邮箱输入的值不是有效的E-mail格式。");
            return;
        }

        //开始计时
        timer = window.setInterval("JlTime(2, 'window.memberinfo.sendEmailCode()')", 1000);

        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_SendEmailCode',
                email: email
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //修改邮箱
    window.memberinfo.modifyEmail = function () {
        var email = $("input[name='email']").val();
        var code = $("input[name='emailcode']").val();

        if (!email) {
            alert("请输入邮箱。");
            return;
        }
        if (!CheckMail(email)) {
            alert("邮箱输入的值不是有效的E-mail格式");
            return;
        }
        if (!code) {
            alert("请输入验证码。");
            return;
        }

        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_ModifyEmail',
                email: email,
                code: code
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        $("span[role='haveemail']").parent().removeClass("not");
                        $("span[role='haveemail']").siblings().eq(0).removeClass("fa-exclamation-circle").addClass("fa-check");
                        $(".haveemail").attr("style", "display:none");
                        $(".noeamil").attr("style", "display:none");
                        $(".informationtop").children().attr("style", "display:none");
                        $(".popbox").css({ "display": "none" });
                        $(".blackbox").css({ "display": "none" });
                        $("input[name='email']").val("");
                        $("input[name='emailcode']").val("");
                        $("a[name='safesetup']").click();
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //发送手机验证码 
    window.memberinfo.sendPhoneCode = function () {
        var mobilePhone = $("input[name='mobilephone']").val();
        if (!mobilePhone) {
            alert("请输入手机号。");
            return;
        }
        if (!CheckMobile(mobilePhone)) {
            alert("手机号输入的值不是有效的手机格式。");
            return;
        }

        //开始计时
        timer = window.setInterval("JlTime(1, 'window.memberinfo.sendPhoneCode()')", 1000);

        //call_ajax({
        //    url: window.location.href,
        //    data: {
        //        action: 'M_SendPhoneCode',
        //        mobilephone: mobilePhone
        //    },
        //    success: function (data) {
        //        var responseData = strToJson(data);

        //        if (responseData) {
        //            if (responseData.Result === "0") {
        //            } else {
        //                alert(responseData.errMsg);
        //            }
        //        }
        //    }
        //});
    }

    //修改手机
    window.memberinfo.modifyMobilePhone = function () {
        var mobilePhone = $("input[name='mobilephone']").val();
        var code = $("input[name='phonecode']").val();

        if (!mobilePhone) {
            alert("请输入手机号。");
            return;
        }
        if (!CheckMobile(mobilePhone)) {
            alert("手机号输入的值不是有效的手机格式。");
            return;
        }
        if (!code) {
            alert("请输入验证码。");
            return;
        }

        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_ModifyMobilePhone',
                mobilephone: mobilePhone,
                code: code
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        $("span[role='havemobilephone']").parent().removeClass("not");
                        $("span[role='havemobilephone']").siblings().eq(0).removeClass("fa-exclamation-circle").addClass("fa-check");
                        $(".havemobilephone").attr("style", "display:none");
                        $(".nomobilephone").attr("style", "display:none");
                        $(".informationtop").children().attr("style", "display:none");
                        $(".popbox").css({ "display": "none" });
                        $(".blackbox").css({ "display": "none" });
                        $("input[name='mobilephone']").val("");
                        $("input[name='phonecode']").val("");
                        $("a[name='safesetup']").click();
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //修改QQ
    window.memberinfo.modifyQQ = function () {
        var qq = $("#qq").val();
        if (!qq) {
            alert("请输入绑定QQ。");
            return;
        }

        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_ModifyQQ',
                qq: qq
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        $("span[role='haveqq']").parent().removeClass("not");
                        $("span[role='haveqq']").siblings().eq(0).removeClass("fa-exclamation-circle").addClass("fa-check");
                        $(".haveqq").attr("style", "display:none");
                        $(".noqq").attr("style", "display:none");
                        $(".informationtop").children().attr("style", "display:none");
                        $(".popbox").css({ "display": "none" });
                        $(".blackbox").css({ "display": "none" });
                        $("#qq").val("");
                        $("a[name='safesetup']").click();
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //添加订单
    window.memberinfo.addOrder = function (id) {
        if (id == null) {
            alret("请选择服务类型。");
            return;
        }

        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_AddOrder',
                id: id
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var orderNo = responseData.OrderNo;

                        location = 'pay-detail-' + orderNo;
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }
    /*******************************会员中心 我的信息  end *********************************/

    /*******************************会员中心 我的关注 begin*********************************/
    if (!window.attention) {
        window.attention = {};
    }

    //加载专利市场关注的专利
    window.attention.loadPAMarket = function () {
        var option = {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "M_LoadPAMarket";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "id",
            height: 580,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [{
                title: '专利名称',
                field: 'PAName',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '专利号',
                field: 'ApplyNo',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '行业领域',
                field: 'Industry',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '法律状态',
                field: 'LegalState',
                datatype: 'select',
                ext: 'like',
                suboption: [{
                    title: '专利权维持',
                    value: '专利权维持'
                }, {
                    title: '等年费滞纳金',
                    value: '等年费滞纳金'
                }, {
                    title: '授权未交费',
                    value: '授权未交费'
                }]
            }, {
                title: '交易状态',
                field: 'TranState',
                datatype: 'select',
                ext: '=',
                suboption: [{
                    title: '发布',
                    value: '1'
                }, {
                    title: '已交易',
                    value: '2'
                }, {
                    title: '已取消',
                    value: '3'
                }]
            }, {
                title: '关注日期',
                field: 'CreateTime',
                datatype: 'datetime',
                ext: 'like'
            }],
            columns: [{
                title: '',
                field: "_row_num_",
                sortable: false,
                width: '30',
                align: 'center',
                valign: 'middle'
            }, {
                title: '',
                field: 'ID',
                visible: false,
                align: 'center'
            }, {
                title: '专利名称',
                field: 'PAName',
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        return "<a class='color-blue' href='market-detail-" + row.FK_ID + "-" + row.ApplyNo.replace('.', '') + "' target='_blank'>" + value + "</a>";
                    } else {
                        return "";
                    }
                }
            }, {
                title: '专利号',
                field: "ApplyNo",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '行业领域',
                field: "Industry",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '法律状态',
                field: "LegalState",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '交易状态',
                field: "TranState",
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        switch (value) {
                            case "1":
                                return "发布";
                                break;
                            case "2":
                                return "已交易";
                                break;
                            case "3":
                                return "已取消"
                                break;
                        }
                    }

                    return "";
                }
            }, {
                title: '关注日期',
                field: 'CreateTime',
                sortable: false,
                width: '100',
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        return createDate(value).Format('yyyy-MM-dd');
                    } else {
                        return '';
                    }
                }
            }, {
                title: '操作',
                sortable: false,
                width: '100',
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    return "<a class='tablebtn btn-blue' onclick='window.attention.cancelAttention(" + row.ID + ",1);' href='javascript:void(0);'>取消关注</a>";
                }
            }]
        };

        $(".pagetablebox").html("");
        $(".pagetablebox").append("<table class='pagetable' id='table'></table>");
        Table.loadTable($("#table"), option);
    }

    //加载专利检索关注的专利
    window.attention.loadPASearch = function () {
        var option = {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "M_LoadPASearch";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "id",
            height: 580,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [{
                title: '专利名称',
                field: 'PAName',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '专利号',
                field: 'ApplyNo',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '行业领域',
                field: 'Industry',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '法律状态',
                field: 'LegalState',
                datatype: 'select',
                ext: 'like',
                suboption: [{
                    title: '专利权维持',
                    value: '专利权维持'
                }, {
                    title: '等年费滞纳金',
                    value: '等年费滞纳金'
                }, {
                    title: '授权未交费',
                    value: '授权未交费'
                }]
            }, {
                title: '关注日期',
                field: 'CreateTime',
                datatype: 'datetime',
                ext: 'like'
            }],
            columns: [{
                title: '',
                field: "_row_num_",
                sortable: false,
                width: '30',
                align: 'center',
                valign: 'middle'
            }, {
                title: '',
                field: 'ID',
                visible: false,
                align: 'center'
            }, {
                title: '专利名称',
                field: 'PAName',
                sortable: false,
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        return "<a class='color-blue' href='search-padetail-" + row.ApplyNo.replace('.', '') + "' target='_blank'>" + value + "</a>";
                    } else {
                        return "";
                    }
                }
            }, {
                title: '专利号',
                field: "ApplyNo",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '行业领域',
                field: "Industry",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '法律状态',
                field: "LegalState",
                sortable: false,
                align: 'center',
                valign: 'middle'
            }, {
                title: '关注日期',
                field: 'CreateTime',
                sortable: false,
                width: '100',
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    if (value) {
                        return createDate(value).Format('yyyy-MM-dd');
                    } else {
                        return '';
                    }
                }
            }, {
                title: '操作',
                sortable: false,
                width: '100',
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                    return "<a class='tablebtn btn-blue' onclick='window.attention.cancelAttention(" + row.ID + ",2);' href='javascript:void(0);'>取消关注</a>";
                }
            }]
        };

        $(".pagetablebox").html("");
        $(".pagetablebox").append("<table class='pagetable' id='table'></table>");
        Table.loadTable($("#table"), option);
    }

    //取消关注
    window.attention.cancelAttention = function (id, type) {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_CancelAttention',
                id: id
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        switch (type) {
                            case 1:
                                $("a[name='market']").click();
                                break;
                            case 2:
                                $("a[name='search']").click();
                                break;
                        }
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }
    /*******************************会员中心 我的关注  end *********************************/

    /*******************************会员中心 我的快递 begin*********************************/
    if (!window.courier) {
        window.courier = {};
    }

    //加载数据
    window.courier.loadData = function () {
        var option = {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "M_LoadData";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "id",
            height: 600,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [{
                title: '快递单号',
                field: 'ExpressNumber',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '寄出日期',
                field: 'CreateDate',
                datatype: 'datetime',
                ext: 'like'
            }],
            columns: [
                {
                    title: '_ckb_',
                    field: "",
                    sortable: false,
                    width: '30',
                    checkbox: true,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '',
                    field: 'ID',
                    visible: false,
                    align: 'center'
                }, {
                    title: '寄出日期',
                    field: 'CreateDate',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            return createDate(value).Format('yyyy-MM-dd');
                        }
                        return '';
                    }
                }, {
                    title: '文件清单',
                    field: 'DetailedList',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            if (value.length > 25) {
                                return "<a href='javascript:void(0);' title='" + value + "'>" + value.substring(0, 25) + "..." + "</a>";
                            } else {
                                return "<a href='javascript:void(0);' title='" + value + "'>" + value + "</a>";
                            }
                        }
                        return "";
                    }
                }, {
                    title: '快递公司',
                    field: "ExpressCompany",
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '快递单号',
                    field: "ExpressNumber",
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '快递跟踪',
                    field: "ExpressNumber",
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return "<a class='color-red' href='https://www.aikuaidi.cn/' target='_blank'>快递跟踪查询</a>";
                    }
                }, {
                    title: '相关订单编号',
                    field: "No",
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            return "<a class='color-blue' href='member-order-list'>" + value + "</a>";
                        }
                        return "";
                    }
                }, {
                    title: '操作',
                    field: "ConfirmReceipt",
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value == 1) {
                            return "<a class='tablebtn btn-yellow' href='javascript:void(0);'>已签收</a>";
                        }

                        return "<a class='tablebtn btn-green' onclick='window.courier.confirmSignFor(" + row.ID + ");' href='javascript:void(0);'>确认签收</a>";
                    }
                }
            ]
        };

        Table.loadTable($("#table"), option);
    }

    //确认签收
    window.courier.confirmSignFor = function (id) {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_ConfirmSignFor',
                id: id
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        $("a[name='list']").click();
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //批量签收
    window.courier.batchSignFor = function () {
        var Id = Table.getIdSelections($("#table"));

        if (Id == "" || Id.length <= 0) {
            alert("没有选中任何数据。");
            return;
        }

        var ID = Table.getIdSelectionsString($("#table"), "ID");

        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_BatchSignFor',
                id: ID
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        $("a[name='list']").click();
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }
    /*******************************会员中心 我的快递 begin*********************************/

    /*******************************会员中心 消息 begin*********************************/
    if (!window.message) {
        window.message = {};
    }

    //加载数据
    window.message.loadData = function (offset, page) {
        call_ajax({
            url: window.location.href,
            data: {
                action: 'M_LoadData',
                offset: offset,
            },
            success: function (data) {
                var responseData = strToJson(data);

                if (responseData) {
                    if (responseData.Result === "0") {
                        var messageData = responseData.Data.rows;
                        var limit = parseInt(responseData.Limit);
                        var count = responseData.Data.total;
                        var html = "";

                        for (var i = 0; i < messageData.length; i++) {
                            if (messageData[i]["IsRead"] == "0") {
                                html += "<li class='unread' title='未读信息'>";
                            } else {
                                html += "<li title='已读信息'>";
                            }

                            html += "   <a onclick=\"window.message.readMessage(" + messageData[i]["ID"] + ",'" + messageData[i]["IsRead"] + "','" + messageData[i]["Url"] + "');\">";

                            if (messageData[i]["IsRead"] == "0") {
                                html += "   <i class='fa fa-envelope fl'></i>";
                            } else {
                                html += "   <i class='fa fa-envelope-open fl'></i>";
                            }

                            switch (messageData[i]["Modular"]) {
                                case "快递管理":
                                    html += "   <p class='fl plate btn-green'>" + messageData[i]["Modular"] + "</p>";
                                    break;
                                case "系统信息":
                                    html += "   <p class='fl plate btn-yellow'>" + messageData[i]["Modular"] + "</p>";
                                    break;
                                case "我的交易":
                                    html += "   <p class='fl plate btn-Lblue'>" + messageData[i]["Modular"] + "</p>";
                                    break;
                                case "我的需求":
                                    html += "   <p class='fl plate btn-red'>" + messageData[i]["Modular"] + "</p>";
                                    break;
                                case "用户审核":
                                    html += "   <p class='fl plate btn-blue'>" + messageData[i]["Modular"] + "</p>";
                                    break;
                            }

                            if (messageData[i]["Title"].length > 90) {
                                html += "   <p class='fl con'>" + messageData[i]["Title"].substring(0, 90) + "..." + "</p>";
                            } else {
                                html += "   <p class='fl con'>" + messageData[i]["Title"] + messageData[i]["State"] + "</p>";
                            }

                            html += "   <span class='fr m-time'>" + createDate(messageData[i]["CreateTime"]).Format("yyyy-MM-dd") + "</span>";
                            html += "   </a>";
                            html += "</li>";
                        }

                        $("#messagelist").html("");
                        $("#messagelist").append(html);

                        if (count <= limit) {
                            getStartedInitialization(page, 1);
                        } else {
                            if (count % responseData.Limit > 0) {
                                getStartedInitialization(page, ((count / limit) + 1));
                            } else {
                                getStartedInitialization(page, (count / limit));
                            }
                        }
                    } else {
                        alert(responseData.errMsg);
                    }
                }
            }
        });
    }

    //读取消息
    window.message.readMessage = function (id, isRead, url) {
        if (isRead == "0") {
            call_ajax({
                url: window.location.href,
                data: {
                    action: 'M_ReadMessage',
                    id: id
                },
                success: function (data) {
                    var responseData = strToJson(data);

                    if (responseData) {
                        if (responseData.Result === "0") {
                            window.location.href = url;
                        } else {
                            alert(responseData.errMsg);
                        }
                    }
                }
            });
        } else {
            window.location.href = url;
        }
    }
    /*******************************会员中心 消息 begin*********************************/









    /*******************************会员中心 修改密码 begin*********************************/
    if (!window.updatepassword) {
        window.updatepassword = {};
    }
    //提交密码修改
    window.updatepassword.submit = function () {
        var pword1 = $("input[name='oldpassword']").val();
        var pword2 = $("input[name='newpassword1']").val();
        var pword3 = $("input[name='newpassword2']").val();
        if (!pword1) {
            alert("请输入原密码");
            return;
        }

        if (pword1.length < 6 || pword1.length > 12) {
            alert("密码必须在6-12位之间请重新输入");
            return;
        }
        if (!pword2) {
            alert("请输入新密码");
            return;
        }
        if (pword2.length < 6 || pword2.length > 12) {
            alert("密码必须在6-12位之间请重新输入");
            return;
        }
        if (pword2 == pword1) {
            alert("新密码与原密码相同，请重新输入新密码");
            return;
        }
        if (!pword3) {
            alert("请输入确认密码");
            return;
        }
        if (pword2 != pword3) {
            alert("密码与确认密码不一致请重新输入");
            return;
        }

        call_ajax({
            url: location.href,
            data: {
                action: "m_update",
                pass1: pword1,
                pass2: pword2
            },
            success: function (data) {
                var aaData = strToJson(data);
                var data = "";
                if (aaData) {
                    if (aaData.Result === "0") {
                        alert("密码修改成功。");

                        $("input[name='oldpassword']").val("");
                        $("input[name='newpassword1']").val("");
                        $("input[name='newpassword2']").val("");
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    /*******************************会员中心 修改密码 end*********************************/

    /*******************************会员中心 培训报名 begin*********************************/
    if (!window.train) {
        window.train = {};
    }
    //提交报名
    window.train.submit = function () {
        var trainnumber = $("input[name='TrainNumber']").val();
        if (!trainnumber) {
            alert("请输入培训人数。");
            return;
        }

        var result = true;
        $("#list input,select").each(function (i) {
            var index = $("#list tr").index($(this).parent().parent()) + 1;

            var ismust = $(this).attr("ismust");
            if (ismust === "true") {
                if (!this.value) {
                    alert("参训人员:请输入第" + index + "行" + $(this).attr("desc"));
                    result = false;
                    return false;
                }
            }

            if ($(this).attr("desc") == "手机") {
                if (!CheckMobile(this.value)) {
                    alert("参训人员:请输入第" + index + "行" + $(this).attr("desc") + "格式不正确，请重新输入。");
                    result = false;
                    return false;
                }
            }

            if ($(this).attr("desc") == "邮箱") {
                if (this.value) {
                    if (!CheckMail(this.value)) {
                        alert("参训人员:请输入第" + index + "行" + $(this).attr("desc") + "格式不正确，请重新输入。");
                        result = false;
                        return false;
                    }
                }
            }
            if ($(this).attr("desc") == "身份证号") {
                if (this.value) {
                    if (!CheckIDNumber(this.value)) {
                        alert("参训人员:请输入第" + index + "行" + $(this).attr("desc") + "格式不正确，请重新输入。");
                        result = false;
                        return false;
                    }
                }
            }
        });

        if (!result) return;


        if (!confirm("确认要提交吗?")) {
            return;
        }

        call_ajax({
            url: location.href,
            data: {
                action: "m_save",
                info: JSON.stringify($("#info").serializeJson()),
                list: JSON.stringify(serializeTable($("#memberlist").serializeArray(), 6))
            },
            success: function (data) {
                var aaData = strToJson(data);
                var data = "";
                if (aaData) {
                    if (aaData.Result === "0") {
                        alert("保存成功");
                        location.href = "member-train-list";
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //加载
    window.train.loadlist = function (type) {

        var option = {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "m_load";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "ID",
            height: $("body").height() - 250,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [
                {
                    field: 'TrainingCourseName',
                    title: '培训项目',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'TrainingCourseDate',
                    title: '培训时间',
                    datatype: 'datetime',
                    ext: 'like'
                }
            ],
            columns: [
                {
                    field: '_ckb_',
                    title: '',
                    checkbox: true,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'ID',
                    title: '',
                    visible: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'Title',
                    title: '标题',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        //var html = "<a class='color-blue'  href=\"member-train-detail-" + row.ID + "-readonly\">" + value + "</a>";
                        //return html;
                        return value;
                    }
                }, {
                    field: 'TrainingCourseName',
                    title: '培训项目',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'TrainingCourseTeacher',
                    title: '主讲老师',
                    sortable: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'ApplyNum',
                    title: '已报人数',
                    sortable: false,
                    valign: 'middle',
                    align: 'center',
                    formatter: function (value, row, index) {
                        if (!value) {
                            value = 0;
                        }
                        return value;
                    }
                }, {
                    field: 'TrainingCourseDate',
                    title: '培训时间',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            var date = createDate(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'TrainingCourseExpiryDate',
                    title: '报名截止日',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            var date = createDate(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'operate',
                    title: '操作',
                    align: 'center',
                    valign: 'middle',
                    events: {
                        'click #apply': function (e, value, row, index) {
                            window.train.apply(row.ID)
                        },
                        'click #applied': function (e, value, row, index) {
                            window.train.apply(row.ID, row.IsApply)
                        }
                    },
                    formatter: function (value, row, index) {

                        return [
                            '<a class="table-btn btn-orange" id="applied" href="javascript:void(0)">查询信息</a>'
                        ].join('');


                    }
                }
            ]
        };
        Table.loadTable($("#table"), option);
    }
    //跳转到详细页面
    window.train.apply = function (trainid, applyid) {
        if (applyid) {
            location.href = "member-train-detail-" + trainid + "-" + applyid;
        } else {
            location.href = "member-train-detail-" + trainid;
        }
    }
    //加载信息
    window.train.loaddetail = function () {
        call_ajax({
            url: location.href,
            data: {
                action: "m_load"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        //报名明细信息
                        var data = aaData.Data.rows;
                        for (var item in data[0]) {
                            switch (item) {
                                default:
                                    $("input[name='" + item + "']").val(data[0][item]);
                                    break;
                            }
                        }

                        //参训人员
                        var mdata = aaData.MemberData.rows;
                        $("#list").html("");
                        for (var i = 0; i < mdata.length; i++) {
                            var classs = "";
                            if (i % 2 == 0)
                                classs = "tableback";

                            var html = "";
                            html += "<tr role='detail" + i + "'>";
                            html += "<td><input name='ID' type='hidden' value=" + mdata[i]["ID"] + " />";
                            html += "    <input name='Name' ismust ='true' desc='姓名' type='text' value=" + mdata[i]["Name"] + " /></td>";
                            html += "<td>";
                            html += '<select name="Duty" ismust="true" desc="职务">';
                            html += '<option value="">-请选择-</option>';
                            html += '<option value="董事长">董事长</option>';
                            html += '<option value="总经理">总经理</option>';
                            html += '<option value="部门主管">部门主管</option>';
                            html += '<option value="行政">行政</option>';
                            html += '<option value="IP主管">IP主管</option>';
                            html += '<option value="IP专员">IP专员</option>';
                            html += '<option value="助理">助理</option>';
                            html += '<option value="财务">财务</option>';
                            html += '<option value="销售">销售</option>';
                            html += '</select>';
                            html += '</td>';
                            html += "<td><input name='MobilePhone' ismust ='true' desc='手机' type='text' value=" + mdata[i]["MobilePhone"] + " /></td>";
                            html += "<td><input name='EMail' ismust ='false' desc='邮箱' type='text' value='" + mdata[i]["EMail"] + "' /></td>";
                            html += "<td><input name='IDNumber' ismust ='false' desc='身份证号' type='text' value='" + mdata[i]["IDNumber"] + "'  onkeyup=\"value=value.replace(/[^\\w\\.\\/]/ig,'')\" /></td>";
                            html += "<td><a onclick=\"\" class='tableminus'><i class='fa fa-minus'></i></a></td>";
                            html += "</tr>";

                            $("#list").append(html);

                            $("tr[role='detail" + i + "']").find("select[name='Duty']").val(mdata[i]["Duty"]);
                        }
                        setcontrolDisabled("memberlist", "disable");//页面控件不可用
                        $(".tableadd").attr('onclick', '').unbind('click');//增加行不可用
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //取消报名
    window.train.cancelapply = function () {

        if (!confirm("是否确认取消此培训报名？")) {
            return;
        }

        call_ajax({
            url: location.href,
            data: {
                action: "m_cancelapply"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        alert("取消报名成功。");
                        location.href = "member-train-list";
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    /*******************************会员中心 培训报名 end*********************************/



    /*******************************会员中心 用户审核 begin*********************************/
    if (!window.userauthentication) {
        window.userauthentication = {};
    }
    //提交校验
    window.userauthentication.submit_check = function () {
        switch (accounttype) {
            case "个人账户":
                if (!$("#RealName_Personal").val()) {
                    alert("真实姓名不能为空");
                    return false;
                }
                if (!$("#IDNumber_Personal").val()) {
                    alert("身份证号码不能为空");
                    return false;
                }
                if (!CheckIDNumber($("#IDNumber_Personal").val())) {
                    alert("身份证号码格式不正确");
                    return false;
                }
                if (!$("#ZipCode_Personal").val()) {
                    alert("邮编不能为空");
                    return false;
                }
                if (!CheckZipCode($("#ZipCode_Personal").val())) {
                    alert("邮编格式不正确");
                    return false;
                }
                if (!$("#hdn_Province_Personal").val()) {
                    alert("请选择省份");
                    return false;
                }
                if (!$("#Address_Personal").val()) {
                    alert("请填写身份证地址");
                    return false;
                }
                if (!$("input[name='file_phono']").val()) {
                    alert("请选择上传身份证附件");
                    return false;
                }
                break;
            case "企业账户":
                if (!$("#Company_Enterprice").val()) {
                    alert("请填写企业名称");
                    return false;
                }
                if (!$("#CompanyXingZhi_Enterprice").val()) {
                    alert("请选择企业性质");
                    return false;
                }
                if ($("select[name='CompanyXingZhi']").val() == '其他') {
                    if (!$("input[name='companyxingzhiqita']").val()) {
                        alert("请填写企业性质。");
                        return false;
                    }
                }
                if (!$("#Address_Enterprice").val()) {
                    alert("请填写详细地址");
                    return false;
                }
                if (!$("#LegalRepresentative_Enterprice").val()) {
                    alert("请填写法人代表");
                    return false;
                }
                if (!$("#RegisteredCapital_Enterprice").val()) {
                    alert("请填写注册资本");
                    return false;
                }
                if ($("#CurrencyType_Enterprice").val() == '其他') {
                    if (!$("input[name='CurrencyType']").val()) {
                        alert("请填写注册资本的货币种类。");
                        return false;
                    }
                }
                if (!$("#RegistrationTime_Enterprice").val()) {
                    alert("请选择注册时间");
                    return false;
                }
                if (!$("input[name='file_businesslicensepath']").val()) {
                    alert("请选择上传营业执照附件");
                    return false;
                }
                if (!$("input[name='IsLongterm'][value='1']").is(":checked")) {
                    if (!$("#BusinessLicenseExpirationDate_Enterprice").val()) {
                        alert("请选择执照到期日期");
                        return false;
                    }
                }
                if (!$("input[name='file_openingpermitpath']").val()) {
                    alert("请选择上传开户许可证附件");
                    return false;
                }

                if (!$("input[name='EMail']").val()) {
                    alert("请填写邮箱(用于密码找回)");
                    return false;
                }
                if (!CheckMail($("input[name='EMail']").val())) {
                    alert("邮箱(用于密码找回)格式错误，请确认");
                    return false;
                }

                if ($("input[name='IsDefault']:checked").val() == "专员") {
                    if (!$("input[name='AssistantName']").val()) {
                        alert("请填写专员姓名");
                        return false;
                    }
                    if (!$("input[name='AssistantMobilePhone']").val()) {
                        alert("请填写专员联系方式");
                        return false;
                    }
                    if (!CheckTel($("input[name='AssistantMobilePhone']").val()) && !CheckMobile($("input[name='AssistantMobilePhone']").val())) {
                        alert("专员联系方式格式错误，请确认");
                        return false;
                    }
                    if (!$("input[name='AssistantEMail']").val()) {
                        alert("请填写专员邮箱");
                        return false;
                    }
                    if (!CheckMail($("input[name='AssistantEMail']").val())) {
                        alert("专员邮箱格式错误，请确认");
                        return false;
                    }
                    if (!$("input[name='AssistantQQ']").val()) {
                        alert("请填写专员QQ");
                        return false;
                    }
                    if (isNaN($("input[name='AssistantQQ']").val())) {
                        alert("专员QQ格式不正确");
                        return false;
                    }
                    if ($("input[name='DirectorMobilePhone']").val()) {
                        if (!CheckTel($("input[name='DirectorMobilePhone']").val()) && !CheckMobile($("input[name='DirectorMobilePhone']").val())) {
                            alert("主管联系方式格式错误，请确认");
                            return false;
                        }
                    }
                    if ($("input[name='DirectorEMail']").val()) {
                        if (!CheckMail($("input[name='DirectorEMail']").val())) {
                            alert("主管邮箱格式错误，请确认");
                            return false;
                        }
                    }
                    if ($("input[name='DirectorQQ']").val()) {
                        if (isNaN($("input[name='DirectorQQ']").val())) {
                            alert("主管QQ格式不正确");
                            return false;
                        }
                    }
                } else {
                    if (!$("input[name='DirectorName']").val()) {
                        alert("请填写主管姓名");
                        return false;
                    }
                    if (!$("input[name='DirectorMobilePhone']").val()) {
                        alert("请填写主管联系方式");
                        return false;
                    }
                    if (!CheckTel($("input[name='DirectorMobilePhone']").val()) && !CheckMobile($("input[name='DirectorMobilePhone']").val())) {
                        alert("主管联系方式格式错误，请确认");
                        return false;
                    }
                    if (!$("input[name='DirectorEMail']").val()) {
                        alert("请填写主管邮箱");
                        return false;
                    }
                    if (!CheckMail($("input[name='DirectorEMail']").val())) {
                        alert("主管邮箱格式错误，请确认");
                        return false;
                    }
                    if (!$("input[name='DirectorQQ']").val()) {
                        alert("请填写主管QQ");
                        return false;
                    }
                    if (isNaN($("input[name='DirectorQQ']").val())) {
                        alert("主管QQ格式不正确");
                        return false;
                    }
                    if (!$("input[name='IsDefault']:checked").val()) {
                        alert("请选择默认联系人");
                        return false;
                    }
                    if ($("input[name='AssistantMobilePhone']").val()) {
                        if (!CheckTel($("input[name='AssistantMobilePhone']").val()) && !CheckMobile($("input[name='AssistantMobilePhone']").val())) {
                            alert("专员联系方式格式错误，请确认");
                            return false;
                        }
                    }
                    if ($("input[name='AssistantEMail']").val()) {
                        if (!CheckMail($("input[name='AssistantEMail']").val())) {
                            alert("专员邮箱格式错误，请确认");
                            return false;
                        }
                    }
                    if ($("input[name='AssistantQQ']").val()) {
                        if (isNaN($("input[name='AssistantQQ']").val())) {
                            alert("专员QQ格式不正确");
                            return false;
                        }
                    }
                }
                if (!$("#ZipCode_Enterprice").val()) {
                    alert("邮编不能为空");
                    return false;
                }
                if (!CheckZipCode($("#ZipCode_Enterprice").val())) {
                    alert("邮编格式不正确");
                    return false;
                }
                if (!$("#PostalAddress_Enterprice").val()) {
                    alert("请填写通讯地址");
                    return false;
                }
                break;
            default:
                alert("无法获取提交类型。");
                return false;
        }

        return true;
    }
    //提交修改
    window.userauthentication.submit = function () {
        if (window.userauthentication.submit_check() === false)
            return;
        if (!confirm("是否确认提交认证信息？")) {
            return;
        }

        var formid = "";
        switch (accounttype) {
            case "个人账户":
                formid = "form_personal";
                break;
            case "企业账户":
                formid = "form_enterprice";
                break;
            default:
                alert("无法获取提交类型。");
                return;
        }

        call_form({
            formid: formid,
            forceSync: true,    //强制同步
            async: false,
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData.Result === "0") {
                    alert("提交成功");
                    location.href = "member-portal";
                }
                else {
                    alert(aaData.errMsg);
                }
            }
        });
    }
    //加载
    window.userauthentication.load = function () {
        call_ajax({
            url: location.href,
            data: {
                action: "m_load"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        var data = aaData.Data.rows[0];
                        var formid = "", filePath = "", selectProvince = "", photo = "";//个人账户（photo身份证） 企业账户（BusinessLicensePath营业执照）
                        switch (accounttype) {
                            case "个人账户":
                                formid = "form_personal";
                                filePath = "file_phono";
                                selectProvince = "Province_Personal";
                                photo = "certificate";
                                break;
                            case "企业账户":
                                formid = "form_enterprice";
                                filePath = "file_businesslicensepath";
                                selectProvince = "Province_Enterprice";
                                photo = "license";
                                break;
                            default:
                                alert("无法获取提交类型。");
                                return;
                        }
                        //如果认证信息不存在，则将注册信息带入到认证页面
                        if (!data) {
                            data = aaData.UserData.rows[0];
                        } else {
                            setcontrolDisabled("form_enterprice", "disable");//页面控件不可用
                            setcontrolDisabled("form_personal", "disable");//页面控件不可用
                            $("#" + formid + " [role='" + photo + "']").hide();
                            $("#" + formid + " [role='openingpermit']").hide();
                            $("a[role='submit']").hide();
                        }
                        for (var item in data) {
                            if (data[item]) {
                                switch (item) {
                                    case "Province":
                                        $("#" + formid + "  #" + selectProvince + " option").each(function () {
                                            if ($(this).text() == data[item]) {
                                                $(this).attr("selected", "selected");
                                            }
                                        });

                                        $("#" + formid + " input[name='Province']").val(data[item]);
                                        break;
                                    case "CompanyXingZhi":
                                        var isqita = true;
                                        $("select[name='" + item + "'] option").each(function () {
                                            if ($(this).text() == data[item]) {
                                                isqita = false;
                                                $(this).attr("selected", "selected");
                                            }
                                        });
                                        if (isqita) {
                                            $("select[name='" + item + "']").removeClass("in-w60");
                                            $("select[name='" + item + "']").addClass("in-w20");
                                            $("input[name='companyxingzhiqita']").show();

                                            $("select[name='" + item + "']").val("其他");
                                            $("input[name='companyxingzhiqita']").val(data[item]);
                                        }

                                        break;
                                    case "CurrencyType":
                                        var isqita = true;
                                        $("#" + item + "_Enterprice option").each(function () {
                                            if ($(this).text() == data[item]) {
                                                isqita = false;
                                                $(this).attr("selected", "selected");
                                            }
                                        });
                                        if (isqita) {
                                            $("#" + item + "_Enterprice").removeClass("in-w40");
                                            $("#" + item + "_Enterprice").addClass("in-w20");
                                            $("input[name='CurrencyType']").show();

                                            $("#" + item + "_Enterprice").val("其他");
                                            $("input[name='CurrencyType']").val(data[item]);
                                        }

                                        break;
                                    case "ShiFanQiYe":
                                    case "YouShiQiYe":
                                    case "GuanBiaoQiYe":
                                    case "CompanyXingZhi":
                                        $("select[name='" + item + "']").val(data[item]);
                                        break;
                                    case "RegistrationTime":
                                    case "BusinessLicenseExpirationDate":
                                        var date = createDate(data[item]);
                                        $("input[name='" + item + "']").val(date.Format("yyyy-MM-dd"));
                                        break;
                                    case "BusinessLicensePath":
                                    case "OpeningPermitPath":
                                    case "Phono":
                                        $("span[role='img_" + item.toLowerCase() + "']").show();
                                        $("#img_" + item.toLowerCase() + "").attr("src", data[item].replace("~/", getRootPath() + "/"));
                                        $("input[name='file_" + item.toLowerCase() + "']").val(data[item]);
                                        $("img[name='file_" + item.toLowerCase() + "']").attr("src", data[item].replace("~/", getRootPath() + "/"));
                                        break;
                                    case "CheckState":
                                        var state = "";
                                        switch (data[item]) {
                                            case "1":
                                                state = "正在审核";
                                                break;
                                            case "2":
                                                state = "审核通过";
                                                break;
                                            case "3":
                                                state = "拒绝";
                                                break;
                                            case "0":
                                                state = "未申请";
                                                break;
                                        }
                                        $("li[role='" + item + "']").show();
                                        $("label[name='" + item + "']").text(state);
                                        break;
                                    case "RefuseReason":
                                        if (data[item]) {
                                            $("a[role='resubmit']").show();
                                            $("ul[role='refusereason']").show();
                                            $("label[name='" + item + "']").text(data[item]);
                                        }
                                        break;
                                    case "IsBusinessLicenseLongterm":
                                        if (data[item] == '1') {
                                            $("input[name='IsLongterm'][value='1']").prop("checked", true);
                                            $("input[name='" + item + "").val(data[item]);
                                        }
                                        break;
                                    default:
                                        $("#" + formid + " input[name='" + item + "']").val(data[item]);
                                        break;
                                }
                            }
                        }



                        //联系人信息
                        var data = aaData.LinkManData.rows;
                        for (var i = 0; i < data.length; i++) {
                            if (data[i]["LinkMainType"] == "专员") {
                                $("input[name='AssistantID']").val(data[i]["ID"]);
                                $("input[name='AssistantName']").val(data[i]["LinkMan"]);
                                $("input[name='AssistantMobilePhone']").val(data[i]["MobilePhone"]);
                                $("input[name='AssistantEMail']").val(data[i]["EMail"]);
                                $("input[name='AssistantQQ']").val(data[i]["QQ"]);
                                if (data[i]["IsDefault"] == 1)
                                    $("input[name='IsDefault'][value='" + data[i]["LinkMainType"] + "']").attr("checked", true);
                                else
                                    $("input[name='IsDefault'][value='" + data[i]["LinkMainType"] + "']").attr("checked", false);
                            }
                            if (data[i]["LinkMainType"] == "主管") {
                                $("input[name='DirectorID']").val(data[i]["ID"]);
                                $("input[name='DirectorName']").val(data[i]["LinkMan"]);
                                $("input[name='DirectorMobilePhone']").val(data[i]["MobilePhone"]);
                                $("input[name='DirectorEMail']").val(data[i]["EMail"]);
                                $("input[name='DirectorQQ']").val(data[i]["QQ"]);
                                if (data[i]["IsDefault"] == 1)
                                    $("input[name='IsDefault'][value='" + data[i]["LinkMainType"] + "']").attr("checked", true);
                                else
                                    $("input[name='IsDefault'][value='" + data[i]["LinkMainType"] + "']").attr("checked", false);
                            }
                        }
                        //重新提交认证
                        if (type == "1") {
                            $("a[role='resubmit']").click();
                        }
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //选中省市，如果为‘-请选择-’，返回空
    window.userauthentication.checkselectvalue = function (value) {
        if (value == "-请选择-" || value == "")
            return "";
        else
            return value;
    }
    /*******************************会员中心 用户审核 end*********************************/



    /*******************************会员中心 企业资助申报 begin*********************************/
    if (!window.funding) {
        window.funding = {};
    }

    var message = {
        fundingsubmitmessage: "资助申报信息提交后不可修改，是否确认提交？",
        fundingsavemessage: "是否确认保存？",
    }



    //页面加载 获取批次号
    window.funding.pageinit = function () {
        call_ajax({
            url: location.href,
            async: false,
            data: {
                action: "m_funding_init"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        $("#BatchNo").text(aaData.BatchNo);
                    }
                    else {
                        alert(aaData.errMsg);
                        //window.history.go(-1);
                        parent.window.funding.closetab(GetQueryString(4));
                    }
                }
            }
        });
    }
    //专利企业资助申报列表页面，进入到其详细页面
    window.funding.topadetail = function (url, id) {
        if (id)
            url += "-" + id;

        Tabs.addTabs({
            id: "detail",
            title: "详情",
            url: url,
            close: true
        });
    }
    //专利企业资助申报列表页面，进入到 添加 页面
    window.funding.topaadd = function (url) {
        Tabs.addTabs({
            id: "add",
            title: "添加",
            url: url,
            close: true
        });
    }
    //专利企业资助申报列表页面，进入到其详细页面
    window.funding.detail = function (page) {

        var id = Table.getIdSelections($("#table"));
        if (id == "") {
            alert("没有选中任何数据");
            return;
        }
        if (id.length < 0) {
            alert("没有选中任何数据");
            return;
        }
        if (id.length > 1) {
            alert("只可编辑一条数据");
            return;
        }

        var ID = Table.getIdSelectionsString($("#table"), "ID");
        var state = Table.getIdSelectionsString($("#table"), "State");
        var url = "";

        switch (state) {
            case "补正":
                url = "member-funding-" + page + "-detail-edite-" + ID;
                break;
            case "暂存":
                url = "member-funding-" + page + "-detail-save-" + ID;
                break;
            default:
                alert("此数据当前状态不可编辑。");
                return;
                break;
        }

        switch (page) {
            case "case03":
                var iptype = Table.getIdSelectionsString($("#table"), "IPType");

                switch (iptype) {
                    case "商标":
                        url += "-TM";
                        break;
                    case "专利":
                        url += "-PA";
                        break;
                    case "版权":
                        url += "-CO";
                        break;
                }
                break;
        }
        window.funding.topadetail(url);
    }

    //打印功能
    window.funding.printing = function (id) {

        call_ajax({
            url: location.href,
            async: false,
            data: {
                action: "m_funding_printing",
                listid: id
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        window.open(aaData.Url, "_blank");
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //校验qq 邮箱、电话格式(如果填写就校验)
    window.funding.check = function () {
        var arr = ["Telphone", "QQ", "EMail"];
        for (var i = 0; i < arr.length; i++) {
            $("input[name='" + arr[i] + "']").attr("ismust", "false");
            if ($("input[name='" + arr[i] + "']").val()) {
                $("input[name='" + arr[i] + "']").attr("ismust", "true");
            }
        }
    }

    //竞争对手专利-关闭tab页
    window.funding.closetab = function (type, tabtype) {
        switch (type) {
            case "add":
                type = "add";
                break;
            case "edite":
            case "read":
            case "save":
                type = "detail";
                break;
        }

        Tabs.closeTab(type);
        if (tabtype) {
            $("#" + tabtype).click();
        } else {
            $("#submit").click();
        }
    }

    /******************* 专利费用资助申报 begin*****************/
    if (!window.funding.case01) {
        window.funding.case01 = {};
    }

    //加载列表
    window.funding.case01.loadlist = function (type) {

        var option = {
            url: window.location.href + "-" + type,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "m_load";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "ID",
            height: $("body").height() - 400,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [
                {
                    field: 'BatchNo',
                    title: '申报批次',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'CreateTime',
                    title: '申报时间',
                    datatype: 'datetime',
                    ext: 'like'
                },
                {
                    field: 'State',
                    title: '流程状态',
                    datatype: 'string',
                    ext: 'like'
                }
            ],
            columns: [
                {
                    field: '_ckb_',
                    title: '',
                    checkbox: true,
                    valign: 'middle',
                    align: 'center'
                },
                {
                    field: 'ID',
                    title: '',
                    visible: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'ApplicantName',
                    title: '申报人名称',
                    sortable: false,
                    valign: 'middle',
                    align: 'center',
                    events: {
                        'click #todetail': function (e, value, row, index) {

                            switch (row.State) {
                                case "补正":
                                    window.funding.topadetail("member-funding-case01-detail-edite", row.ID);
                                    break;
                                case "暂存":
                                    window.funding.topadetail("member-funding-case01-detail-save", row.ID);
                                    break;
                                default:
                                    window.funding.topadetail("member-funding-case01-detail-read", row.ID);
                                    break;
                            }
                        }
                    },
                    formatter: function (value, row, index) {
                        switch (row.State) {
                            case "暂存":
                                return [
                                    '<a class="color-blue" id="todetail" href="javascript:void(0)">' + value + '</a>'
                                ].join('');
                                break;
                            default:
                                return [
                                    '<a class="color-blue" id="todetail" href="javascript:void(0);">' + value + '</a>'
                                ].join('');
                                break;
                        }
                    }
                }, {
                    field: 'BatchNo',
                    title: '申报批次',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'CreateTime',
                    title: '申报时间',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            var date = createDate(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'LinkMan',
                    title: '联系人',
                    sortable: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'State',
                    title: '流程状态',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }
            ]
        };
        Table.loadTable($("#table"), option);
    }
    //提交
    window.funding.case01.submit = function (type) {
        if (!verifydata())
            return;

        if (!$("input[name='FilePath']").val() && !$("input[name='File']").val()) {
            alert("请上传申报材料。");
            return;
        }

        if (type == 'submit') {
            if (!confirm(message.fundingsubmitmessage))
                return;
        } else {
            if (!confirm(message.fundingsavemessage))
                return;
        }

        call_form({
            formid: "info",
            url: window.location.href,
            forceSync: true,    //强制同步
            async: false,
            data: {
                action: "m_save",
                sbmittype: type,
                painfo: JSON.stringify(serializeTable(tableSerializeArray('palist'), 5))
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData.Result === "0") {
                    alert("操作成功");
                    parent.window.funding.closetab(GetQueryString(4), type);
                }
                else {
                    alert(aaData.errMsg);
                }
            }
        });
    }
    //选择附件
    window.funding.case01.choicefile = function () {
        $("input[name='FilePath'][type='file']").click();
    }
    //附件控件上传图片发生变化
    window.funding.case01.filechange = function (index) {

        if (!checkfilemaxsize($("input[name='File'][type='file']")[0], filemaxsize)) {
            alert("选择的文件太大了, 最大允许上传" + filemaxsize + "KB");
            return;
        }

        var extension = $("input[name='File'][type='file']").val().substring($("input[name='File'][type='file']").val().lastIndexOf('.')).toLowerCase();
        var extensionstr = ".jpg, .jpeg, .png, .bmp, .gif, .rar, .zip, .doc, .docx, .xls, .xlsx, .pdf";
        if (extensionstr.indexOf(extension) >= 0) {
            $("#text").html($("input[name='File'][type='file']").val());
        }
        else {
            alert("请上传" + extensionstr + "格式的文件");
            return;
        }

    }
    //专利汇总表 增加一行
    window.funding.case01.addtr = function (obj) {
        //如果保提交钮隐藏 或者 不存在提交按钮，该功能不可使用
        if ($("a[role='submit']").is(":hidden") || $("a[role='submit']").length <= 0) {
            return;
        }
        var tableid = "palist";//获取tbody的id
        var role = $("#" + tableid).find("tr:last").attr("role");//获取tbody下最后一个tr的role     
        var rolenum = "";//role后缀数字
        if (!role) {//如果没有tr，给role赋初始值
            rolenum = "0";
        } else {
            rolenum = role.substring(role.lastIndexOf('_') + 1, role.length);//role后缀数字
            rolenum++;//数字加1
        }

        role = tableid.substring(tableid.lastIndexOf('_') + 1, tableid.length) + "_" + rolenum;//拼接全新role

        var html = "";

        html += '<tr role="' + role + '">';
        html += '    <td></td>';
        html += '    <td><input name="PID" type="hidden" />';
        html += '        <input name="Name" type="text" ismust="true" desc="申请人/权利人" maxlength="450" /></td>';
        html += '    <td>';
        html += '        <input name="PAName" type="text" ismust="true" desc="专利名称" maxlength="200" /></td>';
        html += '    <td>';
        html += '        <input name="ApplyNo" type="text" ismust="true" issqh="true" desc="申请号/专利号" maxlength="20" /></td>';
        html += '    <td>';
        html += '        <select name="PAType" ismust="true" desc="专利类型">';
        html += '            <option value="">-请选择-</option>';
        html += '            <option value="发明专利">发明专利</option>';
        html += '            <option value="实用新型专利">实用新型专利</option>';
        html += '            <option value="外观设计专利">外观设计专利</option>';
        html += '             <option value="PCT发明">PCT发明</option>';
        html += '             <option value="PCT实用新型">PCT实用新型</option>';
        html += '        </select>';
        html += '    </td>';
        html += '    <td><a role="btn_removetr" class="tableminus"><i class="fa fa-trash"></i></a></td>';
        html += '</tr>';

        $("#" + tableid).append(html);

        //计算行号、每行样式
        $("#" + tableid).find("tr").each(function (i) {//寻找当前tbody下所有行数（tr标签）
            var index = i + 1; //每行（tr）下第一列（td）显示的行数
            $(this).find("td:first").text(index);//给td标签赋值 行数
            $(this).find("[ismust='true']").attr("isrownum", index);
        });
    }
    //专利汇总表 删除一行
    window.funding.case01.removetr = function (obj) {
        //如果保提交钮隐藏 或者 不存在提交按钮，该功能不可使用
        if ($("a[role='submit']").is(":hidden") || $("a[role='submit']").length <= 0) {
            return;
        }
        $("tr[role='" + $(obj).parents('tr:first').attr("role") + "']").remove();//删除行

        $("#palist").find("tr").each(function (i) {//寻找当前tbody下所有行数（tr标签）
            var index = i + 1; //每行（tr）下第一列（td）显示的行数          
            $(this).find("td:first").text(index);//给td标签赋值 行数
            $(this).find("[ismust='true']").attr("isrownum", index);
        });
    }
    //详细页面
    window.funding.case01.loaddetail = function (type) {

        switch (type) {
            case "read":
                $("a[role='edite']").remove();//编辑按钮移除   
                $("a[role='submit']").remove();//提交按钮移除    
                $("a[role='save']").remove();//保存按钮移除  
                $("a[role='printing']").show();//打印按钮显示

                $(".modifyInfo_upload").hide();
                $("a[role='btn_downloadfile']").show()
                $("span[class='color-red fblod']").hide();//移除必填符号

                break;
            case "edite":
                $("a[role='edite']").show();//编辑按钮显示  
                $("a[role='save']").remove();//保存按钮移除   
                $("a[role='printing']").show();//打印按钮显示
                $("a[role='submit']").hide();//提交按钮隐藏
                $("span[class='color-red fblod']").hide();//移除必填符号
                break;
            case "save":
                $("a[role='printing']").remove();//打印按钮移除 
                $("a[role='edite']").remove();//编辑按钮移除    
                break;
            default:
                break;
        }

        call_ajax({
            url: location.href,
            data: {
                action: "m_load"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        //明细信息
                        var data = aaData.Data.rows;
                        for (var item in data[0]) {
                            if (data[0][item]) {
                                switch (item) {
                                    case "PACertificate":
                                    case "IntroductionLetter":
                                    case "AgentIDCard":
                                    case "PatenteeIDCard":
                                    case "EvidenceMaterial":
                                        if (data[0][item] == "1") {
                                            $("input[name='" + item + "']").prop("checked", true);
                                        }
                                        break;
                                    case "OnlineDeclaration":
                                        if (data[0][item]) {
                                            $("input[name='" + item + "']").each(function () {
                                                if ($(this).val() == data[0][item]) {
                                                    $(this).prop("checked", true);
                                                }
                                            });
                                        }
                                        break;
                                    case "FilePath":
                                        if (data[0][item]) {
                                            $("#text").html(data[0][item]);
                                            $("input[name='" + item + "']").val(data[0][item]);
                                            $("a[role='btn_downloadfile']").attr("path", data[0][item]);
                                        }
                                        break;
                                    case "State":
                                        $("li[role='" + item + "']").show();
                                        $("#" + item).text(data[0][item]);
                                        break;
                                    case "AuditOpinion":
                                        if (data[0]["State"] == '审核未通过' || data[0]["State"] == '补正') {
                                            $("li[role='" + item + "']").show();
                                            $("#" + item).text(data[0][item]);
                                        }
                                        break;
                                    case "Amount":
                                        $("input[name='Amount']").val(data[0][item]);
                                        break;
                                    case "AmountUpper":
                                    case "BatchNo":
                                        $("#" + item).text(data[0][item]);
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                    case "ApprovedAmount"://部分通过审核金额
                                        if (data[0][item]) {
                                            $("#ApprovedAmount").text("￥" + data[0][item] + "元");
                                            $("li[role='AuditOpinion']").show();
                                            $("#AuditOpinion").text(data[0]["ApprovedAuditOpinion"]);
                                            $("div[role='div_approvedamount']").show();
                                        }
                                        break;
                                    default:
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                }

                            }
                        }

                        var data = aaData.PAInfoData.rows;
                        $('#palist').html('');
                        for (var i = 0; i < data.length; i++) {
                            window.funding.case01.loadpalist(data[i], "palist");

                        }

                        if (type != 'save')//状态不为暂存时
                            setcontrolDisabled("info", "disable");//页面控件不可用
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //加载官费、代理费详细
    window.funding.case01.loadpalist = function (data, tableid) {

        //获取该table下最后一个tr的role值
        var role = $("#" + tableid).find("tr:last").attr("role");

        if (!role) {
            role = tableid + "_1";
        }
        //获取role值的数字部分（下标）
        var index = role.replace(tableid + "_", "");
        //下标自增1
        index++;

        var html = "";
        html += "<tr role='" + tableid + "_" + index + "'>";
        html += '    <td></td>';
        html += '    <td><input name="PID" type="hidden" value="' + data["ID"] + '" />';
        html += '        <input name="Name" type="text" ismust="true" desc="申请人/权利人" maxlength="450" value="' + data["Name"] + '" /></td>';
        html += '    <td>';
        html += '        <input name="PAName" type="text" ismust="true" desc="专利名称" maxlength="200" value="' + data["PAName"] + '" /></td>';
        html += '    <td>';
        html += '        <input name="ApplyNo" type="text" ismust="true" issqh="true" desc="申请号/专利号" maxlength="20" value="' + data["ApplyNo"] + '" /></td>';
        html += '    <td>';
        html += '        <select name="PAType" ismust="true" desc="专利类型">';
        html += '            <option value="">-请选择-</option>';
        html += '            <option value="发明专利">发明专利</option>';
        html += '            <option value="实用新型专利">实用新型专利</option>';
        html += '            <option value="外观设计专利">外观设计专利</option>';
        html += '             <option value="PCT发明">PCT发明</option>';
        html += '             <option value="PCT实用新型">PCT实用新型</option>';
        html += '        </select>';
        html += '    </td>';
        html += '    <td><a role="btn_removetr" class="tableminus"><i class="fa fa-trash"></i></a></td>';
        html += '</tr>';

        $("#" + tableid).append(html);

        //给专利类型赋值
        $("tr[role='" + tableid + "_" + index + "']").find('select[name="PAType"]').val(data["PAType"]);

        //计算行号、每行样式
        $("#" + tableid).find("tr").each(function (i) {//寻找当前tbody下所有行数（tr标签）
            var index = i + 1; //每行（tr）下第一列（td）显示的行数
            $(this).find("td:first").text(index);//给td标签赋值 行数
            $(this).find("[ismust='true']").attr("isrownum", index);
        });
    }
    //加载联系人数据
    window.funding.case01.linkmaninfo = function () {
        call_ajax({
            url: location.href,
            data: {
                action: "m_loadlinkman"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        //明细信息
                        var data = aaData.Data.rows;
                        for (var item in data[0]) {
                            if (data[0][item]) {
                                switch (item) {
                                    case "LinkMan":
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                    case "MobilePhone":
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                }

                            }
                        }
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //计算费用
    window.funding.case01.countfee = function () {

        /********需要资助的金额 begin**********/
        var amount = 0.00;
        $("#palist").find("select[name='PAType']").each(function () {
            switch ($(this).val()) {
                case "发明专利":
                case "PCT发明":
                    amount += parseFloat(3000);
                    break;
                case "实用新型专利":
                case "PCT实用新型":
                    amount += parseFloat(2000);
                    break;
                case "外观设计专利":
                    amount += parseFloat(1000);
                    break;
                default:
                    amount += parseFloat(0.00);
                    break;
            }
        });

        if ($("input[name='EvidenceMaterial'][type='checkbox']").is(':checked'))
            amount = amount * 2;
        /********需要资助的金额 end**********/

        $("#Amount").val(amount);
        $("label[name='AmountUpper']").text(digitUppercase(amount))
        $("input[name='AmountUpper']").val(digitUppercase(amount))
    }
    /******************* 专利费用资助申报 end********************/


    /******************* 知识产权申请申报 begin*****************/
    if (!window.funding.case02) {
        window.funding.case02 = {};
    }

    //加载列表
    window.funding.case02.loadlist = function (type) {

        var option = {
            url: window.location.href + "-" + type,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "m_load";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "ID",
            height: $("body").height() - 400,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [
                {
                    field: 'ProjectName',
                    title: '项目名称',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'CreateTime',
                    title: '申报时间',
                    datatype: 'datetime',
                    ext: 'like'
                },
                {
                    field: 'State',
                    title: '流程状态',
                    datatype: 'string',
                    ext: 'like'
                }
            ],
            columns: [
                {
                    field: '_ckb_',
                    title: '',
                    checkbox: true,
                    valign: 'middle',
                    align: 'center'
                },
                {
                    field: 'ID',
                    title: '',
                    visible: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'ProjectName',
                    title: '项目名称',
                    sortable: false,
                    valign: 'middle',
                    align: 'center',
                    events: {
                        'click #todetail': function (e, value, row, index) {

                            switch (row.State) {
                                case "补正":
                                    window.funding.topadetail("member-funding-case02-detail-edite", row.ID);
                                    break;
                                case "暂存":
                                    window.funding.topadetail("member-funding-case02-detail-save", row.ID);
                                    break;
                                default:
                                    window.funding.topadetail("member-funding-case02-detail-read", row.ID);
                                    break;
                            }
                        }
                    },
                    formatter: function (value, row, index) {
                        switch (row.State) {
                            case "暂存":
                                return [
                                    '<a class="color-blue" id="todetail" href="javascript:void(0)">' + value + '</a>'
                                ].join('');
                                break;
                            default:
                                return [
                                    '<a class="color-blue" id="todetail" href="javascript:void(0);">' + value + '</a>'
                                ].join('');
                                break;
                        }
                    }
                }, {
                    field: 'IPType',
                    title: '申请知产类型',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'CreateTime',
                    title: '申报时间',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            var date = createDate(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'State',
                    title: '流程状态',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }
            ]
        };
        Table.loadTable($("#table"), option);
    }
    //提交
    window.funding.case02.submit = function (type) {
        window.funding.check();

        if (!verifydata())
            return;

        if (type == 'submit') {
            if (!confirm(message.fundingsubmitmessage))
                return;
        } else {
            if (!confirm(message.fundingsavemessage))
                return;
        }

        call_form({
            formid: "info",
            url: window.location.href,
            forceSync: true,    //强制同步
            async: false,
            data: {
                action: "m_save",
                sbmittype: type
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData.Result === "0") {
                    alert("操作成功");
                    parent.window.funding.closetab(GetQueryString(4), type);
                }
                else {
                    alert(aaData.errMsg);
                }
            }
        });
    }
    //详细页面
    window.funding.case02.loaddetail = function (type) {

        switch (type) {
            case "read":
                $("a[role='edite']").remove();//编辑按钮移除   
                $("a[role='submit']").remove();//提交按钮移除    
                $("a[role='save']").remove();//保存按钮移除  
                $("a[role='printing']").show();//打印按钮显示
                $("span[class='color-red fblod']").hide();//移除必填符号
                break;
            case "edite":
                $("a[role='edite']").show();//编辑按钮显示  
                $("a[role='save']").remove();//保存按钮移除   
                $("a[role='printing']").show();//打印按钮显示
                $("a[role='submit']").hide();//提交按钮隐藏
                $("span[class='color-red fblod']").hide();//移除必填符号
                break;
            case "save":
                $("a[role='printing']").remove();//打印按钮移除 
                $("a[role='edite']").remove();//编辑按钮移除    
                break;
            default:
                break;
        }

        call_ajax({
            url: location.href,
            data: {
                action: "m_load"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        //明细信息
                        var data = aaData.Data.rows;
                        for (var item in data[0]) {
                            if (data[0][item]) {
                                switch (item) {
                                    case "State":
                                        $("li[role='" + item + "']").show();
                                        $("#" + item).text(data[0][item]);
                                        break;
                                    case "AuditOpinion":
                                        if (data[0]["State"] == '审核未通过' || data[0]["State"] == '补正') {
                                            $("li[role='" + item + "']").show();
                                            $("#" + item).text(data[0][item]);
                                        }
                                        break;
                                    case "BatchNo":
                                        $("#" + item).text(data[0][item]);
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                    case "CreateTime"://部分通过审核金额
                                        if (data[0][item]) {
                                            $("#" + item).text(createDate(data[0][item]).Format("yyyy-MM-dd HH:mm"));
                                        }
                                        break;
                                    case "ApplicationContent"://部分通过审核金额
                                        if (data[0][item]) {
                                            $("textarea[name='" + item + "']").val(data[0][item]);
                                        }
                                        break;
                                    case "IPType"://部分通过审核金额
                                        if (data[0][item]) {
                                            $("select[name='" + item + "']").val(data[0][item]);
                                        }
                                        break;
                                    case "ApplicationContent"://
                                        if (data[0][item]) {
                                            $("textarea[name='" + item + "']").val(data[0][item]);
                                        }
                                        break;
                                    default:
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                }

                            }
                        }

                        if (type != 'save')//状态不为暂存时
                            setcontrolDisabled("info", "disable");//页面控件不可用
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //加载联系人数据
    window.funding.case02.linkmaninfo = function () {
        call_ajax({
            url: location.href,
            data: {
                action: "m_loadlinkman"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        //明细信息
                        var data = aaData.Data.rows;
                        for (var item in data[0]) {
                            if (data[0][item]) {
                                switch (item) {
                                    case "LinkMan":
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                    case "MobilePhone":
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                    case "EMail":
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                    case "QQ":
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                }

                            }
                        }
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    /******************* 知识产权申请申报 end********************/


    /******************* 企业知识产权备案 begin*****************/
    if (!window.funding.case03) {
        window.funding.case03 = {};
    }

    //加载列表
    window.funding.case03.loadlist = function (type) {

        var option = {
            url: window.location.href + "-" + type,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "m_load";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "ID",
            height: $("body").height() - 400,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [
                {
                    field: 'Name',
                    title: '知识产权名称',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'ApplicantName',
                    title: '申请人',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'CreateTime',
                    title: '申报时间',
                    datatype: 'datetime',
                    ext: 'like'
                },
                {
                    field: 'State',
                    title: '流程状态',
                    datatype: 'string',
                    ext: 'like'
                }
            ],
            columns: [
                {
                    field: '_ckb_',
                    title: '',
                    checkbox: true,
                    valign: 'middle',
                    align: 'center'
                },
                {
                    field: 'ID',
                    title: '',
                    visible: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'Name',
                    title: '知识产权名称',
                    sortable: false,
                    valign: 'middle',
                    align: 'center',
                    events: {
                        'click #todetail': function (e, value, row, index) {
                            var iptype = row.IPType;
                            switch (iptype) {
                                case "商标":
                                    iptype = "TM";
                                    break;
                                case "专利":
                                    iptype = "PA";
                                    break;
                                case "版权":
                                    iptype = "CO";
                                    break;
                            }
                            var url = "member-funding-case03-detail";

                            switch (row.State) {
                                case "补正":
                                    url += "-edite-" + row.ID + "-" + iptype;
                                    break;
                                case "暂存":
                                    url += "-save-" + row.ID + "-" + iptype;
                                    break;
                                default:
                                    url += "-read-" + row.ID + "-" + iptype;
                                    break;
                            }

                            window.funding.topadetail(url);
                        }
                    },
                    formatter: function (value, row, index) {
                        switch (row.State) {
                            case "暂存":
                                return [
                                    '<a class="color-blue" id="todetail" href="javascript:void(0)">' + value + '</a>'
                                ].join('');
                                break;
                            default:
                                return [
                                    '<a class="color-blue" id="todetail" href="javascript:void(0);">' + value + '</a>'
                                ].join('');
                                break;
                        }
                    }
                }, {
                    field: 'ApplicantName',
                    title: '申请人',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'IPType',
                    title: '类型',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'CreateTime',
                    title: '申报时间',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            var date = createDate(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'State',
                    title: '流程状态',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }
            ]
        };
        Table.loadTable($("#table"), option);
    }
    //提交
    window.funding.case03.submit = function (type) {
        window.funding.check();

        if ($("input[name='IsEntrustAgency']:checked").val() == "1") {
            if (!$("input[name='AgencyName']").val()) {
                alert("请输入代理机构名称。");
                return;
            }
        }

        if (!verifydata())
            return;

        if (type == 'submit') {
            if (!confirm(message.fundingsubmitmessage))
                return;
        } else {
            if (!confirm(message.fundingsavemessage))
                return;
        }

        call_form({

            formid: "info",
            url: window.location.href,
            forceSync: true,    //强制同步
            async: false,
            data: {
                action: "m_save",
                sbmittype: type
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData.Result === "0") {
                    alert("操作成功");
                    parent.window.funding.closetab(GetQueryString(4), type);
                }
                else {
                    alert(aaData.errMsg);
                }
            }
        });
    }
    //选择附件
    window.funding.case03.choicefile = function () {
        $("input[name='FilePath'][type='file']").click();
    }
    //附件控件上传图片发生变化
    window.funding.case03.filechange = function (index) {

        if (!checkfilemaxsize($("input[name='File'][type='file']")[0], filemaxsize)) {
            alert("选择的文件太大了, 最大允许上传" + filemaxsize + "KB");
            $("input[name='TM_IsUploadFile']").val('0');
            return;
        }

        var extension = $("input[name='File'][type='file']").val().substring($("input[name='File'][type='file']").val().lastIndexOf('.')).toLowerCase();
        var extensionstr = ".jpg, .jpeg, .png, .bmp, .gif";
        if (extensionstr.indexOf(extension) >= 0) {
            $("#text").html($("input[name='File'][type='file']").val());
            $("input[name='TM_IsUploadFile']").val('1');
        }
        else {
            alert("请上传" + extensionstr + "格式的文件");
            $("input[name='TM_IsUploadFile']").val('0');
            return;
        }

    }
    //详细页面
    window.funding.case03.loaddetail = function (type) {

        switch (type) {
            case "read":
                $("a[role='edite']").remove();//编辑按钮移除   
                $("a[role='submit']").remove();//提交按钮移除    
                $("a[role='save']").remove();//保存按钮移除  
                $("a[role='printing']").show();//打印按钮显示

                $(".modifyInfo_upload").hide();
                $("a[role='btn_downloadfile']").show()

                $("span[class='color-red fblod']").hide();//移除必填符号
                //CaseStatus 案件状态中 请选择清空
                $("select[name='CaseStatus'] option").each(function () {
                    if ($(this).val() == "") {
                        $(this).text("");
                    }
                });
                break;
            case "edite":
                $("a[role='edite']").show();//编辑按钮显示  
                $("a[role='save']").remove();//保存按钮移除   
                $("a[role='printing']").show();//打印按钮显示
                $("a[role='submit']").hide();//提交按钮隐藏

                $("span[class='color-red fblod']").hide();//移除必填符号
                //CaseStatus 案件状态中 请选择清空
                $("select[name='CaseStatus'] option").each(function () {
                    if ($(this).val() == "") {
                        $(this).text("");
                    }
                });
                break;
            case "save":
                $("a[role='printing']").remove();//打印按钮移除 
                $("a[role='edite']").remove();//编辑按钮移除    
                break;
            default:
                break;
        }

        call_ajax({
            url: location.href,
            data: {
                action: "m_load"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        //明细信息
                        var data = aaData.Data.rows;
                        for (var item in data[0]) {
                            if (data[0][item]) {
                                switch (item) {
                                    case "TM_FilePath":
                                        var path = null;
                                        if (data[0][item]) {
                                            $("#text").html(data[0][item]);
                                            $("input[name='FilePath']").val(data[0][item]);

                                            if (data[0]["TM_IsUploadFile"] == "0") {
                                                path = data[0][item].replace("~/", tmsearchengine)
                                            } else {
                                                path = data[0][item].replace("~/", getRootPath() + "/")
                                            }
                                            $("a[role='btn_downloadfile']").attr("path", path);
                                        }
                                        $("#tmimage").attr("onerror", "onerror=null;src=\"../../../../../img/NoPictures.png\"");
                                        if (path) {
                                            $("#tmimage").attr("src", path);
                                        } else {
                                            $("#tmimage").attr("src", "../../../../../img/NoPictures.png");
                                        }
                                        break;
                                    case "State":
                                        $("li[role='" + item + "']").show();
                                        $("#" + item).text(data[0][item]);
                                        break;
                                    case "AuditOpinion":
                                        if (data[0]["State"] == '审核未通过' || data[0]["State"] == '补正') {
                                            $("li[role='" + item + "']").show();
                                            $("#" + item).text(data[0][item]);
                                        }
                                        break;
                                    case "BatchNo":
                                        $("#" + item).text(data[0][item]);
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                    case "FilingDate"://
                                    case "ApplyDate"://
                                        if (data[0][item]) {
                                            $("input[name='" + item + "']").val(createDate(data[0][item]).Format("yyyy-MM-dd"));
                                        }
                                        break;
                                    case "ApplicationContent"://
                                        if (data[0][item]) {
                                            $("textarea[name='" + item + "']").val(data[0][item]);
                                        }
                                        break;
                                    case "CO_Type":
                                    case "IsEntrustAgency":
                                        if (data[0][item]) {
                                            $("input[name='" + item + "']").each(function () {
                                                if ($(this).val() == data[0][item]) {
                                                    $(this).prop("checked", true);
                                                    $(this).trigger("click");
                                                }
                                            });
                                        }
                                        break;
                                    case "IPType":
                                        if (data[0][item]) {
                                            $("input[name='" + item + "s']").each(function () {
                                                if ($(this).val() == data[0][item]) {
                                                    $(this).prop("checked", true);
                                                    $(this).trigger("click");
                                                }
                                            });
                                        }
                                        break;
                                    case "CO_WorksType":
                                    case "PA_Type":
                                    case "CaseStatus":
                                        $("select[name='" + item + "']").val(data[0][item]);
                                        break;
                                    default:
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                }

                            }
                        }

                        if (type != 'save')//状态不为暂存时
                        {
                            setcontrolDisabled("info", "disable");//页面控件不可用
                        }
                        if (type == 'read') {
                            $("a[role='btn_search']").hide()
                        }
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //加载联系人数据
    window.funding.case03.linkmaninfo = function () {
        call_ajax({
            url: location.href,
            data: {
                action: "m_loadlinkman"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        //明细信息
                        var data = aaData.Data.rows;
                        for (var item in data[0]) {
                            if (data[0][item]) {
                                switch (item) {
                                    case "LinkMan":
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                    case "MobilePhone":
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                    case "EMail":
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                    case "QQ":
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                }

                            }
                        }
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }

    //点击搜索按钮
    window.funding.case03.search = function (type) {

        var sqh = $("input[name='ApplyNo']").val();
        if (!sqh) {
            alert("请填写申请号/注册号");
            return;
        }
        var intcls = "";
        if (type == "pa") {
            if (!CheckSQH(sqh)) {
                alert("申请号/注册号格式错误，请确认。");
                return;
            }
        } else {
            intcls = $("input[name='TM_IntCls']").val();
            if (!intcls) {
                alert("请填写国际分类");
                return;
            }
            $("input[name='TM_IsUploadFile']").val("0");
        }

        call_ajax({
            url: location.href,
            data: {
                action: "m_search",
                searchtype: type,
                sqh: sqh,
                intcls: intcls
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        //明细信息
                        var data = aaData.Data;
                        for (var item in data[0]) {
                            if (data[0][item]) {
                                switch (type) {
                                    case "pa":
                                        switch (item) {
                                            case "title":
                                                $("input[name='Name']").val(data[0][item]);
                                                break;
                                            case "sqh":
                                                $("input[name='ApplyNo']").val(data[0][item]);
                                                break;
                                            case "appDate":
                                                $("input[name='ApplyDate']").val(data[0][item]);
                                                break;
                                            case "applicantName":
                                                var childdata = data[0][item];
                                                if (childdata.length > 0) {
                                                    $("input[name='ApplicantName']").val(childdata[0]);
                                                }
                                                break;
                                            case "address":
                                                $("input[name='ApplicantAddress']").val(data[0][item]);
                                                break;
                                            case "patType":
                                                switch (data[0][item]) {
                                                    case "1":
                                                        $("select[name='PA_Type']").val("发明专利");
                                                        break;
                                                    case "2":
                                                        $("select[name='PA_Type']").val("实用新型");
                                                        break;
                                                    case "3":
                                                        $("select[name='PA_Type']").val("外观设计");
                                                        break;
                                                    case "8":
                                                        $("select[name='PA_Type']").val("PCT发明");
                                                        break;
                                                    case "9":
                                                        $("select[name='PA_Type']").val("PCT实用新型");
                                                        break;
                                                }

                                                break;
                                            case "agencyName":
                                                $("input[name='AgencyName']").val(data[0][item]);
                                                $("input[name='IsEntrustAgency'][value='1']").prop("checked", true);
                                                break;
                                        }
                                        break;
                                    case "tm":
                                        switch (item) {
                                            case "name":
                                                $("input[name='Name']").val(data[0][item]);
                                                break;
                                            case "sqh":
                                                $("input[name='ApplyNo']").val(data[0][item]);
                                                break;
                                            case "appDate":
                                                $("input[name='ApplyDate']").val(data[0][item]);
                                                break;
                                            case "registrarList":

                                                var childdata = data[0][item];

                                                if (childdata.length > 0) {
                                                    $("input[name='ApplicantName']").val(data[0][item][0]["registrarCN"]);
                                                    $("input[name='ApplicantAddress']").val(data[0][item][0]["registrarAddressCN"]);
                                                }
                                                break;
                                            case "tmPicturePath":
                                                $("#text").html(data[0][item]);
                                                $("input[name='FilePath']").val(data[0][item]);
                                                break;
                                            case "agentName":
                                                $("input[name='AgencyName']").val(data[0][item]);
                                                $("input[name='IsEntrustAgency'][value='1']").prop("checked", true);
                                                break;
                                        }
                                        break;
                                    default:
                                }

                            }
                        }
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    /******************* 企业知识产权备案 end********************/


    /******************* 法律援助申报 begin*****************/
    if (!window.funding.case04) {
        window.funding.case04 = {};
    }

    //加载列表
    window.funding.case04.loadlist = function (type) {

        var option = {
            url: window.location.href + "-" + type,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "m_load";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "ID",
            height: $("body").height() - 400,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [
                {
                    field: 'ApplicantName',
                    title: '申报人',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'CreateTime',
                    title: '申报时间',
                    datatype: 'datetime',
                    ext: 'like'
                },
                {
                    field: 'State',
                    title: '流程状态',
                    datatype: 'string',
                    ext: 'like'
                }
            ],
            columns: [
                {
                    field: '_ckb_',
                    title: '',
                    checkbox: true,
                    valign: 'middle',
                    align: 'center'
                },
                {
                    field: 'ID',
                    title: '',
                    visible: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'ApplicantName',
                    title: '申报人',
                    sortable: false,
                    valign: 'middle',
                    align: 'center',
                    events: {
                        'click #todetail': function (e, value, row, index) {

                            switch (row.State) {
                                case "补正":
                                    window.funding.topadetail("member-funding-case04-detail-edite", row.ID);
                                    break;
                                case "暂存":
                                    window.funding.topadetail("member-funding-case04-detail-save", row.ID);
                                    break;
                                default:
                                    window.funding.topadetail("member-funding-case04-detail-read", row.ID);
                                    break;
                            }
                        }
                    },
                    formatter: function (value, row, index) {
                        switch (row.State) {
                            case "暂存":
                                return [
                                    '<a class="color-blue" id="todetail" href="javascript:void(0)">' + value + '</a>'
                                ].join('');
                                break;
                            default:
                                return [
                                    '<a class="color-blue" id="todetail" href="javascript:void(0);">' + value + '</a>'
                                ].join('');
                                break;
                        }
                    }
                }, {
                    field: 'ApplicationContent',
                    title: '申报内容',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value.length > 40) {

                            return value.substring(0, 40) + "...";
                        }
                        else {
                            return value;
                        }
                    }
                }, {
                    field: 'CreateTime',
                    title: '申报时间',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            var date = createDate(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'State',
                    title: '流程状态',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }
            ]
        };
        Table.loadTable($("#table"), option);
    }
    //提交
    window.funding.case04.submit = function (type) {
        window.funding.check();

        if (!verifydata())
            return;
        if (!$("input[name='FilePath']").val() && !$("input[name='File']").val()) {
            alert("请上传相关资料。");
            return;
        }

        if (type == 'submit') {
            if (!confirm(message.fundingsubmitmessage))
                return;
        } else {
            if (!confirm(message.fundingsavemessage))
                return;
        }

        call_form({
            formid: "info",
            url: window.location.href,
            forceSync: true,    //强制同步
            async: false,
            data: {
                action: "m_save",
                sbmittype: type
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData.Result === "0") {
                    alert("操作成功");
                    parent.window.funding.closetab(GetQueryString(4), type);
                }
                else {
                    alert(aaData.errMsg);
                }
            }
        });
    }
    //选择附件
    window.funding.case04.choicefile = function () {
        $("input[name='FilePath'][type='file']").click();
    }
    //附件控件上传图片发生变化
    window.funding.case04.filechange = function (index) {

        if (!checkfilemaxsize($("input[name='File'][type='file']")[0], filemaxsize)) {
            alert("选择的文件太大了, 最大允许上传" + filemaxsize + "KB");
            return;
        }

        var extension = $("input[name='File'][type='file']").val().substring($("input[name='File'][type='file']").val().lastIndexOf('.')).toLowerCase();
        var extensionstr = ".jpg, .jpeg, .png, .bmp, .gif, .rar, .zip, .doc, .docx, .xls, .xlsx, .pdf";
        if (extensionstr.indexOf(extension) >= 0) {
            $("#text").html($("input[name='File'][type='file']").val());
        }
        else {
            alert("请上传" + extensionstr + "格式的文件");
            return;
        }

    }
    //详细页面
    window.funding.case04.loaddetail = function (type) {

        switch (type) {
            case "read":
                $("a[role='edite']").remove();//编辑按钮移除   
                $("a[role='submit']").remove();//提交按钮移除    
                $("a[role='save']").remove();//保存按钮移除  
                $("a[role='printing']").show();//打印按钮显示

                $(".modifyInfo_upload").hide();
                $("a[role='btn_downloadfile']").show()
                $("span[class='color-red fblod']").hide();//移除必填符号

                break;
            case "edite":
                $("a[role='edite']").show();//编辑按钮显示  
                $("a[role='save']").remove();//保存按钮移除   
                $("a[role='printing']").show();//打印按钮显示
                $("a[role='submit']").hide();//提交按钮隐藏
                $("span[class='color-red fblod']").hide();//移除必填符号


                $(".modifyInfo_upload").hide();
                $("a[role='btn_downloadfile']").show()
                break;
            case "save":
                $("a[role='printing']").remove();//打印按钮移除 
                $("a[role='edite']").remove();//编辑按钮移除    
                break;
            default:
                break;
        }

        call_ajax({
            url: location.href,
            data: {
                action: "m_load"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        //明细信息
                        var data = aaData.Data.rows;
                        for (var item in data[0]) {
                            if (data[0][item]) {
                                switch (item) {
                                    case "FilePath":
                                        if (data[0][item]) {
                                            $("#text").html(data[0][item]);
                                            $("input[name='" + item + "']").val(data[0][item]);
                                            $("a[role='btn_downloadfile']").attr("path", data[0][item]);
                                        }
                                        break;
                                    case "State":
                                        $("li[role='" + item + "']").show();
                                        $("#" + item).text(data[0][item]);
                                        break;
                                    case "AuditOpinion":
                                        if (data[0]["State"] == '审核未通过' || data[0]["State"] == '补正') {
                                            $("li[role='" + item + "']").show();
                                            $("#" + item).text(data[0][item]);
                                        }
                                        break;
                                    case "BatchNo":
                                        $("#" + item).text(data[0][item]);
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                    case "CreateTime"://部分通过审核金额
                                        if (data[0][item]) {
                                            $("#" + item).text(createDate(data[0][item]).Format("yyyy-MM-dd HH:mm"));
                                        }
                                        break;
                                    case "ApplicationContent"://
                                        if (data[0][item]) {
                                            $("textarea[name='" + item + "']").val(data[0][item]);
                                        }
                                        break;
                                    default:
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                }

                            }
                        }

                        if (type != 'save')//状态不为暂存时
                            setcontrolDisabled("info", "disable");//页面控件不可用
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //加载联系人数据
    window.funding.case04.linkmaninfo = function () {
        call_ajax({
            url: location.href,
            data: {
                action: "m_loadlinkman"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        //明细信息
                        var data = aaData.Data.rows;
                        for (var item in data[0]) {
                            if (data[0][item]) {
                                switch (item) {
                                    case "LinkMan":
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                    case "MobilePhone":
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                    case "EMail":
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                    case "QQ":
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                }

                            }
                        }
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    /******************* 法律援助申报 end********************/




    /*******************************会员中心 企业信息 end*********************************/
    //if (!window.memberinfo) {
    //    window.memberinfo = {};
    //}
    //加载信息
    window.memberinfo.loaddetail = function () {

        if (accounttype == "个人账户") {
            $("#title").text("个人信息");
            $("#form_enterprice").remove();
        } else {
            $("#title").text("企业信息");
            $("#form_personal").remove();
        }
        call_ajax({
            url: location.href,
            data: {
                action: "m_load"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        //信息
                        var data = aaData.Data.rows;

                        for (var item in data[0]) {
                            if (data[0][item]) {
                                switch (item) {
                                    case "CheckState":
                                        var state = "";
                                        switch (data[0][item]) {
                                            case "1":
                                                state = "正在审核";
                                                break;
                                            case "2":
                                                state = "审核通过";
                                                break;
                                            case "3":
                                                state = "拒绝";
                                                break;
                                            case "0":
                                                state = "未申请";
                                                break;
                                        }

                                        $("label[name='" + item + "']").text(state);
                                    case "ShiFanQiYe":
                                    case "YouShiQiYe":
                                    case "GuanBiaoQiYe":
                                        $("select[name='" + item + "']").val(data[0][item]);
                                        break;
                                    case "Province":
                                        $("span[name='" + item + "']").text(data[0]["Province"] + "-" + data[0]["Address"]);
                                        break;
                                    case "BusinessLicensePath":
                                    case "OpeningPermitPath":
                                    case "Phono":
                                        $("#img_" + item.toLowerCase()).attr("src", data[0][item].replace("~/", getRootPath() + "/"));;
                                        break;
                                    case "RegistrationTime":
                                        var date = createDate(data[0][item]);
                                        $("span[name='" + item + "']").text(date.Format("yyyy-MM-dd"));
                                        break;
                                    case "IsBusinessLicenseLongterm":
                                        if (data[0]["IsBusinessLicenseLongterm"] == '0') {
                                            $("span[name='BusinessLicenseExpirationDate']").text(createDate(data[0]["BusinessLicenseExpirationDate"]).Format("yyyy-MM-dd"));
                                        } else {
                                            $("span[name='BusinessLicenseExpirationDate']").text("长期有效");
                                        }
                                    default:
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        $("span[name='" + item + "']").text(data[0][item]);
                                        break;
                                }
                            }
                        }
                        setcontrolDisabled("form_enterprice", "disable");//页面控件不可用
                        setcontrolDisabled("form_personal", "disable");//页面控件不可用


                        //联系人信息
                        var data = aaData.LinkManData.rows;
                        for (var i = 0; i < data.length; i++) {
                            if (data[i]["LinkMainType"] == "专员") {
                                $("input[name='AssistantID']").val(data[i]["ID"]);
                                $("input[name='AssistantName']").val(data[i]["LinkMan"]);
                                $("input[name='AssistantMobilePhone']").val(data[i]["MobilePhone"]);
                                $("input[name='AssistantEMail']").val(data[i]["EMail"]);
                                $("input[name='AssistantQQ']").val(data[i]["QQ"]);
                                if (data[i]["IsDefault"] == 1)
                                    $("input[name='IsDefault'][value='" + data[i]["LinkMainType"] + "']").attr("checked", true);
                                else
                                    $("input[name='IsDefault'][value='" + data[i]["LinkMainType"] + "']").attr("checked", false);
                            }
                            if (data[i]["LinkMainType"] == "主管") {
                                $("input[name='DirectorID']").val(data[i]["ID"]);
                                $("input[name='DirectorName']").val(data[i]["LinkMan"]);
                                $("input[name='DirectorMobilePhone']").val(data[i]["MobilePhone"]);
                                $("input[name='DirectorEMail']").val(data[i]["EMail"]);
                                $("input[name='DirectorQQ']").val(data[i]["QQ"]);
                                if (data[i]["IsDefault"] == 1)
                                    $("input[name='IsDefault'][value='" + data[i]["LinkMainType"] + "']").attr("checked", true);
                                else
                                    $("input[name='IsDefault'][value='" + data[i]["LinkMainType"] + "']").attr("checked", false);
                            }
                        }
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }

    window.memberinfo.submit_check = function (formid) {
        var ret = true;


        $("#" + formid + " [ismust='true']").each(function (index, element) {
            var desc = $(element).attr("desc");

            if ($(element).val() == "" || $(element).val() == null || typeof ($(element).val()) == "undefined") {

                alert("请输入" + desc + "。");
                ret = false;
                return false;
            }
        });

        if (ret == false) return ret;

        $("#" + formid + " [isnum='true']").each(function (index, element) {
            var desc = $(element).attr("desc");

            if ($(element).val() != "") {
                if (isNaN($(element).val())) {
                    alert("" + desc + ", 输入的值不是有效的数字格式。");
                    ret = false;
                    return false;
                }
            }
        });

        if (ret == false) return ret;

        $("#" + formid + " [isphone='true']").each(function (index, element) {
            var desc = $(element).attr("desc");

            if ($(element).val() != "") {
                if (!CheckMobile($(element).val()) && !CheckTel($(element).val())) {
                    alert("" + desc + ", 输入的值不是有效的电话格式。");
                    ret = false;
                    return false;
                }
            }
        });

        if (ret == false) return ret;

        $("#" + formid + " [isemail='true']").each(function (index, element) {
            var desc = $(element).attr("desc");

            if ($(element).val() != "") {
                if (!CheckMail($(element).val())) {
                    alert("" + desc + ", 输入的值不是有效的E-mail格式。");
                    ret = false;
                    return false;
                }
            }
        });
        return ret
    }
    //保存
    window.memberinfo.submit = function () {

        var formid = "";
        switch (accounttype) {
            case "个人账户":
                formid = "form_personal";
                break;
            case "企业账户":
                formid = "form_enterprice";
                if ($("input[name='IsDefault']:checked").val() == "专员") {
                    $("#form_enterprice").find("input").each(function () {
                        if ($(this).attr("role") == "Assistant") {
                            $(this).attr("ismust", "true")
                        }
                        if ($(this).attr("role") == "Director") {
                            if ($(this).val()) {
                                $(this).attr("ismust", "true")
                            } else {
                                $(this).removeAttr("ismust")
                            }
                        }
                    });
                } else if ($("input[name='IsDefault']:checked").val() == "主管") {
                    $("#form_enterprice").find("input").each(function () {
                        if ($(this).attr("role") == "Director") {
                            $(this).attr("ismust", "true")
                        }
                        if ($(this).attr("role") == "Assistant") {
                            if ($(this).val()) {
                                $(this).attr("ismust", "true")
                            } else {
                                $(this).removeAttr("ismust")
                            }
                        }
                    });
                }
                break;
        }

        if (!window.memberinfo.submit_check(formid)) {
            return;
        }
        if (!confirm("是否确认保存？")) {
            return;
        }

        call_ajax({
            url: location.href,
            data: {
                action: "m_save",
                info: JSON.stringify($("#" + formid).serializeJson())
            },
            success: function (data) {
                var aaData = strToJson(data);
                var data = "";
                if (aaData) {
                    if (aaData.Result === "0") {
                        alert("保存成功");
                        $("a[role='cancel']").click();
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    /*******************************会员中心 企业信息 end*********************************/


    /*******************************会员中心 首页 begin*********************************/

    //加载
    window.portal.load = function () {
        call_ajax({
            url: location.href,
            data: {
                action: "m_load"
            },
            success: function (data) {
                var aaData = strToJson(data);
                var data = "";
                if (aaData) {
                    if (aaData.Result === "0") {
                        //首页上部 资助申报、诉前联调、培训报名 统计
                        var data = aaData.StatisticsData.rows;
                        for (var i = 0; i < data.length; i++) {
                            var datahtml = "";
                            if (data[i]["Date"]) {
                                datahtml = '<div class="b-todo-bottom"><p>更新时间：' + createDate(data[i]["Date"]).Format("yyyy-MM-dd HH:mm") + '</p></div>';
                            }
                            var html = "";
                            html += '<li>';
                            html += '<a class="' + data[i]["Color"] + '" href="' + data[i]["Link"].replace("~/", getRootPath() + "/") + '">';
                            html += '<div class="b-todo-top">';
                            html += '<div class="b-icon fl"><i class="fa ' + data[i]["Icon"] + '"></i></div>';
                            html += '<div class="fl b-todo-txt">';
                            if (data[i]["Type"] == "知识产权备案" || data[i]["Type"] == "法律援助申报")
                                html += '<p><label>' + data[i]["Count"] + '</label><span>/个</span></p>';
                            else
                                html += '<p><label>' + data[i]["Count"] + '</label><span>/件</span></p>';
                            html += '<p>' + data[i]["Type"] + '</p></div></div>';
                            html += datahtml;
                            html += '</a></li>';

                            $("#todoitem").append(html);
                        }
                        //用户名称信息
                        var data = aaData.AuthData.rows;
                        for (var item in data[0]) {
                            if (data[0][item]) {
                                switch (item) {
                                    case "CheckState":
                                        var state = "";
                                        switch (data[0][item]) {
                                            case "1":
                                                state = "正在审核";
                                                break;
                                            case "2":
                                                state = "审核通过";
                                                break;
                                            case "3":
                                                state = "拒绝";
                                                break;
                                            case "0":
                                                state = "未申请";
                                                break;
                                        }

                                        $("#" + item).text(state);
                                        break;
                                    default:
                                        $("#" + item).text(data[0][item]);
                                        break;
                                }
                            }
                        }

                        //消息列表
                        var data = aaData.MessageData.rows;
                        if (data.length > 0) {
                            for (var i = 0; i < data.length; i++) {
                                var html = "";

                                if (data[i]["IsRead"] == 0) {
                                    html += "<li class='unread' title='未读信息' id='" + data[i]["ID"] + "'>";
                                    //已审核用户可以访问所有页
                                    if (checkstate == 2) {
                                        html += "<a href=" + data[i]["Url"] + " onclick=\"window.portal.changemessagestate('" + data[i]["ID"] + "','id')\">";
                                    } else {
                                        if (data[i]["Modular"] == "用户审核") {
                                            //审核状态为未审核的，只可以跳转审核相关页面，之前的资助申报、诉前联调页面都不宜跳转
                                            html += "<a href=" + data[i]["Url"] + " onclick=\"window.portal.changemessagestate('" + data[i]["ID"] + "','id')\">";
                                        } else {
                                            html += "<a href='javascript:void(0);'>";
                                        }
                                    }
                                }
                                else {
                                    html += "<li title='已读信息'>";
                                    //已审核用户可以访问所有页
                                    if (checkstate == 2) {
                                        html += "<a href=" + data[i]["Url"] + ">";
                                    } else {
                                        if (data[i]["Modular"] == "用户审核") {
                                            //审核状态为未审核的，只可以跳转审核相关页面，之前的资助申报、诉前联调页面都不宜跳转
                                            html += "<a href=" + data[i]["Url"] + ">";
                                        } else {
                                            html += "<a href='javascript:void(0);'>";
                                        }
                                    }
                                }

                                if (data[i]["IsRead"] == 0)
                                    html += "<i class='fa fa-envelope fl' ></i>";
                                else
                                    html += "<i class='fa fa-envelope-open fl' ></i>";

                                switch (data[i]["Modular"]) {
                                    case "用户审核":
                                        html += "<p class='fl plate btn-yellow'>" + data[i]["Modular"] + "</p>";
                                        html += "<p class='fl con'>" + data[i]["Title"] + "：<span class='color-blue'>" + data[i]["State"] + "</span></p>";
                                        break;
                                    case "培训报名":
                                        html += "<p class='fl plate btn-orange'>" + data[i]["Modular"] + "</p>";
                                        html += "<p class='fl con'>" + data[i]["Title"] + "</span></p>";
                                        break;
                                    default:
                                        html += "<p class='fl plate btn-Lpurple'>" + data[i]["Modular"] + "</p>";
                                        html += "<p class='fl con'>" + data[i]["Title"] + "，申报状态：<span class='color-blue'>" + data[i]["State"] + "</span></p>";
                                        break;
                                }

                                html += '<span class="fr n-time">' + createDate(data[i]["CreateTime"]).Format("yyyy-MM-dd") + '</span>';
                                html += "</a>";
                                html += "</li>";
                                $("#messagelist").append(html);

                                //右下角消息提示框
                                if (data[i]["IsRead"] == 0) {
                                    var boxhtml = "";
                                    boxhtml += "<li>";

                                    //已审核用户可以访问所有页
                                    if (checkstate == 2) {
                                        boxhtml += "<a href=" + data[i]["Url"] + " onclick=\"window.portal.changemessagestate('" + data[i]["ID"] + "','id')\">";
                                    } else {
                                        if (data[i]["Modular"] == "用户审核") {
                                            //审核状态为未审核的，只可以跳转审核相关页面，之前的资助申报、诉前联调页面都不宜跳转
                                            boxhtml += "<a href=" + data[i]["Url"] + " onclick=\"window.portal.changemessagestate('" + data[i]["ID"] + "','id')\">";
                                        } else {
                                            boxhtml += "<a href='javascript:void(0);'>";
                                        }
                                    }

                                    boxhtml += "<i class='fa fa-exclamation-triangle fl'></i>";
                                    boxhtml += "<div class='fl'>";
                                    boxhtml += "<label class='fl'>【" + data[i]["Modular"] + "】</label>";
                                    boxhtml += "<p class='fl con'>" + data[i]["Title"] + "：<span>" + data[i]["State"] + "</span></p>";
                                    boxhtml += "<span class='fr n-time'>" + createDate(data[i]["CreateTime"]).Format("yyyy-MM-dd") + "</span>";
                                    boxhtml += "</div>";
                                    boxhtml += " </a>";
                                    boxhtml += "</li>";
                                    $("#messagelistbox").append(boxhtml);
                                    $(".messagebox").show();//右下角消息提示框显示
                                }

                            }
                        }
                        else {
                            $("#messagelist").append("<li><p class='n-listtxt' style='text-align: center;;width:100%'>未找到相关的数据</p></li>");
                            $(".m-close").click();//右下角消息提示框关闭
                        }

                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }

    //加载消息列表
    window.portal.loadlist = function () {

        var listid = "list";
        var paginationid = "pagination";

        call_ajax({
            url: location.href,
            data: {
                action: "m_loadlist",
                sort: "",
                order: null
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    $("#" + listid).empty();

                    if (aaData.Result === "0") {
                        var limit = aaData.Limit;
                        var offset = aaData.Offset;
                        var data = aaData.Data.rows;

                        //绑定列表
                        if (data.length > 0) {
                            for (var i = 0; i < data.length; i++) {
                                var html = "";

                                if (data[i]["IsRead"] == 0) {
                                    html += "<li class='unread' title='未读信息'>";
                                    html += "<a href=" + data[i]["Url"] + " onclick=\"window.portal.changemessagestate('" + data[i]["ID"] + "','id')\">";
                                }
                                else {
                                    html += "<li title='已读信息'>";
                                    html += "<a href=" + data[i]["Url"] + ">";
                                }


                                if (data[i]["IsRead"] == 0)
                                    html += "<i class='fa fa-envelope fl' ></i>";
                                else
                                    html += "<i class='fa fa-envelope-open fl' ></i>";

                                switch (data[i]["Modular"]) {
                                    case "用户审核":
                                        html += "<p class='fl plate btn-yellow'>" + data[i]["Modular"] + "</p>";
                                        html += "<p class='fl con'>" + data[i]["Title"] + "：<span class='color-blue'>" + data[i]["State"] + "</span></p>";
                                        break;
                                    case "培训报名":
                                        html += "<p class='fl plate btn-orange'>" + data[i]["Modular"] + "</p>";
                                        html += "<p class='fl con'>" + data[i]["Title"] + "</span></p>";
                                        break;
                                    default:
                                        html += "<p class='fl plate btn-Lpurple'>" + data[i]["Modular"] + "</p>";
                                        html += "<p class='fl con'>" + data[i]["Title"] + "，申报状态：<span class='color-blue'>" + data[i]["State"] + "</span></p>";
                                        break;
                                }

                                html += '<span class="fr n-time">' + createDate(data[i]["CreateTime"]).Format("yyyy-MM-dd") + '</span>';
                                html += "</a>";
                                html += "</li>";
                                $("#" + listid).append(html);
                            }
                        }
                        else {
                            $("#" + listid).append("<li><p class='n-listtxt' style='text-align: center;;width:100%'>未找到相关的数据</p></li>");
                        }

                        //处理分页控件
                        var allpagecount = 0;                   //分页控件总页数
                        var allrowscount = aaData.Data.total;   //所有数据总行数

                        //计算总页数
                        if (allrowscount <= limit)
                            allpagecount = 1;
                        else {
                            if (allrowscount % limit > 0)
                                allpagecount = (allrowscount / limit) + 1;
                            else
                                allpagecount = allrowscount / limit;
                        }

                        var options = {
                            currentPage: offset,//默认页
                            totalPages: allpagecount,//总页数
                            //----------非必选参数------------------
                            size: "normal",//大小  Mini Small normal large  
                            alignment: "center",//位置
                            onPageClicked: function (event, originalEvent, type, page) {

                                window.location = "member-message-list-" + page;
                            }
                        }
                        $('#' + paginationid).bootstrapPaginator(options);
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //变更消息为已读
    window.portal.changemessagestate = function (id, type) {

        call_ajax({
            url: location.href,
            data: {
                action: "m_readmessage",
                id: id,
                type: type
            },
            success: function (data) {
                var aaData = strToJson(data);
                var data = "";
                if (aaData) {
                    if (aaData.Result === "0") {
                        if (type != "all") {
                            $("#messagelist").find("li").each(function () {
                                if ($(this).attr("id") == id) {
                                    $(this).attr("class", "");
                                    $(this).find("i[class='fa fa-envelope fl']").attr("class", "fa fa-envelope-open fl");
                                }
                            });
                        }
                        $(".messagebox").hide();
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    /*******************************会员中心 首页 end*********************************/


    /*******************************知识产权管理 begin*********************************/
    if (!window.ipmanager) {
        window.ipmanager = {};
    }

    /******************* 我的知识产权 begin*****************/
    if (!window.ipmanager.myip) {
        window.ipmanager.myip = {};
    }

    //我的知识产权-加载列表
    window.ipmanager.myip.loadlist = function (type) {
        var option = {};
        switch (type) {
            case "pa":
                option = window.ipmanager.myip.pa(type);
                break;
            case "tm":
                option = window.ipmanager.myip.tm(type);
                break;
            case "co":
                option = window.ipmanager.myip.co(type);
                break;
        }
        $("[role='searchpanel']").html('');
        $(".m-tablebox").html("");
        $(".m-tablebox").append('<table class="m-table" id="table"></table>');

        Table.loadTable($("#table"), option);
    }
    //我的知识产权-专利
    window.ipmanager.myip.pa = function (type) {
        return {
            url: window.location.href + "?type=" + type,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "m_load";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "ID",
            height: $("body").height() - 370,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [
                {
                    field: 'ApplyDate',
                    title: '申请日',
                    datatype: 'datetime',
                    ext: 'like'
                },
                {
                    field: 'ApplyNo',
                    title: '申请号/专利号',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'Name',
                    title: '专利名称',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'PA_Type',
                    title: '专利类型',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'ApplicantName',
                    title: '申请人/权利人',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'CaseStatus',
                    title: '案件状态',
                    datatype: 'string',
                    ext: 'like'
                }
            ],
            columns: [
                {
                    field: '_ckb_',
                    title: '',
                    checkbox: true,
                    valign: 'middle',
                    align: 'center'
                },
                {
                    field: 'ID',
                    title: '',
                    visible: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'ApplyDate',
                    title: '申请日',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            var date = createDate(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'ApplyNo',
                    title: '申请号/专利号',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'Name',
                    title: '专利名称',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    events: {
                        'click #todetail': function (e, value, row, index) {
                            var url = "member-ip-my-detail-pa-" + row.ApplyNo.replace('.', '') + "-" + row.ID;

                            Tabs.addTabs({
                                id: "detail_pa",
                                title: "专利详情",
                                url: url,
                                close: true
                            });
                        }
                    },
                    formatter: function (value, row, index) {

                        return [
                            '<a class="color-blue" id="todetail" href="javascript:void(0);">' + value + '</a>'
                        ].join('');
                    }
                }, {
                    field: 'PA_Type',
                    title: '专利类型',
                    sortable: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'ApplicantName',
                    title: '申请人/权利人',
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'CaseStatus',
                    title: '案件状态',
                    align: 'center',
                    valign: 'middle'
                }
            ]
        };
    }
    //我的知识产权-商标
    window.ipmanager.myip.tm = function (type) {
        return {
            url: window.location.href + "?type=" + type,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "m_load";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "ID",
            height: $("body").height() - 370,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [
                {
                    field: 'ApplyDate',
                    title: '申请日',
                    datatype: 'datetime',
                    ext: 'like'
                },
                {
                    field: 'Name',
                    title: '商标名称',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'TM_IntCls',
                    title: '国际分类',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'ApplyNo',
                    title: '申请号/注册号',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'ApplicantName',
                    title: '申请人',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'CaseStatus',
                    title: '案件状态',
                    datatype: 'string',
                    ext: 'like'
                }
            ],
            columns: [
                {
                    field: '_ckb_',
                    title: '',
                    checkbox: true,
                    valign: 'middle',
                    align: 'center'
                },
                {
                    field: 'ID',
                    title: '',
                    visible: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'TM_FilePath',
                    title: '商标图样',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        var path = "";
                        if (value) {
                            if (row.TM_IsUploadFile == 0) {
                                path = value.replace("~/", tmsearchengine)
                            } else {
                                path = value.replace("~/", getRootPath() + "/")
                            }
                        }
                        return [
                            '<img class="m-pageimg" src="' + path + '" onerror=onerror=null;src="img/NoPictures.png">'
                        ].join('');
                    }
                }, {
                    field: 'Name',
                    title: '商标名称',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    events: {
                        'click #todetail': function (e, value, row, index) {
                            var url = "member-ip-my-detail-tm-" + row.ApplyNo + "-" + row.TM_IntCls + "-" + row.ID;

                            Tabs.addTabs({
                                id: "detail_tm",
                                title: "商标详情",
                                url: url,
                                close: true
                            });
                        }
                    },
                    formatter: function (value, row, index) {

                        return [
                            '<a class="color-blue" id="todetail" href="javascript:void(0);">' + value + '</a>'
                        ].join('');
                    }
                }, {
                    field: 'TM_IntCls',
                    title: '国际分类',
                    sortable: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'ApplyNo',
                    title: '申请号/注册号',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'ApplyDate',
                    title: '申请日',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            var date = createDate(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'ApplicantName',
                    title: '申请人',
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'CaseStatus',
                    title: '案件状态',
                    align: 'center',
                    valign: 'middle'
                }
            ]
        };
    }
    //我的知识产权-版权
    window.ipmanager.myip.co = function (type) {
        return {
            url: window.location.href + "?type=" + type,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "m_load";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "ID",
            height: $("body").height() - 370,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [
                {
                    field: 'ApplyDate',
                    title: '申请日',
                    datatype: 'datetime',
                    ext: 'like'
                },
                {
                    field: 'CO_Type',
                    title: '版权类型',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'Name',
                    title: '作品名称',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'ApplyNo',
                    title: '申请号/注册号',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'ApplicantName',
                    title: '著作权人',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'CaseStatus',
                    title: '案件状态',
                    datatype: 'string',
                    ext: 'like'
                }
            ],
            columns: [
                {
                    field: '_ckb_',
                    title: '',
                    checkbox: true,
                    valign: 'middle',
                    align: 'center'
                },
                {
                    field: 'ID',
                    title: '',
                    visible: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'CO_Type',
                    title: '版权类型',
                    sortable: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'Name',
                    title: '作品名称',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    events: {
                        'click #todetail': function (e, value, row, index) {
                            var url = "member-ip-my-detail-co-" + row.ID;

                            Tabs.addTabs({
                                id: "detail_co",
                                title: "版权详情",
                                url: url,
                                close: true
                            });
                        }
                    },
                    formatter: function (value, row, index) {

                        return [
                            '<a class="color-blue" id="todetail" href="javascript:void(0);">' + value + '</a>'
                        ].join('');
                    }
                }, {
                    field: 'ApplyNo',
                    title: '申请号/注册号',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'ApplyDate',
                    title: '申请日',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            var date = createDate(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'ApplicantName',
                    title: '著作权人',
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'CaseStatus',
                    title: '案件状态',
                    align: 'center',
                    valign: 'middle'
                }
            ]
        };
    }
    //我的知识产权-删除
    window.ipmanager.myip.delete = function () {

        var ID = Table.getIdSelectionsString($("#table"), "ID");

        call_ajax({
            url: location.href,
            data: {
                action: "m_delete",
                id: ID
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result === "0") {
                        $(".deletebox").hide();
                        $(".successbox").show();
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //点击搜索按钮
    window.ipmanager.myip.search = function (type) {

        var sqh = $("input[name='ApplyNo']").val();
        if (!sqh) {
            alert("请填写申请号/注册号");
            return;
        }
        var intcls = "";
        if (type == "pa") {
            if (!CheckSQH(sqh)) {
                alert("申请号/注册号格式错误，请确认。");
                return;
            }
        } else {
            intcls = $("input[name='TM_IntCls']").val();
            if (!intcls) {
                alert("请填写国际分类");
                return;
            }
            $("input[name='TM_IsUploadFile']").val("0");
        }

        call_ajax({
            url: location.href,
            data: {
                action: "m_search",
                searchtype: type,
                sqh: sqh,
                intcls: intcls
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        //明细信息
                        var data = aaData.Data;
                        for (var item in data[0]) {
                            if (data[0][item]) {
                                switch (type) {
                                    case "pa":
                                        switch (item) {
                                            case "title":
                                                $("input[name='Name']").val(data[0][item]);
                                                break;
                                            case "sqh":
                                                $("input[name='ApplyNo']").val(data[0][item]);
                                                break;
                                            case "appDate":
                                                $("input[name='ApplyDate']").val(data[0][item]);
                                                break;
                                            case "applicantName":
                                                var childdata = data[0][item];
                                                if (childdata.length > 0) {
                                                    $("input[name='ApplicantName']").val(childdata[0]);
                                                }
                                                break;
                                            case "address":
                                                $("input[name='ApplicantAddress']").val(data[0][item]);
                                                break;
                                            case "patType":
                                                switch (data[0][item]) {
                                                    case "1":
                                                        $("select[name='PA_Type']").val("发明专利");
                                                        break;
                                                    case "2":
                                                        $("select[name='PA_Type']").val("实用新型");
                                                        break;
                                                    case "3":
                                                        $("select[name='PA_Type']").val("外观设计");
                                                        break;
                                                    case "8":
                                                        $("select[name='PA_Type']").val("PCT发明");
                                                        break;
                                                    case "9":
                                                        $("select[name='PA_Type']").val("PCT实用新型");
                                                        break;
                                                }

                                                break;
                                            case "agencyName":
                                                $("input[name='AgencyName']").val(data[0][item]);
                                                $("input[name='IsEntrustAgency'][value='1']").prop("checked", true);
                                                break;
                                        }
                                        break;
                                    case "tm":
                                        switch (item) {
                                            case "name":
                                                $("input[name='Name']").val(data[0][item]);
                                                break;
                                            case "sqh":
                                                $("input[name='ApplyNo']").val(data[0][item]);
                                                break;
                                            case "appDate":
                                                $("input[name='ApplyDate']").val(data[0][item]);
                                                break;
                                            case "registrarList":

                                                var childdata = data[0][item];

                                                if (childdata.length > 0) {
                                                    $("input[name='ApplicantName']").val(data[0][item][0]["registrarCN"]);
                                                    $("input[name='ApplicantAddress']").val(data[0][item][0]["registrarAddressCN"]);
                                                }
                                                break;
                                            case "tmPicturePath":
                                                $("#text").html(data[0][item]);
                                                $("input[name='FilePath']").val(data[0][item]);
                                                break;
                                            case "agentName":
                                                $("input[name='AgencyName']").val(data[0][item]);
                                                $("input[name='IsEntrustAgency'][value='1']").prop("checked", true);
                                                break;
                                        }
                                        break;
                                    default:
                                }

                            }
                        }
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //提交
    window.ipmanager.myip.submit = function (type) {

        if ($("input[name='IsEntrustAgency']:checked").val() == "1") {
            if (!$("input[name='AgencyName']").val()) {
                alert("请输入代理机构名称。");
                return;
            }
        }

        if (!verifydata())
            return;

        call_form({

            formid: "info",
            url: window.location.href,
            forceSync: true,    //强制同步
            async: false,
            data: {
                action: "m_save",
                iptype: type
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData.Result === "0") {
                    $("a[role='btn_cancel']").click();
                }
                else {
                    alert(aaData.errMsg);
                }
            }
        });
    }
    //我的知识产权-关闭tab页
    window.ipmanager.myip.closetab = function (type) {
        Tabs.closeTab(type);
        $("#" + type.substring(type.indexOf('_') + 1, type.length)).click();
    }
    //加载详细页面信息
    window.ipmanager.myip.loaddetail = function () {

        call_ajax({
            url: location.href,
            data: {
                action: "m_load"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result === "0") {
                        var type = aaData.Type;

                        switch (type) {
                            case "pa":
                                var data = aaData.Data;
                                // 1.绑定列表
                                if (data.length > 0) {
                                    for (var item in data[0]) {
                                        switch (item) {

                                            case "patType":
                                                $("[name='" + item + "']").val(getpatype(data[0][item]));
                                                break;
                                            default:
                                                $("[name='" + item + "']").val(data[0][item]);
                                                break;
                                        }
                                    }
                                } else {
                                    var data = aaData.Data.rows;
                                    // 1.绑定列表
                                    if (data.length > 0) {
                                        for (var item in data[0]) {
                                            switch (item) {
                                                case "Name":
                                                    if (data[0][item])
                                                        $("[name='title']").val((data[0][item]));
                                                    break;
                                                case "PA_Type":
                                                    if (data[0][item])
                                                        $("[name='patType']").val((data[0][item]));
                                                    break;
                                                case "ApplyNo":
                                                    if (data[0][item])
                                                        $("[name='sqh']").val((data[0][item]));
                                                    break;
                                                case "ApplyDate":
                                                    if (data[0][item])
                                                        $("[name='appDate']").val(createDate(data[0][item]).Format("yyyy-MM-dd"));
                                                    break;
                                                case "ApplicantName":
                                                    if (data[0][item])
                                                        $("[name='applicantName']").val((data[0][item]));
                                                    break;
                                                case "ApplicantAddress":
                                                    if (data[0][item])
                                                        $("[name='address']").val((data[0][item]));
                                                    break;
                                                case "CaseStatus":
                                                    if (data[0][item])
                                                        $("[name='flState']").val((data[0][item]));
                                                    break;
                                                case "AgencyName":
                                                    if (data[0][item])
                                                        $("[name='agencyName']").val((data[0][item]));
                                                    break;
                                            }
                                        }
                                    }
                                }
                                break;
                            case "tm":
                                var data = aaData.Data;
                                // 1.绑定列表
                                if (data.length > 0) {
                                    for (var item in data[0]) {
                                        switch (item) {
                                            case "commodityList":
                                                var ledata = data[0][item];
                                                if (ledata.length > 0) {
                                                    $("#" + item).empty();

                                                    for (var i = 0; i < ledata.length; i++) {
                                                        var html = '';
                                                        html += '<li>' + ledata[i]["commodityAnalogousGroups"] + ' ' + ledata[i]["commodityNameCN"] + '</li>';

                                                        $("#" + item).append(html);
                                                    }
                                                }
                                                break;
                                            case "registrarList":
                                                var ledata = data[0][item];
                                                if (ledata.length > 0) {
                                                    $("[name='registrarCN']").html(ledata[0]["registrarCN"]);
                                                    $("[name='registrarAddressCN']").html(ledata[0]["registrarAddressCN"]);
                                                    $("[name='registrarEN']").html(ledata[0]["registrarEN"]);
                                                    $("[name='registrarAddressEN']").html(ledata[0]["registrarAddressEN"]);
                                                }
                                                break;
                                            case "tmPicturePath"://
                                                if (data[0][item]) {
                                                    data[0][item] = data[0][item].replace("~/", searchenginepath)
                                                    $("#" + item).html("<img class='tra-pageimg' src='" + data[0][item] + "' onerror='onerror=null; src=\"img/NoPictures.png\"' />");
                                                }
                                                break;
                                            case "isJointApp":
                                                var isjointapp = "";
                                                if (data[0][item] == "0") {
                                                    isjointapp = '否';
                                                } else {
                                                    isjointapp = '是';
                                                }

                                                $("[name='" + item + "']").html(isjointapp);
                                                break;
                                            default:
                                                $("[name='" + item + "']").html(data[0][item]);
                                                break;
                                        }
                                    }

                                } else {
                                    var data = aaData.Data.rows;
                                    // 1.绑定列表
                                    if (data.length > 0) {
                                        for (var item in data[0]) {
                                            switch (item) {
                                                case "Name":
                                                    if (data[0][item])
                                                        $("[name='name']").html((data[0][item]));
                                                    break;
                                                case "TM_IntCls":
                                                    if (data[0][item])
                                                        $("[name='intCls']").html((data[0][item]));
                                                    break;
                                                case "TM_FilePath":
                                                    if (data[0][item]) {
                                                        data[0][item] = data[0][item].replace("~/", getRootPath() + "/")
                                                    }
                                                    $("#tmPicturePath").html("<img class='tra-pageimg' src='" + data[0][item] + "' onerror='onerror=null; src=\"../../../img/NoPictures.png\"' />");
                                                    break;
                                                case "ApplyNo":
                                                    if (data[0][item])
                                                        $("[name='sqh']").html((data[0][item]));
                                                    break;
                                                case "ApplyDate":
                                                    if (data[0][item])
                                                        $("[name='appDate']").html(createDate(data[0][item]).Format("yyyy-MM-dd"));
                                                    break;
                                                case "ApplicantName":
                                                    if (data[0][item])
                                                        $("[name='registrarCN']").html((data[0][item]));
                                                    break;
                                                case "ApplicantAddress":
                                                    if (data[0][item])
                                                        $("[name='registrarAddressCN']").html((data[0][item]));
                                                    break;
                                                case "CaseStatus":
                                                    if (data[0][item])
                                                        $("[name='flState']").html((data[0][item]));
                                                    break;
                                                case "AgencyName":
                                                    if (data[0][item])
                                                        $("[name='agentName']").html((data[0][item]));
                                                    break;
                                            }
                                        }
                                    }
                                }
                                break;
                            case "co":
                                var data = aaData.Data.rows;
                                for (var item in data[0]) {
                                    if (data[0][item]) {
                                        switch (item) {
                                            case "ApplyDate"://
                                                if (data[0][item]) {
                                                    $("input[name='" + item + "']").val(createDate(data[0][item]).Format("yyyy-MM-dd"));
                                                }
                                                break;
                                            case "CO_Type":
                                            case "IsEntrustAgency":
                                                if (data[0][item]) {
                                                    $("input[name='" + item + "']").each(function () {
                                                        if ($(this).val() == data[0][item]) {
                                                            $(this).prop("checked", true);
                                                            $(this).trigger("click");
                                                        }
                                                    });
                                                }
                                                break;
                                            default:
                                                $("input[name='" + item + "']").val(data[0][item]);
                                                break;
                                        }

                                    }
                                }
                                break;
                            default:
                        }

                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }

    //选择附件
    window.ipmanager.myip.choicefile = function () {
        $("input[name='File'][type='file']").click();
    }
    //附件控件上传图片发生变化
    window.ipmanager.myip.filechange = function (index) {

        if (!checkfilemaxsize($("input[name='File'][type='file']")[0], filemaxsize)) {
            alert("选择的文件太大了, 最大允许上传" + filemaxsize + "KB");
            return;
        }

        var extension = $("input[name='File'][type='file']").val().substring($("input[name='File'][type='file']").val().lastIndexOf('.')).toLowerCase();
        var extensionstr = ".jpg, .jpeg, .png, .bmp, .gif";
        if (extensionstr.indexOf(extension) >= 0) {
            $("#text").html($("input[name='File'][type='file']").val());
            $("input[name='TM_IsUploadFile']").val("1");
        }
        else {
            alert("请上传" + extensionstr + "格式的文件");
            return;
        }

    }
    /******************* 我的知识产权 end*****************/



    /******************* 竞争对手专利 begin*****************/
    if (!window.ipmanager.competitorpa) {
        window.ipmanager.competitorpa = {};
    }

    //竞争对手专利-加载列表
    window.ipmanager.competitorpa.loadlist = function () {

        var option = {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "m_load";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "ID",
            height: $("body").height() - 400,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [
                {
                    field: 'ApplyDate',
                    title: '申请日',
                    datatype: 'datetime',
                    ext: 'like'
                },
                {
                    field: 'ApplyNo',
                    title: '申请号/专利号',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'PAName',
                    title: '专利名称',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'PAType',
                    title: '专利类型',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'ApplicantName',
                    title: '申请人/权利人',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'CaseStatus',
                    title: '案件状态',
                    datatype: 'string',
                    ext: 'like'
                }
            ],
            columns: [
                {
                    field: '_ckb_',
                    title: '',
                    checkbox: true,
                    valign: 'middle',
                    align: 'center'
                },
                {
                    field: 'ID',
                    title: '',
                    visible: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'ApplyDate',
                    title: '申请日',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            var date = createDate(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'ApplyNo',
                    title: '申请号/专利号',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'PAName',
                    title: '专利名称',
                    sortable: false,
                    valign: 'middle',
                    align: 'center',
                    events: {
                        'click #todetail': function (e, value, row, index) {

                            var url = "member-ip-competitorpa-detail-" + row.ApplyNo.replace('.', '');
                            Tabs.addTabs({
                                id: "detail",
                                title: "专利详情",
                                url: url,
                                close: true
                            });
                        }
                    },
                    formatter: function (value, row, index) {

                        return [
                            '<a class="color-blue" id="todetail" href="javascript:void(0);">' + value + '</a>'
                        ].join('');
                    }
                }, {
                    field: 'PAType',
                    title: '专利类型',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'ApplicantName',
                    title: '申请人/权利人',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'CaseStatus',
                    title: '案件状态',
                    sortable: false,
                    valign: 'middle',
                    align: 'center'
                }
            ]
        };
        Table.loadTable($("#table"), option);
    }

    //竞争对手专利-加载搜索列表
    window.ipmanager.competitorpa.loadsearchlist = function (type) {

        $("[type='search']").show();

        var url = window.location.href + "?name=" + $("select[name='name']").val() + "&value=" + $("input[name='value']").val();
        url = encodeURI(url);
        url = encodeURI(url);

        var option = {
            url: url,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "m_search";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "ID",
            height: $("body").height() - 250,
            columns: [
                {
                    field: '_ckb_',
                    title: '',
                    checkbox: true,
                    valign: 'middle',
                    align: 'center'
                },
                {
                    field: 'ID',
                    title: '',
                    visible: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'appDate',
                    title: '申请日',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'sqh',
                    title: '申请号/专利号',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'title',
                    title: '专利名称',
                    sortable: false,
                    valign: 'middle',
                    align: 'center',
                    formatter: function (value, row, index) {

                        return [
                            '<a class="color-blue" id="todetail" target="_blank" href="search-padetail-' + row.sqh.replace('.', '') + '">' + value + '</a>'
                        ].join('');
                    }
                }, {
                    field: 'patType',
                    title: '专利类型',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return getpatype(value);
                    }
                }, {
                    field: 'sqr',
                    title: '申请人/权利人',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'flState',
                    title: '案件状态',
                    sortable: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'agencyName',
                    title: '代理机构',
                    sortable: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'operate',
                    title: '操作',
                    align: 'center',
                    valign: 'middle',
                    events: {
                        'click #monitor': function (e, value, row, index) {
                            window.ipmanager.competitorpa.monitor(row.sqh);
                        }
                    },
                    formatter: function (value, row, index) {
                        switch (row.state) {
                            case "Y":
                                return [
                                    '<a class="table-btn btn-gray" href="javascript:void(0)">已监控</a>'
                                ].join('');
                                break;
                            default:
                                return [
                                    '<a class="table-btn btn-orange" id="monitor" sqh=' + row.sqh + ' href="javascript:void(0)">监控</a>'
                                ].join('');
                                break;
                        }
                    }
                }
            ]
        };
        Table.loadTable($("#table"), option);
    }
    //竞争对手专利-加载详情页
    window.ipmanager.competitorpa.loaddetail = function () {

        call_ajax({
            url: location.href,
            data: {
                action: "m_loaddetail"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result === "0") {
                        var data = aaData.Data;

                        // 1.绑定列表
                        if (data.length > 0) {
                            for (var item in data[0]) {
                                switch (item) {
                                    case "patType":
                                        $("[name='" + item + "']").html(getpatype(data[0][item]));
                                        break;
                                    default:
                                        $("[name='" + item + "']").html(data[0][item]);
                                        break;
                                }
                            }
                        }
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //竞争对手专利-监控
    window.ipmanager.competitorpa.monitor = function (sqh) {

        call_ajax({
            url: location.href,
            data: {
                action: "m_monitor",
                sqh: sqh
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result === "0") {
                        var arr = sqh.split(',');
                        for (var i = 0; i < arr.length; i++) {
                            $("a[sqh='" + arr[i] + "']").removeClass("btn-orange");
                            $("a[sqh='" + arr[i] + "']").addClass("btn-gray");
                            $("a[sqh='" + arr[i] + "']").html("已监控");
                            $("a[sqh='" + arr[i] + "']").unbind("click");
                        }
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //竞争对手专利-删除
    window.ipmanager.competitorpa.delete = function () {

        var ID = Table.getIdSelectionsString($("#table"), "ID");

        call_ajax({
            url: location.href,
            data: {
                action: "m_delete",
                id: ID
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result === "0") {
                        $(".deletebox").hide();
                        $(".successbox").show();
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //竞争对手专利-关闭tab页
    window.ipmanager.competitorpa.closetab = function (type) {
        Tabs.closeTab(type);
        if (type == "add") {
            window.ipmanager.competitorpa.loadlist();
        }
    }
    /******************* 竞争对手专利 begin*****************/

    /*******************************知识产权管理 end*********************************/


    /*******************************我的关注 begin*********************************/
    if (!window.concerns) {
        window.concerns = {};
    }

    //加载列表
    window.concerns.loadlist = function (type) {
        var option = {};
        switch (type) {
            case "policy":
                option = window.concerns.policy(type);
                break;
            case "pasearch":
                option = window.concerns.pasearch(type);
                break;
            case "patrans":
                option = window.concerns.patrans(type);
                break;
        }
        $("[role='searchpanel']").html('');
        $(".m-tablebox").html("");
        $(".m-tablebox").append('<table class="m-table" id="table"></table>');

        Table.loadTable($("#table"), option);
    }
    //我的关注-政策
    window.concerns.policy = function (type) {
        return {
            url: window.location.href + "?type=" + type,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "m_loadlist";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "ID",
            height: $("body").height() - 370,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [
                {
                    field: 'Title',
                    title: '标题',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'CreateTime',
                    title: '关注日期',
                    datatype: 'datetime',
                    ext: 'like'
                }
            ],
            columns: [
                {
                    field: '_ckb_',
                    title: '',
                    checkbox: true,
                    valign: 'middle',
                    align: 'center'
                },
                {
                    field: 'ID',
                    title: '',
                    visible: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'Title',
                    title: '标题',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    events: {
                        'click #todetail': function (e, value, row, index) {
                            var url = "policiesregulations-detail-" + row.NewsType + "-" + row.PoliciesRegulationsID;
                            window.open(url, "_blank");
                        }
                    },
                    formatter: function (value, row, index) {
                        if (value.length > 30) {
                            value = value.substring(0, 27) + "..."
                        }
                        return [
                            '<a class="color-blue" id="todetail" href="javascript:void(0);">' + value + '</a>'
                        ].join('');
                    }
                }, {
                    field: 'PublishDate',
                    title: '发布时间',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            var date = createDate(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'NewsType',
                    title: '类型',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            switch (value) {
                                case "1":
                                    return "国际条约";
                                case "2":
                                    return "国内法规";
                                case "3":
                                    return "省市政策";
                                case "4":
                                    return "本区政策";
                            }
                        }
                    }
                }, {
                    field: 'Summary',
                    title: '摘要',
                    sortable: false,
                    valign: 'middle',
                    align: 'center',
                    formatter: function (value, row, index) {
                        if (value.length > 30) {
                            value = value.substring(0, 27) + "..."
                        }
                        return value;
                    }
                }, {
                    field: 'CreateTime',
                    title: '关注日期',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            var date = createDate(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'operate',
                    title: '操作',
                    align: 'center',
                    valign: 'middle',
                    events: {
                        'click #cancelmonitor': function (e, value, row, index) {
                            window.concerns.cancelmonitor(row.ID, "policy")
                        }
                    },
                    formatter: function (value, row, index) {

                        return [
                            '<a class="table-btn btn-red" id="cancelmonitor" href="javascript:void(0)">取消关注</a>'
                        ].join('');


                    }
                }
            ]
        };
    }
    //我的关注-专利检索
    window.concerns.pasearch = function (type) {
        return {
            url: window.location.href + "?type=" + type,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "m_loadlist";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "ID",
            height: $("body").height() - 400,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [
                {
                    field: 'PAName',
                    title: '专利名称',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'ApplyNo',
                    title: '申请号/专利号',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'PAType',
                    title: '专利类型',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'LegalState',
                    title: '法律状态',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'CreateTime',
                    title: '关注日期',
                    datatype: 'datetime',
                    ext: 'like'
                }
            ],
            columns: [
                {
                    field: '_ckb_',
                    title: '',
                    checkbox: true,
                    valign: 'middle',
                    align: 'center'
                },
                {
                    field: 'ID',
                    title: '',
                    visible: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'PAName',
                    title: '专利名称',
                    sortable: false,
                    valign: 'middle',
                    align: 'center',
                    events: {
                        'click #todetail': function (e, value, row, index) {
                            var url = "search-padetail-" + row.ApplyNo.replace('.', '');
                            window.open(url, "_blank");
                        }
                    },
                    formatter: function (value, row, index) {
                        if (value.length > 30) {
                            value = value.substring(0, 27) + "..."
                        }
                        return [
                            '<a class="color-blue" id="todetail" href="javascript:void(0);">' + value + '</a>'
                        ].join('');
                    }
                }, {
                    field: 'ApplyNo',
                    title: '申请号/专利号',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'ApplyDate',
                    title: '申请日',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            var date = createDate(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'PAType',
                    title: '专利类型',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'LegalState',
                    title: '法律状态',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'CreateTime',
                    title: '关注日期',
                    sortable: false,
                    valign: 'middle',
                    align: 'center',
                    formatter: function (value, row, index) {
                        if (value) {
                            var date = createDate(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'operate',
                    title: '操作',
                    align: 'center',
                    valign: 'middle',
                    events: {
                        'click #cancelmonitor': function (e, value, row, index) {
                            window.concerns.cancelmonitor(row.ID, "pasearch")
                        }
                    },
                    formatter: function (value, row, index) {

                        return [
                            '<a class="table-btn btn-red" id="cancelmonitor" href="javascript:void(0)">取消关注</a>'
                        ].join('');


                    }
                }
            ]
        };
    }
    //我的关注-专利交易
    window.concerns.patrans = function (type) {
        return {
            url: window.location.href + "?type=" + type,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "m_loadlist";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "ID",
            height: $("body").height() - 400,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [
                {
                    field: 'PAName',
                    title: '专利名称',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'ApplyNo',
                    title: '申请号/专利号',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'Industry',
                    title: '行业领域',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'LegalState',
                    title: '法律状态',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'TranState',
                    title: '交易状态',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'CreateTime',
                    title: '关注日期',
                    datatype: 'datetime',
                    ext: 'like'
                }
            ],
            columns: [
                {
                    field: '_ckb_',
                    title: '',
                    checkbox: true,
                    valign: 'middle',
                    align: 'center'
                },
                {
                    field: 'ID',
                    title: '',
                    visible: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'PAName',
                    title: '专利名称',
                    sortable: false,
                    valign: 'middle',
                    align: 'center',
                    events: {
                        'click #todetail': function (e, value, row, index) {
                            var url = "operation-detail-1-" + row.FK_TransactionPA_ID;
                            window.open(url, "_blank");

                        }
                    },
                    formatter: function (value, row, index) {
                        if (value.length > 30) {
                            value = value.substring(0, 27) + "..."
                        }
                        return [
                            '<a class="color-blue" id="todetail" href="javascript:void(0);">' + value + '</a>'
                        ].join('');
                    }
                }, {
                    field: 'ApplyNo',
                    title: '申请号/专利号',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'Industry',
                    title: '行业领域',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'LegalState',
                    title: '法律状态',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'TranState',
                    title: '交易状态',
                    sortable: false,
                    valign: 'middle',
                    align: 'center',
                    formatter: function (value, row, index) {
                        if (value) {
                            switch (value) {
                                case "1":
                                    return "未交易";
                                    break;
                                case "2":
                                    return "交易成功";
                                    break;
                                case "3":
                                    return "取消交易";
                                    break;
                                default:
                            }
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'CreateTime',
                    title: '关注日期',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            var date = createDate(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'operate',
                    title: '操作',
                    align: 'center',
                    valign: 'middle',
                    events: {
                        'click #cancelmonitor': function (e, value, row, index) {
                            window.concerns.cancelmonitor(row.ID, "patrans");
                        }
                    },
                    formatter: function (value, row, index) {

                        return [
                            '<a class="table-btn btn-red" id="cancelmonitor" href="javascript:void(0)">取消关注</a>'
                        ].join('');


                    }
                }
            ]
        };
    }
    //取消关注
    window.concerns.cancelmonitor = function (id, type) {

        if (!confirm("是否确认取消监控此数据？")) {
            return;
        }

        call_ajax({
            url: location.href,
            data: {
                action: "m_delete",
                type: type,
                id: id
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result === "0") {
                        alert("取消监控成功");
                        $("li[type='" + type + "']").click();
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    /*******************************我的关注 end*********************************/


    /*******************************专利运营 begin*********************************/
    if (!window.operation) {
        window.operation = {};
    }
    /******************* 专利交易 begin*****************/
    if (!window.operation.trans) {
        window.operation.trans = {};
    }

    //专利交易-加载列表
    window.operation.trans.loadlist = function () {

        var option = {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "m_load";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "ID",
            height: $("body").height() - 400,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [
                {
                    field: 'PAName',
                    title: '专利名称',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'ApplyNo',
                    title: '申请号/专利号',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'TranType',
                    title: '交易方式',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'PublishDate',
                    title: '发布日期',
                    datatype: 'datetime',
                    ext: 'like'
                },
                {
                    field: 'TranState',
                    title: '交易状态',
                    datatype: 'string',
                    ext: 'like'
                }
            ],
            columns: [
                {
                    field: '_ckb_',
                    title: '',
                    checkbox: true,
                    valign: 'middle',
                    align: 'center'
                },
                {
                    field: 'ID',
                    title: '',
                    visible: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'PAName',
                    title: '专利名称',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    events: {
                        'click #todetail': function (e, value, row, index) {

                            var url = "member-operation-trans-detail-detail-" + row.ID;
                            Tabs.addTabs({
                                id: "detail",
                                title: "详情",
                                url: url,
                                close: true
                            });
                        }
                    },
                    formatter: function (value, row, index) {

                        return [
                            '<a class="color-blue" id="todetail" href="javascript:void(0);">' + value + '</a>'
                        ].join('');
                    }
                }, {
                    field: 'ApplyNo',
                    title: '申请号/专利号',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'PAType',
                    title: '专利类型',
                    sortable: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'MainIPC',
                    title: 'IPC分类',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'TranPrice',
                    title: '售价',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'TranType',
                    title: '交易方式',
                    sortable: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'PublishDate',
                    title: '发布日期',
                    sortable: false,
                    valign: 'middle',
                    align: 'center',
                    formatter: function (value, row, index) {
                        if (value) {
                            var date = createDate(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'TranState',
                    title: '交易状态',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            switch (value) {
                                case "1":
                                    return "未交易";
                                    break;
                                case "2":
                                    return "交易成功";
                                    break;
                                case "3":
                                    return "取消交易";
                                    break;
                                default:
                            }
                        }
                        else {
                            return "";
                        }
                    }
                }
            ]
        };
        Table.loadTable($("#table"), option);
    }
    //专利交易-保存
    window.operation.trans.submit = function () {
        //如果填写，增加格式校验
        var arr = ["LinkPhone", "LinkQQ", "LinkEmail"];
        for (var i = 0; i < arr.length; i++) {
            $("input[name='" + arr[i] + "']").attr("ismust", "false");
            if ($("input[name='" + arr[i] + "']").val()) {
                $("input[name='" + arr[i] + "']").attr("ismust", "true");
            }
        }

        if (!verifydata())
            return;

        if (!confirm("是否确认保存？")) {
            return;
        }

        call_form({
            formid: "info",
            url: window.location.href,
            forceSync: true,    //强制同步
            async: false,
            data: {
                action: "m_save"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData.Result === "0") {
                    alert("保存成功");
                    $("a[role='btn_cancel']").click();
                }
                else {
                    alert(aaData.errMsg);
                }
            }
        });
    }
    //专利交易-加载详情页
    window.operation.trans.loaddetail = function () {

        call_ajax({
            url: location.href,
            data: {
                action: "m_load"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result === "0") {
                        var data = aaData.Data.rows;

                        // 1.绑定列表
                        if (data.length > 0) {
                            for (var item in data[0]) {
                                switch (item) {
                                    case "IndustryP":
                                        if (data[0][item]) {

                                            var industryc = "";
                                            if (data[0]["IndustryC"]) {
                                                industryc = "-" + data[0]["IndustryC"];
                                            }

                                            $("input[name='Industry']").val(data[0][item] + industryc);
                                        }
                                        break;
                                    case "TranPrice":
                                        if (data[0][item]) {
                                            $("input[name='TranPrice']").val(data[0][item]);
                                            if (data[0][item] == "商议") {
                                                $("input[name='check_discuss']").each(function () {
                                                    if ($(this).val() == data[0][item]) {
                                                        $(this).prop("checked", true);
                                                    }
                                                });
                                            }
                                        }
                                        break;
                                    case "PAImage":
                                        var path = null;
                                        if (data[0][item]) {
                                            $("#text").html(data[0][item]);
                                            $("input[name='FilePath']").val(data[0][item]);

                                            if (data[0]["IsUploadPAImage"] == "0") {
                                                path = data[0][item].replace("~/", searchengine)
                                            } else {
                                                path = data[0][item].replace("~/", getRootPath() + "/")
                                            }
                                        }

                                        $("#tmimage").attr("onerror", "onerror=null;src=\"../../../../../img/NoPictures.png\"");
                                        if (path) {
                                            $("#tmimage").attr("src", path);
                                        } else {
                                            $("#tmimage").attr("src", "../../../../../img/NoPictures.png");
                                        }
                                        break;
                                    case "ShenQingRi":
                                    case "GongKaiRi":
                                        if (data[0][item]) {
                                            $("[name='" + item + "']").val(createDate(data[0][item]).Format("yyyy-MM-dd"));
                                        }
                                        break;
                                    default:
                                        $("[name='" + item + "']").val(data[0][item]);
                                        break;
                                }
                            }

                        }
                        if (GetQueryString(4) == 'detail')//状态不为暂存时
                        {
                            $("a[role='btn_search']").remove();
                            $("a[role='btn_confirm']").remove();
                            setcontrolDisabled("info", "disable");//页面控件不可用

                            $(".modifyInfo_upload").hide();
                            $("span[role='btn_downloadfile']").show();
                            $("span[class='color-red fblod']").remove();//去掉必填符号
                        }
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //专利交易-取消交易）
    window.operation.trans.delete = function () {

        var ID = Table.getIdSelectionsString($("#table"), "ID");

        call_ajax({
            url: location.href,
            data: {
                action: "m_canceltrans",
                id: ID
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result === "0") {
                        $(".deletebox").hide();
                        $(".successbox").show();
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //专利交易-关闭tab页
    window.operation.trans.closetab = function (type) {
        Tabs.closeTab(type);
        if (type != "detail") {
            window.operation.trans.loadlist();
        }
    }
    //选择附件
    window.operation.trans.choicefile = function () {
        $("input[name='File'][type='file']").click();
    }
    //附件控件上传图片发生变化
    window.operation.trans.filechange = function (index) {

        if (!checkfilemaxsize($("input[name='File'][type='file']")[0], filemaxsize)) {
            alert("选择的文件太大了, 最大允许上传" + filemaxsize + "KB");
            return;
        }

        var extension = $("input[name='File'][type='file']").val().substring($("input[name='File'][type='file']").val().lastIndexOf('.')).toLowerCase();
        var extensionstr = " .png, .jpg, .bmp, .gif";
        if (extensionstr.indexOf(extension) >= 0) {
            $("#text").html($("input[name='File'][type='file']").val());
        }
        else {
            alert("请上传.png, .jpg, .bmp, .gif格式的文件");
            return;
        }
        $("input[name='IsUploadPAImage']").val("1");
    }
    //加载联系人数据
    window.operation.trans.linkmaninfo = function () {
        call_ajax({
            url: location.href,
            data: {
                action: "m_loadlinkman"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        //明细信息
                        var data = aaData.Data.rows;
                        for (var item in data[0]) {
                            if (data[0][item]) {
                                switch (item) {
                                    case "LinkMan":
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                    case "MobilePhone":
                                        $("input[name='LinkMobile']").val(data[0][item]);
                                        break;
                                }

                            }
                        }
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //点击搜索按钮
    window.operation.trans.search = function () {

        var sqh = $("input[name='ApplyNo']").val();
        if (!sqh) {
            alert("请填写申请号/注册号");
            return;
        }

        if (!CheckSQH(sqh)) {
            alert("申请号/注册号格式错误，请确认。");
            return;
        }
        call_ajax({
            url: location.href,
            data: {
                action: "m_search",
                sqh: sqh
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        //明细信息
                        var data = aaData.Data.rows;
                        for (var item in data[0]) {
                            if (data[0][item]) {
                                switch (item) {
                                    case "title":
                                        $("input[name='PAName']").val(data[0][item]);
                                        break;
                                    case "sqh":
                                        $("input[name='ApplyNo']").val(data[0][item]);
                                        break;
                                    case "appDate":
                                        $("input[name='ShenQingRi']").val(data[0][item]);
                                        break;
                                    case "pubNumber":
                                        $("input[name='GongGaoHao']").val(data[0][item]);
                                        break;
                                    case "pubDate":
                                        $("input[name='GongKaiRi']").val(data[0][item]);
                                        break;
                                    case "abs":
                                        $("textarea[name='Description']").val(data[0][item]);
                                        break;
                                    case "industry":
                                        $("input[name='Industry']").val(data[0][item]);
                                        break;
                                    case "abs":
                                        $("textarea[name='Description']").val(data[0][item]);
                                        break;
                                    case "sqr":
                                        $("input[name='ApplicantName']").val(data[0][item]);
                                        break;
                                    case "patType":
                                        var imagepath = data[0]["drawsPath"];

                                        switch (data[0][item]) {
                                            case "1":
                                                $("select[name='PAType']").val("发明专利");
                                                break;
                                            case "2":
                                                $("select[name='PAType']").val("实用新型");
                                                break;
                                            case "3":
                                                $("select[name='PAType']").val("外观设计");
                                                imagepath = data[0]["tifDistributePath"];
                                                break;
                                            case "8":
                                                $("select[name='PAType']").val("PCT发明");
                                                break;
                                            case "9":
                                                $("select[name='PAType']").val("PCT实用新型");
                                                break;
                                        }

                                        $("#text").html(imagepath);
                                        $("input[name='FilePath']").val(imagepath);
                                        break;
                                    case "mainIpc":
                                        $("input[name='MainIPC']").val(data[0][item]);
                                        break;
                                }

                            }
                        }
                        $("input[name='IsUploadPAImage']").val("0");
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    /******************* 专利交易 begin*****************/


    /******************* 专利需求 begin*****************/
    if (!window.operation.req) {
        window.operation.req = {};
    }

    //专利需求-加载列表
    window.operation.req.loadlist = function () {

        var option = {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "m_load";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "ID",
            height: $("body").height() - 400,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [
                {
                    field: 'PAName',
                    title: '名称',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'Industry',
                    title: '行业领域',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'TranType',
                    title: '交易方式',
                    datatype: 'string',
                    ext: 'like'
                },
                {
                    field: 'PublishDate',
                    title: '发布日期',
                    datatype: 'datetime',
                    ext: 'like'
                }
            ],
            columns: [
                {
                    field: '_ckb_',
                    title: '',
                    checkbox: true,
                    valign: 'middle',
                    align: 'center'
                },
                {
                    field: 'ID',
                    title: '',
                    visible: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'PAName',
                    title: '名称',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    events: {
                        'click #todetail': function (e, value, row, index) {

                            var url = "member-operation-req-detail-detail-" + row.ID;
                            Tabs.addTabs({
                                id: "detail",
                                title: "详情",
                                url: url,
                                close: true
                            });
                        }
                    },
                    formatter: function (value, row, index) {

                        return [
                            '<a class="color-blue" id="todetail" href="javascript:void(0);">' + value + '</a>'
                        ].join('');
                    }
                }, {
                    field: 'Industry',
                    title: '行业领域',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'TranType',
                    title: '交易方式',
                    sortable: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'PublishDate',
                    title: '发布日期',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            var date = createDate(value);
                            return date.Format("yyyy-MM-dd");
                        }
                        else {
                            return "";
                        }
                    }
                }, {
                    field: 'Budget',
                    title: '投资预算',
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    field: 'LinkMan',
                    title: '联系人',
                    sortable: false,
                    valign: 'middle',
                    align: 'center'
                }, {
                    field: 'LinkMobile',
                    title: '联系方式',
                    sortable: false,
                    valign: 'middle',
                    align: 'center'
                }
            ]
        };
        Table.loadTable($("#table"), option);
    }
    //保存
    window.operation.req.submit = function () {
        //如果填写，增加格式校验
        var arr = ["LinkPhone", "LinkQQ", "LinkEmail"];
        for (var i = 0; i < arr.length; i++) {
            $("input[name='" + arr[i] + "']").attr("ismust", "false");
            if ($("input[name='" + arr[i] + "']").val()) {
                $("input[name='" + arr[i] + "']").attr("ismust", "true");
            }
        }
        if (!verifydata())
            return;

        if (!confirm("是否确认保存？")) {
            return;
        }

        call_ajax({
            url: location.href,
            data: {
                action: "m_save",
                info: JSON.stringify($("#info").serializeJson())
            },
            success: function (data) {
                var aaData = strToJson(data);
                var data = "";
                if (aaData) {
                    if (aaData.Result === "0") {
                        alert("保存成功");
                        $("a[role='btn_cancel']").click();
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //专利需求-加载详情页
    window.operation.req.loaddetail = function () {

        call_ajax({
            url: location.href,
            data: {
                action: "m_load"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result === "0") {
                        var data = aaData.Data.rows;

                        // 1.绑定列表
                        if (data.length > 0) {
                            for (var item in data[0]) {
                                switch (item) {
                                    case "ReqTime":
                                        if (data[0][item]) {
                                            $("[name='" + item + "']").val(createDate(data[0][item]).Format("yyyy-MM-dd"));
                                        }
                                        break;
                                    default:
                                        $("[name='" + item + "']").val(data[0][item]);
                                        break;
                                }
                            }

                        }
                        if (GetQueryString(4) == 'detail')//状态不为暂存时
                        {
                            $("a[role='btn_confirm']").remove();
                            setcontrolDisabled("info", "disable");//页面控件不可用
                            $("span[class='color-red fblod']").remove();//去掉必填符号
                        }
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //专利需求-删除
    window.operation.req.delete = function () {

        var ID = Table.getIdSelectionsString($("#table"), "ID");

        call_ajax({
            url: location.href,
            data: {
                action: "m_delete",
                id: ID
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result === "0") {
                        $(".deletebox").hide();
                        $(".successbox").show();
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //专利需求-关闭tab页
    window.operation.req.closetab = function (type) {
        Tabs.closeTab(type);
        if (type != "detail") {
            window.operation.req.loadlist();
        }
    }
    //加载联系人数据
    window.operation.req.linkmaninfo = function () {
        call_ajax({
            url: location.href,
            data: {
                action: "m_loadlinkman"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result == "0") {
                        //明细信息
                        var data = aaData.Data.rows;
                        for (var item in data[0]) {
                            if (data[0][item]) {
                                switch (item) {
                                    case "LinkMan":
                                        $("input[name='" + item + "']").val(data[0][item]);
                                        break;
                                    case "MobilePhone":
                                        $("input[name='LinkMobile']").val(data[0][item]);
                                        break;
                                }

                            }
                        }
                    }
                    else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    /******************* 专利需求 begin*****************/
    /*******************************专利运营 end*********************************/


    /*******************************我的专家 begin*********************************/
    if (!window.expert) {
        window.expert = {};
    }

    //加载列表
    window.expert.loadlist = function () {
        var option = {
            url: window.location.href,
            method: "post",
            cache: false,
            queryParams: function getParams(params) {
                params.action = "m_loadlist";
                return params;
            },
            pagination: true,
            sidePagination: 'server',//设置为服务器端分页
            smartDisplay: false, // 设置分页选择列表是否全部显示
            search: false,//搜索
            showRefresh: false,//显示刷新
            showToggle: false,//显示向下延伸
            showColumns: false,//显示列
            showExport: false,//显示导出
            detailView: false,
            striped: true,
            idField: "id",
            height: $(".memberbox").height() + 60,
            searchShowType: "detail",
            searchPanel: $("div[role='searchpanel']"),
            searchColumns: [{
                title: '姓名',
                field: 'Name',
                datatype: 'string',
                ext: 'like'
            }, {
                title: '学院/部门',
                field: "Department",
                datatype: 'string',
                ext: 'like'
            }, {
                title: '录入时间',
                field: 'CreateTime',
                datatype: 'datetime',
                ext: 'like'
            }],
            columns: [
                {
                    title: '_ckb_',
                    field: "",
                    sortable: false,
                    width: '30',
                    checkbox: true,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '',
                    field: 'ID',
                    visible: false,
                    align: 'center'
                }, {
                    title: '姓名',
                    field: 'Name',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            var html = "<a class='color-blue' href='javascript:void(0)' onclick=\"window.expert.opentab('detail'," + row.ID + ");\">" + value + "</a>";
                            return html;
                        } else {
                            return "";
                        }
                    }
                }, {
                    title: '学院/部门',
                    field: "Department",
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '专业',
                    field: "Major",
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '电话',
                    field: "Phone",
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '微信',
                    field: "WeChat",
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: 'QQ',
                    field: "QQ",
                    sortable: false,
                    align: 'center',
                    valign: 'middle'
                }, {
                    title: '录入时间',
                    field: 'CreateTime',
                    sortable: false,
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        if (value) {
                            return createDate(value).Format('yyyy-MM-dd HH:mm');
                        } else {
                            return '';
                        }
                    }
                }
            ]
        };

        Table.loadTable($("#table"), option);
    }


    //打开 Tab
    window.expert.opentab = function (type, id) {
        var url = "member-expert-detail-" + type;
        if (id) {
            url = url + "-" + id
        }

        var tabid = "";
        var tabtitle = "";
        switch (type) {
            case "edite":
                tabid = "editetab";
                tabtitle = "编辑";
                break;
            case "add":
                tabid = "addtab";
                tabtitle = "添加";
                break;
            default:
                tabid = "detailtab";
                tabtitle = "详细";
                break;
        }

        Tabs.addTabs({
            id: tabid,
            title: tabtitle,
            url: url,
            close: true
        });
    }

    //关闭tab
    window.expert.closetab = function (type) {
        var tabid = "";
        switch (type) {
            case "edite":
                tabid = "editetab";
                break;
            case "add":
                tabid = "addtab";
                break;
            default:
                tabid = "detailtab";
                break;
        }
        Tabs.closeTab(tabid);

        window.expert.loadlist();
    }

    //删除
    window.expert.deletes = function () {
        var ids = Table.getIdSelections($("#table"));

        if (ids.length <= 0) {
            alert("没有选中任何数据");
            return;
        }
        var id = Table.getIdSelectionsString($("#table"), "ID");

        call_ajax({
            url: window.location.href,
            data: {
                action: 'm_delete',
                id: id
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result === "0") {
                        alert("删除成功。");
                        window.expert.loadlist();
                    } else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }
    //加载详细信息
    window.expert.loaddetail = function () {

        call_ajax({
            url: window.location.href,
            data: {
                action: 'm_loaddetail'
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData) {
                    if (aaData.Result === "0") {
                        var data = aaData.Data.rows;
                        if (data.length > 0) {
                            //页面信息赋值                        
                            for (var item in data[0]) {
                                if (data[0][item]) {
                                    var pageitem = "expert_" + item.toLowerCase();
                                    switch (item) {
                                        case "CreateTime":
                                            $("*[name='" + pageitem + "']").text(createDate(data[0][item]).Format('yyyy-MM-dd HH:mm'));
                                            break;
                                        case "Image":
                                            if (data[0][item]) {
                                                $("img[name='expert_image_src']").attr("src", data[0][item].replace("~/", getRootPath() + "/"));
                                                $("input[name='expert_image']").val(data[0][item]);
                                            }
                                            break;
                                        default:
                                            $("*[name='" + pageitem + "']").val(data[0][item])
                                            break;
                                    }
                                }
                            }
                        }
                        if (GetQueryString(3) == "detail") {
                            $("body").find("input").each(function () {
                                $(this).attr("disabled", "disabled");
                            });
                            $("body").find("select").each(function () {
                                $(this).attr("disabled", "disabled");
                            });
                            $("body").find("textarea").each(function () {
                                $(this).attr("disabled", "disabled");
                            });

                            $("*[pagetype='detail_hide']").remove();
                        }

                    } else {
                        alert(aaData.errMsg);
                    }
                }
            }
        });
    }

    //加载保存信息
    window.expert.confirm = function () {
        if (!verifydata()) {
            return;
        }
        call_form({
            formid: "info",
            url: window.location.href,
            forceSync: true,    //强制同步
            async: false,
            data: {
                action: "m_save"
            },
            success: function (data) {
                var aaData = strToJson(data);
                if (aaData.Result === "0") {
                    alert("操作成功");
                    $("a[role='btn_cancel']").click();
                }
                else {
                    alert(aaData.errMsg);
                }
            }
        });
    }

    //文件选择
    window.expert.filechange = function (obj) {
        var extension = $(obj).val().substring($(obj).val().lastIndexOf('.')).toLowerCase();
        var extensionstr = " .png, .jpg, .jpeg";
        if (extensionstr.indexOf(extension) >= 0) {
            $("img[name='expert_image_src']").attr("src", window.URL.createObjectURL($(obj)[0].files[0]));
        }
        else {
            alert("请上传.png, .jpg, .jpeg格式的文件");
            return;
        }
    }
    /*******************************我的专家 end*********************************/
});