using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CP.Common;
using CP.EntityBase;


namespace CP.Entiry
{
    class sys_UserRole:CP.EntityBase.Entity
    {
        public sys_UserRole()
        {
            base.ReadConnStrName = sysConst.ReadConnStrName;
            base.WriteConnStrName = sysConst.WriteConnStrName;
        }



        private int? FK_user_ID;
        public int? FK_User_ID
        {
            get { return FK_user_ID; }
            set { FK_user_ID = value; }
        }

        private int? FK_role_ID;
        public int? FK_Role_ID
        {
            get { return FK_role_ID; }
            set { FK_role_ID = value; }
        }

      

        /// <summary>
        /// 子类必须继承
        /// </summary>
        public override Map CreateEnMap()
        {
            Map map = new Map();
            map.PhysicsTable = "sys_UserRole";

            map.AddTBString("FK_User_ID", null, "用户ID", 0, 50);
            map.AddTBString("FK_Role_ID", null, "角色ID", 0, 50);
        
            return map;
        }
    }

}

