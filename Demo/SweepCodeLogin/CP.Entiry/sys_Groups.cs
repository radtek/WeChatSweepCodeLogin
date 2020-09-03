using System;
using System.Collections.Generic;
using System.Text;
using CP.Common;
using CP.EntityBase;

namespace CP.Entiry
{
    public class sys_GroupsAttr
    {
        public const string ID = "ID";
        public const string GroupCode = "GroupCode";
        public const string GroupName = "GroupName";
        public const string IsFlag = "IsFlag";
        public const string Remark = "Remark";

    }
    public class sys_Groups : CP.EntityBase.Entity
    {
        public sys_Groups()
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
            map.PhysicsTable = "sys_Groups";
            map.AddTBStringID(sys_GroupsAttr.ID, null, "标识列", 0, 50);
            map.AddTBString(sys_GroupsAttr.GroupCode, null, "", 0, 20);
            map.AddTBString(sys_GroupsAttr.GroupName, null, "", 0, 50);
            map.AddTBString(sys_GroupsAttr.IsFlag, null, "是否启用 Y(启用)/N(停用)", 0, 1);
            map.AddTBString(sys_GroupsAttr.Remark, null, "备注", 0, 200);
            return map;
        }

        #region
        private int? _iD;
        private string _groupCode;
        private string _groupName;
        private string _isFlag;
        private string _remark;

        public int? ID
        {
            get { return _iD; }
            set { _iD = value; }
        }
        public string GroupCode
        {
            get { return _groupCode; }
            set { _groupCode = value; }
        }
        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }
        public string IsFlag
        {
            get { return _isFlag; }
            set { _isFlag = value; }
        }
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        #endregion
    }
}
