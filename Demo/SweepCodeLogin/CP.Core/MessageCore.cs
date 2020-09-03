using CP.Common;
using CP.Entiry;
using CP.Entiry.Web;
using CP.EntityBase;
using System;
using System.Data;
using System.Data.Common;

namespace CP.Core
{
    public class MessageCore
    {

        #region 添加系统消息
        /// <summary>
        /// 添加系统消息
        /// </summary>
        /// <param name="modular">模块名称</param>
        /// <param name="title">标题</param>
        /// <param name="State">状态</param>
        /// <param name="url">跳转地址</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool AddMessage(int? receptUserID, string modular, string title, string State, string url, out string errMsg)
        {

            try
            {
                return AddMessage(null, receptUserID, modular, title, State, url, out errMsg);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 添加系统消息
        /// </summary>
        /// <param name="tran">事务</param>
        /// <param name="modular">模块名称</param>
        /// <param name="title">标题</param>
        /// <param name="State">状态</param>
        /// <param name="url">跳转地址</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool AddMessage(DbTransaction tran, int? receptUserID, string modular, string title, string State, string url, out string errMsg)
        {
            try
            {
                return AddMessage(tran, receptUserID, modular, title, State, url, CP.Entiry.Web.WebUser.ID, CP.Entiry.Web.WebUser.UserCode, CP.Entiry.Web.WebUser.UserName, out errMsg);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 添加系统消息
        /// </summary>
        /// <param name="tran">事务</param>
        /// <param name="modular">模块名称</param>
        /// <param name="title">标题</param>
        /// <param name="State">状态</param>
        /// <param name="url">跳转地址</param>
        /// <param name="errMsg">错误信息</param>
        /// <param name="MemberID">发送人ID</param>
        /// <param name="MemberCode">发送人Code</param>
        /// <param name="MemberName">发送人名称</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool AddMessage(DbTransaction tran, int? receptUserID, string modular, string title, string State, string url, int? MemberID, string MemberCode, string MemberName, out string errMsg)
        {
            try
            {
                CP.Entiry.sys_Message entry = new Entiry.sys_Message();

                entry.FK_User_ID = receptUserID;
                entry.Modular = modular;
                entry.Title = title;
                entry.State = State;
                entry.Url = url;

                return AddMessage(tran, entry, MemberID, MemberCode, MemberName, out errMsg);
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// 添加系统消息
        /// </summary>
        /// <param name="tran">事务</param>
        /// <param name="entry">实体类</param>
        /// <param name="MemberID">发送人ID</param>
        /// <param name="MemberCode">发送人Code</param>
        /// <param name="MemberName">发送人名称</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool AddMessage(DbTransaction tran, CP.Entiry.sys_Message entry, int? MemberID, string MemberCode, string MemberName, out string errMsg)
        {
            try
            {
                errMsg = string.Empty;

                #region 校验
                if (string.IsNullOrEmpty(entry.FK_User_ID.ToString()))
                    errMsg = "CP.Core.MessageCore.AddMessage引发了系统异常：FK_User_ID为空。";
                if (string.IsNullOrEmpty(entry.Modular))
                    errMsg = "CP.Core.MessageCore.AddMessage引发了系统异常：Modular为空。";
                if (string.IsNullOrEmpty(entry.Title))
                    errMsg = "CP.Core.MessageCore.AddMessage引发了系统异常：Title为空。";
                if (string.IsNullOrEmpty(entry.State))
                    errMsg = "CP.Core.MessageCore.AddMessage引发了系统异常：State为空。";
                if (string.IsNullOrEmpty(entry.Url))
                    errMsg = "CP.Core.MessageCore.AddMessage引发了系统异常：Url为空。";

                #endregion

                entry.CreateTime = System.DateTime.Now;
                entry.Sender_ID = MemberID;
                entry.Sender_Code = MemberCode;
                entry.Sender_Name = MemberName;
                entry.IsRead = ((int)CP.Common.MessageIsRead.UnRead).ToString();

                entry.Insert(tran);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 审核消息


        /// <summary>
        /// 添加审核消息
        /// </summary>
        /// <param name="id">审核的案件ID</param>
        /// <param name="type">审核的资助类别</param>
        /// <param name="summary">拒绝、驳回原因</param>
        /// <param name="state">审核状态</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool AddCheckMessage(int id, string type, string summary, string state, out string errMsg)
        {

            try
            {
                return AddCheckMessage(null, id, type, summary, state, out errMsg);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 添加审核消息
        /// </summary>
        /// <param name="id">审核的案件ID</param>
        /// <param name="type">审核的资助类别</param>
        /// <param name="summary">拒绝、驳回原因</param>
        /// <param name="state">审核状态</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool AddCheckMessage(DbTransaction tran, int? id, string type, string summary, string state, out string errMsg)
        {
            try
            {
                return AddCheckMessage(tran, id, type, summary, state, CP.Entiry.Web.WebUser.ID, CP.Entiry.Web.WebUser.UserCode, CP.Entiry.Web.WebUser.UserName, out errMsg);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 添加审核消息
        /// </summary>
        /// <param name="tran">事务</param>
        /// <param name="id">审核的案件ID</param>
        /// <param name="type">审核的资助类别</param>
        /// <param name="summary">拒绝、驳回原因</param>
        /// <param name="state">审核状态</param>
        /// <param name="MemberID">发送人ID</param>
        /// <param name="MemberCode">发送人Code</param>
        /// <param name="MemberName">发送人名称</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool AddCheckMessage(DbTransaction tran, int? id, string type, string summary, string state, int? MemberID, string MemberCode, string MemberName, out string errMsg)
        {
            try
            {
                CP.Entiry.sys_Message_Check entry = new Entiry.sys_Message_Check();

                entry.FK_Funding_ID = id;
                entry.Type = type;
                entry.Summary = summary;
                entry.State = state;

                return AddCheckMessage(tran, entry, MemberID, MemberCode, MemberName, out errMsg);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 添加审核消息
        /// </summary>
        /// <param name="tran">事务</param>
        /// <param name="entry">实体类</param>
        /// <param name="MemberID">发送人ID</param>
        /// <param name="MemberCode">发送人Code</param>
        /// <param name="MemberName">发送人名称</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool AddCheckMessage(DbTransaction tran, CP.Entiry.sys_Message_Check entry, int? MemberID, string MemberCode, string MemberName, out string errMsg)
        {
            try
            {
                errMsg = string.Empty;

                #region 校验
                if (string.IsNullOrEmpty(entry.FK_Funding_ID.ToString()))
                    errMsg = "CP.Core.MessageCore.AddMessage引发了系统异常：FK_Funding_ID为空。";
                if (string.IsNullOrEmpty(entry.Type))
                    errMsg = "CP.Core.MessageCore.AddMessage引发了系统异常：Type为空。";
                if (string.IsNullOrEmpty(entry.State))
                    errMsg = "CP.Core.MessageCore.AddMessage引发了系统异常：State为空。";

                #endregion

                entry.CreateTime = System.DateTime.Now;
                entry.CheckCMID = MemberID;
                entry.CheckCMCode = MemberCode;
                entry.CheckCMName = MemberName;

                entry.Insert(tran);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 提交（补正）消息


        /// <summary>
        /// 添加提交（补正）消息
        /// </summary>
        /// <param name="id">审核的案件ID</param>
        /// <param name="type">审核的资助类别</param>
        /// <param name="state">审核状态</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool AddSubmitMessage(int id, string type, string state, out string errMsg)
        {

            try
            {
                return AddSubmitMessage(null, id, type, state, out errMsg);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 添加提交（补正）消息
        /// </summary>
        /// <param name="id">审核的案件ID</param>
        /// <param name="type">审核的资助类别</param>
        /// <param name="state">审核状态</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool AddSubmitMessage(DbTransaction tran, int? id, string type, string state, out string errMsg)
        {
            try
            {
                return AddSubmitMessage(tran, id, type, state, CP.Entiry.Web.WebUser.ID, CP.Entiry.Web.WebUser.UserCode, CP.Entiry.Web.WebUser.UserName, out errMsg);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 添加提交（补正）消息
        /// </summary>
        /// <param name="tran">事务</param>
        /// <param name="id">审核的案件ID</param>
        /// <param name="type">审核的资助类别</param>
        /// <param name="state">审核状态</param>
        /// <param name="MemberID">发送人ID</param>
        /// <param name="MemberCode">发送人Code</param>
        /// <param name="MemberName">发送人名称</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool AddSubmitMessage(DbTransaction tran, int? id, string type, string state, int? MemberID, string MemberCode, string MemberName, out string errMsg)
        {
            try
            {
                CP.Entiry.sys_Message_Submit entry = new Entiry.sys_Message_Submit();

                entry.FK_Funding_ID = id;
                entry.Type = type;
                entry.State = state;

                return AddSubmitMessage(tran, entry, MemberID, MemberCode, MemberName, out errMsg);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 添加提交（补正）消息
        /// </summary>
        /// <param name="tran">事务</param>
        /// <param name="entry">实体类</param>
        /// <param name="MemberID">发送人ID</param>
        /// <param name="MemberCode">发送人Code</param>
        /// <param name="MemberName">发送人名称</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool AddSubmitMessage(DbTransaction tran, CP.Entiry.sys_Message_Submit entry, int? MemberID, string MemberCode, string MemberName, out string errMsg)
        {
            try
            {
                errMsg = string.Empty;

                #region 校验
                if (string.IsNullOrEmpty(entry.FK_Funding_ID.ToString()))
                    errMsg = "CP.Core.MessageCore.AddMessage引发了系统异常：FK_Funding_ID为空。";
                if (string.IsNullOrEmpty(entry.Type))
                    errMsg = "CP.Core.MessageCore.AddMessage引发了系统异常：Type为空。";
                if (string.IsNullOrEmpty(entry.State))
                    errMsg = "CP.Core.MessageCore.AddMessage引发了系统异常：State为空。";

                #endregion

                entry.CreateTime = System.DateTime.Now;
                entry.MemberID = MemberID;
                entry.MemberCode = MemberCode;
                entry.MemberName = MemberName;

                entry.Insert(tran);

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 获取消息 崔萌 2020-4-4 09:58:10
        /// <remarks>崔萌 2020-4-4 09:58:10</remarks>
        /// <summary>
        /// 获取消息
        /// </summary>
        /// <returns>消息</returns>
        public DataTable GetMessage()
        {
            QueryParams message_param = new QueryParams();

            string[] message_fileds = new string[] {
                sys_MessageAttr.CreateTime,
                sys_MessageAttr.IsRead,
                sys_MessageAttr.Title,
                sys_MessageAttr.State,
                sys_MessageAttr.Modular,
                sys_MessageAttr.Url,
                sys_MessageAttr.ID
            };

            message_param.addWhere(sys_MessageAttr.FK_User_ID, "=", "@FK_User_ID", WebUser.ID);
            message_param.addAnd();
            message_param.addWhere("(");
            message_param.addWhere(" YEAR(" + sys_MessageAttr.CreateTime + ")", "=", "@Year", DateTime.Now.Year);
            message_param.addAnd();
            message_param.addWhere(" MONTH(" + sys_MessageAttr.CreateTime + ")", "=", "@Month", DateTime.Now.Month);
            message_param.addWhere(")");
            message_param.addAnd();
            message_param.addWhere(sys_MessageAttr.IsRead, "=", "@IsRead", MessageIsRead.UnRead);

            message_param.addOrder(sys_MessageAttr.CreateTime + " DESC ");

            sys_Message message = new sys_Message();
            return message.QueryToDataTable(message_param, message_fileds, sysConst.MemberCenter_Message_DisplayNum);
        }
        #endregion
    }
}
