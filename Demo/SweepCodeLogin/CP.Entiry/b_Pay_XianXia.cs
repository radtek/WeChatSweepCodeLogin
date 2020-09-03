using System;
using System.Collections.Generic;
using System.Text;
using CP.Common;
using CP.EntityBase;

namespace CP.Entiry
{
    public class b_Pay_XianXiaAttr
    {
        public const string ID = "ID";
        public const string FK_Order_ID = "FK_Order_ID";
        public const string AccountType = "AccountType";
        public const string PayType = "PayType";
        public const string PayMan = "PayMan";
        public const string PayDate = "PayDate";
        public const string SellerID = "SellerID";
        public const string MemberID = "MemberID";
        public const string MemberCode = "MemberCode";
        public const string MemberName = "MemberName";
        public const string CreateTime = "CreateTime";
        public const string CheckDate = "CheckDate";
        public const string CWMemberID = "CWMemberID";
        public const string CWMemberCode = "CWMemberCode";
        public const string CWMemberName = "CWMemberName";
        public const string Remark = "Remark";

    }
    public class b_Pay_XianXia : CP.EntityBase.Entity
    {
        public b_Pay_XianXia()
        {
            base.ReadConnStrName = sysConst.ReadConnStrName;
            base.WriteConnStrName = sysConst.WriteConnStrName;
        }
        /// <summary>
        /// 子类必须继承
        /// </summary>
        public override Map CreateEnMap()
        {
            Map map = new Map();
            map.PhysicsTable = "b_Pay_XianXia";
            map.AddTBStringID(b_Pay_XianXiaAttr.ID, null, "标识列", 0, 50);
            map.AddTBString(b_Pay_XianXiaAttr.FK_Order_ID, null, "外键 订单表ID", 0, 50);
            map.AddTBString(b_Pay_XianXiaAttr.AccountType, null, "账户类型 公司账户、个人账户", 0, 50);
            map.AddTBString(b_Pay_XianXiaAttr.PayType, null, "支付方式 （支付宝、微信、线下）", 0, 50);
            map.AddTBString(b_Pay_XianXiaAttr.PayMan, null, "付款人", 0, 50);
            map.AddTBString(b_Pay_XianXiaAttr.PayDate, null, "付款日", 0, 50);
            map.AddTBString(b_Pay_XianXiaAttr.SellerID, null, "在线付款 收款账号(支付宝) 下线付款 收款银行账户 或 个人 支付宝账户", 0, 25);
            map.AddTBString(b_Pay_XianXiaAttr.MemberID, null, "登录帐号ID", 0, 50);
            map.AddTBString(b_Pay_XianXiaAttr.MemberCode, null, "登录账号", 0, 50);
            map.AddTBString(b_Pay_XianXiaAttr.MemberName, null, "登录账户姓名", 0, 50);
            map.AddTBString(b_Pay_XianXiaAttr.CreateTime, null, "", 0, 50);
            map.AddTBString(b_Pay_XianXiaAttr.CheckDate, null, "财务确认日期", 0, 50);
            map.AddTBString(b_Pay_XianXiaAttr.CWMemberID, null, "财务确认 财务ID", 0, 50);
            map.AddTBString(b_Pay_XianXiaAttr.CWMemberCode, null, "财务确认 财务登录账号", 0, 50);
            map.AddTBString(b_Pay_XianXiaAttr.CWMemberName, null, "财务确认 财务姓名", 0, 50);
            map.AddTBString(b_Pay_XianXiaAttr.Remark, null, "备注/说明", 0, 300);
            return map;
        }

        #region
        private int? _iD;
        private int? _fK_Order_ID;
        private string _accountType;
        private string _payType;
        private string _payMan;
        private object _payDate;
        private string _sellerID;
        private int? _memberID;
        private string _memberCode;
        private string _memberName;
        private DateTime? _createTime;
        private DateTime? _checkDate;
        private int? _cWMemberID;
        private string _cWMemberCode;
        private string _cWMemberName;
        private string _remark;

        public int? ID
        {
            get { return _iD; }
            set { _iD = value; }
        }
        public int? FK_Order_ID
        {
            get { return _fK_Order_ID; }
            set { _fK_Order_ID = value; }
        }
        public string AccountType
        {
            get { return _accountType; }
            set { _accountType = value; }
        }
        public string PayType
        {
            get { return _payType; }
            set { _payType = value; }
        }
        public string PayMan
        {
            get { return _payMan; }
            set { _payMan = value; }
        }
        public object PayDate
        {
            get { return _payDate; }
            set { _payDate = value; }
        }
        public string SellerID
        {
            get { return _sellerID; }
            set { _sellerID = value; }
        }
        public int? MemberID
        {
            get { return _memberID; }
            set { _memberID = value; }
        }
        public string MemberCode
        {
            get { return _memberCode; }
            set { _memberCode = value; }
        }
        public string MemberName
        {
            get { return _memberName; }
            set { _memberName = value; }
        }
        public DateTime? CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        public DateTime? CheckDate
        {
            get { return _checkDate; }
            set { _checkDate = value; }
        }
        public int? CWMemberID
        {
            get { return _cWMemberID; }
            set { _cWMemberID = value; }
        }
        public string CWMemberCode
        {
            get { return _cWMemberCode; }
            set { _cWMemberCode = value; }
        }
        public string CWMemberName
        {
            get { return _cWMemberName; }
            set { _cWMemberName = value; }
        }
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        #endregion
    }
}