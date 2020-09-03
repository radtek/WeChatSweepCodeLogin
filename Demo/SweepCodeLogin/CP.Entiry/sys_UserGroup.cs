using System;
using System.Collections.Generic;
using System.Text;
using CP.Common;
using CP.EntityBase;

namespace CP.Entiry
{
    public class sys_UserGroupAttr
    {
        public const string FK_User_ID = "FK_User_ID";
        public const string FK_Group_ID = "FK_Group_ID";

    }
    public class sys_UserGroup : CP.EntityBase.Entity
    {
        public sys_UserGroup()
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
            map.PhysicsTable = "sys_UserGroup";

            map.AddTBString(sys_UserGroupAttr.FK_User_ID, null, "用户ID", 0, 50);
            map.AddTBString(sys_UserGroupAttr.FK_Group_ID, null, "业务组ID", 0, 50);
            return map;
        }

        #region
        private int? _fK_User_ID;
        private int? _fK_Group_ID;

        public int? FK_User_ID
        {
            get { return _fK_User_ID; }
            set { _fK_User_ID = value; }
        }
        public int? FK_Group_ID
        {
            get { return _fK_Group_ID; }
            set { _fK_Group_ID = value; }
        }
        #endregion
    }
}
