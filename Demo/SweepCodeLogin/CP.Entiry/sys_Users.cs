using CP.Common;
using CP.EntityBase;
using System;

namespace CP.Entiry
{
    public class sys_UsersAttr
    {
        public const string ID = "ID";
        public const string UserCode = "UserCode";
        public const string UserName = "UserName";
        public const string RealName = "RealName";
        public const string Phono = "Phono";
        public const string BusinessLicenseExpirationDate = "BusinessLicenseExpirationDate";
        public const string BusinessLicensePath = "BusinessLicensePath";
        public const string OpeningPermitPath = "OpeningPermitPath";
        public const string Industry = "Industry";
        public const string IDNumber = "IDNumber";
        public const string RoleId = "RoleId";
        public const string RoleCode = "RoleCode";
        public const string RoleName = "RoleName";
        public const string PassWord = "PassWord";
        public const string LoginType = "LoginType";
        public const string AccountType = "AccountType";
        public const string RegIP = "RegIP";
        public const string RegTime = "RegTime";
        public const string LastIP = "LastIP";
        public const string LastTIme = "LastTIme";
        public const string OLTime = "OLTime";
        public const string CheckState = "CheckState";
        public const string Company = "Company";
        public const string CompanyXingZhi = "CompanyXingZhi";
        public const string AppayNumber = "AppayNumber";
        public const string Province = "Province";
        public const string City = "City";
        public const string Region = "Region";
        public const string District = "District";
        public const string Address = "Address";
        public const string LegalRepresentative = "LegalRepresentative";
        public const string RegisteredCapital = "RegisteredCapital";
        public const string CurrencyType = "CurrencyType";
        public const string RegistrationTime = "RegistrationTime";
        public const string ShiFanQiYe = "ShiFanQiYe";
        public const string YouShiQiYe = "YouShiQiYe";
        public const string GuanBiaoQiYe = "GuanBiaoQiYe";
        public const string LinkMan = "LinkMan";
        public const string MobilePhone = "MobilePhone";
        public const string EMail = "EMail";
        public const string QQ = "QQ";
        public const string ZipCode = "ZipCode";
        public const string PostalAddress = "PostalAddress";
        public const string IsDelete = "IsDelete";
        public const string Position = "Position";
        public const string CheckTime = "CheckTime";
        public const string ApplyCheckTime = "ApplyCheckTime";
        public const string IsBusinessLicenseLongterm = "IsBusinessLicenseLongterm";
        public const string IsDLJG = "IsDLJG";
        public const string NeiBuBianHao = "NeiBuBianHao";
        public const string GroupId = "GroupId";
        public const string GroupCode = "GroupCode";
        public const string GroupName = "GroupName";
        public const string WeChat = "WeChat";
        public const string TelePhone = "TelePhone";
        public const string ProfilePhotoPath = "ProfilePhotoPath";
        public const string Integrals = "Integrals";
        public const string VIPGrade = "VIPGrade";
        public const string UniversitiesName = "UniversitiesName";
        public const string CorporateCertificatePath = "CorporateCertificatePath";
        public const string CompanyIntroduce = "CompanyIntroduce";
        public const string ServiceProject = "ServiceProject";
    }
    public class sys_Users : CP.EntityBase.Entity
    {
        public sys_Users()
        {
            base.ReadConnStrName = sysConst.ReadConnStrName;
            base.WriteConnStrName = sysConst.WriteConnStrName;
        }
        public const string PhysicsTable = "sys_Users";
        /// <summary>
        /// 子类必须继承
        /// </summary>
        public override Map CreateEnMap()
        {
            Map map = new Map();
            map.PhysicsTable = PhysicsTable;
            map.AddTBStringID(sys_UsersAttr.ID, null, "标识列", 0, 50);
            map.AddTBString(sys_UsersAttr.UserCode, null, "用户编号", 0, 50);
            map.AddTBString(sys_UsersAttr.UserName, null, "用户名", 0, 50);
            map.AddTBString(sys_UsersAttr.RealName, null, "真是姓名", 0, 20);
            map.AddTBString(sys_UsersAttr.Phono, null, "图片", 0, 1500);
            map.AddTBString(sys_UsersAttr.Industry, null, "所属行业", 0, 50);
            map.AddTBString(sys_UsersAttr.IDNumber, null, "身份证", 0, 50);
            map.AddTBString(sys_UsersAttr.RoleId, null, "", 0, 50);
            map.AddTBString(sys_UsersAttr.RoleCode, null, "角色编号", 0, 50);
            map.AddTBString(sys_UsersAttr.RoleName, null, "角色名称", 0, 50);
            map.AddTBString(sys_UsersAttr.PassWord, null, "密码", 0, 25);
            map.AddTBString(sys_UsersAttr.LoginType, null, "登录类别  1前台登录 2后台登录 3所有 0禁止登录", 0, 50);
            map.AddTBString(sys_UsersAttr.AccountType, null, "账户类型  个人账户/企业账户", 0, 15);
            map.AddTBString(sys_UsersAttr.RegIP, null, "注册IP", 0, 20);
            map.AddTBString(sys_UsersAttr.RegTime, null, "注册时间", 0, 50);
            map.AddTBString(sys_UsersAttr.LastIP, null, "最后登录IP", 0, 20);
            map.AddTBString(sys_UsersAttr.LastTIme, null, "最后登录时间", 0, 50);
            map.AddTBString(sys_UsersAttr.OLTime, null, "在线时间", 0, 50);
            map.AddTBString(sys_UsersAttr.CheckState, null, "账户是否正在审核  个人账户审核姓名 公司账户审核三证 0未申请 1正在申请 2申请通过", 0, 50);
            map.AddTBString(sys_UsersAttr.Company, null, "公司名称", 0, 100);
            map.AddTBString(sys_UsersAttr.CompanyXingZhi, null, "企业性质 国有企业、外商独资企业、中外合资企业、民营企业、小微、其他", 0, 30);
            map.AddTBString(sys_UsersAttr.AppayNumber, null, "注册号/社会信用代码/证件号码", 0, 50);
            map.AddTBString(sys_UsersAttr.Province, null, "省", 0, 20);
            map.AddTBString(sys_UsersAttr.City, null, "市", 0, 20);
            map.AddTBString(sys_UsersAttr.Region, null, "地区", 0, 20);
            map.AddTBString(sys_UsersAttr.District, null, "区", 0, 50);
            map.AddTBString(sys_UsersAttr.Address, null, "详细地址", 0, 50);
            map.AddTBString(sys_UsersAttr.LegalRepresentative, null, "法人代表", 0, 50);
            map.AddTBString(sys_UsersAttr.RegisteredCapital, null, "注册资本", 0, 50);
            map.AddTBString(sys_UsersAttr.CurrencyType, null, "货币类型（人民币、美元、欧元、港元、其他（手动填写））", 0, 50);
            map.AddTBString(sys_UsersAttr.RegistrationTime, null, "注册时间（企业）", 0, 50);
            map.AddTBString(sys_UsersAttr.ShiFanQiYe, null, "知识产权示范企业 国家级、市级、县区级、否", 0, 50);
            map.AddTBString(sys_UsersAttr.YouShiQiYe, null, "知识产权优势企业 国家级、市级、县区级、否", 0, 50);
            map.AddTBString(sys_UsersAttr.GuanBiaoQiYe, null, "知识产权贯标企业  通过认证、通过验收、开始实施、否", 0, 50);
            map.AddTBString(sys_UsersAttr.LinkMan, null, "", 0, 50);
            map.AddTBString(sys_UsersAttr.MobilePhone, null, "手机", 0, 15);
            map.AddTBString(sys_UsersAttr.EMail, null, "eamil", 0, 50);
            map.AddTBString(sys_UsersAttr.QQ, null, "QQ", 0, 15);
            map.AddTBString(sys_UsersAttr.ZipCode, null, "邮编", 0, 20);
            map.AddTBString(sys_UsersAttr.PostalAddress, null, "通讯地址", 0, 50);
            map.AddTBString(sys_UsersAttr.IsDelete, null, "是否删除 Y/N", 0, 0);
            map.AddTBString(sys_UsersAttr.BusinessLicensePath, null, "营业执照图片路径（企业账户）", 0, 1500);
            map.AddTBString(sys_UsersAttr.OpeningPermitPath, null, "开户许可证图片路径（企业账户）", 0, 1500);
            map.AddTBString(sys_UsersAttr.Position, null, "职位 ", 0, 20);
            map.AddTBString(sys_UsersAttr.BusinessLicenseExpirationDate, null, "营业执照到期时间（企业账户）", 0, 50);
            map.AddTBString(sys_UsersAttr.CheckTime, null, "审核时间", 0, 50);
            map.AddTBString(sys_UsersAttr.ApplyCheckTime, null, "申请审核时间", 0, 50);
            map.AddTBString(sys_UsersAttr.IsBusinessLicenseLongterm, null, "营业执照是否为长期（是/1 否/0）", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.IsDLJG, null, "是否为代理机构（0否，1是）", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.NeiBuBianHao, null, "内部编号", 0, 50);
            map.AddTBString(sys_UsersAttr.GroupId, null, "业务部ID", 0, 50);
            map.AddTBString(sys_UsersAttr.GroupCode, null, "业务部编号", 0, 50);
            map.AddTBString(sys_UsersAttr.GroupName, null, "业务部名称", 0, 50);
            map.AddTBString(sys_UsersAttr.WeChat, null, "微信", 0, 50);
            map.AddTBString(sys_UsersAttr.TelePhone, null, "电话", 0, 15);
            map.AddTBString(sys_UsersAttr.ProfilePhotoPath, null, "头像", 0, 1500);
            map.AddTBString(sys_UsersAttr.Integrals, null, "积分", 0, 50);
            map.AddTBString(sys_UsersAttr.VIPGrade, null, "会员等级(0:普通会员;1:VIP会员;2:VIPPlus会员)", 0, 50);
            map.AddTBString(sys_UsersAttr.UniversitiesName, null, "院校名称", 0, 100);
            map.AddTBString(sys_UsersAttr.CorporateCertificatePath, null, "法人证书图片路径(高校院所、其他事业单位)", 0, 1500);
            map.AddTBString(sys_UsersAttr.CompanyIntroduce, null, "公司介绍", 0, 500);
            map.AddTBString(sys_UsersAttr.ServiceProject, null, "服务项目", 0, 100);
            return map;
        }

