using System;
using System.Collections.Generic;
using System.Text;
using CP.Common;
using CP.EntityBase;

namespace CP.Entiry
{
    public class sys_DictionariesAttr
    {
        public const string ID = "ID";
        public const string PartentID = "PartentID";
        public const string Code = "Code";
        public const string Text = "Text";
        public const string Type = "Type";
        public const string Remark = "Remark";

    }
    public class sys_Dictionaries : CP.EntityBase.Entity
    {
        public sys_Dictionaries()
        {
            base.ReadConnStrName = sysConst.ReadConnStrName;
            base.WriteConnStrName = sysConst.WriteConnStrName;
        }

        public const string PhysicsTable = "sys_Dictionaries";

        /// <summary>
        /// 子类必须继承
        /// </summary>
        public override Map CreateEnMap()
        {
            Map map = new Map();
            map.PhysicsTable = "sys_Dictionaries";
            map.AddTBStringID(sys_DictionariesAttr.ID, null, "标识列", 0, 50);
            map.AddTBString(sys_DictionariesAttr.PartentID, null, "父级ID", 0, 50);
            map.AddTBString(sys_DictionariesAttr.Code, null, "编号", 0, 50);
            map.AddTBString(sys_DictionariesAttr.Text, null, "内容", 0, 50);
            map.AddTBString(sys_DictionariesAttr.Type, null, "类别", 0, 50);
            map.AddTBString(sys_DictionariesAttr.Remark, null, "备注", 0, 500);
            return map;
        }

        #region
        private int? _iD;
        private int? _partentID;
        private string _code;
        private string _text;
        private string _type;
        private string _remark;

        public int? ID
        {
            get { return _iD; }
            set { _iD = value; }
        }
        public int? PartentID
        {
            get { return _partentID; }
            set { _partentID = value; }
        }
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        #endregion
    }
}
