//去掉最后一条边
$(function () {
    $(".mlist li:last").css("border", "none");
})
//左侧菜单展开和关闭按钮事件
$(document).on("click", ".layout-side-arrow", function () {
    if ($(".layout-side").hasClass("close")) {
        $(".layout-side").removeClass("close");
        $(".layout-main").removeClass("full-page");
        $(this).removeClass("close");
        $(".layout-side-arrow-icon").removeClass("close");
    } else {
        $(".layout-side").addClass("close");
        $(".layout-main").addClass("full-page");
        $(this).addClass("close");
        $(".layout-side-arrow-icon").addClass("close");
    }
});

window.onload = function () {
    var aLi = document.getElementById("membernav").getElementsByTagName("li");
    var i = 0;
    for (i = 0; i < aLi.length; i++) {
        aLi[i].onclick = function () {
            for (i = 0; i < aLi.length; i++) aLi[i].className = aLi[i].className.replace(/\s?active/, "");
            this.className += " active";
        };
    }
};

$(function () {
    var Accordion = function (el, multiple) {
        this.el = el || {};
        this.multiple = multiple || false;

        var links = this.el.find('.listnav-title');

        links.on('click', {
            el: this.el,
            multiple: this.multiple
        }, this.dropdown)
    };

    Accordion.prototype.dropdown = function (e) {
        var $el = e.data.el;
        $this = $(this);
        $next = $this.next();

        $next.slideToggle();
        $this.parent().toggleClass('active');

        if (!e.data.multiple) {
            $el.find('.submenu').not($next).slideUp().parent().removeClass('active');
        }
    };

    var accordion = new Accordion($('#accordion'), false);
    $('.submenu li').click(function () {
        $("li").removeClass('current');
        $(this).addClass('current');
    });
    var accordion = new Accordion($('#membernav'), false);
    $('.submenu li').click(function () {
        $("li").removeClass('current');
        $(this).addClass('current');
    });
});

// 上传文件
$("#fileinp").change(function () {
    $("#text").html($("#fileinp").val());
})

