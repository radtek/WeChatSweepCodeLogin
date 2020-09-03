using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CP.EntityBase
{
    public class Map
    {
        #region 属性
        /// <summary>
        /// 标识列
        /// </summary>
        private string _IDENTITY;
        public string IDENTITY { get { return _IDENTITY; } }

        /// <summary>
        /// 主键列表
        /// </summary>
        List<string> _PrimaryKey = new List<string>();
        public List<string> PrimaryKey { get { return _PrimaryKey; } }

        /// <summary>
        /// 物理表名称
        /// </summary>
        public string PhysicsTable { get; set; }

        /// <summary>
        /// 实体类说明
        /// </summary>
        public string EnDesc { get; set; }
          
        /// <summary>
        /// 是否物理删除数据
        /// </summary>
        private string _DeleteFeild = "IsDelete"; 
        /// <summary>
        /// 是否非物理删除 修改字段
        /// </summary>
        public string DeleteFeild { get { return _DeleteFeild; } set { _DeleteFeild = value; } }


        private Attrs _attrs = new Attrs(); 
        public Attrs Attrs
        {
          get { return _attrs; }
          set { _attrs = value; }
        }
        #endregion

        #region  AddTBStringPK
        public void AddTBStringPK(string key, string defaultVal, string desc, int minLength, int maxLength)
        {
            PrimaryKey.Add(key);
            AddTBString(key, key, defaultVal, FieldType.PK, desc, minLength, maxLength);
        }
        public void AddTBStringPK(string key, string field, object defaultVal, string desc, bool isReadonly, int minLength, int maxLength )
        {
            PrimaryKey.Add(key);
            AddTBString(key, field, defaultVal, FieldType.PK, desc, minLength, maxLength);
        }
        #endregion
         
        #region AddTBStringID

        public void AddTBStringID(string key, string defaultVal, string desc, int minLength, int maxLength)
        {
            AddTBStringID(key, key, defaultVal, desc, minLength, maxLength);
        }

        public void AddTBStringID(string key, string field, string defaultVal, string desc, int minLength, int maxLength)
        {
            if (!string.IsNullOrEmpty(_IDENTITY))
            {
                if (!Equals(key, this.IDENTITY)) 
                    throw new Exception("实体类配置错误, 重复配置标识列字段");
            }

            this._IDENTITY = key;
            AddTBString(key, field, defaultVal, FieldType.IDENTITY, desc, minLength, maxLength);
        }

        #endregion 

        #region AddTBString

        public void AddTBString(string key, object defaultVal, string desc, int minLength, int maxLength)
        {
            AddTBString(key, key, defaultVal, FieldType.Normal, desc, minLength, maxLength);
        }

        public void AddTBString(string key, string field, object defaultVal, FieldType _FieldType, string desc, int minLength, int maxLength)
        {
            Attr attr = new Attr();
            attr.Key = key;

            attr.Field = field;
            attr.DefaultVal = defaultVal;
           
            attr.Desc = desc; 
            attr.MaxLength = maxLength;
            attr.MinLength = minLength;
            attr.MyFieldType = _FieldType;
           
            this.Attrs.Add(attr);
        }
        #endregion  
    }
}
