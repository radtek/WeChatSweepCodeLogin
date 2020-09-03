using System;
using System.Collections.Generic;
using System.Text;
using CP.Common;
using CP.EntityBase;

namespace CP.Entiry
{
    public class sys_MessageAttr
    {
        public const string ID = "ID";
        public const string FK_User_ID = "FK_User_ID"; 
        public const string IsRead = "IsRead";
        public const string Title = "Title";
        public const string State = "State";
        public const string Modular = "Modular";
        public const string Url = "Url";
        public const string CreateTime = "CreateTime";
        public const string Sender_ID = "Sender_ID";
        public const string Sender_Code = "Sender_Code";
        public const string Sender_Name = "Sender_Name";

    }
    public class sys_Message : CP.EntityBase.Entity
    {
        public sys_Message()
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
            map.PhysicsTable = "sys_Message";
            map.AddTBStringID(sys_MessageAttr.ID, null, "标识列", 0, 50); 
            map.AddTBString(sys_MessageAttr.FK_User_ID, null, "代办负责人ID", 0, 50);
            map.AddTBString(sys_MessageAttr.IsRead, null, "是否已读 0未读 1已读", 0, 50);
            map.AddTBString(sys_MessageAttr.Title, null, "标题", 0, 50);
            map.AddTBString(sys_MessageAttr.State, null, "状态", 0, 10);
            map.AddTBString(sys_MessageAttr.Modular, null, "模块", 0, 10);
            map.AddTBString(sys_MessageAttr.Url, null, "跳转地址", 0, 50);
            map.AddTBString(sys_MessageAttr.CreateTime, null, "消息时间", 0, 50);
            map.AddTBString(sys_MessageAttr.Sender_ID, null, "发送人ID", 0, 50);
            map.AddTBString(sys_MessageAttr.Sender_Code, null, "发送人账号", 0, 50);
            map.AddTBString(sys_MessageAttr.Sender_Name, null, "发送人名称", 0, 50);
            return map;
        }

        #region
        private int? _iD;
        private int? _fK_User_ID; 
        private string _isRead;
        private string _title;
        private string _state;
        private string _modular;
        private string _url;
        private DateTime? _createTime;
        private int? _sender_ID;
        private string _sender_Code;
        private string _sender_Name;

        public int? ID
        {
            get { return _iD; }
            set { _iD = value; }
        }
        public int? FK_User_ID
        {
            get { return _fK_User_ID; }
            set { _fK_User_ID = value; }
        }
        public string IsRead
        {
            get { return _isRead; }
            set { _isRead = value; }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }
        public string Modular
        {
            get { return _modular; }
            set { _modular = value; }
        }
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        public DateTime? CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        public int? Sender_ID
        {
            get { return _sender_ID; }
            set { _sender_ID = value; }
        }
        public string Sender_Code
        {
            get { return _sender_Code; }
            set { _sender_Code = value; }
        }
        public string Sender_Name
        {
            get { return _sender_Name; }
            set { _sender_Name = value; }
        }
        #endregion
    }
}
