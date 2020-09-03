using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CP.Entiry;

namespace CP.DA
{
    public class Log
    {
        public static void WriteInfo(string menuID, string operation, string runState, string description)
        {
            try
            {
                //sys_Menu menu = new sys_Menu(); 
                //menu.Query(menuID);

                //sys_LogOperation log = new sys_LogOperation();

                //if (menu.ID != null)
                //{
                //    log.MenuCode = menu.MenuCode;
                //    log.MenuName = menu.MenuName;
                //}
                
                //log.Operation = operation;
                //log.RunState = runState;
                //log.RunPerson = CP.Entiry.Web.WebUser.UserCode;
                //log.Description = description;
                //log.UserAgent = CP.Common.Common.CurrentHttpContext.Request.UserAgent;
                //log.IP = CP.Common.Common.IPAddress;
                //log.RequestUrl = CP.Common.Common.CurrentHttpContext.Request.Url.ToString();
                //log.RequestParams = CP.Common.Common.CurrentHttpContext.Request.Form.ToString();
                //log.LogType = "消息记录";
                //log.RunTime = DateTime.Now;
                //log.Insert();
            }
            catch (Exception ex)
            {
                CP.Common.Log4.Log4Error("CP.DA.Log.WriteInfo 引发了异常 异常描述: " + ex.Message);
            }
        }

        public static void WriteError(string menuID, string operation, string runState, string description)
        {
            try
            {
                //sys_Menu menu = new sys_Menu();
                //menu.Query(menuID);

                //sys_LogOperation log = new sys_LogOperation();

                //if (menu != null)
                //{
                //    log.MenuCode = menu.MenuCode;
                //    log.MenuName = menu.MenuName;
                //}

                //log.Operation = operation;
                //log.RunState = runState;
                //log.RunPerson = CP.Entiry.Web.WebUser.UserCode;
                //log.Description = description;
                //log.UserAgent = CP.Common.Common.CurrentHttpContext.Request.UserAgent;
                //log.IP = CP.Common.Common.IPAddress;
                //log.RequestUrl = CP.Common.Common.CurrentHttpContext.Request.Url.PathAndQuery;
                //log.RequestParams = CP.Common.Common.CurrentHttpContext.Request.Form.ToString();
                //log.LogType = "错误记录";
                //log.RunTime = DateTime.Now;
                //log.Insert();
            }
            catch (Exception ex)
            {
                CP.Common.Log4.Log4Error("CP.DA.Log.WriteError 引发了异常 异常描述: " + ex.Message);
            }
        }
         
    }

    public class LoginLog
    {
        /// <summary>
        /// 添加登陆日志
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="uname"></param>
        /// <param name="state"></param>
        public static void WriteInfo(string uid, string uname, string state, CP.Common.LoginType type)
        {
            try
            {
                CP.Entiry.sys_LogLogin LogLogin = new Entiry.sys_LogLogin();
                LogLogin.UserId = uid;
                LogLogin.UserName = uname;
                LogLogin.IP = CP.Common.Common.IPAddress;
                LogLogin.LoginType = ((int)type).ToString();
                LogLogin.RunState = state;
                LogLogin.RunTime = DateTime.Now;
                LogLogin.Insert();
            }
            catch (Exception ex)
            {
                CP.Common.Log4.Log4Error("CP.DA.LoginLog.WriteInfo 引发了异常 异常描述: " + ex.Message);
            }
          
        }
    }
     
}
