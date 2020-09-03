<%@ Page Language="C#" MasterPageFile="~/App/Index.master" AutoEventWireup="true" CodeFile="Contact.aspx.cs" Inherits="App_Footer_Contact" %>

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
                <h3>联系我们</h3>
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
                            <li><a href="footer-about">关于我们</a></li>
                            <li><a class="active" href="footer-contact">联系我们</a></li>
                        </ul>
                    </div>
                </div>
                <div class="footerpage-r">
                    <div class="newstit">
                        <h3>联系我们</h3>
                        <div class="nt_list">
                            <div class="fl">
                                <span>我们期待与您合作！</span>
                            </div>
                        </div>
                    </div>
                    <div class="group margint30">
                        <div class="fl contacttxt">
                            <h3>江苏知聚知识产权服务有限公司</h3>
                            <p><i class="fa fa-location-arrow"></i>南京市江宁区秣周东路12号南京未来科技城3号楼P533</p>
                            <p><i class="fa fa-phone-square"></i>025-86111632，4000-025-699</p>

                            <p>
                                <i class="fa fa-wechat"></i>
                                <img src="../img/QR.jpg" />
                            </p>
                        </div>
                        <img class="fr contactimg" src="../img/map.png" />
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
