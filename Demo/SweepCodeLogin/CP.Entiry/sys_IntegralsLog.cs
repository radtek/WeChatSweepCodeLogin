using System;
using System.Collections.Generic;
using System.Text;
using CP.Common;
using CP.EntityBase;

namespace CP.Entiry
{
    public class sys_IntegralsLogAttr
    {
        public const string ID = "ID";
        public const string FK_UserID = "FK_UserID";
        public const string Source = "Source";
        public const string Integrals = "Integrals";
        public const string Type = "Type";
        public const string Remark = "Remark";
        public const string MemberID = "MemberID";
        public const string MemberCode = "MemberCode";
        public const string MemberName = "MemberName";
        public const string CreateTime = "CreateTime";

    }
    public class sys_IntegralsLog : CP.EntityBase.Entity
    {
        public sys_IntegralsLog()
        {
            base.ReadConnStrName = sysConst.ReadConnStrName;
            base.WriteConnStrName = sysConst.WriteConnStrName;
        }
        public const string PhysicsTable = "sys_IntegralsLog";
        /// <summary>
        /// 子类必须继承
        /// </summary>
        public override Map CreateEnMap()
        {
            Map map = new Map();
            map.PhysicsTable = "sys_IntegralsLog";
            map.AddTBStringID(sys_IntegralsLogAttr.ID, null, "标识列", 0, 50);
            map.AddTBString(sys_IntegralsLogAttr.FK_UserID, null, "用户主键", 0, 50);
            map.AddTBString(sys_IntegralsLogAttr.Source, null, "来源:1:账户注册;2:实名认证;3:每日首次登录;4:发布需求;5:充值;6:购买会员;7:预留专利;8:查询卖家信息;9:费用信息;10:法律状态", 0, 50);
            map.AddTBString(sys_IntegralsLogAttr.Integrals, null, "积分", 0, 50);
            map.AddTBString(sys_IntegralsLogAttr.Type, null, "类型：1:获取;2:消费", 0, 50);
            map.AddTBString(sys_IntegralsLogAttr.Remark, null, "备注", 0, 300);
            map.AddTBString(sys_IntegralsLogAttr.MemberID, null, "系统User表 ID", 0, 50);
            map.AddTBString(sys_IntegralsLogAttr.MemberCode, null, "账号", 0, 30);
            map.AddTBString(sys_IntegralsLogAttr.MemberName, null, "账号名称", 0, 100);
            map.AddTBString(sys_IntegralsLogAttr.CreateTime, null, "创建时间", 0, 50);
            return map;
        }

        #region
        private int? _iD;
        private int? _fK_UserID;
        private int? _source;
        private int? _integrals;
        private int? _type;
        private string _remark;
        private int? _memberID;
        private string _memberCode;
        private string _memberName;
        private DateTime? _createTime;

        public int? ID
        {
            get { return _iD; }
            set { _iD = value; }
        }
        public int? FK_UserID
        {
            get { return _fK_UserID; }
            set { _fK_UserID = value; }
        }
        public int? Source
        {
            get { return _source; }
            set { _source = value; }
        }
        public int? Integrals
        {
            get { return _integrals; }
            set { _integrals = value; }
        }
        public int? Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
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
        #endregion
    }
}