using System;
using System.Collections.Generic;
using System.Text;
using CP.Common;
using CP.EntityBase;

namespace CP.Entiry
{
    public class sys_MemberIntegralsAttr
    {
        public const string ID = "ID";
        public const string ServiceType = "ServiceType";
        public const string Sort = "Sort";
        public const string Price = "Price";
        public const string Integrals = "Integrals";
        public const string GiveIntegrals = "GiveIntegrals";
        public const string IsVIP = "IsVIP";
        public const string UploadExplain = "UploadExplain";
        public const string CreateTime = "CreateTime";
        public const string MemberID = "MemberID";
        public const string MemberCode = "MemberCode";
        public const string MemberName = "MemberName";
        public const string IsDelete = "IsDelete";

    }
    public class sys_MemberIntegrals : CP.EntityBase.Entity
    {
        public sys_MemberIntegrals()
        {
            base.ReadConnStrName = sysConst.ReadConnStrName;
            base.WriteConnStrName = sysConst.WriteConnStrName;
        }
        public const string PhysicsTable = "sys_MemberIntegrals";
        /// <summary>
        /// 子类必须继承
        /// </summary>
        public override Map CreateEnMap()
        {
            Map map = new Map();
            map.PhysicsTable = "sys_MemberIntegrals";
            map.AddTBStringID(sys_MemberIntegralsAttr.ID, null, "标识列", 0, 50);
            map.AddTBString(sys_MemberIntegralsAttr.ServiceType, null, "服务类型", 0, 50);
            map.AddTBString(sys_MemberIntegralsAttr.Sort, null, "排序", 0, 50);
            map.AddTBString(sys_MemberIntegralsAttr.Price, null, "价格/元", 0, 50);
            map.AddTBString(sys_MemberIntegralsAttr.Integrals, null, "充值积分", 0, 50);
            map.AddTBString(sys_MemberIntegralsAttr.GiveIntegrals, null, "赠送积分", 0, 50);
            map.AddTBString(sys_MemberIntegralsAttr.IsVIP, null, "是否VIP(Y:是;N:否)", 0, 0);
            map.AddTBString(sys_MemberIntegralsAttr.UploadExplain, null, "上传说明", 0, 500);
            map.AddTBString(sys_MemberIntegralsAttr.CreateTime, null, "创建时间", 0, 50);
            map.AddTBString(sys_MemberIntegralsAttr.MemberID, null, "系统User表 ID", 0, 50);
            map.AddTBString(sys_MemberIntegralsAttr.MemberCode, null, "账号", 0, 50);
            map.AddTBString(sys_MemberIntegralsAttr.MemberName, null, "账号名称", 0, 100);
            map.AddTBString(sys_MemberIntegralsAttr.IsDelete, null, "是否删除（Y:删除；N:不删除）", 0, 0);
            return map;
        }

        #region
        private int? _iD;
        private string _serviceType;
        private int? _sort;
        private double? _price;
        private int? _integrals;
        private int? _giveIntegrals;
        private char? _isVIP;
        private string _uploadExplain;
        private DateTime? _createTime;
        private int? _memberID;
        private string _memberCode;
        private string _memberName;
        private char? _isDelete;

        public int? ID
        {
            get { return _iD; }
            set { _iD = value; }
        }
        public string ServiceType
        {
            get { return _serviceType; }
            set { _serviceType = value; }
        }
        public int? Sort
        {
            get { return _sort; }
            set { _sort = value; }
        }
        public double? Price
        {
            get { return _price; }
            set { _price = value; }
        }
        public int? Integrals
        {
            get { return _integrals; }
            set { _integrals = value; }
        }
        public int? GiveIntegrals
        {
            get { return _giveIntegrals; }
            set { _giveIntegrals = value; }
        }
        public char? IsVIP
        {
            get { return _isVIP; }
            set { _isVIP = value; }
        }
        public string UploadExplain
        {
            get { return _uploadExplain; }
            set { _uploadExplain = value; }
        }
        public DateTime? CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        public int? MemberID
        {
            get { return _memberID; }
            set { _memberID = value; }
        }
        public string MemberCode
        {
            get { return _memberCode; }
            set { _memberCode = value; }
        }
        public string MemberName
        {
            get { return _memberName; }
            set { _memberName = value; }
        }
        public char? IsDelete
        {
            get { return _isDelete; }
            set { _isDelete = value; }
        }
        #endregion
    }
}