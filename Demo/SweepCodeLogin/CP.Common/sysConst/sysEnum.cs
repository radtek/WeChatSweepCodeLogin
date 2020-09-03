namespace CP.Common
{
    /// <summary>
    /// 平台运行状态
    /// </summary>
    public enum SysInit
    {
        /// <summary>
        /// 未初始化
        /// </summary>
        Uninitialized = 0,
        /// <summary>
        /// 初始化中
        /// </summary>
        Initializing = 1,
        /// <summary>
        /// 正常运行
        /// </summary>
        Normal = 2
    }

    /// <summary>
    /// 登陆类别
    /// </summary>
    public enum LoginType
    {
        /// <summary>
        /// 禁止登陆
        /// </summary>
        Banned = 0,
        /// <summary>
        /// 前端登录
        /// </summary>
        Forepart = 1,
        /// <summary>
        /// 管理后台登录
        /// </summary>
        Management = 2,
        /// <summary>
        /// 所有
        /// </summary>
        ALL = 3

    }

    /// <summary>
    /// 执行状态 
    /// </summary>
    public enum RunState
    {
        /// <summary>
        /// 未执行
        /// </summary>
        NotRun = 0,
        /// <summary>
        /// 已执行
        /// </summary>
        HasRun = 1
    }

    /// <summary>
    /// 用户认证状态  
    /// 0未申请 1正在申请 2申请通过
    /// </summary>
    public enum CheckState
    {
        /// <summary>
        /// 未申请
        /// </summary>
        NotCheck = 0,
        /// <summary>
        /// 正在申请
        /// </summary>
        Checking = 1,
        /// <summary>
        /// 申请通过
        /// </summary>
        Approved = 2,
        /// <summary>
        /// 拒绝
        /// </summary>
        Refuse = 3
    }


    /// <summary>
    /// 订单状态
    /// 1 未付款 2 已付款 3 取消 4 财务正在审核
    /// </summary>
    public enum OrderState
    {
        /// <summary>
        /// 未付款
        /// </summary>
        NoPay = 1,
        /// <summary>
        /// 已付款
        /// </summary>
        HasPay = 2,
        /// <summary>
        /// 取消
        /// </summary>
        Cancel = 3,
        /// <summary>
        /// 正在审核
        /// </summary>
        Checking = 4
    }


    /// <summary>
    /// 订单类型   
    /// </summary>
    public struct OrderType
    {
        public const string Income = "收入";
        public const string Payment = "支出";
    }
    /// <summary>
    /// 发票申请状态 
    /// 0未开票 1正在申请 2已开票
    /// </summary>
    public enum InvoiceApplyState
    {
        /// <summary>
        /// 未开票
        /// </summary>
        NotCheck = 0,
        /// <summary>
        /// 正在申请 
        /// </summary>
        Checking = 1,
        /// <summary>
        /// 已开票
        /// </summary>
        Approved = 2,
        /// <summary> 
        /// 已作废
        /// </summary>
        Obsolete = 3
    }

    /// <summary>
    /// 支付方式
    /// </summary>
    public struct PaymentMethod
    {
        /// <summary>
        /// 支付宝
        /// </summary>
        public const string Alipay = "支付宝";
        /// <summary>
        /// 个人支付宝转账
        /// </summary>
        public const string PersonalAlipay = "个人支付宝转账";
        /// <summary>
        /// 微信
        /// </summary>
        public const string WeChat = "微信";
        /// <summary>
        /// 线下
        /// </summary>
        public const string Line = "线下";
    }
    /// <summary>
    /// 菜单类型
    /// 1前端菜单 2管理后台菜单
    /// </summary>
    public enum MenuType
    {
        /// <summary>
        /// 前端菜单
        /// </summary>
        Forepart = 1,
        /// <summary>
        /// 管理后台菜单
        /// </summary>
        Management = 2,
    }

    public class LogEnum
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public struct Operation
        {
            /// <summary>
            /// 加载列表
            /// </summary>
            public const string LoadList = "加载列表";
            /// <summary>
            /// 新增
            /// </summary>
            public const string Add = "新增";
            /// <summary>
            /// 修改
            /// </summary>
            public const string Edit = "修改";
            /// <summary>
            /// 删除
            /// </summary>
            public const string Delete = "删除";
            /// <summary>
            /// 移除
            /// </summary>
            public const string Remove = "移除";
            /// <summary>
            /// 审核
            /// </summary>
            public const string Check = "审核";
        }
        /// <summary>
        /// 执行状态
        /// </summary>
        public struct LogRunState
        {
            /// <summary>
            /// 执行成功
            /// </summary>
            public const string Succeed = "执行成功";
            /// <summary>
            /// 执行失败
            /// </summary>
            public const string Failure = "执行失败";
        }
    }


    /// <summary>
    /// 账户类型
    /// </summary>
    public struct AccountType
    {
        /// <summary>
        /// 个人账户
        /// </summary>
        public const string Personal = "个人账户";
        /// <summary>
        /// 企业账户
        /// </summary>
        public const string Enterprise = "企业账户";
        /// <summary>
        /// 高校院所
        /// </summary>
        public const string University = "高校院所";
        /// <summary>
        /// 其他事业单位
        /// </summary>
        public const string OtherInstitution = "其他事业单位";
    }

    /// <summary>
    /// 注册类别
    /// </summary>
    public struct RegType
    {
        /// <summary>
        /// 手机注册
        /// </summary>
        public const string MobilePhone = "手机注册";
        /// <summary>
        /// 邮箱注册
        /// </summary>
        public const string Email = "邮箱注册";
    }

    /// <summary>
    /// 是否删除
    /// </summary>
    public struct IsDelete
    {
        public const string Y = "Y";
        public const string N = "N";
    }

    /// <summary>
    /// 是否启用
    /// </summary>
    public struct IsFlag
    {
        public const string Y = "Y";
        public const string N = "N";
    }


    /// <summary>
    /// 专利运营 新闻类型
    /// 1 专利交易 2 专利需求 3 本区交易
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 专利交易
        /// </summary>
        PATrans = 1,
        /// <summary>
        /// 专利需求
        /// </summary>
        PAReq = 2,
        /// <summary>
        /// 本区交易
        /// </summary>
        Success = 3
    }

    /// <summary>
    /// 新闻资讯 新闻类型
    /// 1公司新闻 2行业要闻 3科技政策 4知识百科
    /// </summary>
    public struct NewsType
    {
        /// <summary>
        /// 公司新闻
        /// </summary>
        public const string company = "company";

        /// <summary>
        /// 行业要闻
        /// </summary>
        public const string industry = "industry";

        /// <summary>
        /// 科技政策
        /// </summary>
        public const string technology = "technology";

        /// <summary>
        /// 知识百科
        /// </summary>
        public const string knowledge = "knowledge";
    }

    /// <summary>
    /// 维权援助 新闻类型
    /// 1 服务指南 2 维权案例 3 维权援助
    /// </summary>
    public enum RightsAidType
    {
        /// <summary>
        /// 服务指南
        /// </summary>
        FuWuZhiNan = 1,
        /// <summary>
        /// 维权案例
        /// </summary>
        WeiQuanAnLi = 2,
        /// <summary>
        /// 维权援助
        /// </summary>
        WeiQuanYuanZhu = 3
    }


    /// <summary>
    /// 政策法规 新闻类型
    /// 1 国际条约 2 国内法规 3 省市政策 4 本区政策
    /// </summary>
    public enum PoliciesRegulationsType
    {
        /// <summary>
        /// 国际条约
        /// </summary>
        GuoJiTiaoYue = 1,
        /// <summary>
        /// 国内法规
        /// </summary>
        GuoNeiFaGui = 2,
        /// <summary>
        /// 省市政策
        /// </summary>
        ShengShiZhengCe = 3,
        /// <summary>
        /// 本区政策
        /// </summary>
        BenQuZhengCe = 4
    }

    /// <summary>
    /// 办事指南 新闻类型
    /// 1 申请指南 2 专利资助 3 费用减免备案 4 专利优先审查 5 中介服务
    /// </summary>
    public enum HandingGuideType
    {
        /// <summary>
        /// 申请指南
        /// </summary>
        ShenQingZhiNan = 1,
        /// <summary>
        /// 专利资助
        /// </summary>
        ZhuanLiZhiZhu = 2,
        /// <summary>
        /// 费用减免备案
        /// </summary>
        FeiYongJianMian = 3,
        /// <summary>
        /// 专利优先审查
        /// </summary>
        ZhuanLiYouXian = 4,
        /// <summary>
        /// 中介服务
        /// </summary>
        ZhongJieFuWu = 5
    }


    /// <summary>
    /// 新闻法规类别
    /// 综合改革 政策法规 通知要闻
    /// </summary>
    public struct NewsModularType
    {
        /// <summary>
        /// 综合改革
        /// </summary>
        public const string ZongHeGaiGe = "综合改革";
        /// <summary>
        /// 政策法规
        /// </summary>
        public const string ZhengCeFaGui = "政策法规";
        /// <summary>
        /// 通知要闻
        /// </summary>
        public const string TongZhiYaoWen = "通知要闻";
    }


    /// <summary>
    /// 培训服务 类型
    /// 1 培训动态 2 培训课件
    /// </summary>
    public enum TrainType
    {
        /// <summary>
        /// 培训动态
        /// </summary>
        PeiXunDongTai = 1,
        /// <summary>
        /// 培训课件
        /// </summary>
        PeiXunKeJian = 2
    }

    /// <summary>
    /// 培训服务类别
    /// 培训动态 培训课件
    /// </summary>
    public struct TrainModularType
    {
        /// <summary>
        /// 培训动态
        /// </summary>
        public const string PeiXunDongTai = "培训动态";
        /// <summary>
        /// 培训课件
        /// </summary>
        public const string PeiXunKeJian = "培训课件";
    }


    /// <summary>
    /// 培训服务 报名状态
    /// 正常报名 取消报名
    /// </summary>
    public struct TrainApplyState
    {
        /// <summary>
        /// 正常报名
        /// </summary>
        public const string Normal = "正常报名";
        /// <summary>
        /// 取消报名
        /// </summary>
        public const string Cancel = "取消报名";
    }

    /// <summary>
    /// 专利运营 北斗专利 联系状态
    /// 已联系 未联系
    /// </summary>
    public struct BeiDouContactState
    {
        /// <summary>
        /// 正常报名
        /// </summary>
        public const string AlreadyConnected = "已联系";
        /// <summary>
        /// 取消报名
        /// </summary>
        public const string UnConnected = "未联系";
    }

    /// <summary>
    /// 专利运营 北斗专利 交易状态
    /// 已交易 未交易
    /// </summary>
    public struct BeiDouTranscationState
    {
        /// <summary>
        /// 已交易
        /// </summary>
        public const string AlreadyTraded = "已交易";
        /// <summary>
        /// 未交易
        /// </summary>
        public const string Untraded = "未交易";
    }


    /// <summary>
    /// 企业资助申报 案件状态
    /// 未审核 审核未通过 审核通过 补正 暂存 部分通过
    /// </summary>
    public struct SupportState
    {
        /// <summary>
        /// 未审核
        /// </summary>
        public const string NotCheck = "未审核";
        /// <summary>
        /// 审核未通过
        /// </summary>
        public const string CheckFailed = "审核未通过";
        /// <summary>
        /// 补正
        /// </summary>
        public const string Corrections = "补正";
        /// <summary>
        /// 审核通过
        /// </summary>
        public const string Checked = "审核通过";
        /// <summary>
        /// 暂存
        /// </summary>
        public const string Save = "暂存";
    }
    /// <summary>
    /// 企业资助申报 提交类型
    /// 提交 暂存
    /// </summary>
    public struct SupportSubmitType
    {
        /// <summary>
        /// 提交
        /// </summary>
        public const string Submit = "submit";
        /// <summary>
        /// 暂存
        /// </summary>
        public const string Save = "save";
    }

    /// <summary>
    /// 企业资助申报 页面加载类型
    /// add、edite、read、save
    /// </summary>
    public struct SupportLoadType
    {
        /// <summary>
        /// 新增
        /// </summary>
        public const string Add = "add";
        /// <summary>
        /// 补正编辑
        /// </summary>
        public const string Edite = "edite";
        /// <summary>
        /// 普通加载
        /// </summary>
        public const string Read = "read";
        /// <summary>
        /// 暂存
        /// </summary>
        public const string Save = "save";
    }

    /// <summary>
    /// 系统消息是否已读
    /// </summary>
    public enum MessageIsRead
    {
        /// <summary>
        /// 未读
        /// </summary>
        UnRead = 0,
        /// <summary>
        /// 已读
        /// </summary>
        AlreadyRead = 1
    }

    /// <summary>
    /// 联系人类别
    /// </summary>
    public struct LinkManType
    {
        /// <summary>
        /// 主管
        /// </summary>
        public const string Director = "主管";
        /// <summary>
        /// 专员
        /// </summary>
        public const string Assistant = "专员";
    }

    /// <summary>
    /// 页面checkbox是否选中
    /// </summary>
    public enum IsChecked
    {
        /// <summary>
        /// 未选中
        /// </summary>
        Unchecked = 0,
        /// <summary>
        /// 选中
        /// </summary>
        Checked = 1
    }

    /// <summary>
    /// 专利资助类型
    /// </summary>
    public struct FundingType
    {
        /// <summary>
        /// 专利费用资助申报
        /// </summary>
        public const string Case01 = "Case01";
        /// <summary>
        /// 知识产权申请申报
        /// </summary>
        public const string Case02 = "Case02";
        /// <summary>
        /// 企业知识产权备案
        /// </summary>
        public const string Case03 = "Case03";
        /// <summary>
        /// 法律援助申报
        /// </summary>
        public const string Case04 = "Case04";
    }
    /// <summary>
    /// 专利资助 打印类型
    /// </summary>
    public struct PAFundingPrintType
    {
        /// <summary>
        /// 专利 
        /// </summary>
        public const string PA = "pa";
        /// <summary>
        /// 版权
        /// </summary>
        public const string CO = "co";
    }

    /// <summary>
    /// 版权资助，子表清单类型
    /// </summary>
    public struct COFundingListType
    {
        /// <summary>
        /// 代理著作权登记清单 
        /// </summary>
        public const string ProxyCO = "代理著作权";
        /// <summary>
        /// 代理计算机软件著作权登记清单
        /// </summary>
        public const string ProxySWCO = "代理计算机软件著作权";
    }

    /// <summary>
    /// 技术转移 交易状态
    /// 1 发布 2 成交 3 取消
    /// </summary>
    public enum TechTranState
    {
        /// <summary>
        /// 未发布
        /// </summary>
        UnPublish = 0,
        /// <summary>
        /// 发布
        /// </summary>
        Publish = 1,
        /// <summary>
        /// 成交
        /// </summary>
        Deal = 2,
        /// <summary>
        /// 取消
        /// </summary>
        Cancel = 3,
        /// <summary>
        /// 合同签订中
        /// </summary>
        SignContract = 4
    }
    /// <summary>
    /// 购买申请 审核状态 (0已拒绝,1正在申请，2已交易）
    /// </summary>
    public enum BuyCheckState
    {
        /// <summary>
        /// 已拒绝
        /// </summary>
        Refuse = 0,
        /// <summary>
        /// 正在申请
        /// </summary>
        Publish = 1,
        /// <summary>
        /// 已交易
        /// </summary>
        Deal = 2
    }

    /// <summary>
    /// 技术转移--专利交易--发布交易--专利权归属
    /// </summary>
    public struct ZLQGS
    {
        /// <summary>
        /// 自营
        /// </summary>
        public const string ZiYing = "自营";
        /// <summary>
        /// 他营
        /// </summary>
        public const string TaYing = "他营";
        /// <summary>
        /// 联营
        /// </summary>
        public const string LianYing = "联营";
    }

    /// <summary>
    /// 需求管理--专利需求--处理状态
    /// </summary>
    public enum REQDealState
    {
        /// <summary>
        /// 已交易
        /// </summary>
        AlreadyTraded = 1,
        /// <summary>
        /// 未交易
        /// </summary>
        Untraded = 2,
        /// <summary>
        /// 取消交易
        /// </summary>
        CancelTraded = 3
    }

    /// <summary>
    /// QQ数据抓取--类型 
    /// 1 求购  2 转让  3 其他
    /// </summary>
    public struct QQFlushDataType
    {
        /// <summary>
        /// 求购
        /// </summary>
        public const int QiuGou = 1;
        /// <summary>
        /// 转让
        /// </summary>
        public const int ZhuanRang = 2;
        /// <summary>
        /// 其他
        /// </summary>
        public const int Others = 3;
    }

    /// <summary>
    /// 确认收货 1已签收 0未签收
    /// </summary>
    public enum ConfirmReceiptState
    {
        /// <summary>
        /// 1已签收
        /// </summary>
        Receipt = 1,

        /// <summary>
        /// 未签收
        /// </summary>
        NoReceipt = 0
    }

    /// <summary>
    /// 专利估价：变更情况、证书原件、用于高企申报、转让资料原件状态
    /// </summary>
    public enum ValuesState
    {
        /// <summary>
        /// 有
        /// </summary>
        Have = 'Y',

        /// <summary>
        /// 无
        /// </summary>
        NOHave = 'N'
    }

    /// <summary>
    /// 合同状态：0:未签；1:已签；2:作废
    /// </summary>
    public enum ContractState
    {
        /// <summary>
        /// 未签
        /// </summary>
        NoSign = 0,

        /// <summary>
        /// 已签
        /// </summary>
        Sign = 1,

        /// <summary>
        /// 作废
        /// </summary>
        Delete = 2
    }

    /// <summary>
    /// 访问数据类型
    /// </summary>
    public struct AccessDataType
    {
        /// <summary>
        /// 今日
        /// </summary>
        public const string today = "today";

        /// <summary>
        /// 昨日
        /// </summary>
        public const string yesterday = "yesterday";

        /// <summary>
        /// 本周
        /// </summary>
        public const string week = "week";

        /// <summary>
        /// 本月
        /// </summary>
        public const string month = "month";
    }

    /// <remarks>崔萌 2020-1-13 09:40:47</remarks>
    /// <summary>
    /// 专利法律状态
    /// </summary>
    public struct PAFLState
    {
        /// <summary>
        /// 有权-授权
        /// </summary>
        public const string Entitled_Accredit = "有权-授权";

        /// <summary>
        /// 专利权维持
        /// </summary>
        public const string PatentRightMaintenance = "专利权维持";

        /// <summary>
        /// 等年费滞纳金
        /// </summary>
        public const string AnnualFeeOverdueFine = "等年费滞纳金";

        /// <summary>
        /// 授权未交费
        /// </summary>
        public const string AuthorizedNonPayment = "授权未交费";

        /// <summary>
        /// 无权-避重放弃
        /// </summary>
        public const string NoRight_AvoidGivingUp = "无权-避重放弃";

        /// <summary>
        /// 无权-视为放弃
        /// </summary>
        public const string NoRight_RegardAsGivingUp = "无权-视为放弃";

        /// <summary>
        /// 无权-终止
        /// </summary>
        public const string NoRight_Stop = "无权-终止";

        /// <summary>
        /// 审中-公开
        /// </summary>
        public const string InAudit_Open = "审中-公开";

        /// <summary>
        /// 审中-实审
        /// </summary>
        public const string InAudit_RealTrial = "审中-实审";
    }

    /// <remarks>崔萌 2020-3-18 09:49:50</remarks>
    /// <summary>
    /// 会员等级(0:普通会员;1:VIP会员;2:VIPPlus会员)
    /// </summary>
    public enum VIPGrade
    {
        /// <summary>
        /// 普通会员
        /// </summary>
        Ordinary = 0,

        /// <summary>
        /// VIP会员
        /// </summary>
        VIP = 1,

        /// <summary>
        /// VIPPlus会员
        /// </summary>
        VIPPlus = 2,
    }

    /// <remarks>崔萌 2020-3-18 10:03:29</remarks>
    /// <summary>
    /// 积分来源1:账户注册;2:实名认证;3:每日首次登录;4:发布需求;5:充值;6:购买会员;7:预留专利;
    /// 8:查询卖家信息;9:费用信息;10:法律状态
    /// </summary>
    public enum IntegralsSource
    {
        /// <summary>
        /// 账户注册
        /// </summary>
        Register = 1,

        /// <summary>
        /// 实名认证
        /// </summary>
        Authentication = 2,

        /// <summary>
        /// 每日首次登录
        /// </summary>
        DailyLogin = 3,

        /// <summary>
        /// 发布需求
        /// </summary>
        PublishSEQ = 4,

        /// <summary>
        /// 充值
        /// </summary>
        Recharge = 5,

        /// <summary>
        /// 购买会员
        /// </summary>
        BuyVIP = 6,

        /// <summary>
        /// 预留专利
        /// </summary>
        ReservePA = 7,

        /// <summary>
        /// 查询卖家信息
        /// </summary>
        SellerInfo = 8,

        /// <summary>
        /// 费用信息
        /// </summary>
        FeeInfo = 9,

        /// <summary>
        /// 法律状态
        /// </summary>
        LawState = 10
    }
}