// 弹出
$(document).ready(function ($) {
    $(".m-close").click(function (event) {
        $(".messagebox").css({ "display": "none" });
    });
    $(".phonebtn").click(function (event) {
        $(".nav-blackbox").css({ "display": "block" });
        $(".navlist-r").css({ "display": "block" });
    });
    $(".nav-blackbox").click(function (event) {
        $(".nav-blackbox").css({ "display": "none" });
        $(".navlist-r").css({ "display": "none" });
    });
    $(".floatclose").click(function (event) {
        $(".floatbox").css({ "display": "none" });
        $(".floatbtn").css({ "display": "block" });
    });
    $(".floatbtn").click(function (event) {
        $(".floatbox").css({ "display": "block" });
        $(".floatbtn").css({ "display": "none" });
    });

    $(".loading").click(function (event) {
        $(".loadingbox").css({
            "display": "block"
        });
    });
    $(".loadingbox").click(function (event) {
        $(".loadingbox").css({
            "display": "none"
        });
    });
    $(".delete").click(function (event) {
        $(".deletebox").css({
            "display": "block"
        });
        $(".blackbox").css({
            "display": "block"
        });
    });
    $(".key-m").click(function (event) {
        $(".keybox").css({
            "display": "block"
        });
        $(".blackbox").css({
            "display": "block"
        });
    });
    $(".email-m").click(function (event) {
        $(".emailbox").css({
            "display": "block"
        });
        $(".blackbox").css({
            "display": "block"
        });
    });
    $(".mobile-m").click(function (event) {
        $(".mobilebox").css({
            "display": "block"
        });
        $(".blackbox").css({
            "display": "block"
        });
    });
    $(".qq-m").click(function (event) {
        $(".qqbox").css({
            "display": "block"
        });
        $(".blackbox").css({
            "display": "block"
        });
    });
    $(".p-page").click(function (event) {
        $(".p-pagebox").css({
            "display": "block"
        });
        $(".blackbox").css({
            "display": "block"
        });
    });
    $(".add").click(function (event) {
        $(".addbox").css({
            "display": "block"
        });
        $(".blackbox").css({
            "display": "block"
        });
    });
    $(".modify").click(function (event) {
        $(".modifybox").css({
            "display": "block"
        });
        $(".blackbox").css({
            "display": "block"
        });
    });
    $(".hint").click(function (event) {
        $(".hintbox").css({
            "display": "block"
        });
        $(".blackbox").css({
            "display": "block"
        });
    });
    $(".contact").click(function (event) {
        $(".contactbox").css({
            "display": "block"
        });
        $(".blackbox").css({
            "display": "block"
        });
    });

    $(".jumpbtn").click(function (event) {
        $(".popbox").css({
            "display": "none"
        });
        $(".blackbox").css({
            "display": "none"
        });
    });
    $(".pbox-close").click(function (event) {
        $(".popbox").css({
            "display": "none"
        });
        $(".blackbox").css({
            "display": "none"
        });
    });
    $(".btn-cancel").click(function (event) {
        $(".popbox").css({
            "display": "none"
        });
        $(".blackbox").css({
            "display": "none"
        });
    });
    $(".c-modify").click(function (event) {
        $(".c-modify").css({
            "display": "none"
        });
        $(".c-save").css({
            "display": "block"
        });
        $(".c-cancel").css({
            "display": "block"
        });
    });
    $(".c-save").click(function (event) {
        $(".c-modify").css({
            "display": "block"
        });
        $(".c-save").css({
            "display": "none"
        });
        $(".c-cancel").css({
            "display": "none"
        });
    });
    $(".c-cancel").click(function (event) {
        $(".c-modify").css({
            "display": "block"
        });
        $(".c-save").css({
            "display": "none"
        });
        $(".c-cancel").css({
            "display": "none"
        });
    });
    $(".btn-confirm").click(function (event) {
        $(".popbox").css({
            "display": "none"
        });
        $(".blackbox").css({
            "display": "none"
        });
    });
    $(".b-modify").click(function (event) {
        $(".b-modify").css({
            "display": "none"
        });
        $(".b-certification").css({
            "display": "none"
        });

        $(".t-increase").css({
            "display": "block"
        });
        $(".b-save").css({
            "display": "block"
        });
        $(".b-cancel").css({
            "display": "block"
        });
        $(".t-minus").css({
            "display": "block"
        });
    });

    $(".b-modify").click(function (event) {
        $(".b-modify").css({
            "display": "none"
        });
        $(".b-certification").css({
            "display": "none"
        });
        $(".t-increase").css({
            "display": "block"
        });
        $(".b-save").css({
            "display": "block"
        });
        $(".b-cancel").css({
            "display": "block"
        });
        $(".t-minus").css({
            "display": "block"
        });
        $(".p-up").css({
            "display": "block"
        });
    });

    $(".b-save").click(function (event) {
        //$(".b-modify").css({
        //	"display": "block"
        //});
        //$(".b-certification").css({
        //	"display": "block"
        //});
        //$(".license02").css({
        //	"display": "block"
        //});
        $(".p-up").css({
            "display": "none"
        });
        //$(".t-increase").css({
        //	"display": "none"
        //});
        //$(".b-save").css({
        //	"display": "none"
        //});
        //$(".b-cancel").css({
        //	"display": "none"
        //});
        //$(".t-minus").css({
        //	"display": "none"
        //});
        //$(".license01").css({
        //	"display": "none"
        //});
        //$(".asterisk").css({
        //	"display": "none"
        //});
    });

    $(".b-cancel").click(function (event) {
        $(".b-modify").css({
            "display": "block"
        });
        $(".b-certification").css({
            "display": "block"
        });
        $(".license02").css({
            "display": "block"
        });
        $(".p-up").css({
            "display": "none"
        });
        $(".t-increase").css({
            "display": "none"
        });
        $(".b-save").css({
            "display": "none"
        });
        $(".b-cancel").css({
            "display": "none"
        });
        $(".t-minus").css({
            "display": "none"
        });
        $(".license01").css({
            "display": "none"
        });
        $(".asterisk").css({
            "display": "none"
        });
    });
    $(".pay-choose li").click(function (event) {
        $(".pay-choose li").removeClass();
        $(".pay-choose li:eq(" + $(".pay-choose li").index(event.target) + ")").addClass("on");
        if ($(".pay-choose li").index(event.target) == 1) {
            $(".paybox").show();
            $(".paybox").animate({
                "height": "200px"
            }, 200);
        } else {
            $(".paybox").hide(0);
        }
    })
});

