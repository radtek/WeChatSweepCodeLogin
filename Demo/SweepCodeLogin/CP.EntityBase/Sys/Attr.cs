using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace CP.EntityBase 
{
    /// <summary>
    /// 编辑类型
    /// </summary>
    public enum EditType
    {
        /// <summary>
        /// 可编辑
        /// </summary>
        Edit,
        /// <summary>
        /// 不可删除
        /// </summary>
        UnDel,
        /// <summary>
        /// 只读,不可删除。
        /// </summary>
        Readonly
    }
    /// <summary>
    /// 自动填充方式
    /// </summary>
    public enum AutoFullWay
    {
        /// <summary>
        /// 不设置
        /// </summary>
        Way0,
        /// <summary>
        /// 方式1
        /// </summary>
        Way1_JS,
        /// <summary>
        /// sql 方式。
        /// </summary>
        Way2_SQL,
        /// <summary>
        /// 外键
        /// </summary>
        Way3_FK,
        /// <summary>
        /// 明细
        /// </summary>
        Way4_Dtl,
        /// <summary>
        /// 脚本
        /// </summary>
        Way5_JS
    }
    /// <summary>
    ///  控件类型
    /// </summary>
    public enum UIContralType
    {
        /// <summary>
        /// 文本框
        /// </summary>
        TB = 0,
        /// <summary>
        /// 下拉框
        /// </summary>
        DDL = 1,
        /// <summary>
        /// CheckBok
        /// </summary>
        CheckBok = 2,
        /// <summary>
        /// 单选择按钮
        /// </summary>
        RadioBtn = 3
    }
    /// <summary>
    /// 逻辑类型
    /// </summary>
    public enum FieldTypeS
    {
        /// <summary>
        /// 普通类型
        /// </summary>
        Normal,
        /// <summary>
        /// 枚举类型
        /// </summary>
        Enum,
        /// <summary>
        /// 外键
        /// </summary>
        FK
    }
    /// <summary>
    /// 字段类型
    /// </summary>
    public enum FieldType
    {
        /// <summary>
        /// 正常的
        /// </summary>
        Normal,
        /// <summary>
        /// 主键
        /// </summary>
        PK,
        /// <summary>
        /// 标识列
        /// </summary>
        IDENTITY,
        /// <summary>
        /// 外键
        /// </summary>
        FK,
        /// <summary>
        /// 枚举
        /// </summary>
        Enum,
        /// <summary>
        /// 既是主键又是外键
        /// </summary>
        PKFK,
        /// <summary>
        /// 既是主键又是枚举
        /// </summary>
        PKEnum,
        /// <summary>
        /// 关连的文本.
        /// </summary>
        RefText,
        /// <summary>
        /// 虚拟的
        /// </summary>
        NormalVirtual,
        /// <summary>
        /// 多值的
        /// </summary>
        MultiValues
    }

    public class Attr
    {
         
        public bool IsFK
        {
            get
            {
                if (this.MyFieldType == FieldType.FK || this.MyFieldType == FieldType.PKFK)
                    return true;
                else
                    return false;
            }
        }
        public bool IsFKorEnum
        {
            get
            {
                if (
                this.MyFieldType == FieldType.Enum
                            || this.MyFieldType == FieldType.PKEnum
                            || this.MyFieldType == FieldType.FK
                            || this.MyFieldType == FieldType.PKFK)
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// 是不是能使用默认值。
        /// </summary>
        public bool IsCanUseDefaultValues
        {
            get
            {
                if (this.MyDataType == CP.Common.Common.AppString && this.UIIsReadonly == false)
                    return true;
                return false;
            }
        }
        public bool IsNum
        {
            get
            {
                if (MyDataType == CP.Common.Common.AppBoolean || MyDataType == CP.Common.Common.AppDouble
                    || MyDataType == CP.Common.Common.AppFloat
                    || MyDataType == CP.Common.Common.AppInt
                    || MyDataType == CP.Common.Common.AppMoney
                    || MyDataType == CP.Common.Common.AppRate
                    )
                    return true;
                else
                    return false;
            }
        }
        public bool IsEnum
        {
            get
            {
                if (MyFieldType == FieldType.Enum || MyFieldType == FieldType.PKEnum)
                    return true;
                else
                    return false;
            }
        }
        public bool IsRefAttr
        {
            get
            {
                if (this.MyFieldType == FieldType.RefText)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// 计算属性是不是PK
        /// </summary>
        public bool IsPK
        {
            get
            {
                if (MyFieldType == FieldType.PK || MyFieldType == FieldType.PKFK || MyFieldType == FieldType.PKEnum)
                    return true;
                else
                    return false;
            }
        }
        private int _IsKeyEqualField = -1;
        public bool IsKeyEqualField
        {
            get
            {
                if (_IsKeyEqualField == -1)
                {
                    if (this.Key == this.Field)
                        _IsKeyEqualField = 1;
                    else
                        _IsKeyEqualField = 0;
                }

                if (_IsKeyEqualField == 1)
                    return true;
                return false;
            }
        }
        /// <summary>
        /// 输入描述
        /// </summary>
        public string EnterDesc
        {
            get
            {
                if (this.UIContralType == UIContralType.TB)
                {
                    if (this.UIIsReadonly || this.UIVisible == false)
                    {
                        return "此字段只读";
                    }
                    else
                    {
                        if (this.MyDataType == CP.Common.Common.AppDate)
                        {
                            return "输入日期类型" + CP.Common.Common.SysDataFormat;
                        }
                        else if (this.MyDataType == CP.Common.Common.AppDateTime)
                        {
                            return "输入日期时间类型" + CP.Common.Common.SysDataTimeFormat;
                        }
                        else if (this.MyDataType == CP.Common.Common.AppString)
                        {
                            return "输入要求最小长度" + this.MinLength + "字符，最大长度" + this.MaxLength + "字符";
                        }
                        else if (this.MyDataType == CP.Common.Common.AppMoney)
                        {
                            return "金额类型 0.00";
                        }
                        else
                        {
                            return "输入数值类型";
                        }
                    }

                }
                else if (this.UIContralType == UIContralType.DDL || this.UIContralType == UIContralType.CheckBok)
                {
                    if (this.UIIsReadonly)
                    {
                        return "此字段只读";
                    }
                    else
                    {
                        if (this.MyDataType == CP.Common.Common.AppBoolean)
                        {
                            return "是/否";
                        }
                        else
                        {
                            return "列表选择";
                        }
                    }
                }

                return "";
            }
        }

        #region 构造函数
        public Attr()
        { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="defaultVal"></param>
        /// <param name="dataType"></param>
        /// <param name="isPK"></param>
        /// <param name="desc"></param>
        public Attr(string key, string field, object defaultVal, int dataType, bool isPK, string desc, int minLength, int maxlength)
        {
            this._key = key;
            this._field = field;
            this._desc = desc;
            if (isPK)
                this.MyFieldType = FieldType.PK;
            this._dataType = dataType;
            this._defaultVal = defaultVal;
            this._minLength = minLength;
            this._maxLength = maxlength;
        }
        public Attr(string key, string field, object defaultVal, int dataType, bool isPK, string desc)
        {
            this._key = key;
            this._field = field;
            this._desc = desc;
            if (isPK)
                this.MyFieldType = FieldType.PK;
            this._dataType = dataType;
            this._defaultVal = defaultVal;
        }
        #endregion

        #region 属性
        public string HelperUrl = null;
        public AutoFullWay AutoFullWay = AutoFullWay.Way0;
        public string AutoFullDoc = null;
        /// <summary>
        /// 属性名称
        /// </summary>
        private string _key = null;
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Key
        {
            get
            {
                return this._key;
            }
            set
            {
                if (value != null)
                    this._key = value.Trim();
            }
        }
        /// <summary>
        /// 属性对应的字段
        /// </summary>
        private string _field = null;
        /// <summary>
        /// 属性对应的字段
        /// </summary>
        /// <returns></returns>
        public string Field
        {
            get
            {
                return this._field;
            }
            set
            {
                if (value != null)
                    this._field = value.Trim();
            }
        }
        /// <summary>
        /// 字段默认值
        /// </summary>
        private object _defaultVal = null;
        public string DefaultValOfReal
        {
            get
            {
                if (_defaultVal == null)
                    return null;
                return _defaultVal.ToString();
            }
            set
            {
                _defaultVal = value;
            }
        }
        /// <summary>
        /// 字段默认值
        /// </summary>
        public object DefaultVal
        {
            get
            {
                switch (this.MyDataType)
                {
                    case CP.Common.Common.AppString:
                        if (this._defaultVal == null)
                            return "";
                        break;
                    case CP.Common.Common.AppInt:
                        if (this._defaultVal == null)
                            return 0;
                        try
                        {
                            return int.Parse(this._defaultVal.ToString());
                        }
                        catch
                        {
                            return 0;
                            //throw new Exception("@设置["+this.Key+"]默认值出现错误，["+_defaultVal.ToString()+"]不能向 int 转换。");
                        }
                    case CP.Common.Common.AppMoney:
                        if (this._defaultVal == null)
                            return 0;
                        try
                        {
                            return float.Parse(this._defaultVal.ToString());
                        }
                        catch
                        {
                            return 0;
                            //	throw new Exception("@设置["+this.Key+"]默认值出现错误，["+_defaultVal.ToString()+"]不能向 AppMoney 转换。");
                        }
                    case CP.Common.Common.AppFloat:
                        if (this._defaultVal == null)
                            return 0;
                        try
                        {
                            return float.Parse(this._defaultVal.ToString());
                        }
                        catch
                        {
                            return 0;
                            //	throw new Exception("@设置["+this.Key+"]默认值出现错误，["+_defaultVal.ToString()+"]不能向 float 转换。");
                        }

                    case CP.Common.Common.AppBoolean:
                        if (this._defaultVal == null || this._defaultVal.ToString() == "")
                            return 0;
                        try
                        {
                            if (CP.Common.Common.StringToBoolean(this._defaultVal.ToString()))
                                return 1;
                            else
                                return 0;
                        }
                        catch
                        {
                            throw new Exception("@设置[" + this.Key + "]默认值出现错误，[" + this._defaultVal.ToString() + "]不能向 bool 转换，请设置0/1。");
                        }

                    case 5:
                        if (this._defaultVal == null)
                            return 0;
                        try
                        {
                            return double.Parse(this._defaultVal.ToString());
                        }
                        catch
                        {
                            throw new Exception("@设置[" + this.Key + "]默认值出现错误，[" + _defaultVal.ToString() + "]不能向 double 转换。");
                        }

                    case CP.Common.Common.AppDate:
                        if (this._defaultVal == null)
                            return "";
                        break;
                    case CP.Common.Common.AppDateTime:
                        if (this._defaultVal == null)
                            return "";
                        break;
                    default:
                        throw new Exception("@bulider insert sql error: 没有这个数据类型，字段名称:" + this.Desc + " 英文:" + this.Key);
                }
                return this._defaultVal;
            }

            set
            {
                this._defaultVal = value;
            }
        }
        /// <summary>
        /// 数据类型。
        /// </summary>
        private int _dataType = 0;
        /// <summary>
        /// 数据类型。
        /// </summary>
        public int MyDataType
        {
            get
            {
                return this._dataType;
            }
            set
            {
                this._dataType = value;
            }
        }
        

        /// <summary>
        /// 是不是主键。
        /// </summary>
        private FieldType _FieldType = FieldType.Normal;
        /// <summary>
        /// 是不是主键
        /// </summary>
        /// <returns> yes / no</returns>
        public FieldType MyFieldType
        {
            get
            {
                return this._FieldType;
            }
            set
            {
                this._FieldType = value;
            }
        }
        /// <summary>
        /// 描述。
        /// </summary>
        private string _desc = null;
        public string Desc
        {
            get
            {
                return this._desc;
            }
            set
            {
                this._desc = value;
            }
        }
        /// <summary>
        /// 在线帮助
        /// </summary>
        public string DescHelper
        {
            get
            {
                if (this.HelperUrl == null)
                    return this._desc;

                if (this.HelperUrl.Contains("script"))
                    return "<a href=\"" + this.HelperUrl + "\"  ><img src='../../Img/Help.png'  height='20px' border=0/>" + this._desc + "</a>";
                else
                    return "<a href=\"" + this.HelperUrl + "\" target=_blank ><img src='../../Img/Help.png'  height='20px' border=0/>" + this._desc + "</a>";
            }
        }
        public string DescHelperIcon
        {
            get
            {
                if (this.HelperUrl == null)
                    return this._desc;
                return "<a href=\"" + this.HelperUrl + "\" ><img src='../../Img/Help.png' height='20px' border=0/></a>";
            }
        }
        /// <summary>
        /// 最大长度。
        /// </summary>
        private int _maxLength = 4000;
        /// <summary>
        /// 最大长度。
        /// </summary>
        public int MaxLength
        {
            get
            {
                switch (this.MyDataType)
                {
                    case CP.Common.Common.AppDate:
                        return 50;
                    case CP.Common.Common.AppDateTime:
                        return 50;
                    default:
                        if (this.IsFK)
                            return 100;
                        else
                            return this._maxLength;
                }
            }
            set
            {
                this._maxLength = value;
            }
        }
        /// <summary>
        /// 最小长度。
        /// </summary>
        private int _minLength = 0;
        /// <summary>
        /// 最小长度。
        /// </summary>
        public int MinLength
        {
            get
            {
                return this._minLength;
            }
            set
            {
                this._minLength = value;
            }
        }
        /// <summary>
        /// 是否可以为空, 对数值类型的数据有效.
        /// </summary>
        public bool IsNull
        {
            get
            {
                if (this.MinLength == 0)
                    return false;
                else
                    return true;
            }
        }
        #endregion

        #region UI 的扩展属性
        public int UIWidthInt
        {
            get
            {
                return (int)this.UIWidth;
            }
        }
        private float _UIWidth = 80;
        /// <summary>
        /// 宽度
        /// </summary>
        public float UIWidth
        {
            get
            {
                if (this._UIWidth <= 10)
                    return 15;
                else
                    return this._UIWidth;
            }
            set
            {
                this._UIWidth = value;
            }
        }

        private int _UIHeight = 0;
        /// <summary>
        /// 高度
        /// </summary>
        public int UIHeight
        {
            get
            {
                return this._UIHeight * 10;
            }
            set
            {
                this._UIHeight = value;
            }
        }

        private bool _UIVisible = true;
        /// <summary>
        /// 是不是可见
        /// </summary>
        public bool UIVisible
        {
            get
            {
                return this._UIVisible;
            }
            set
            {
                this._UIVisible = value;
            }
        }
        /// <summary>
        /// 是否单行显示
        /// </summary>
        public bool UIIsLine = false;
        private bool _UIIsReadonly = false;
        /// <summary>
        /// 是不是只读
        /// </summary>
        public bool UIIsReadonly
        {
            get
            {
                return this._UIIsReadonly;
            }
            set
            {
                this._UIIsReadonly = value;
            }
        }
        private UIContralType _UIContralType = UIContralType.TB;
        /// <summary>
        /// 控件类型。
        /// </summary>
        public UIContralType UIContralType
        {
            get
            {
                return this._UIContralType;
            }
            set
            {
                this._UIContralType = value;
            }
        }
        private string _UIBindKey = null;
        /// <summary>
        /// 要Bind 的Key.
        /// 在TB 里面就是 DataHelpKey
        /// 在DDL 里面是  SelfBindKey.
        /// </summary>
        public string UIBindKey
        {
            get
            {
                return this._UIBindKey;
            }
            set
            {
                this._UIBindKey = value;
            }
        }
        private string _UIBindKeyOfEn = null;
        public bool UIIsDoc
        {
            get
            {
                if (this.UIHeight != 0 && this.UIContralType == UIContralType.TB)
                    return true;
                else
                    return false;
            }
        }
       
      

        private string _UIRefKey = null;
        /// <summary>
        /// 要Bind 的Key. 在TB 里面就是 DataHelpKey 
        /// 在DDL 里面是SelfBindKey.
        /// </summary>
        public string UIRefKeyValue
        {
            get
            {
                return this._UIRefKey;
            }
            set
            {
                this._UIRefKey = value;
            }
        }
        private string _UIRefText = null;
        /// <summary>
        /// 关联的实体valkey	 
        /// </summary>
        public string UIRefKeyText
        {
            get
            {
                return this._UIRefText;
            }
            set
            {
                this._UIRefText = value;
            }
        }
        public string UITag = null;
        #endregion
    }


    /// <summary>
    /// 属性集合
    /// </summary>
    [Serializable]
    public class Attrs : CollectionBase
    {
        
        public Attrs Clone()
        {
            Attrs attrs = new Attrs();
            foreach (Attr attr in this)
            {
                attrs.Add(attr);
            }
            return attrs;
        }
        /// <summary>
        /// 下一个Attr 是否是 Doc 类型.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Attr NextAttr(string CurrentKey)
        {
            int i = this.GetIndexByKey(CurrentKey);

            if (this.Count > i)
                return null;

            return this[i + 1] as Attr;
        }
        public Attr PrvAttr(string CurrentKey)
        {
            int i = this.GetIndexByKey(CurrentKey);

            if (this.Count < i)
                return null;

            return this[i - 1] as Attr;
        }
        /// <summary>
        /// 是否包含属性key。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            foreach (Attr attr in this)
            {
                if (attr.Key == key)
                    return true;
            }
            return false;
        }
        public bool ContainsUpper(string key)
        {
            foreach (Attr attr in this)
            {
                if (attr.Key.ToUpper() == key.ToUpper())
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 物理字段Num
        /// </summary>
        public int ConutOfPhysicsFields
        {
            get
            {
                int i = 0;
                foreach (Attr attr in this)
                {
                    if (attr.MyFieldType != FieldType.RefText)
                        i++;
                }
                return i;
            }
        }

        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsertComplete(index, value);
        }

        /// <summary>
        /// 通过Key ， 取出他的Index.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>index</returns>
        public int GetIndexByKey(string key)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].Key == key)
                    return i;
            }
            return -1;
        }
        public Attr GetAttrByKey(string key)
        {
            foreach (Attr item in this)
            {
                if (item.Key == key)
                    return item;
            }
            return null;
        }
        public Attr GetAttrByDesc(string desc)
        {
            foreach (Attr item in this)
            {
                if (item.Desc == desc)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// 属性集合
        /// </summary>
        public Attrs() { }

        public void Add(Attr attr)
        {
            if (attr.Field == null || attr.Field == "")
                throw new Exception("属性设置错误：您不能设置 key='" + attr.Key + "',得字段值为空");

            bool k = attr.IsKeyEqualField;
            this.Add(attr, true, false);
        }
        /// <summary>
        /// 加入一个属性。
        /// </summary>
        /// <param name="attr">attr</param>
        /// <param name="isAddHisRefText">isAddHisRefText</param>
        public void Add(Attr attr, bool isAddHisRefText, bool isAddHisRefName)
        {
            foreach (Attr myattr in this)
            {
                if (myattr.Key == attr.Key)
                    return;
            }

            this.InnerList.Add(attr); 
        }
       
        /// <summary>
        /// 根据索引访问集合内的元素Attr。
        /// </summary>
        public Attr this[int index]
        {
            get
            {
                return (Attr)this.InnerList[index];
            }
        }
    }	
}
