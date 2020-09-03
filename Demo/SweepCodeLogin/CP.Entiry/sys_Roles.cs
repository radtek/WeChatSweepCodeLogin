using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CP.Common;
using CP.EntityBase;

namespace CP.Entiry
{
    public class sys_RolesAttr
    {
        public const string ID = "ID";
        public const string RoleCode = "RoleCode";
        public const string RoleName = "RoleName";
        public const string IsFlag = "IsFlag";
    }
    public class sys_Roles :CP.EntityBase.Entity
    {
        public sys_Roles()
        {
            base.ReadConnStrName = sysConst.ReadConnStrName;
            base.WriteConnStrName = sysConst.WriteConnStrName;
        }

        private int? iD;
        public int? ID
        {
            get { return iD; }
            set { iD = value; }
        }

        private string roleCode;
        public string RoleCode
        {
            get { return roleCode; }
            set { roleCode = value; }
        }

        private string roleName;
        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        private string isFlag;
        public string IsFlag
        {
            get { return isFlag; }
            set { isFlag = value; }
        }

        /// <summary>
        /// 子类必须继承
        /// </summary>
        public override Map CreateEnMap()
        {
            Map map = new Map();
            map.PhysicsTable = "sys_Roles";
            map.AddTBStringID(sys_RolesAttr.ID, null, "标识列", 0, 50);
            map.AddTBString(sys_RolesAttr.RoleCode, null, "", 0, 20);
            map.AddTBString(sys_RolesAttr.RoleName, null, "", 0, 50);
            map.AddTBString(sys_RolesAttr.IsFlag, null, "是否启用 Y(启用)/N(停用)", 0, 1);
            return map;
        }
    }

}
