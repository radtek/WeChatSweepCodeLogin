using System;
using System.Collections.Generic;
using System.Text;
using CP.Common;
using CP.EntityBase;

namespace CP.Entiry
{
    public class sys_DistrictAttr
    {
        public const string ID = "ID";
        public const string DistrictID = "DistrictID";
        public const string DistrictName = "DistrictName";
        public const string CityID = "CityID";
        public const string DateCreated = "DateCreated";
        public const string DateUpdated = "DateUpdated";

    }
    public class sys_District : CP.EntityBase.Entity
    {
        public sys_District()
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
            map.PhysicsTable = "sys_District";
            map.AddTBStringID(sys_DistrictAttr.ID, null, "标识列", 0, 50);
            map.AddTBString(sys_DistrictAttr.DistrictID, null, "", 0, 50);
            map.AddTBString(sys_DistrictAttr.DistrictName, null, "地区名称", 0, 50);
            map.AddTBString(sys_DistrictAttr.CityID, null, "所属城市", 0, 50);
            map.AddTBString(sys_DistrictAttr.DateCreated, null, "创建日期", 0, 50);
            map.AddTBString(sys_DistrictAttr.DateUpdated, null, "修改日期", 0, 50);
            return map;
        }

        #region
        private int? _iD;
        private int? _districtID;
        private string _districtName;
        private int? _cityID;
        private DateTime? _dateCreated;
        private DateTime? _dateUpdated;

        public int? ID
        {
            get { return _iD; }
            set { _iD = value; }
        }
        public int? DistrictID
        {
            get { return _districtID; }
            set { _districtID = value; }
        }
        public string DistrictName
        {
            get { return _districtName; }
            set { _districtName = value; }
        }
        public int? CityID
        {
            get { return _cityID; }
            set { _cityID = value; }
        }
        public DateTime? DateCreated
        {
            get { return _dateCreated; }
            set { _dateCreated = value; }
        }
        public DateTime? DateUpdated
        {
            get { return _dateUpdated; }
            set { _dateUpdated = value; }
        }
        #endregion
    }
}