function policy01() {
    if (document.getElementById('policybox01').style.display == 'none') {
        document.getElementById('policybox01').style.display = 'block'
        document.getElementById('arrow01').style.transform = 'rotate(180deg)'
    } else {
        document.getElementById('policybox01').style.display = 'none'
        document.getElementById('arrow01').style.transform = 'rotate(0deg)'
    }
}
function policy02() {
    if (document.getElementById('policybox02').style.display == 'none') {
        document.getElementById('policybox02').style.display = 'block'
        document.getElementById('arrow02').style.transform = 'rotate(180deg)'
    } else {
        document.getElementById('policybox02').style.display = 'none'
        document.getElementById('arrow02').style.transform = 'rotate(0deg)'
    }
}

// 搜索框
function s1() {
    if (document.getElementById('searchbox-con01').style.display == 'block') {
        document.getElementById('searchbox-con01').style.display = 'none';
    } else {
        document.getElementById('searchbox-con01').style.display = 'block';
    };
    if (document.getElementById('arrow01').style.transform == 'rotate(0deg)') {
        document.getElementById('arrow01').style.transform = 'rotate(180deg)';
    } else {
        document.getElementById('arrow01').style.transform = 'rotate(0deg)';
    };
}
function s2() {
    if (document.getElementById('searchbox-con02').style.display == 'block') {
        document.getElementById('searchbox-con02').style.display = 'none';
    } else {
        document.getElementById('searchbox-con02').style.display = 'block';
    };
    if (document.getElementById('arrow02').style.transform == 'rotate(0deg)') {
        document.getElementById('arrow02').style.transform = 'rotate(180deg)';
    } else {
        document.getElementById('arrow02').style.transform = 'rotate(0deg)';
    };
}
function s3() {
    if (document.getElementById('searchbox-con03').style.display == 'block') {
        document.getElementById('searchbox-con03').style.display = 'none';
    } else {
        document.getElementById('searchbox-con03').style.display = 'block';
    };
    if (document.getElementById('arrow03').style.transform == 'rotate(0deg)') {
        document.getElementById('arrow03').style.transform = 'rotate(180deg)';
    } else {
        document.getElementById('arrow03').style.transform = 'rotate(0deg)';
    };
}
function s4() {
    if (document.getElementById('searchbox-con04').style.display == 'block') {
        document.getElementById('searchbox-con04').style.display = 'none';
    } else {
        document.getElementById('searchbox-con04').style.display = 'block';
    };
    if (document.getElementById('arrow04').style.transform == 'rotate(0deg)') {
        document.getElementById('arrow04').style.transform = 'rotate(180deg)';
    } else {
        document.getElementById('arrow04').style.transform = 'rotate(0deg)';
    };
}
function s5() {
    if (document.getElementById('searchbox-con05').style.display == 'block') {
        document.getElementById('searchbox-con05').style.display = 'none';
    } else {
        document.getElementById('searchbox-con05').style.display = 'block';
    };
    if (document.getElementById('arrow05').style.transform == 'rotate(0deg)') {
        document.getElementById('arrow05').style.transform = 'rotate(180deg)';
    } else {
        document.getElementById('arrow05').style.transform = 'rotate(0deg)';
    };
}
function s6() {
    if (document.getElementById('searchbox-con06').style.display == 'block') {
        document.getElementById('searchbox-con06').style.display = 'none';
    } else {
        document.getElementById('searchbox-con06').style.display = 'block';
    };
    if (document.getElementById('arrow06').style.transform == 'rotate(0deg)') {
        document.getElementById('arrow06').style.transform = 'rotate(180deg)';
    } else {
        document.getElementById('arrow06').style.transform = 'rotate(0deg)';
    };
}

