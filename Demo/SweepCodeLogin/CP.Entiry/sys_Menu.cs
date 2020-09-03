using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CP.Common;
using CP.EntityBase;

namespace CP.Entiry
{

    public class sys_MenuAttr {
        public const string ID = "ID";
        public const string MenuCode = "MenuCode";
        public const string MenuName = "MenuName";
        public const string ParentId = "ParentId";
        public const string LinkAddress = "LinkAddress";
        public const string PseStaticAddress = "PseStaticAddress";
        public const string Icon = "Icon";
        public const string Sort = "Sort";
        public const string MenuType = "MenuType";
        public const string IsFlag = "IsFlag";
        public const string CreateDate = "CreateDate";
        public const string MenuClass = "MenuClass";
        public const string IsCheck = "IsCheck";
    }


    public class sys_Menu : CP.EntityBase.Entity
    {
        public sys_Menu()
        {
            base.ReadConnStrName = sysConst.ReadConnStrName;
            base.WriteConnStrName = sysConst.WriteConnStrName;
        }
        public const string PhysicsTable = "sys_Menu";

        #region 属性
        private int? _ID;
        private string _MenuCode;
        private string _MenuName;
        private int? _ParentId;
        private string _LinkAddress;
        private string _PseStaticAddress;
        private string _Icon;
        private int? _Sort;
        private string _MenuType;
        private string _IsFlag;
        private DateTime? _CreateDate;
        private string _MenuClass;
        private int? _IsCheck;


        public int? ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string MenuCode
        {
            get { return _MenuCode; }
            set { _MenuCode = value; }
        }
        public string MenuName
        {
            get { return _MenuName; }
            set { _MenuName = value; }
        }
        public int? ParentId
        {
            get { return _ParentId; }
            set { _ParentId = value; }
        }
        public string LinkAddress
        {
            get { return _LinkAddress; }
            set { _LinkAddress = value; }
        }
        public string PseStaticAddress
        {
            get { return _PseStaticAddress; }
            set { _PseStaticAddress = value; }
        }
        public string Icon
        {
            get { return _Icon; }
            set { _Icon = value; }
        }
        public int? Sort
        {
            get { return _Sort; }
            set { _Sort = value; }
        }
        public string MenuType
        {
            get { return _MenuType; }
            set { _MenuType = value; }
        }
        public string IsFlag
        {
            get { return _IsFlag; }
            set { _IsFlag = value; }
        }
        public DateTime? CreateDate
        {
            get { return _CreateDate; }
            set { _CreateDate = value; }
        }
        public string MenuClass
        {
            get { return _MenuClass; }
            set { _MenuClass = value; }
        }
        public int? IsCheck
        {
            get { return _IsCheck; }
            set { _IsCheck = value; }
        }  


        #endregion

        #region 方法
        /// <summary>
        /// 子类必须继承
        /// </summary>
        public override Map CreateEnMap()
        {
            Map map = new Map();
            map.PhysicsTable = PhysicsTable;
            map.AddTBStringID(sys_MenuAttr.ID, null, "标识列", 0, 50);
            map.AddTBString(sys_MenuAttr.MenuCode, null, "菜单编号", 0, 50);
            map.AddTBString(sys_MenuAttr.MenuName, null, "菜单名称", 0, 50);
            map.AddTBString(sys_MenuAttr.ParentId, null, "", 0, 50);
            map.AddTBString(sys_MenuAttr.LinkAddress, null, "链接地址", 0, 50);
            map.AddTBString(sys_MenuAttr.PseStaticAddress, null, "伪静态地址", 0, 50);
            map.AddTBString(sys_MenuAttr.Icon, null, "图标", 0, 25);
            map.AddTBString(sys_MenuAttr.Sort, null, "排序", 0, 50);
            map.AddTBString(sys_MenuAttr.MenuType, null, "菜单类型 0前台菜单 1后台菜单", 0, 10);
            map.AddTBString(sys_MenuAttr.IsFlag, null, "是否启用 Y(启用)/N(停用)", 0, 1);
            map.AddTBString(sys_MenuAttr.CreateDate, null, "创建时间", 0, 50);
            map.AddTBString(sys_MenuAttr.MenuClass, null, "菜单样式", 0, 50);
            map.AddTBString(sys_MenuAttr.IsCheck, null, "是否需要认证", 0, 50);
            return map;
        }
        #endregion
    }
}
