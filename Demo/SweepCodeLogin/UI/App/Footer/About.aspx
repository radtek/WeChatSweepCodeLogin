<%@ Page Language="C#" MasterPageFile="~/App/Index.master" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="App_Footer_About" %>

<asp:Content ID="childjs" ContentPlaceHolderID="quote" runat="server">
    <script type="text/javascript">
        $(function () {
            //给所有分类添加样式
            $(".navlist-l").addClass("news-nav");

            //隐藏底部
            $(".floatbox").css({ "display": "none" });

            //移除 首页 选中
            $("#portal").removeClass("active");
        })
    </script>
</asp:Content>

<asp:Content ID="childcontext" ContentPlaceHolderID="content" runat="server">
    <div class="back-Lgray">
        <div class="contentbox paddingt10 paddingb50">
            <div class="pagesbanner p-footer">
                <h3>关于我们</h3>
                <div class="pagesb-box">
                    <div class="pagesb-con">
                        <p><span>知</span>行天下<i class="fa fa-circle"></i><span>聚</span>力行远</p>
                    </div>
                </div>
            </div>
            <div class="group pr">
                <script>
                    var offset = 380;
                    $(window).scroll(function () {
                        ($(this).scrollTop() > offset) ? $('.footerpage-l').addClass('fixed') : $('.footerpage-l').removeClass('fixed');
                    })
                </script>
                <div class="footerpage-l">
                    <div class="public-fixleftbar">
                        <ul>
                            <li><a href="footer-problem">常见问题</a></li>
                            <li><a class="active" href="footer-about">关于我们</a></li>
                            <li><a href="footer-contact">联系我们</a></li>
                        </ul>
                    </div>
                </div>
                <div class="footerpage-r">
                    <div class="newstit">
                        <h3>关于我们</h3>
                        <div class="nt_list">
                            <div class="fl">
                                <span>我们期待与您合作！</span>
                            </div>
                        </div>
                    </div>
                    <div class="newstxt">
                        <h3 class="search-t"><i class="fa fa-circle"></i>公司简介</h3>
                        <p class="news-w">
                            知聚网是江苏知聚知识产权服务有限公司旗下的知产服务平台。知聚网是对接广大的知产用户。服务范围涉及知产各个方面，另有国际专利等多种服务。
                        </p>
                        <p class="news-w">
                            我们是一群充满激情的互联网从业者，我们期望为全球提供最好的知产服务。
                        </p>
                        <p class="news-w">
                            多年深耕在知识产权领域，一直走在知识产权维权的第一线。我们希望通过搭建知识产权第三方平台，用我们的专业性，以公平和公证的态度链接客户与代理人，为客户找到最匹配的代理人。
                        </p>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
