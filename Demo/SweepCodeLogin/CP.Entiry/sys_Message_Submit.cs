using System;
using System.Collections.Generic;
using System.Text;
using CP.Common;
using CP.EntityBase;

namespace CP.Entiry
{
    public class sys_Message_SubmitAttr
    {
        public const string ID = "ID";
        public const string FK_Funding_ID = "FK_Funding_ID";
        public const string Type = "Type";
        public const string State = "State";
        public const string CreateTime = "CreateTime";
        public const string MemberID = "MemberID";
        public const string MemberCode = "MemberCode";
        public const string MemberName = "MemberName";

    }
    public class sys_Message_Submit : CP.EntityBase.Entity
    {
        public sys_Message_Submit()
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
            map.PhysicsTable = "sys_Message_Submit";
            map.AddTBStringID(sys_Message_SubmitAttr.ID, null, "标识列", 0, 50);
            map.AddTBString(sys_Message_SubmitAttr.FK_Funding_ID, null, "审核的案件ID", 0, 50);
            map.AddTBString(sys_Message_SubmitAttr.Type, null, "审核的资助类别", 0, 10);
            map.AddTBString(sys_Message_SubmitAttr.State, null, "审核状态", 0, 20);
            map.AddTBString(sys_Message_SubmitAttr.CreateTime, null, "创建时间", 0, 50);
            map.AddTBString(sys_Message_SubmitAttr.MemberID, null, "", 0, 50);
            map.AddTBString(sys_Message_SubmitAttr.MemberCode, null, "", 0, 50);
            map.AddTBString(sys_Message_SubmitAttr.MemberName, null, "", 0, 100);
            return map;
        }

        #region
        private int? _iD;
        private int? _fK_Funding_ID;
        private string _type;
        private string _state;
        private DateTime? _createTime;
        private int? _memberID;
        private string _memberCode;
        private string _memberName;

        public int? ID
        {
            get { return _iD; }
            set { _iD = value; }
        }
        public int? FK_Funding_ID
        {
            get { return _fK_Funding_ID; }
            set { _fK_Funding_ID = value; }
        }
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }
        public DateTime? CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
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
        #endregion
    }
}
