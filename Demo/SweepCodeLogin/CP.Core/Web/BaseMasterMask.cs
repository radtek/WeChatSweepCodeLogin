using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.Core.Web
{
    public class BaseMasterMask : SuperMasterMask, System.Web.SessionState.IRequiresSessionState
    {
        public BaseMasterMask()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }


        #region Page Event
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                base.Page_Init(sender, e);

                #region 校验当前登陆用户是否失效 失效则跳转至登陆界面

                if (!IsLogin())
                {
                    base.GoToLogin("#2001", "");
                    return;
                }
                #endregion

                #region 校验用户登录类型
                if (Request.Url.AbsoluteUri.ToUpper().Contains("/MANAGER/"))
                {
                    if (CP.Entiry.Web.WebUser.LoginType != (int)CP.Common.LoginType.Management && CP.Entiry.Web.WebUser.LoginType != (int)CP.Common.LoginType.ALL)
                    {
                        base.GoToLogin("#2002", "您的账号不允许在此登陆。");
                        return;
                    }
                }
                else if (Request.Url.AbsoluteUri.ToUpper().Contains("/APP/"))
                {
                    if (CP.Entiry.Web.WebUser.LoginType != (int)CP.Common.LoginType.Forepart && CP.Entiry.Web.WebUser.LoginType != (int)CP.Common.LoginType.ALL)
                    {
                        base.GoToLogin("#2003", "您的账号不允许在此登陆。");
                        return;
                    }
                }
                #endregion

                #region 校验用户权限

                if (string.IsNullOrEmpty(this.MenuID))
                {
                    string _msg = "当前浏览的页面链接未注册, 已终止请求。";
                    string _log = string.Format(_msg + "\r\n请求链接:[{0}]\r\n登陆账号:[{1}]\r\n角色ID:[{2}]\r\n", Request.Url.AbsolutePath, CP.Entiry.Web.WebUser.UserCode, CP.Entiry.Web.WebUser.RoleId);

                    CP.Common.Log4.Log4Error(_log);

                    base.GoTo("#9001", null, _msg);
                    return;
                }

                #region 校验浏览权限
                CP.Core.SystemCore.CurrentPermission currentPermission = CP.Core.SystemCore.GetCurrentPermission(CP.Entiry.Web.WebUser.RoleId.Value);

                if (currentPermission.Permisslist.Where(
                    T => T.FK_Menu_ID.ToString() == this.MenuID &&
                    T.FK_MenuButton_ID == CP.Common.sysConst.C_Permission_PerViewID)
                    .Count() < 1)
                {
                    string _msg = "您没有浏览当前模块的权限。";
                    string _log = string.Format(_msg + "\r\nMenuID:[{0}]\r\n请求链接:[{1}]\r\n登陆账号:[{2}]\r\n角色ID:[{3}]\r\n", this.MenuID, Request.Url.AbsolutePath, CP.Entiry.Web.WebUser.UserCode, CP.Entiry.Web.WebUser.RoleId);

                    CP.Common.Log4.Log4Error(_log);

                    base.GoTo("#9001", null, _msg);
                    return;
                }
                #endregion

                #region 如果有浏览权限 校验提交的Action权限
                if (!string.IsNullOrEmpty(this.Action))
                {
                    bool _ischeck = true;
                    #region 如果是P开头的Action 则不校验权限
                    if (this.Action.Length > 2)
                    {
                        if (Equals(this.Action.Substring(0, 2).ToUpper(), "P_"))
                            _ischeck = false;
                    }
                    #endregion
                    if (_ischeck)
                    {
                        if (currentPermission.Buttonlist.Where(T => T.Action == this.Action && T.FK_Menu_ID.ToString() == this.MenuID).Count() < 1)
                        {

                            string _msg = "您没有使用此功能的权限。";
                            string _log = string.Format(_msg + "\r\nMenuID:[{0}]\r\n请求链接:[{1}]\r\nAction:[{2}]\r\n登陆账号:[{3}]\r\n角色ID:[{4}]\r\n", this.MenuID, Request.Url.AbsolutePath, this.Action, CP.Entiry.Web.WebUser.UserCode, CP.Entiry.Web.WebUser.RoleId);

                            CP.Common.Log4.Log4Error(_log);

                            base.GoTo("#9001", null, _msg);
                            return;
                        }
                    }
                }
                #endregion

                #endregion
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                // Response.End() 引发此异常. 无需处理.
            }
            catch (Exception ex)
            {
                base.Exception(ex);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.Page_Load(sender, e);
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                // Response.End() 引发此异常. 无需处理.
            }
            catch (Exception ex)
            {
                base.Exception(ex);
            }
        }
        #endregion
    }
}
