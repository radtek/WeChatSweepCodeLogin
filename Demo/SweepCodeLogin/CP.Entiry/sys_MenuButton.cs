
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CP.Common;
using CP.EntityBase;

namespace CP.Entiry
{
    public class sys_MenuButtonAttr {
        public const string ID = "ID";
        public const string FK_Menu_ID = "FK_Menu_ID";
        public const string ButtonId = "ButtonId";
        public const string ButtonCode = "ButtonCode";
        public const string ButtonName = "ButtonName";
        public const string ButtonColor = "ButtonColor";
        public const string JsFunction = "JsFunction";
        public const string Icon = "Icon";
        public const string Description = "Description";
        public const string IsFlag = "IsFlag";
        public const string Sort = "Sort";
        public const string Action = "Action";
        public const string Tag = "Tag";
    
    }
    public class sys_MenuButton:CP.EntityBase.Entity
    {

        public sys_MenuButton()
        {
            base.ReadConnStrName = sysConst.ReadConnStrName;
            base.WriteConnStrName = sysConst.WriteConnStrName;
        }
        public const string PhysicsTable = "sys_MenuButton";

        #region 属性
        private int? _ID;
        private int? _FK_Menu_ID;
        private string _ButtonId;
        private string _ButtonCode;
        private string _ButtonName;
        private string _ButtonColor;
        private string _JsFunction;
        private string _Icon;
        private string _Description;
        private string _IsFlag;
        private int? _Sort;
        private string _Action;
        private string _Tag;
         

       
        public int? ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public int? FK_Menu_ID
        {
            get { return _FK_Menu_ID; }
            set { _FK_Menu_ID = value; }
        }
        public string ButtonId
        {
            get { return _ButtonId; }
            set { _ButtonId = value; }
        }
        public string ButtonCode
        {
            get { return _ButtonCode; }
            set { _ButtonCode = value; }
        }
        public string ButtonName
        {
            get { return _ButtonName; }
            set { _ButtonName = value; }
        }
        public string ButtonColor
        {
            get { return _ButtonColor; }
            set { _ButtonColor = value; }
        }
        public string JsFunction
        {
            get { return _JsFunction; }
            set { _JsFunction = value; }
        }
        public string Icon
        {
            get { return _Icon; }
            set { _Icon = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string IsFlag
        {
            get { return _IsFlag; }
            set { _IsFlag = value; }
        }
        public int? Sort
        {
            get { return _Sort; }
            set { _Sort = value; }
        }
        public string Action
        {
            get { return _Action; }
            set { _Action = value; }
        }
        public string Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }       
        #endregion
         
        /// <summary>
        /// 子类必须继承
        /// </summary>
        public override Map CreateEnMap()
        {
            Map map = new Map();
            map.PhysicsTable = PhysicsTable;
            map.AddTBStringID(sys_MenuButtonAttr.ID, null, "标识列", 0, 50);
            map.AddTBString(sys_MenuButtonAttr.FK_Menu_ID, null, "", 0, 50);
            map.AddTBString(sys_MenuButtonAttr.ButtonId, null, "", 0, 50);
            map.AddTBString(sys_MenuButtonAttr.ButtonCode, null, "", 0, 50);
            map.AddTBString(sys_MenuButtonAttr.ButtonName, null, "", 0, 50);
            map.AddTBString(sys_MenuButtonAttr.ButtonColor, null, "", 0, 50);
            map.AddTBString(sys_MenuButtonAttr.JsFunction, null, "", 0, 50);
            map.AddTBString(sys_MenuButtonAttr.Icon, null, "", 0, 50);
            map.AddTBString(sys_MenuButtonAttr.Description, null, "", 0, 150);
            map.AddTBString(sys_MenuButtonAttr.IsFlag, null, "是否启用 Y(启用)/N(停用)", 0, 1);
            map.AddTBString(sys_MenuButtonAttr.Sort, null, "", 0, 50);
            map.AddTBString(sys_MenuButtonAttr.Action, null, "按钮调用函数Action名称", 0, 50);
            map.AddTBString(sys_MenuButtonAttr.Tag, null, "html控件标签   <a> <button>", 0, 50);
            return map;
        } 
    }
}
