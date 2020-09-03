<%@ Page Language="C#" MasterPageFile="~/App/Index.master" AutoEventWireup="true" CodeFile="Problem.aspx.cs" Inherits="App_Footer_Problem" %>

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
                <h3>常见问题</h3>
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
                            <li><a class="active" href="footer-problem">常见问题</a></li>
                            <li><a href="footer-about">关于我们</a></li>
                            <li><a href="footer-contact">联系我们</a></li>
                        </ul>
                    </div>
                </div>
                <div class="footerpage-r">
                    <div class="newstit">
                        <h3>常见问题</h3>
                        <div class="nt_list">
                            <div class="fl">
                                <span>我们期待与您合作！</span>
                            </div>
                        </div>
                    </div>
                    <div class="newstxt">
                        <h3 class="search-t"><i class="fa fa-circle"></i>专利问题</h3>
                        <p class="news-w fblod">一、怎么知道专利中保护的范围和内容是什么？</p>
                        <p class="news-w">专利保护的范围是以专利文件中权利要求书划定的范围为准的，保护的内容也就是权利要求书公开的内容。</p>
                        <p class="n-line"></p>
                        <p class="news-w fblod">二、专利的作用是什么？</p>
                        <p class="news-w">1、保护自己的技术成果不受他人侵犯</p>
                        <p class="news-w">2、防止他人滥用专利权恶意、主动造成侵权行为</p>
                        <p class="news-w">3、办理高新技术企业、申请政府补贴、资助等必备资料</p>
                        <p class="news-w">4、提高企业核心竞争力、保护企业产品市场占有份额</p>
                        <p class="news-w">5、形成企业知识产权资产，提升企业注册资本、企业价值 。</p>
                        <p class="n-line"></p>
                        <p class="news-w fblod">三、怎样知道有没有其他人申报过与我相同或相似的技术方案呢？</p>
                        <p class="news-w">
                            可以在国家知识产权局官网专利检索网页输入关键词进行检索，实用新型和外观专利只有授权才能公开，发明专利在提交之后6-18个月才会公开，因此没有公开的专利文件是检索不到的。
                        </p>
                        <p class="n-line"></p>
                        <p class="news-w fblod">四、专利如何才能授权呢？</p>
                        <p class="news-w">
                            申报专利的技术方案和已有的技术方案存在区别点，并且该区别点会带来有益效果才会有可能被授权，已有的技术方案包括已申报的专利、已经公开的文献、视听材料等。发明专利是将已有技术结合起来与申请的技术方案作对比，仍有区别点，且区别点能够带来有益效果，才有可能会被授权，实用新型和外观是一对一的与现有技术做对比，只要有区别点，且区别点会带来有益效果就会被授权，例如现有技术中风扇需要手动才能够实现转动，现在将手动的结构改进成电机带动风扇转动的结构，这样电动的结构与手动的结构存在区别点，并且带来了能够省力的有益效果。
                        </p>
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
