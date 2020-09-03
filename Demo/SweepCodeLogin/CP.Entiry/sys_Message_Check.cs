using System;
using System.Collections.Generic;
using System.Text;
using CP.Common;
using CP.EntityBase;

namespace CP.Entiry
{
    public class sys_Message_CheckAttr
    {
        public const string ID = "ID";
        public const string FK_Funding_ID = "FK_Funding_ID";
        public const string Type = "Type";
        public const string Summary = "Summary";
        public const string State = "State";
        public const string CreateTime = "CreateTime";
        public const string CheckCMID = "CheckCMID";
        public const string CheckCMCode = "CheckCMCode";
        public const string CheckCMName = "CheckCMName";

    }
    public class sys_Message_Check : CP.EntityBase.Entity
    {
        public sys_Message_Check()
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
            map.PhysicsTable = "sys_Message_Check";
            map.AddTBStringID(sys_Message_CheckAttr.ID, null, "标识列", 0, 50);
            map.AddTBString(sys_Message_CheckAttr.FK_Funding_ID, null, "审核的案件ID", 0, 50);
            map.AddTBString(sys_Message_CheckAttr.Type, null, "审核的资助类别", 0, 10);
            map.AddTBString(sys_Message_CheckAttr.Summary, null, "拒绝、驳回原因", 0, 50);
            map.AddTBString(sys_Message_CheckAttr.State, null, "审核状态", 0, 20);
            map.AddTBString(sys_Message_CheckAttr.CreateTime, null, "", 0, 50);
            map.AddTBString(sys_Message_CheckAttr.CheckCMID, null, "", 0, 50);
            map.AddTBString(sys_Message_CheckAttr.CheckCMCode, null, "", 0, 50);
            map.AddTBString(sys_Message_CheckAttr.CheckCMName, null, "", 0, 50);
            return map;
        }

        #region
        private int? _iD;
        private int? _fK_Funding_ID;
        private string _type;
        private string _summary;
        private string _state;
        private DateTime? _createTime;
        private int? _checkCMID;
        private string _checkCMCode;
        private string _checkCMName;

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
        public string Summary
        {
            get { return _summary; }
            set { _summary = value; }
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
        public int? CheckCMID
        {
            get { return _checkCMID; }
            set { _checkCMID = value; }
        }
        public string CheckCMCode
        {
            get { return _checkCMCode; }
            set { _checkCMCode = value; }
        }
        public string CheckCMName
        {
            get { return _checkCMName; }
            set { _checkCMName = value; }
        }
        #endregion
    }
}