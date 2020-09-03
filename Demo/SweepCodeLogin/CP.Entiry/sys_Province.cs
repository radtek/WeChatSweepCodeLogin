using System;
using System.Collections.Generic;
using System.Text;
using CP.Common;
using CP.EntityBase;

namespace CP.Entiry //�޸����ֿռ�
{
	public class sys_Province : CP.EntityBase.Entity
	{
        public sys_Province()
        {
            base.ReadConnStrName = sysConst.ReadConnStrName;
            base.WriteConnStrName = sysConst.WriteConnStrName;
        }   
        /// <summary>
        /// �������̳�
        /// </summary>
        public override Map CreateEnMap()
        {
            Map map = new Map();
            map.PhysicsTable = "sys_Province";
            map.AddTBStringID("ProvinceID", null, "��ʶ��", 0, 50);
            map.AddTBString("ProvinceName", null, "��ť���", 0, 50);
            map.AddTBString("DateUpdated", null, "��ť����", 0, 50);
            return map;
        }


        private string provinceID;
        public string ProvinceID
		{
			get { return provinceID; }
			set { provinceID = value; }
		}
	
		private string provinceName;
		public string ProvinceName
		{
			get { return provinceName; }
			set { provinceName = value; }
		}
	
		private DateTime? dateCreated;
		public DateTime? DateCreated
		{
			get { return dateCreated; }
			set { dateCreated = value; }
		}
	
		private DateTime? dateUpdated;
		public DateTime? DateUpdated
		{
			get { return dateUpdated; }
			set { dateUpdated = value; }
		}
	}
}