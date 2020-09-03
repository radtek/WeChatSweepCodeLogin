using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CP.Common;
using CP.EntityBase;

namespace CP.Entiry
{
    public class sys_RunParameterAttr {
        public const string ID = "ID";
        public const string Key = "[Key]";
        public const string Value = "Value";
        public const string DataType = "DataType";
        public const string Remark = "Remark";
    }
    public class sys_RunParameter : CP.EntityBase.Entity
    {

        /// <summary>
        /// 子类必须继承
        /// </summary>
        public override Map CreateEnMap()
        {
            Map map = new Map();
            map.PhysicsTable = "sys_RunParameter";
            map.AddTBStringID(sys_RunParameterAttr.ID, null, "标识列", 0, 50);
            map.AddTBString("Key", null, "键", 0, 50);
            map.AddTBString(sys_RunParameterAttr.Value, null, "值", 0, 50);
            map.AddTBString(sys_RunParameterAttr.DataType, null, "数据类型", 0, 50);            
            map.AddTBString(sys_RunParameterAttr.Remark, null, "备注", 0, 50);
            return map;
        }



        public sys_RunParameter()
        {
            base.ReadConnStrName = sysConst.ReadConnStrName;
            base.WriteConnStrName = sysConst.WriteConnStrName;
        }

        private int? _ID;
        private string _Key;
        private string _Value;
        private string _DataType;
        private string _ApplicationName;
        private string _Remark;


        public int? ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        public string DataType
        {
            get { return _DataType; }
            set { _DataType = value; }
        }
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }

 
    }
}
