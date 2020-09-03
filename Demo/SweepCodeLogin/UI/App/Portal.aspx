<%@ Page Title="" Language="C#" MasterPageFile="~/App/Index.master" AutoEventWireup="true" CodeFile="Portal.aspx.cs" Inherits="App_Portal" %>

<asp:Content ID="childjs" ContentPlaceHolderID="quote" runat="Server">
    <script type="text/javascript" src="../js/lib/jquery.js"></script>
    <script type="text/javascript" src="../js/lib/raphael-min.js"></script>
    <script type="text/javascript" src="../js/res/chinaMapConfig.js"></script>
    <script type="text/javascript" src="../js/map.js"></script>
</asp:Content>

<asp:Content ID="childcontext" ContentPlaceHolderID="content" runat="Server">
    <div class="bannerbox">
        <%--轮播图--%>
        <div class="banner">
            <div id="slideBox" class="slideBox">
                <div class="hd">
                    <ul>
                        <li></li>
                        <li></li>
                    </ul>
                </div>
                <div class="bd">
                    <ul>
                        <li class="b-back01">
                            <div class="item">
                                <div class="box">
                                    <a href="javascript:void(0);">
                                        <img class="banner-img" src="../img/banner-img01.png" />
                                    </a>
                                </div>
                            </div>
                        </li>
                        <li class="b-back02">
                            <div class="item">
                                <div class="box">
                                    <a href="javascript:void(0);">
                                        <img class="banner-img" src="../img/banner-img02.png" />
                                    </a>
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>

        <%--专利交易库、交易金额、技术需求、成功案例--%>
        <div class="totalbox">
            <div class="contentbox">
                <ul class="total-l">
                    <li>
                        <img src="../img/totalimg01.png" />
                        <p>专利交易库<span class="counter-value" id="pacount"></span>条</p>
                    </li>
                    <li>
                        <img src="../img/totalimg02.png" />
                        <p>交易金额<span class="counter-value" id="transactionamount"></span>万元</p>
                    </li>
                    <li>
                        <img src="../img/totalimg03.png" />
                        <p>技术需求<span class="counter-value" id="reqcount"></span>条</p>
                    </li>
                    <li>
                        <img src="../img/totalimg04.png" />
                        <p>成功案例<span class="counter-value" id="success"></span>条</p>
                    </li>
                </ul>
            </div>
        </div>

        <div class="contentbox paddingb100">
            <%--特价--%>
            <div class="specialoffer">
                <div class="s-name"></div>
                <div class="s-box01">
                    <div class="s-listcon">
                        <ul class="list" id="bargainlist">
                        </ul>
                    </div>
                </div>
                <div class="s-box02"></div>
            </div>

            <%--最新发布、推荐专利--%>
            <div class="overflowh margint50">
                <%--最新发布--%>
                <div class="fl newest">
                    <div class="titlebox">
                        <a href="javascript:void(0);">
                            <h2 class="title">最新发布</h2>
                        </a>
                        <p class="subtitle">全网最新专利交易资源</p>
                    </div>
                    <div class="boxtab">
                        <div class="tab" role="tabpanel">
                            <ul class="nav nav-tabs tab-item" role="tablist" id="tab-item">
                                <li role="presentation" id="todayli" class="active in-w33"><a href="#Section5" aria-controls="home" role="tab" data-toggle="tab"><span id="today"></span></a></li>
                                <li role="presentation" id="weekli" class="in-w33"><a href="#Section5" aria-controls="home" role="tab" data-toggle="tab"><span id="week"></span></a></li>
                                <li role="presentation" id="monthli" class="in-w33"><a href="#Section5" aria-controls="home" role="tab" data-toggle="tab"><span id="month"></span></a></li>
                            </ul>
                            <div class="tab-content tabs" id="newpublishdiv">
                                <div role="tabpanel" class="tab-pane fade in active" id="Section5">
                                    <div class="rollingbox">
                                        <div class="rolling">
                                            <ul id="newpublish">
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <%--推荐专利--%>
                <div class="fr recommend">
                    <div class="titlebox">
                        <a href="javascript:void(0);">
                            <h2 class="title">推荐专利</h2>
                        </a>
                        <p class="subtitle">为您优选高价值专利</p>
                    </div>
                    <div class="boxtab">
                        <div class="tab" role="tabpanel">
                            <ul class="nav nav-tabs tab-item" role="tablist" id="tab-item">
                                <li role="presentation" id="inventcertificateli" class="active in-w25"><a href="#Section6" aria-controls="home" role="tab" data-toggle="tab"><span>发明已下证</span></a></li>
                                <li role="presentation" id="inventnopayli" class="in-w25"><a href="#Section6" aria-controls="home" role="tab" data-toggle="tab"><span>发明未缴费</span></a></li>
                                <li role="presentation" id="newcertificateli" class="in-w25"><a href="#Section6" aria-controls="home" role="tab" data-toggle="tab"><span>新型已下证</span></a></li>
                                <li role="presentation" id="newnopayli" class="in-w25"><a href="#Section6" aria-controls="home" role="tab" data-toggle="tab"><span>新型未缴费</span></a></li>
                            </ul>
                            <div class="tab-content tabs" id="recommendpadiv">
                                <div role="tabpanel" class="tab-pane fade in active" id="Section6">
                                    <div class="rollingbox">
                                        <div class="rolling">
                                            <ul id="recommendpa">
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <%--专利交易--%>
            <div class="trading">
                <div class="titlebox">
                    <a href="javascript:void(0);">
                        <h2 class="title">专利交易</h2>
                    </a>
                    <p class="subtitle">汇聚全网络可交易专利数据</p>
                </div>
                <div class="showbox">
                    <div class="fl showimg">
                        <img src="../img/img01.png" />
                        <a class="showimg-btn btn-blue" href="member-transaction-list"><i class="fa fa-pencil"></i>发布交易</a>
                    </div>
                    <div class="fl showcon01">
                        <div class="boxtab">
                            <div class="tab" role="tabpanel">
                                <ul class="nav nav-tabs tab-item" role="tablist" id="tab-item">
                                    <li role="presentation" id="painventcertificateli" class="active in-w25"><a href="#Section8" aria-controls="home" role="tab" data-toggle="tab"><span>发明已下证</span></a></li>
                                    <li role="presentation" id="painventnopayli" class="in-w25"><a href="#Section8" aria-controls="home" role="tab" data-toggle="tab"><span>发明未缴费</span></a></li>
                                    <li role="presentation" id="panewcertificateli" class="in-w25"><a href="#Section8" aria-controls="home" role="tab" data-toggle="tab"><span>新型已下证</span></a></li>
                                    <li role="presentation" id="panewnopayli" class="in-w25"><a href="#Section8" aria-controls="home" role="tab" data-toggle="tab"><span>新型未缴费</span></a></li>
                                </ul>
                                <div class="tab-content tabs">
                                    <div role="tabpanel" class="tab-pane fade in active" id="Section8">
                                        <ul class="boxlist" id="transactionpaul">
                                            <!-- 类型不同颜色不同。红色-发明boxlist-red，蓝色-外观boxlist-blue，绿色-新型boxlist-green -->
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="fr showbtn">
                        <a href="javascript:void(0);" role="transactionpachange">
                            <p><i class="fa fa-refresh"></i>换一批</p>
                        </a>
                        <a href="market-list">
                            <p>查看更多</p>
                        </a>
                    </div>
                </div>
            </div>

            <%--高校成果--%>
            <div class="colleges">
                <div class="titlebox">
                    <a href="javascript:void(0);">
                        <h2 class="title">高校成果</h2>
                    </a>
                    <p class="subtitle">助力高校专利资源转移转化</p>
                </div>
                <div class="showbox">
                    <div class="fl showimg">
                        <img src="../img/img02.png" />
                        <a class="showimg-btn btn-yellow" href="member-info-detail"><i class="fa fa-pencil"></i>申请加入</a>
                    </div>
                    <div class="fl showcon02">
                        <div class="items" id="Item6">
                            <div class="itemCon">
                                <div class="map">
                                    <div id="ChinaMap"></div>
                                    <%--点击地图区域时，不仅变更当前省市，还要变更下面的学校--%>
                                </div>

                                <div class="c-list">
                                    <div class="c-list-title">
                                        <p class="fl">当前省市：<span id="ClickCallback"></span></p>
                                        <a class="c-more fr" href="colleges-list-1">
                                            <img src="../img/more.png" />
                                        </a>
                                    </div>
                                    <div class="c-list-top">
                                        <div class="c-search">
                                            <input type="text" name="collegename" />
                                            <a href="javascript:void(0);" onclick="window.homepage.loadCollegeList();"><i class="fa fa-search"></i></a>
                                        </div>
                                    </div>
                                    <ul class="c-list-txt" id="collegelist">
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <%--技术需求--%>
            <div class="demand">
                <div class="titlebox">
                    <a href="javascript:void(0);">
                        <h2 class="title">技术需求</h2>
                    </a>
                    <p class="subtitle">解决创新创业技术难题</p>
                </div>
                <div class="showbox">
                    <div class="fl showimg">
                        <img src="../img/img03.png" />
                        <a class="showimg-btn btn-purple" href="member-demand-list"><i class="fa fa-pencil"></i>发布需求</a>
                    </div>
                    <div class="fl showcon01">
                        <ul class="boxlist" id="techreq">
                        </ul>
                    </div>
                    <div class="fr showbtn">
                        <a href="javascript:void(0);" role="techreqchange">
                            <p><i class="fa fa-refresh"></i>换一批</p>
                        </a>
                        <a href="req-list">
                            <p>查看更多</p>
                        </a>
                    </div>
                </div>
            </div>

            <%--成交动态、中介推荐--%>
            <div class="overflowh margint50">
                <%--成交动态--%>
                <div class="fl deal">
                    <div class="titlebox">
                        <a href="javascript:void(0);">
                            <h2 class="title">成交动态</h2>
                        </a>
                        <p class="subtitle">知聚平台专利成交案例</p>
                    </div>
                    <div class="rollingbox" id="transactiontrendsdiv">
                        <div class="rolling">
                            <ul id="transactiontrends">
                            </ul>
                        </div>
                    </div>
                </div>

                <%--中介推荐--%>
                <div class="fr agent">
                    <div class="titlebox">
                        <a href="javascript:void(0);">
                            <h2 class="title">中介推荐</h2>
                        </a>
                        <p class="subtitle"><span>开放资源</span><span class="marginl10">同台分享</span><span class="marginl10">合作共赢</span> </p>
                    </div>
                    <div class="fl showcon03">
                        <ul class="boxlist" id="agentlist">
                        </ul>
                    </div>
                    <div class="fr showbtn">
                        <a href="agent-list">
                            <p>查看更多</p>
                        </a>
                    </div>
                </div>
            </div>

            <%--新闻资讯--%>
            <div class="newsbox">
                <div class="titlebox">
                    <a href="javascript:void(0);">
                        <h2 class="title">新闻资讯</h2>
                    </a>
                    <p class="subtitle"><span>宣传知识产权</span><span class="marginl10">推进知识产权强国</span></p>
                </div>
                <div class="tab-block information-tab">
                    <div class="tab-buttons ">
                        <h3 class="tab-button cur" data-tab="one">公司新闻<a class="more" href="news-company-list"><img src="../img/more.png" />
                        </a></h3>
                        <h3 class="tab-button" data-tab="two">行业新闻<a class="more" href="news-industry-list"><img src="../img/more.png" />
                        </a></h3>
                        <h3 class="tab-button" data-tab="three">科技政策<a class="more" href="news-policiesregulations-list"><img src="../img/more.png" />
                        </a></h3>
                        <h3 class="tab-button" data-tab="four">知识百科<a class="more" href="news-knowledgeencyclopedia-list"><img src="../img/more.png" />
                        </a></h3>
                    </div>
                    <div class="tabs">
                        <%--公司新闻--%>
                        <div class="tab-item active" id="tab-one">
                            <div class="information-tab">
                                <%--存放有图的第一条新闻--%>
                                <div class="information-left" id="companynewsleft">
                                </div>
                                <div class="information-right" id="companynewsright">
                                </div>
                            </div>
                        </div>

                        <%--行业新闻--%>
                        <div class="tab-item" id="tab-two">
                            <div class="information-tab ">
                                <%--存放有图的第一条新闻--%>
                                <div class="information-left" id="industryleft">
                                </div>
                                <div class="information-right" id="industryright">
                                </div>
                            </div>
                        </div>

                        <%--科技政策--%>
                        <div class="tab-item" id="tab-three">
                            <div class="information-tab ">
                                <%--存放有图的第一条新闻--%>
                                <div class="information-left" id="technologyleft">
                                </div>
                                <div class="information-right" id="technologyright">
                                </div>
                            </div>
                        </div>

                        <%--知识百科--%>
                        <div class="tab-item" id="tab-four">
                            <div class="information-tab ">
                                <div class="information fl" id="knowledgediv">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <%--专利交易流程--%>
            <div class="newsbox">
                <div class="titlebox">
                    <a href="javascript:void(0);">
                        <h2 class="title">专利交易流程</h2>
                    </a>
                    <p class="subtitle"><span>规范交易流程</span><span class="marginl10">保障交易双方利益</span></p>
                </div>
                <div class="processlist">
                    <div class="processline"></div>
                    <ul class="processbox">
                        <li>
                            <p class="processnum">1</p>
                            <div class="processcon">
                                <div>
                                    <img src="../img/process-img01.png" />
                                </div>
                                <p>
                                    用户完成<br />
                                    实名认证
                                </p>
                            </div>
                        </li>
                        <li>
                            <p class="processnum">2</p>
                            <div class="processcon">
                                <div>
                                    <img src="../img/process-img02.png" />
                                </div>
                                <p>
                                    选中专利下单或<br />
                                    咨询经纪人下单
                                </p>
                            </div>
                        </li>
                        <li>
                            <p class="processnum">3</p>
                            <div class="processcon">
                                <div>
                                    <img src="../img/process-img03.png" />
                                </div>
                                <p>
                                    签订相关<br />
                                    交易合同
                                </p>
                            </div>
                        </li>
                        <li>
                            <p class="processnum">4</p>
                            <div class="processcon">
                                <div>
                                    <img src="../img/process-img04.png" />
                                </div>
                                <p>
                                    支付交易款至<br />
                                    资金托管账户
                                </p>
                            </div>
                        </li>
                        <li>
                            <p class="processnum">5</p>
                            <div class="processcon">
                                <div>
                                    <img src="../img/process-img05.png" />
                                </div>
                                <p>
                                    用户确认收到<br />
                                    官文、证书
                                </p>
                            </div>
                        </li>
                        <li>
                            <p class="processnum">6</p>
                            <div class="processcon">
                                <div>
                                    <img src="../img/process-img06.png" />
                                </div>
                                <p>
                                    平台支付款项<br />
                                    给交易相对方
                                </p>
                            </div>
                        </li>
                        <li>
                            <p class="processnum">7</p>
                            <div class="processcon">
                                <div>
                                    <img src="../img/process-img07.png" />
                                </div>
                                <p>交易完成</p>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
