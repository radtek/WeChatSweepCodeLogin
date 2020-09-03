using System;
using System.Collections.Generic;
using System.Text;
using CP.Common;
using CP.EntityBase;

namespace CP.Entiry
{
    public class sys_CityAttr
    {
        public const string ID = "ID";
        public const string CityID = "CityID";
        public const string CityName = "CityName";
        public const string ZipCode = "ZipCode";
        public const string ProvinceID = "ProvinceID";
        public const string DateCreated = "DateCreated";
        public const string DateUpdated = "DateUpdated";

    }
    public class sys_City : CP.EntityBase.Entity
    {
        public sys_City()
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
            map.PhysicsTable = "sys_City";
            map.AddTBStringID(sys_CityAttr.ID, null, "标识列", 0, 50);
            map.AddTBString(sys_CityAttr.CityID, null, "", 0, 50);
            map.AddTBString(sys_CityAttr.CityName, null, "城市名称", 0, 50);
            map.AddTBString(sys_CityAttr.ZipCode, null, "邮编", 0, 50);
            map.AddTBString(sys_CityAttr.ProvinceID, null, "所属省", 0, 50);
            map.AddTBString(sys_CityAttr.DateCreated, null, "创建日期", 0, 50);
            map.AddTBString(sys_CityAttr.DateUpdated, null, "修改日期", 0, 50);
            return map;
        }

        #region
        private int? _iD;
        private int? _cityID;
        private string _cityName;
        private string _zipCode;
        private int? _provinceID;
        private DateTime? _dateCreated;
        private DateTime? _dateUpdated;

        public int? ID
        {
            get { return _iD; }
            set { _iD = value; }
        }
        public int? CityID
        {
            get { return _cityID; }
            set { _cityID = value; }
        }
        public string CityName
        {
            get { return _cityName; }
            set { _cityName = value; }
        }
        public string ZipCode
        {
            get { return _zipCode; }
            set { _zipCode = value; }
        }
        public int? ProvinceID
        {
            get { return _provinceID; }
            set { _provinceID = value; }
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