        #region
        private int? _iD;
        private string _userCode;
        private string _userName;
        private string _realName;
        private string _phono;
        private string _industry;
        private string _iDNumber;
        private int? _roleId;
        private string _roleCode;
        private string _roleName;
        private string _passWord;
        private int? _loginType;
        private string _accountType;
        private string _regIP;
        private DateTime? _regTime;
        private string _lastIP;
        private DateTime? _lastTIme;
        private int? _oLTime;
        private int? _checkState;
        private string _company;
        private string _companyXingZhi;
        private string _appayNumber;
        private string _province;
        private string _city;
        private string _region;
        private string _district;
        private string _address;
        private string _legalRepresentative;
        private double? _registeredCapital;
        private string _currencyType;
        private object _registrationTime;
        private string _shiFanQiYe;
        private string _youShiQiYe;
        private string _guanBiaoQiYe;
        private string _linkMan;
        private string _mobilePhone;
        private string _eMail;
        private string _qQ;
        private string _zipCode;
        private string _postalAddress;
        private string _isDelete;
        private string _position;
        private object _businessLicenseExpirationDate;
        private string _businessLicensePath;
        private string _openingPermitPath;
        private DateTime? _checkTime;
        private DateTime? _applyCheckTime;
        private int? _isBusinessLicenseLongterm;
        private int? _isDLJG;
        private string _neiBuBianHao;
        private string _groupId;
        private string _groupCode;
        private string _groupName;
        private string _weChat;
        private string _telePhone;
        private string _profilePhotoPath;
        private int? _integrals;
        private int? _vIPGrade;
        private string _universitiesName;
        private string _corporateCertificatePath;
        private string _companyIntroduce;
        private string _serviceProject;
        public int? ID
        {
            get { return _iD; }
            set { _iD = value; }
        }
        public string UserCode
        {
            get { return _userCode; }
            set { _userCode = value; }
        }
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        public string RealName
        {
            get { return _realName; }
            set { _realName = value; }
        }
        public string Phono
        {
            get { return _phono; }
            set { _phono = value; }
        }
        public string Industry
        {
            get { return _industry; }
            set { _industry = value; }
        }
        public string IDNumber
        {
            get { return _iDNumber; }
            set { _iDNumber = value; }
        }
        public int? RoleId
        {
            get { return _roleId; }
            set { _roleId = value; }
        }
        public string RoleCode
        {
            get { return _roleCode; }
            set { _roleCode = value; }
        }
        public string RoleName
        {
            get { return _roleName; }
            set { _roleName = value; }
        }
        public string PassWord
        {
            get { return _passWord; }
            set { _passWord = value; }
        }
        public int? LoginType
        {
            get { return _loginType; }
            set { _loginType = value; }
        }
        public string AccountType
        {
            get { return _accountType; }
            set { _accountType = value; }
        }
        public string RegIP
        {
            get { return _regIP; }
            set { _regIP = value; }
        }
        public DateTime? RegTime
        {
            get { return _regTime; }
            set { _regTime = value; }
        }
        public string LastIP
        {
            get { return _lastIP; }
            set { _lastIP = value; }
        }
        public DateTime? LastTIme
        {
            get { return _lastTIme; }
            set { _lastTIme = value; }
        }
        public int? OLTime
        {
            get { return _oLTime; }
            set { _oLTime = value; }
        }
        public int? CheckState
        {
            get { return _checkState; }
            set { _checkState = value; }
        }
        public string Company
        {
            get { return _company; }
            set { _company = value; }
        }
        public string CompanyXingZhi
        {
            get { return _companyXingZhi; }
            set { _companyXingZhi = value; }
        }
        public string AppayNumber
        {
            get { return _appayNumber; }
            set { _appayNumber = value; }
        }
        public string Province
        {
            get { return _province; }
            set { _province = value; }
        }
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }
        public string Region
        {
            get { return _region; }
            set { _region = value; }
        }
        public string District
        {
            get { return _district; }
            set { _district = value; }
        }
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        public string LegalRepresentative
        {
            get { return _legalRepresentative; }
            set { _legalRepresentative = value; }
        }
        public double? RegisteredCapital
        {
            get { return _registeredCapital; }
            set { _registeredCapital = value; }
        }
        public string CurrencyType
        {
            get { return _currencyType; }
            set { _currencyType = value; }
        }
        public object RegistrationTime
        {
            get { return _registrationTime; }
            set { _registrationTime = value; }
        }
        public string ShiFanQiYe
        {
            get { return _shiFanQiYe; }
            set { _shiFanQiYe = value; }
        }
        public string YouShiQiYe
        {
            get { return _youShiQiYe; }
            set { _youShiQiYe = value; }
        }
        public string GuanBiaoQiYe
        {
            get { return _guanBiaoQiYe; }
            set { _guanBiaoQiYe = value; }
        }
        public string LinkMan
        {
            get { return _linkMan; }
            set { _linkMan = value; }
        }
        public string MobilePhone
        {
            get { return _mobilePhone; }
            set { _mobilePhone = value; }
        }
        public string EMail
        {
            get { return _eMail; }
            set { _eMail = value; }
        }
        public string QQ
        {
            get { return _qQ; }
            set { _qQ = value; }
        }
        public string ZipCode
        {
            get { return _zipCode; }
            set { _zipCode = value; }
        }
        public string PostalAddress
        {
            get { return _postalAddress; }
            set { _postalAddress = value; }
        }
        public string IsDelete
        {
            get { return _isDelete; }
            set { _isDelete = value; }
        }
        public string Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public object BusinessLicenseExpirationDate
        {
            get { return _businessLicenseExpirationDate; }
            set { _businessLicenseExpirationDate = value; }
        }
        public string BusinessLicensePath
        {
            get { return _businessLicensePath; }
            set { _businessLicensePath = value; }
        }
        public string OpeningPermitPath
        {
            get { return _openingPermitPath; }
            set { _openingPermitPath = value; }
        }
        public DateTime? CheckTime
        {
            get { return _checkTime; }
            set { _checkTime = value; }
        }
        public DateTime? ApplyCheckTime
        {
            get { return _applyCheckTime; }
            set { _applyCheckTime = value; }
        }
        public int? IsBusinessLicenseLongterm
        {
            get { return _isBusinessLicenseLongterm; }
            set { _isBusinessLicenseLongterm = value; }
        }
        public int? IsDLJG
        {
            get { return _isDLJG; }
            set { _isDLJG = value; }
        }
        public string NeiBuBianHao
        {
            get { return _neiBuBianHao; }
            set { _neiBuBianHao = value; }
        }
        public string GroupId
        {
            get { return _groupId; }
            set { _groupId = value; }
        }
        public string GroupCode
        {
            get { return _groupCode; }
            set { _groupCode = value; }
        }
        public string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }
        public string WeChat
        {
            get { return _weChat; }
            set { _weChat = value; }
        }
        public string TelePhone
        {
            get { return _telePhone; }
            set { _telePhone = value; }
        }
        public string ProfilePhotoPath
        {
            get { return _profilePhotoPath; }
            set { _profilePhotoPath = value; }
        }
        public int? Integrals
        {
            get { return _integrals; }
            set { _integrals = value; }
        }
        public int? VIPGrade
        {
            get { return _vIPGrade; }
            set { _vIPGrade = value; }
        }
        public string UniversitiesName
        {
            get { return _universitiesName; }
            set { _universitiesName = value; }
        }
        public string CorporateCertificatePath
        {
            get { return _corporateCertificatePath; }
            set { _corporateCertificatePath = value; }
        }
        public string CompanyIntroduce
        {
            get { return _companyIntroduce; }
            set { _companyIntroduce = value; }
        }
        public string ServiceProject
        {
            get { return _serviceProject; }
            set { _serviceProject = value; }
        }
        #endregion
    }
}