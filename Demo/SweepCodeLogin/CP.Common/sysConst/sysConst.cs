namespace CP.Common
{
    public class sysConst
    {

        /*
            注意命名规则: 
         * 系统常量: C_模块名称_功能名称_路径/目录/其他
               示例: C_SystemLog_Alipay_LogDirectory
         * 系统变量: 模块名称_功能描述
               示例: SNS_Interface_Key
         */

        #region 系统常量

        #region 用户信息
        /// <summary>
        /// 账户认证上传图片文件夹路径
        /// </summary>
        public const string C_UserAuthentication_IMGDirectory = "~/Data/UserData/UpLoad/UserAuthentication";
        /// <summary>
        /// 用户头像 崔萌 2020-3-30 13:52:08
        /// </summary>
        public const string C_UserProfilePhoto_IMGDirectory = "~/Data/MemberInfo/HeadPortrait";
        /// <summary>
        /// 用户身份证 崔萌 2020-3-30 13:55:31
        /// </summary>
        public const string C_UserIDCard_IMGDirectory = "~/Data/MemberInfo/IDCard";
        /// <summary>
        /// 用户营业执照 崔萌 2020-3-30 13:58:03
        /// </summary>
        public const string C_UserBusinessLicense_IMGDirectory = "~/Data/MemberInfo/BusinessLicense";
        /// <summary>
        /// 用户法人证书 崔萌 2020-3-30 13:56:42
        /// </summary>
        public const string C_UserCorporateCertificate_IMGDirectory = "~/Data/MemberInfo/CorporateCertificate";
        #endregion

        #region 系统运行参数
        /// <summary>
        /// 浏览权限ID 判断模块有没有浏览权限的依据 不可大于0 大于0会出现权限混乱问题
        /// </summary>
        public const int C_Permission_PerViewID = -1;
        /// <summary>
        /// 当前权限Application名称
        /// </summary>
        public const string C_Permission_ApplicationName = "CurrentPermission";

        /// <summary>
        /// 当前菜单Application名称
        /// </summary>
        public const string C_Menu_ApplicationName = "CurrentMenu";
        #endregion

        #region 系统日志存放路径
        /// <summary>
        /// 系统日志文件存放文件夹路径
        /// </summary>
        public const string C_SystemLog_LogDirectory = "~/Data/Log";

        /// <summary>
        /// 支付宝接口日志
        /// </summary>
        public const string C_SystemLog_AlipayLogDirectory = "~/Data/Log/Alipay";
        #endregion

        #region 新闻资讯存放路径
        /// <summary>
        /// 新闻图片存放总路径
        /// </summary>
        public const string B_News_Directory = "~/Data/News";
        /// <summary>
        /// 新闻资讯-公司新闻
        /// </summary>
        public const string B_News_Company = "~/Data/News/Company";
        /// <summary>
        /// 新闻资讯--行业新闻
        /// </summary>
        public const string B_News_Industry = "~/Data/News/Industry";
        /// <summary>
        /// 新闻资讯--政策法规
        /// </summary>
        public const string B_News_PoliciesRegulations = "~/Data/News/PoliciesRegulations";
        /// <summary>
        /// 新闻资讯--知识百科
        /// </summary>
        public const string B_News_KnowledgeEncyclopedia = "~/Data/News/KnowledgeEncyclopedia";
        /// <summary>
        /// 高校成果-高校管理-高校图标
        /// </summary>
        public const string B_College_CollegeImg = "~/Data/News/CollegeImg";
        #endregion

        #region 中介推荐-头像存放路径
        /// <summary>
        /// 中介推荐-头像存放路径
        /// </summary>
        public const string B_IntermediaryRecommend_ProfilePhoto_Directory = "~/Data/ProfilePhoto/IntermediaryRecommend";
        #endregion

        #region 会员权利管理-说明存放路径 崔萌 2020-4-9 18:25:57
        /// <summary>
        /// 会员权利管理-说明存放路径
        /// </summary>
        public const string B_MemberEquityManager_UploadExplain_Directory = "~/Data/MemberIntegrals";
        #endregion

        #region 上传图片
        /// <summary>
        /// 上传文件类型
        /// </summary>
        public static string C_Image_ExtensionString = "jpg,jepg,png,bmp";
        /// <summary>
        /// 上传图片文件类型
        /// </summary>
        public static string[] C_Image_ExtensionArray = new string[] { ".jpg", ".jepg", ".png", ".bmp", ".gif" };
        /// <summary>
        /// 上传文件类型
        /// </summary>
        public static string[] C_File_ExtensionArray = new string[] { ".jpg", ".jepg", ".png", ".bmp", ".gif", ".rar", ".zip", ".doc", ".docx", ".xls", ".xlsx", ".pdf" };

        #endregion

        #region 注册验证码
        /// <summary>
        /// 生成验证码 验证码信息 SESSION 名称
        /// </summary>
        public const string C_Register_SessionName = "VerifyCode";

        // <summary>
        /// 生成验证码 验证码 字符串长度
        /// </summary>
        public const int C_RegisterCode_Length = 4;
        #endregion

        #region 邮箱找回密码

        /// <summary>
        /// 邮箱找回密码 发送短信验证码信息 SESSION 名称
        /// </summary>
        public const string C_ForgetPasswordEmail_SessionName = "ForgetPasswordEmail";

        // <summary>
        /// 修改密码 邮箱发送验证码 字符串长度
        /// </summary>
        public const int C_ForgetPasswordEmailCode_Length = 6;

        // <summary>
        /// 密码修改 SESSION有效期内 最大上限次数
        /// </summary>
        public const int C_PostEmailCode_MaxCount = 10;

        #endregion

        #region 手机绑定、变更

        /// <summary>
        /// 手机绑定、变更 发送短信验证码信息 SESSION 名称
        /// </summary>
        public const string C_BgSNS_SessionName = "BgSNS";

        /// <summary>
        /// 注册发送短信验证码信息 SESSION 名称
        /// </summary>
        public const string C_RegisterSNS_SessionName = "RegisterSNS";

        /// <summary>
        /// 找回密码手机发送短信验证码信息 SESSION 名称
        /// </summary>
        public const string C_RetrievePasswordSNS_SessionName = "RetrievePasswordSNS";

        // <summary>
        /// 用户注册 密码修改 发送短信 SESSION有效期内 最大上限次数
        /// </summary>
        public const int C_PostSNSCode_MaxCount = 10;
        // <summary>
        /// 用户注册 手机发送短信验证码 字符串长度
        /// </summary>
        public const int C_BgSNSCode_Length = 6;
        #endregion

        #region 邮箱绑定、变更

        /// <summary>
        /// 邮箱绑定、变更 发送验证码信息 SESSION 名称
        /// </summary>
        public const string C_BgEmail_SessionName = "BgEmail";

        // <summary>
        /// 邮箱绑定、变更 发送邮箱验证码 字符串长度
        /// </summary>
        public const int C_BgEmailCode_Length = 6;
        #endregion


        #region UrlRewriter xml文件存放路径
        /// <summary>
        /// UrlRewriter xml文件存放路径
        /// </summary>
        public static string C_UrlRewriter_XMLFilePath = "~/data/urlrewriter/mapping.xml";
        #endregion

        #region  付款凭证
        /// <summary>
        /// 付款凭证-订单 文件上传目录
        /// </summary>
        public const string C_ReceiptVoucher_Order_Directory = "~/Data/ReceiptVoucher/Order";
        /// <summary>
        /// 付款凭证-专利交易_预留_定金 文件上传目录
        /// </summary>
        public const string C_ReceiptVoucher_Retain_Directory = "~/Data/ReceiptVoucher/Retain";
        #endregion

        #region 知识产权管理
        /// <summary>
        /// 专利图片
        /// </summary>
        public const string C_IPManage_PA_Directory = "~/Data/IPManage/PA";
        /// <summary>
        /// 商标图片
        /// </summary>
        public const string C_IPManage_TM_Directory = "~/Data/IPManage/TM";
        #endregion

        #region 打印模板路径
        /// <summary>
        /// 企业资助-打印模板-存放目录
        /// </summary>
        public const string C_Funding_PrintTemplate_Directory = "~/Data/funding/printtemplate";

        /// <summary>
        /// 企业资助-专利-打印模板-存放目录
        /// </summary>
        public const string C_Funding_PA_PrintTemplate_Directory = C_Funding_PrintTemplate_Directory + "/pa";
        /// <summary>
        /// 企业资助-版权-打印模板-存放目录
        /// </summary>
        public const string C_Funding_CO_PrintTemplate_Directory = C_Funding_PrintTemplate_Directory + "/co";
        #endregion

        #region 后台交易管理发布交易批量发布 excel上传路径
        /// <summary>
        /// 后台交易管理发布交易批量发布 excel上传路径
        /// </summary>
        public const string B_Transaction_PA_BatchPublishDirectory = "~/Data/Transfer/PATrans/BatchPublish";
        #endregion

        #region 后台需求管理发布需求批量发布 excel上传路径
        /// <summary>
        /// 后台需求管理发布需求批量发布 excel上传路径
        /// </summary>
        public const string B_Transaction_Req_BatchPublishDirectory = "~/Data/Transfer/REQ/BatchPublish";
        #endregion

        #region 后台专利估价批量发布 excel上传路径
        /// <summary>
        /// 后台专利估价批量发布 excel上传路径
        /// </summary>
        public const string B_Transaction_PA_Values_BatchPublishDirectory = "~/Data/Transfer/PAValues/BatchPublish";
        #endregion

        #region 后台数据与分析导入数据 excel上传路径
        /// <summary>
        /// 后台数据与分析导入数据 excel上传路径
        /// </summary>
        public const string B_ImportData = "~/Data/Transfer/ImportData";
        #endregion

        #region 后台需求管理发布需求批量发布 模板下载地址
        /// <summary>
        /// 后台需求管理发布需求批量发布 模板下载地址
        /// </summary>
        public const string B_Transaction_Req_BatchPublishFilePath = "~/Data/Template/Req/BatchPublish/专利交易信息批量发布模板(内部).xlsx";
        #endregion

        #region 后台交易管理发布交易批量发布 模板下载地址
        /// <summary>
        /// 后台交易管理发布交易批量发布 模板下载地址
        /// </summary>
        public const string B_Transaction_PA_BatchPublishFilePath = "~/Data/Template/PATrans/BatchPublish/专利交易信息批量发布模板(内部).xlsx";
        #endregion

        #region 前台会员中心我的交易批量发布 模板下载地址
        /// <summary>
        /// 前台会员中心我的交易批量发布 模板下载地址
        /// </summary>
        public const string MemberCenter_MyTransaction_BatchPublishFilePath = "~/Data/Template/PATrans/BatchPublish/我的交易信息批量发布模板.xlsx";
        #endregion

        #region 后台专利估价批量发布 模板下载地址
        /// <summary>
        /// 后台专利估价批量发布 模板下载地址
        /// </summary>
        public const string B_Transaction_PA_Values_BatchPublishFilePath = "~/Data/Template/PAValues/BatchPublish/专利估价信息批量发布模板.xlsx";
        #endregion

        #region 发布交易发布专利图片路径
        /// <summary>
        /// 发布交易发布专利图片路径
        /// </summary>
        public const string B_Transaction_PA_IMGDirectory = "~/Data/Transaction_PA";
        #endregion

        #region  专利交易联系记录上传附件路径
        /// <summary>
        /// 专利交易联系记录上传附件路径
        /// </summary>
        public static string C_Tech_PATransaction_LinkRecordDirectory = "~/Data/Tech/PA/LinkRecord";
        #endregion

        #region 专利需求联系记录上传附件路径
        /// <summary>
        /// 专利需求联系记录上传附件路径
        /// </summary>
        public static string C_Tech_SEQ_LinkRecordDirectory = "~/Data/Tech/SEQ/LinkRecord";
        #endregion

        #region 后台合同管理默认合同模板
        /// <summary>
        /// 后台合同管理默认合同模板
        /// </summary>
        public const string B_Contract_DefaulTemplate = "~/Data/Template/Contract/2019年 专利权转让合同.docx";
        #endregion

        #region 后台合同设置上传合同模板路径
        /// <summary>
        /// 后台合同设置上传合同模板路径
        /// </summary>
        public const string B_Contract_UploadDirectory = "~/Data/Template/Contract";
        #endregion

        #region 后台合同设置下载合同模板路径
        /// <summary>
        /// 后台合同设置下载合同模板路径
        /// </summary>
        public const string B_Contract_DownloadDirectory = "~/Data/Transfer/Contract";
        #endregion

        #region 前台-专利市场-列表-导出清单-模板地址
        /// <summary>
        /// 前台-专利市场-列表-导出清单-模板地址
        /// </summary>
        public const string C_PATrans_ExportExecl_Forepart_TemplateFilePath = "~/Data/Template/PATrans/TransExportExcel/知聚网专利交易清单.xlsx";
        #endregion

        #region 前台-技术需求-列表-导出清单-模板地址
        /// <summary>
        /// 前台-技术需求-列表-导出清单-模板地址
        /// </summary>
        public const string C_TechREQ_ExportExecl_Forepart_TemplateFilePath = "~/Data/Template/Req/ExportExcel/知聚网技术需求清单.xlsx";
        #endregion

        #region 提示消息
        /// <summary>
        /// 数据无效提示信息
        /// </summary> 
        public const string C_Message_NoData = "您选择的数据不存在或已被删除。";
        #endregion

        #region 订单可以申请发票的时间间隔
        public const int InvoiceDay = 90;
        #endregion
        #endregion

        #region 系统变量
        public static string ReadConnStrName = Common.AppSetting("ReadConnStrName");
        public static string WriteConnStrName = Common.AppSetting("WriteConnStrName");


        #region 管理员角色ID列表 用,(英文逗号)分割
        public static string AdminRoleIDList = "";
        #endregion

        #region 前台-首页-显示数量 崔萌
        /// <summary>
        /// 特价秒杀专利显示数量
        /// </summary>
        public static int QueryBargain_PA_DisplayNum = 0;

        /// <summary>
        /// 最新发布显示专利数量
        /// </summary>
        public static int NewPublish_PA_DisplayNum = 0;

        /// <summary>
        /// 推荐专利显示数量
        /// </summary>
        public static int Recommend_PA_DisplayNum = 0;

        /// <summary>
        /// 专利交易显示数量
        /// </summary>
        public static int Transaction_PA_DisplayNum = 0;

        /// <summary>
        /// 技术需求显示数量
        /// </summary>
        public static int Technology_REQ_DisplayNum = 0;

        /// <summary>
        /// 成交动态显示数量
        /// </summary>
        public static int TransactionTrends_DisplayNum = 0;

        /// <summary>
        /// 新闻资讯显示数量
        /// </summary>
        public static int NewsInfo_DisplayNum = 0;

        /// <summary>
        /// 中介推荐显示数量 9
        /// </summary>
        public static int Agent_DisplayNum = 0;
        #endregion

        #region 专利市场
        /// <summary>
        /// 专利市场显示数量
        /// </summary>
        public static int PAMarket_DisplayNum = 0;
        #endregion

        #region 技术需求
        /// <summary>
        /// 技术需求显示数量
        /// </summary>
        public static int TechReq_DisplayNum = 0;
        #endregion

        #region 高校成果
        /// <summary>
        /// 高校成果详细页面显示专利数量 6
        /// </summary>
        public static int CollegesDetail_Expert_DisplayNum = 0;
        /// <summary>
        /// 高校成果-专家列表显示专家数量 18
        /// </summary>
        public static int Colleges_Expert_DisplayNum = 0;

        #endregion

        #region 中介推荐 崔萌 2020-4-7 13:40:51
        /// <summary>
        /// 中介推荐列表显示数量 24
        /// </summary>
        public static int AgentRecommend_DisplayNum = 0;

        /// <summary>
        /// 中介推荐-中介详情-专利列表显示数量 10
        /// </summary>
        public static int AgentRecommend_PAList_DisplayNum = 0;
        #endregion

        #region 前台-新闻资讯、科技政策、知产百科-显示新闻数量 崔萌 2020-2-13 11:12:11
        /// <summary>
        /// 公司新闻显示数量
        /// </summary>
        public static int CompanyNews_DisplayNum = 0;

        /// <summary>
        /// 行业新闻显示数量
        /// </summary>
        public static int IndustryNews_DisplayNum = 0;

        /// <summary>
        /// 科技政策显示数量
        /// </summary>
        public static int PoliciesRegulations_DisplayNum = 0;

        /// <summary>
        /// 知产百科显示数量
        /// </summary>
        public static int KnowledgeEncyclopedia_DisplayNum = 0;
        #endregion

        #region 前台-首页-通知要闻显示新闻数量
        /// <summary>
        /// 前台-会员中心-消息列表显示数量
        /// </summary>
        public static int Member_MessageList_DisplayNum = 0;//25
        #endregion

        #region 前台-首页-通知要闻显示新闻数量
        /// <summary>
        /// 前台-首页-通知要闻-图片新闻显示数量
        /// </summary>
        public static int Notifications_TuPianNews_DisplayNum = 0;//3--
        /// <summary>
        /// 前台-首页-通知要闻-显示数量
        /// </summary>
        public static int Portal_Notifications_DisplayNum = 0;//6--

        /// <summary>
        /// 前台-首页-政策法规、办事指南、维权援助、培训服务显示数量
        /// </summary>
        public static int Portal_Common_DisplayNum = 0;//5--

        /// <summary>
        /// 前台-首页-专利运营-北斗专利显示数量
        /// </summary>
        public static int Operation_BeiDou_DisplayNum = 0;//4
        #endregion

        #region 前台--我的订单订单--付款账户
        /// <summary>
        /// 前台--我的订单订单--付款，个人账户线下支付账户
        /// </summary>
        public static string Unionpay_Personal_Account = "";
        /// <summary>
        /// 前台--我的订单订单--付款，公司账户线下支付账户
        /// </summary>
        public static string Unionpay_Company_Account = "";
        /// <summary>
        /// 前台--我的订单订单--付款，个人账户支付宝账户
        /// </summary>
        public static string Alipay_Personal_Account = "";
        #endregion

        #region 积分说明 崔萌 2020-3-18 10:50:31
        /// <summary>
        /// 获取积分-账户注册
        /// </summary>
        public static int GetIntegrals_Register = 0;
        /// <summary>
        /// 获取积分-实名认证
        /// </summary>
        public static int GetIntegrals_Authentication = 0;
        /// <summary>
        /// 获取积分-每日首次登录
        /// </summary>
        public static int GetIntegrals_DailyLogin = 0;
        /// <summary>
        /// 获取积分-发布需求
        /// </summary>
        public static int GetIntegrals_PublishSEQ = 0;
        /// <summary>
        /// 获取积分-预留专利
        /// </summary>
        public static int GetIntegrals_ReservePA = 0;
        /// <summary>
        /// 获取积分-查询卖家信息
        /// </summary>
        public static int GetIntegrals_SellerInfo = 0;
        /// <summary>
        /// 获取积分-费用信息
        /// </summary>
        public static int GetIntegrals_FeeInfo = 0;
        /// <summary>
        /// 获取积分-法律状态
        /// </summary>
        public static int GetIntegrals_LawState = 0;
        #endregion

        #region 会员中心-我的工作台-推送消息数量 崔萌 2020-4-4 10:07:20
        /// <summary>
        /// 会员中心-我的工作台-推送消息数量
        /// </summary>
        public static int MemberCenter_Message_DisplayNum = 0;
        #endregion

        #region 会员中心-消息列表-显示数量 崔萌 2020-4-4 15:37:11
        /// <summary>
        /// 会员中心-消息列表-显示数量
        /// </summary>
        public static int MemberCenter_MessageList_DisplayNum = 0;
        #endregion

        #region 专利运营
        /// <summary>
        /// 首页-专利运营显示数据数量
        /// </summary>
        public static int Portal_Operation_DisplayNum = 0;//10
        /// <summary>
        /// 专利运营-列表显示数据数量
        /// </summary>
        public static int Operation_Common_DisplayNum = 0;//12
        #endregion

        #region 前台-新闻列表页面-显示新闻数量
        /// <summary>
        /// 前台-新闻列表页面-图片新闻显示数量
        /// </summary>
        public static int List_TuPianNews_DisplayNum = 0;//12
        /// <summary>
        /// 前台-新闻列表页面-显示数量(通用)
        /// </summary>
        public static int List_Common_DisplayNum = 0;//15--
        /// <summary>
        /// 前台-新闻列表页面-专利运营显示数量
        /// </summary>
        public static int List_PA_DisplayNum = 0;//10
        #endregion

        #region 前台注册默认角色
        public static int RegisterRoleID = 0;
        public static string RegisterRoleCode = "";
        public static string RegisterRoleName = "";
        #endregion

        #region 用户认证后的角色
        public static int AuthenticationRoleID = 0;
        public static string AuthenticationRoleCode = "";
        public static string AuthenticationRoleName = "";
        #endregion

        #region 业务主管角色
        public static int SupervisorRoleID = 0;
        public static string SupervisorRoleCode = "";
        public static string SupervisorRoleName = "";
        #endregion

        #region 业务总监角色
        /// <summary>
        /// 业务总监
        /// </summary>
        public static int DirectorRoleID = 0;
        public static string DirectorRoleCode = "";
        public static string DirectorRoleName = "";
        #endregion

        #region 系统版本
        public static string Sys_Version_Code = "";
        #endregion

        // <summary>
        /// 上传图片的图片大小 5M
        /// </summary>
        public static int Sys_Image_Size = 0;

        // <summary>
        /// 上传文件的文件大小 10M
        /// </summary>
        public static int Sys_File_Size = 0;
        /// <summary>
        /// 专利交易-预留天数
        /// </summary>
        public static int PATrans_Retain_Days = 0;

        #region 邮箱发送
        public static string Email_Interface_Key = "";//guoziip
        public static string Email_Interface_Password = "";//remoting36
        public static string Email_Interface_Host = "";//smtp.126.com  
        public static int Email_Interface_Port = 0;//25
        public static string Email_Interface_PostFix = "";//126.com
        public static string Email_Interface_SSL = "";// Y/N
        #endregion


        #region 短信发送
        public static string SNS_Interface_Key = "";
        public static string SNS_Interface_Password = "";
        public static string SNS_Interface_Enterprise = "";
        public static string SNS_Interface_Host = "";
        public static string SNS_Interface_Method = "";
        public static string SNS_Interface_ContentType = "";
        #endregion

        #region 引擎检索
        /// <summary>
        /// 引擎端口
        /// </summary>
        public static string SearchEngine_Interface_Host = "";
        /// <summary>
        /// 引擎接口名称-专利案件
        /// </summary>
        public static string SearchEngine_Interface_PACaseInfo = "";
        /// <summary>
        /// 引擎接口名称-专利检索（分页）
        /// </summary>
        public static string SearchEngine_Interface_PASearch = "";
        /// <summary>
        /// 引擎接口名称-专利推荐(开头、最后不带/)
        /// </summary>
        public static string SearchEngine_Interface_PARecommended = "";
        /// <summary>
        /// 引擎接口名称-根据多个申请号检索相关的专利数据(开头、最后不带/)
        /// </summary>
        public static string SearchEngine_Interface_PASQH = "";
        /// <summary>
        /// 引擎接口名称-商标检索（分页）
        /// </summary>
        public static string SearchEngine_Interface_TMSearch = "";
        /// <summary>
        /// 引擎接口名称-商标推荐(开头、最后不带/)
        /// </summary>
        public static string SearchEngine_Interface_TMRecommended = "";
        /// <summary>
        /// 引擎接口名称-商标案件
        /// </summary>
        public static string SearchEngine_Interface_TMCaseInfo = "";
        /// <summary>
        /// 引擎接口-token
        /// </summary>
        public static string SearchEngine_Interface_Token = "";
        /// <summary>
        /// 引擎接口-用户ID
        /// </summary>
        public static string SearchEngine_Interface_UserID = "";
        /// <summary>
        /// 引擎目录-专利图片路径
        /// </summary>
        public static string SearchEngine_Directory_PAImage = "";
        /// <summary>
        /// 引擎目录-版权图片路径
        /// </summary>
        public static string SearchEngine_Directory_TMImage = "";
        /// <summary>
        /// 引擎目录-专利XML路径
        /// </summary>
        public static string SearchEngine_Directory_PAXML = "";



        /// <summary>
        /// 商标、专利检索列表-每页显示数量 5
        /// </summary>
        public static int Search_List_DisplayNum = 0;
        /// <summary>
        /// 专利检索-专利推荐显示数量 5
        /// </summary>
        public static int Search_PARecommanded_DisplayNum = 0;
        /// <summary>
        /// 商标检索-商标推荐显示数量 4
        /// </summary>
        public static int Search_TMRecommanded_DisplayNum = 0;



        /// <summary>
        /// 搜索引擎下载pdf文件路径（{0}发明或新型拼接串；{1}公告号）
        /// </summary>
        public static string SearchEngine_DownloadPDF_URL = "";
        /// <summary>
        /// 搜索引擎下载pdf文件,发明专利路径拼接串
        /// </summary>
        public static string SearchEngine_DownloadPDF_FM = "";
        /// <summary>
        /// 搜索引擎下载pdf文件,实用新型路径拼接串
        /// </summary>
        public static string SearchEngine_DownloadPDF_XX = "";


        /// <summary>
        /// 引擎端口
        /// </summary>
        public static string SearchEngine_Interface_Port = "";
        /// <summary>
        /// 引擎接口名称-费用信息和最新法律状态
        /// </summary>
        public static string SearchEngine_Interface_FeeInfoAndLawState = "";
        /// <summary>
        /// 引擎接口-token
        /// </summary>
        public static string SearchEngine_Interface_TokenParam = "";

        /// <summary>
        /// 引擎接口-高校成果-名称
        /// </summary>
        public static string SearchEngine_Interface_CollegesData_Name = "";
        /// <summary>
        /// 引擎接口-高校成果-用户ID
        /// </summary>
        public static string SearchEngine_Interface_CollegesData_UserID = "";
        /// <summary>
        /// 引擎接口-高校成果-Token
        /// </summary>
        public static string SearchEngine_Interface_CollegesData_Token = "";
        #endregion

        /// <summary>
        /// 首页-IPC备案号
        /// </summary>
        public static string Portal_IPC_BeiAnHao = "";// 14058701号-1
        /// <summary>
        /// 首页-公网安备案号
        /// </summary>
        public static string Portal_PublicSecurity_BeiAnHao = "";// 44011602000502
        /// <summary>
        /// 首页-版权搜索 url
        /// </summary>
        public static string Portal_COSearch_URL = "";// 

        #region 支付宝接口信息

        //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        // 合作身份者ID，签约账号，以2088开头由16位纯数字组成的字符串，查看地址：https://b.alipay.com/order/pidAndKey.htm
        public static string Alipay_Partner = "";

        // 收款支付宝账号，以2088开头由16位纯数字组成的字符串，一般情况下收款账号就是签约账号
        public static string Alipay_Seller_Id = "";

        // MD5密钥，安全检验码，由数字和字母组成的32位字符串，查看地址：https://b.alipay.com/order/pidAndKey.htm
        public static string Alipay_Key = "";

        // 服务器异步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数,必须外网可以正常访问
        public static string Alipay_Notify_Url = "";

        // 页面跳转同步通知页面路径，需http://格式的完整路径，不能加?id=123这类自定义参数，必须外网可以正常访问
        public static string Alipay_Return_Url = "";

        //↑↑↑↑↑↑↑↑↑↑请在这里配置防钓鱼信息，如果没开通防钓鱼功能，请忽视不要填写 ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑

        #endregion
        #endregion

        #region 微信开放平台信息
        public static string AppId = "";
        public static string AppSecret = "";
        #endregion
    }
}
