using CP.Common;
using CP.EntityBase;
using System;

namespace CP.Entiry
{
    public class b_UserAuthenticationAttr
    {
        public const string ID = "ID";
        public const string MemberID = "MemberID";
        public const string MemberCode = "MemberCode";
        public const string RealName = "RealName";
        public const string Company = "Company";
        public const string CompanyXingZhi = "CompanyXingZhi";
        public const string LegalRepresentative = "LegalRepresentative";
        public const string RegisteredCapital = "RegisteredCapital";
        public const string CurrencyType = "CurrencyType";
        public const string ShiFanQiYe = "ShiFanQiYe";
        public const string YouShiQiYe = "YouShiQiYe";
        public const string GuanBiaoQiYe = "GuanBiaoQiYe";
        public const string RegistrationTime = "RegistrationTime";
        public const string AppayNumber = "AppayNumber";
        public const string Industry = "Industry";
        public const string Post = "Post";
        public const string IDNumber = "IDNumber";
        public const string ZipCode = "ZipCode";
        public const string Province = "Province";
        public const string City = "City";
        public const string Region = "Region";
        public const string District = "District";
        public const string Phono = "Phono";
        public const string BusinessLicensePath = "BusinessLicensePath";
        public const string OpeningPermitPath = "OpeningPermitPath";
        public const string BusinessLicenseExpirationDate = "BusinessLicenseExpirationDate";
        public const string Address = "Address";
        public const string LinkMan = "LinkMan";
        public const string MobilePhone = "MobilePhone";
        public const string EMail = "EMail";
        public const string CreateTime = "CreateTime";
        public const string RefuseReason = "RefuseReason";
        public const string CheckState = "CheckState";
        public const string CheckTime = "CheckTime";
        public const string CheckCMID = "CheckCMID";
        public const string CheckCMCode = "CheckCMCode";
        public const string CheckCMName = "CheckCMName";
        public const string AccountType = "AccountType";
        public const string PostalAddress = "PostalAddress";
        public const string RegTime = "RegTime";
        public const string IsBusinessLicenseLongterm = "IsBusinessLicenseLongterm";
        public const string IsDLJG = "IsDLJG";
        public const string NeiBuBianHao = "NeiBuBianHao";
        public const string QQ = "QQ";
        public const string Position = "Position";
        public const string UniversitiesName = "UniversitiesName";
        public const string CorporateCertificatePath = "CorporateCertificatePath";

    }
    public class b_UserAuthentication : CP.EntityBase.Entity
    {
        public b_UserAuthentication()
        {
            base.ReadConnStrName = sysConst.ReadConnStrName;
            base.WriteConnStrName = sysConst.WriteConnStrName;
        }
        /// <summary>
        /// ??????????????????
        /// </summary>
        public override Map CreateEnMap()
        {
            Map map = new Map();
            map.PhysicsTable = "b_UserAuthentication";
            map.AddTBStringID(b_UserAuthenticationAttr.ID, null, "?????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.MemberID, null, "?????????ID??????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.MemberCode, null, "????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.RealName, null, "????????????", 0, 20);
            map.AddTBString(b_UserAuthenticationAttr.Company, null, "????????????", 0, 100);
            map.AddTBString(b_UserAuthenticationAttr.CompanyXingZhi, null, "???????????? ???????????????????????????????????????????????????????????????????????????????????????", 0, 30);
            map.AddTBString(b_UserAuthenticationAttr.LegalRepresentative, null, "????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.RegisteredCapital, null, "????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.CurrencyType, null, "?????????????????????????????????????????????????????????????????????????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.ShiFanQiYe, null, "???????????????????????? ????????????????????????????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.YouShiQiYe, null, "???????????????????????? ????????????????????????????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.GuanBiaoQiYe, null, "????????????????????????  ????????????????????????????????????????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.RegistrationTime, null, "????????????????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.AppayNumber, null, "?????????/??????????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.Industry, null, "????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.PostalAddress, null, "????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.Post, null, "?????? ?????? ?????????", 0, 20);
            map.AddTBString(b_UserAuthenticationAttr.IDNumber, null, "?????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.ZipCode, null, "??????", 0, 20);
            map.AddTBString(b_UserAuthenticationAttr.Province, null, "???", 0, 20);
            map.AddTBString(b_UserAuthenticationAttr.City, null, "???", 0, 20);
            map.AddTBString(b_UserAuthenticationAttr.Region, null, "??????", 0, 20);
            map.AddTBString(b_UserAuthenticationAttr.District, null, "???", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.Phono, null, "???????????????????????????????????????", 0, 1500);
            map.AddTBString(b_UserAuthenticationAttr.BusinessLicensePath, null, "??????????????????????????????????????????", 0, 1500);
            map.AddTBString(b_UserAuthenticationAttr.OpeningPermitPath, null, "?????????????????????????????????????????????", 0, 1500);
            map.AddTBString(b_UserAuthenticationAttr.BusinessLicenseExpirationDate, null, "??????????????????????????????????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.Address, null, "????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.LinkMan, null, "", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.MobilePhone, null, "??????", 0, 15);
            map.AddTBString(b_UserAuthenticationAttr.EMail, null, "eamil", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.CreateTime, null, "????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.RefuseReason, null, "????????????", 0, 300);
            map.AddTBString(b_UserAuthenticationAttr.CheckState, null, "????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.CheckTime, null, "????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.CheckCMID, null, "", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.CheckCMCode, null, "", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.CheckCMName, null, "", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.AccountType, null, "????????????  ????????????/????????????", 0, 15);
            map.AddTBString(b_UserAuthenticationAttr.RegTime, null, "????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.IsBusinessLicenseLongterm, null, "?????????????????????????????????/1 ???/0???", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.IsDLJG, null, "????????????????????????0??????1??????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.NeiBuBianHao, null, "????????????", 0, 50);
            map.AddTBString(b_UserAuthenticationAttr.QQ, null, "QQ", 0, 15);
            map.AddTBString(b_UserAuthenticationAttr.Position, null, "??????", 0, 20);
            map.AddTBString(b_UserAuthenticationAttr.UniversitiesName, null, "????????????", 0, 100);
            map.AddTBString(b_UserAuthenticationAttr.CorporateCertificatePath, null, "????????????????????????(?????????????????????????????????)", 0, 1500);
            return map;

        }
        #region
        private int? _iD;
        private int? _memberID;
        private string _memberCode;
        private string _realName;
        private string _company;
        private string _companyXingZhi;
        private string _legalRepresentative;
        private double? _registeredCapital;
        private string _currencyType;
        private string _shiFanQiYe;
        private string _youShiQiYe;
        private string _guanBiaoQiYe;
        private object _registrationTime;
        private string _appayNumber;
        private string _industry;
        private string _post;
        private string _iDNumber;
        private string _zipCode;
        private string _province;
        private string _postalAddress;
        private string _city;
        private string _region;
        private string _district;
        private string _phono;
        private string _businessLicensePath;
        private string _openingPermitPath;
        private object _businessLicenseExpirationDate;
        private string _address;
        private string _linkMan;
        private string _mobilePhone;
        private string _eMail;
        private DateTime? _createTime;
        private string _refuseReason;
        private int? _checkState;
        private DateTime? _checkTime;
        private int? _checkCMID;
        private string _checkCMCode;
        private string _checkCMName;
        private string _accountType;
        private DateTime? _regTime;
        private int? _isBusinessLicenseLongterm;
        private int? _isDLJG;
        private string _neiBuBianHao;
        private string _qQ;
        private string _position;
        private string _universitiesName;
        private string _corporateCertificatePath;


        public int? ID
        {
            get { return _iD; }
            set { _iD = value; }
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
        public string RealName
        {
            get { return _realName; }
            set { _realName = value; }
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
        public object RegistrationTime
        {
            get { return _registrationTime; }
            set { _registrationTime = value; }
        }
        public string AppayNumber
        {
            get { return _appayNumber; }
            set { _appayNumber = value; }
        }
        public string Industry
        {
            get { return _industry; }
            set { _industry = value; }
        }
        public string Post
        {
            get { return _post; }
            set { _post = value; }
        }
        public string IDNumber
        {
            get { return _iDNumber; }
            set { _iDNumber = value; }
        }
        public string ZipCode
        {
            get { return _zipCode; }
            set { _zipCode = value; }
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
        public string Phono
        {
            get { return _phono; }
            set { _phono = value; }
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
        public object BusinessLicenseExpirationDate
        {
            get { return _businessLicenseExpirationDate; }
            set { _businessLicenseExpirationDate = value; }
        }
        public string Address
        {
            get { return _address; }
            set { _address = value; }
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
        public DateTime? CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        public string RefuseReason
        {
            get { return _refuseReason; }
            set { _refuseReason = value; }
        }
        public int? CheckState
        {
            get { return _checkState; }
            set { _checkState = value; }
        }
        public DateTime? CheckTime
        {
            get { return _checkTime; }
            set { _checkTime = value; }
        }
        public int? CheckCMID
        {
            get { return _checkCMID; }
            set { _checkCMID = value; }
        }
        public string CheckCMCode
        {
            get { return _checkCMCode; }
            set { _checkCMCode = value; }
        }
        public string CheckCMName
        {
            get { return _checkCMName; }
            set { _checkCMName = value; }
        }
        public string AccountType
        {
            get { return _accountType; }
            set { _accountType = value; }
        }
        public string PostalAddress
        {
            get { return _postalAddress; }
            set { _postalAddress = value; }
        }
        public DateTime? RegTime
        {
            get { return _regTime; }
            set { _regTime = value; }
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
        public string QQ
        {
            get { return _qQ; }
            set { _qQ = value; }
        }
        public string Position
        {
            get { return _position; }
            set { _position = value; }
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
        #endregion

    }
}