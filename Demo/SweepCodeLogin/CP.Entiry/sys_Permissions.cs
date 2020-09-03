using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CP.Common;
using CP.EntityBase;

namespace CP.Entiry
{

    public class sys_Permissions : CP.EntityBase.Entity
    {
        public sys_Permissions()
        {
            base.ReadConnStrName = sysConst.ReadConnStrName;
            base.WriteConnStrName = sysConst.WriteConnStrName;
        }

        
        private int? iD;
        public int?  ID
        {
            get { return iD; }
            set { iD = value; }
        }


        private int? fK_Role_ID;
        public int? FK_Role_ID
        {
            get { return fK_Role_ID; }
            set { fK_Role_ID = value; }
        } 
        private int? fK_Menu_ID;
        public int? FK_Menu_ID
        {
            get { return fK_Menu_ID; }
            set { fK_Menu_ID = value; }
        }
        private int? fK_MenuButton_ID;
        public int? FK_MenuButton_ID
        {
            get { return fK_MenuButton_ID; }
            set { fK_MenuButton_ID = value; }
        }
        private bool permission;
        public bool Permission
        {
            get { return permission; }
            set { permission = value; }
        }
        /// <summary>
        /// 子类必须继承
        /// </summary>
        public override Map CreateEnMap()
        {
            Map map = new Map();
            map.PhysicsTable = "sys_Permissions";
            map.AddTBStringID("ID", null, "标识列", 0, 50);
            map.AddTBString("FK_Role_ID", null, "角色ID", 0, 50);
            map.AddTBString("FK_Menu_ID", null, "菜单ID", 0, 50); 
            map.AddTBString("FK_MenuButton_ID", null, "按钮ID", 0, 50);
            map.AddTBString("Permission", null, "权限", 0, 50);
            return map;
        }
    }
}