function disablet() {
    document.getElementById("radio1").disabled = "true";
    document.getElementById("radio2").disabled = "true";
    document.getElementById("p-agency").disabled = "true";
    document.getElementById("p-name").disabled = "true";
    document.getElementById("p-countries").disabled = "true";
    document.getElementById("p-number").disabled = "true";
    document.getElementById("p-code").disabled = "true";
    document.getElementById("p-province").disabled = "true";
    document.getElementById("p-city").disabled = "true";
    document.getElementById("p-add").disabled = "true";
    document.getElementById("c-agency").disabled = "true";
    document.getElementById("c-name").disabled = "true";
    document.getElementById("c-countries").disabled = "true";
    document.getElementById("c-number").disabled = "true";
    document.getElementById("c-code").disabled = "true";
    document.getElementById("c-province").disabled = "true";
    document.getElementById("c-city").disabled = "true";
    document.getElementById("c-add").disabled = "true";
    document.getElementById("c-industry").disabled = "true";
    document.getElementById("c-legal").disabled = "true";
    document.getElementById("c-agent").disabled = "true";
    document.getElementById("c-name").disabled = "true";
    document.getElementById("contact").disabled = "true";
    document.getElementById("c-phone").disabled = "true";
    document.getElementById("c-tel").disabled = "true";
    document.getElementById("c-qq").disabled = "true";
    document.getElementById("c-add").disabled = "true";
    document.getElementById("c-zipcode").disabled = "true";
}

//新闻tab切换		
//$('.tab-button').hover(function () {
//    var tab = $(this).data('tab')
//    $(this).addClass('cur').siblings('.tab-button').removeClass('cur');
//    $('#tab-' + tab + '').addClass('active').siblings('.tab-item').removeClass('active');
//});
//新闻列表切换
//$('.information-tab .article-list').hover(function () {
//    $(this).addClass('current').siblings('.article-list').removeClass('current');
//}, function () {
//    $(this).parent('.information-right').find('.article-list:first-of-type').addClass('current').siblings('.article-list').removeClass('current');
//});

//左侧分类导航
$('.category-option .cat-item').hover(function () { $(this).toggleClass('hover') })

//轮播图
jQuery(".slideBox").slide({
    mainCell: ".bd ul",
    effect: "left",
    autoPlay: true,
    trigger: "click"
});

// 特价滚动
function autoScroll(obj) {
    $(obj).find(".list").animate({
        marginTop: "-44px"
    }, 500, function () {
        $(this).css({ marginTop: "0px" }).find("li:first").appendTo(this);
    })
}
$(function () {
    setInterval('autoScroll(".s-listcon")', 3000)
})

//图片滚动 调用方法 imgscroll({speed: 30,amount: 1,dir: "up"});
$.fn.imgscroll = function (o) {
    var defaults = {
        speed: 40,
        amount: 0,
        width: 1,
        dir: "left"
    };
    o = $.extend(defaults, o);

    return this.each(function () {
        var _li = $("li", this);
        _li.parent().parent().css({
            overflow: "hidden",
        }); //div
        _li.parent().css({
            margin: "0",
            padding: "0",
            overflow: "hidden",
            position: "relative",
            "list-style": "none"
        }); //ul
        _li.css({
            position: "relative",
            overflow: "hidden"
        }); //li
        if (o.dir == "left") _li.css({
            float: "left"
        });

        //初始大小
        var _li_size = 0;
        for (var i = 0; i < _li.size(); i++)
            _li_size += o.dir == "left" ? _li.eq(i).outerWidth(true) : _li.eq(i).outerHeight(true);

        //循环所需要的元素
        if (o.dir == "left") _li.parent().css({
            width: (_li_size * 3) + "px"
        });
        _li.parent().empty().append(_li.clone()).append(_li.clone()).append(_li.clone());
        _li = $("li", this);

        //滚动
        var _li_scroll = 0;

        function goto() {
            _li_scroll += o.width;
            if (_li_scroll > _li_size) {
                _li_scroll = 0;
                _li.parent().css(o.dir == "left" ? {
                    left: -_li_scroll
                } : {
                        top: -_li_scroll
                    });
                _li_scroll += o.width;
            }
            _li.parent().animate(o.dir == "left" ? {
                left: -_li_scroll
            } : {
                    top: -_li_scroll
                }, o.amount);
        }

        //开始
        var move = setInterval(function () {
            goto();
        }, o.speed);
        _li.parent().hover(function () {
            clearInterval(move);
        }, function () {
            clearInterval(move);
            move = setInterval(function () {
                goto();
            }, o.speed);
        });
    });
};
// 统计数字效果
$(document).ready(function () {
    //$(".rolling").imgscroll({
    //    speed: 80, //图片滚动速度
    //    amount: 0, //图片滚动过渡时间
    //    width: 1, //图片滚动步数
    //    dir: "up" // "left" 或 "up" 向左或向上滚动
    //});
});