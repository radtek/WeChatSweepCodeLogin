using CP.Common;
using CP.EntityBase;

namespace CP.Entiry
{
    public class sys_StrategicEmergingIndustriesClassAttr
    {
        public const string ID = "ID";
        public const string Code = "Code";
        public const string Name = "Name";
        public const string PartentID = "PartentID";
        public const string IndustryCode = "IndustryCode";
        public const string IndustryName = "IndustryName";
        public const string IsPartBelong = "IsPartBelong";

    }
    public class sys_StrategicEmergingIndustriesClass : Entity
    {
        public sys_StrategicEmergingIndustriesClass()
        {
            base.ReadConnStrName = sysConst.ReadConnStrName;
            base.WriteConnStrName = sysConst.WriteConnStrName;
        }

        public const string PhysicsTable = "sys_StrategicEmergingIndustriesClass";

        /// <summary>
        /// 子类必须继承
        /// </summary>
        public override Map CreateEnMap()
        {
            Map map = new Map();
            map.PhysicsTable = "sys_StrategicEmergingIndustriesClass";
            map.AddTBStringID(sys_StrategicEmergingIndustriesClassAttr.ID, null, "标识列", 0, 50);
            map.AddTBString(sys_StrategicEmergingIndustriesClassAttr.Code, null, "代码", 0, 50);
            map.AddTBString(sys_StrategicEmergingIndustriesClassAttr.Name, null, "战略性新兴产业分类名称", 0, 255);
            map.AddTBString(sys_StrategicEmergingIndustriesClassAttr.PartentID, null, "父级ID", 0, 50);
            map.AddTBString(sys_StrategicEmergingIndustriesClassAttr.IndustryCode, null, "国民经济行业代码（2017）", 0, 50);
            map.AddTBString(sys_StrategicEmergingIndustriesClassAttr.IndustryName, null, "国民经济行业名称", 0, 255);
            map.AddTBString(sys_StrategicEmergingIndustriesClassAttr.IsPartBelong, null, "是否部分属于:0(否);1(是)", 0, 0);
            return map;
        }

        #region
        private int? _iD;
        private string _code;
        private string _name;
        private int? _partentID;
        private string _industryCode;
        private string _industryName;
        private char? _isPartBelong;

        public int? ID
        {
            get { return _iD; }
            set { _iD = value; }
        }
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int? PartentID
        {
            get { return _partentID; }
            set { _partentID = value; }
        }
        public string IndustryCode
        {
            get { return _industryCode; }
            set { _industryCode = value; }
        }
        public string IndustryName
        {
            get { return _industryName; }
            set { _industryName = value; }
        }
        public char? IsPartBelong
        {
            get { return _isPartBelong; }
            set { _isPartBelong = value; }
        }
        #endregion
    }
}