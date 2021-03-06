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
        /// ??????????????????
        /// </summary>
        public override Map CreateEnMap()
        {
            Map map = new Map();
            map.PhysicsTable = PhysicsTable;
            map.AddTBStringID(sys_UsersAttr.ID, null, "?????????", 0, 50);
            map.AddTBString(sys_UsersAttr.UserCode, null, "????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.UserName, null, "?????????", 0, 50);
            map.AddTBString(sys_UsersAttr.RealName, null, "????????????", 0, 20);
            map.AddTBString(sys_UsersAttr.Phono, null, "??????", 0, 1500);
            map.AddTBString(sys_UsersAttr.Industry, null, "????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.IDNumber, null, "?????????", 0, 50);
            map.AddTBString(sys_UsersAttr.RoleId, null, "", 0, 50);
            map.AddTBString(sys_UsersAttr.RoleCode, null, "????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.RoleName, null, "????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.PassWord, null, "??????", 0, 25);
            map.AddTBString(sys_UsersAttr.LoginType, null, "????????????  1???????????? 2???????????? 3?????? 0????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.AccountType, null, "????????????  ????????????/????????????", 0, 15);
            map.AddTBString(sys_UsersAttr.RegIP, null, "??????IP", 0, 20);
            map.AddTBString(sys_UsersAttr.RegTime, null, "????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.LastIP, null, "????????????IP", 0, 20);
            map.AddTBString(sys_UsersAttr.LastTIme, null, "??????????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.OLTime, null, "????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.CheckState, null, "????????????????????????  ???????????????????????? ???????????????????????? 0????????? 1???????????? 2????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.Company, null, "????????????", 0, 100);
            map.AddTBString(sys_UsersAttr.CompanyXingZhi, null, "???????????? ???????????????????????????????????????????????????????????????????????????????????????", 0, 30);
            map.AddTBString(sys_UsersAttr.AppayNumber, null, "?????????/??????????????????/????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.Province, null, "???", 0, 20);
            map.AddTBString(sys_UsersAttr.City, null, "???", 0, 20);
            map.AddTBString(sys_UsersAttr.Region, null, "??????", 0, 20);
            map.AddTBString(sys_UsersAttr.District, null, "???", 0, 50);
            map.AddTBString(sys_UsersAttr.Address, null, "????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.LegalRepresentative, null, "????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.RegisteredCapital, null, "????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.CurrencyType, null, "?????????????????????????????????????????????????????????????????????????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.RegistrationTime, null, "????????????????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.ShiFanQiYe, null, "???????????????????????? ????????????????????????????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.YouShiQiYe, null, "???????????????????????? ????????????????????????????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.GuanBiaoQiYe, null, "????????????????????????  ????????????????????????????????????????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.LinkMan, null, "", 0, 50);
            map.AddTBString(sys_UsersAttr.MobilePhone, null, "??????", 0, 15);
            map.AddTBString(sys_UsersAttr.EMail, null, "eamil", 0, 50);
            map.AddTBString(sys_UsersAttr.QQ, null, "QQ", 0, 15);
            map.AddTBString(sys_UsersAttr.ZipCode, null, "??????", 0, 20);
            map.AddTBString(sys_UsersAttr.PostalAddress, null, "????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.IsDelete, null, "???????????? Y/N", 0, 0);
            map.AddTBString(sys_UsersAttr.BusinessLicensePath, null, "??????????????????????????????????????????", 0, 1500);
            map.AddTBString(sys_UsersAttr.OpeningPermitPath, null, "?????????????????????????????????????????????", 0, 1500);
            map.AddTBString(sys_UsersAttr.Position, null, "?????? ", 0, 20);
            map.AddTBString(sys_UsersAttr.BusinessLicenseExpirationDate, null, "??????????????????????????????????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.CheckTime, null, "????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.ApplyCheckTime, null, "??????????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.IsBusinessLicenseLongterm, null, "?????????????????????????????????/1 ???/0???", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.IsDLJG, null, "????????????????????????0??????1??????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.NeiBuBianHao, null, "????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.GroupId, null, "?????????ID", 0, 50);
            map.AddTBString(sys_UsersAttr.GroupCode, null, "???????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.GroupName, null, "???????????????", 0, 50);
            map.AddTBString(sys_UsersAttr.WeChat, null, "??????", 0, 50);
            map.AddTBString(sys_UsersAttr.TelePhone, null, "??????", 0, 15);
            map.AddTBString(sys_UsersAttr.ProfilePhotoPath, null, "??????", 0, 1500);
            map.AddTBString(sys_UsersAttr.Integrals, null, "??????", 0, 50);
            map.AddTBString(sys_UsersAttr.VIPGrade, null, "????????????(0:????????????;1:VIP??????;2:VIPPlus??????)", 0, 50);
            map.AddTBString(sys_UsersAttr.UniversitiesName, null, "????????????", 0, 100);
            map.AddTBString(sys_UsersAttr.CorporateCertificatePath, null, "????????????????????????(?????????????????????????????????)", 0, 1500);
            map.AddTBString(sys_UsersAttr.CompanyIntroduce, null, "????????????", 0, 500);
            map.AddTBString(sys_UsersAttr.ServiceProject, null, "????????????", 0, 100);
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