using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CP.Entiry.Web
{
    public class WebUser  
    {
        #region 属性
        private static CP.Entiry.sys_Users Instance
        {
            get { return CP.Common.Common.Session["WebUser"] as CP.Entiry.sys_Users; }
            set { CP.Common.Common.Session["WebUser"] = value; }
        }

        public static int? ID
        {
            get { if (Instance == null) return null; else return Instance.ID; }
        }
        public static string UserCode
        {
            get { if (Instance == null) return null; else return Instance.UserCode; }

        }
        public static string UserName
        {
            get { if (Instance == null) return null; else return Instance.UserName; }

        }
        public static string PassWord
        {
            get { if (Instance == null) return null; else return Instance.PassWord; }

        }
        public static string EMail
        {
            get { if (Instance == null) return null; else return Instance.EMail; }
        }
        public static string QQ
        {
            get { if (Instance == null) return null; else return Instance.QQ; }
        }
        public static string TelePhone
        {
            get { if (Instance == null) return null; else return Instance.TelePhone; }
        }
        public static string WeChat
        {
            get { if (Instance == null) return null; else return Instance.WeChat; }
        }
        public static string ProfilePhotoPath
        {
            get { if (Instance == null) return null; else return Instance.ProfilePhotoPath; }
        }
        public static int? LoginType
        {
            get { if (Instance == null) return null; else return Instance.LoginType; }
        }
        public static string AccountType
        {
            get { if (Instance == null) return null; else return Instance.AccountType; }
        }
        public static string RegIP
        {
            get { if (Instance == null) return null; else return Instance.RegIP; }
        }
        public static DateTime? RegTime
        {
            get { if (Instance == null) return null; else return Instance.RegTime; }
        }
        public static object BusinessLicenseExpirationDate
        {
            get { if (Instance == null) return null; else return Instance.BusinessLicenseExpirationDate; }
            set
            {
                if (Instance != null)
                    Instance.BusinessLicenseExpirationDate = value;
            }
        }

        public static string MobilePhone
        {
            get { if (Instance == null) return null; else return Instance.MobilePhone; }
        }

        public static string Company
        {
            get { if (Instance == null) return null; else return Instance.Company; }
        }
        public static string LastIP
        {
            get { if (Instance == null) return null; else return Instance.LastIP; }
        }
        public static string AppayNumber
        {
            get { if (Instance == null) return null; else return Instance.AppayNumber; }
        }
        public static DateTime? LastTIme
        {
            get { if (Instance == null) return null; else return Instance.LastTIme; }
        }
        public static string Position
        {
            get { if (Instance == null) return null; else return Instance.Position; }
        }
        public static string LinkMan
        {
            get { if (Instance == null) return null; else return Instance.LinkMan; }
        }
        public static string ZipCode
        {
            get { if (Instance == null) return null; else return Instance.ZipCode; }
        }
        public static string Province
        {
            get { if (Instance == null) return null; else return Instance.Province; }
            set
            {
                if (Instance != null)
                    Instance.Province = value;
            }
        }
        public static string Address
        {
            get { if (Instance == null) return null; else return Instance.Address; }
            set
            {
                if (Instance != null)
                    Instance.Address = value;
            }
        }
        public static int? IsBusinessLicenseLongterm
        {
            get { if (Instance == null) return null; else return Instance.IsBusinessLicenseLongterm; }
            set
            {
                if (Instance != null)
                    Instance.IsBusinessLicenseLongterm = value;
            }
        }

        public static string LegalRepresentative
        {
            get { if (Instance == null) return null; else return Instance.LegalRepresentative; }
        }


        public static string GroupId
        {
            get { if (Instance == null) return null; else return Instance.GroupId; }
            set
            {
                if (Instance != null)
                    Instance.GroupId = value;
            }
        }
        public static string GroupCode
        {
            get { if (Instance == null) return null; else return Instance.GroupCode; }
            set
            {
                if (Instance != null)
                    Instance.GroupCode = value;
            }
        }
        public static string GroupName
        {
            get { if (Instance == null) return null; else return Instance.GroupName; }
            set
            {
                if (Instance != null)
                    Instance.GroupName = value;
            }
        }
        public static int? RoleId
        {
            get { if (Instance == null) return null; else return Instance.RoleId; }
            set
            {
                if (Instance != null)
                    Instance.RoleId = value;
            }
        }
        public static string RoleCode
        {
            get { if (Instance == null) return null; else return Instance.RoleCode; }
            set
            {
                if (Instance != null)
                    Instance.RoleCode = value;
            }
        }
        public static string RoleName
        {
            get { if (Instance == null) return null; else return Instance.RoleName; }
            set
            {
                if (Instance != null)
                    Instance.RoleName = value;
            }
        }
        public static int? CheckState
        {
            get
            {
                if (Instance == null)
                    return null;
                else
                    return Instance.CheckState;
            }
            set
            {
                if (Instance != null)
                    Instance.CheckState = value;
            }
        }

        #endregion

        #region 函数

        public static void SetWebUser(CP.Entiry.sys_Users user)
        {
            Instance = user;
        }


        public static void LoginOut()
        {
            Instance = null;
        }
        #endregion 

    }
}
