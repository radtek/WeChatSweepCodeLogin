using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.Core
{
    public class PayXianXiaCore
    {

        #region 创建线下支付信息
        /// <summary>
        /// 创建线下支付信息 -- 前台 订单线下付款时创建
        /// </summary>
        /// <param name="OrderID">订单表ID</param>
        /// <param name="PayMan">付款人</param>
        /// <param name="PayDate">付款日</param>
        /// <param name="PayType">支付类型</param>
        /// <param name="AccountType">账户类型</param>
        /// <param name="SellerID">收款帐号</param>
        /// <param name="errMsg"></param>
        public bool CreatePayXianXia(int OrderID, string PayMan, DateTime PayDate, string PayType, string AccountType, string SellerID, out string errMsg)
        {
            DbTransaction tran = null;
            try
            {
                CP.Entiry.b_Pay_XianXia pay = new Entiry.b_Pay_XianXia();

                pay.FK_Order_ID = OrderID;
                pay.PayMan = PayMan;
                pay.PayDate = PayDate;
                pay.SellerID = SellerID;
                pay.AccountType = AccountType;
                pay.PayType = PayType;
                pay.CreateTime = System.DateTime.Now;

                tran = CP.Common.DBHelper.GetTransaction(CP.Common.sysConst.WriteConnStrName);
                CreatePayXianXia(tran, pay, CP.Entiry.Web.WebUser.ID, CP.Entiry.Web.WebUser.UserCode, CP.Entiry.Web.WebUser.UserName, out errMsg);

                tran.Commit();
                errMsg = "";
                return true;
            }
            catch (Exception)
            {
                if (tran != null)
                    tran.Rollback();
                throw;
            }
            finally
            {
                if (tran != null)
                {
                    tran.Dispose();
                }
            }
        }
        #endregion


        #region
        /// <summary>
        /// 创建线下支付信息
        /// </summary>
        /// <param name="tran">事务</param>
        /// <param name="fee">实体类对象</param>
        /// <param name="MemberID">创建者ID</param>
        /// <param name="MemberCode">创建者编号</param>
        /// <param name="MemberName">创建者名</param>
        /// <param name="errMsg"></param>
        private bool CreatePayXianXia(DbTransaction tran, CP.Entiry.b_Pay_XianXia pay, int? MemberID, string MemberCode, string MemberName, out string errMsg)
        {
            try
            {
                pay.MemberID = CP.Entiry.Web.WebUser.ID;
                pay.MemberCode = CP.Entiry.Web.WebUser.UserCode;
                pay.MemberName = CP.Entiry.Web.WebUser.UserName;

                pay.Insert(tran);

                #region 更改订单状态为正在审核 CP.Common.OrderState.Checking

                int orderid = (int)pay.FK_Order_ID;
                //new CP.Core.OrderCore().ChangeOrderState(tran, orderid, out errMsg);

                #endregion

                errMsg = "";
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region 更新财务确认信息 -- 批量/单个
        /// <summary>
        /// 更新财务确认信息 -- 财务确认时
        /// </summary>
        /// <param name="tran">事务</param>
        /// <param name="IDArr">订单表ID</param>
        /// <param name="Remark">备注</param>
        /// <param name="errMsg">返回信息</param>
        //public bool UpdateCWCheckInfo(string IDArr, string Remark, out string errMsg)
        //{
        //    DbTransaction tran = null;
        //    try
        //    {
        //        #region 根据订单表id，获取线下支付表id
        //        CP.Entiry.b_Pay_XianXia query = new CP.Entiry.b_Pay_XianXia();
        //        CP.EntityBase.QueryParams param = new CP.EntityBase.QueryParams();

        //        param.addWhere(CP.Entiry.b_Pay_XianXiaAttr.FK_Order_ID + " in (select col from Split(@FK_Order_ID,','))", "@FK_Order_ID", string.Join(",", IDArr));

        //        DataTable query_dt = query.QueryToDataTable(param);

        //        if (query_dt.Rows.Count <= 0)
        //        {
        //            errMsg = "该线下支付订单不存在，或已完成确认。";
        //            return false;
        //        }
        //        #endregion

        //        #region 根据订单表id，检测该订单状态是否为 正在审核
        //        CP.Entiry.b_MyOrders order = new CP.Entiry.b_MyOrders();
        //        CP.EntityBase.QueryParams order_param = new CP.EntityBase.QueryParams();

        //        order_param.addWhere(CP.Entiry.b_MyOrdersAttr.ID + " in (select col from Split(@ID,','))", "@ID", string.Join(",", IDArr));

        //        DataTable order_dt = query.QueryToDataTable(order_param);

        //        if (order_dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in order_dt.Rows)
        //            {
        //                if ((int)dr[CP.Entiry.b_MyOrdersAttr.State] != (int)CP.Common.OrderState.Checking)
        //                {
        //                    errMsg = "该线下支付订单不存在，或已完成确认。";
        //                    return false;
        //                }
        //            }
        //        }

        //        #endregion

        //        tran = CP.Common.DBHelper.GetTransaction(CP.Common.sysConst.WriteConnStrName);

        //        #region 更新财务确认信息

        //        foreach (DataRow dr in query_dt.Rows)
        //        {
        //            CP.Entiry.b_Pay_XianXia pay = new CP.Entiry.b_Pay_XianXia();

        //            pay.ID = (int)dr[CP.Entiry.b_Pay_XianXiaAttr.ID];
        //            pay.CWMemberID = CP.Entiry.Web.WebUser.ID;
        //            pay.CWMemberCode = CP.Entiry.Web.WebUser.UserCode;
        //            pay.CWMemberName = CP.Entiry.Web.WebUser.UserName;
        //            pay.Remark = Remark;
        //            pay.CheckDate = System.DateTime.Now;

        //            pay.Update(tran);

        //            new CP.Core.OrderCore().ChangeOrderPayInfo(tran,
        //                (int)dr[CP.Entiry.b_Pay_XianXiaAttr.FK_Order_ID],
        //                dr[CP.Entiry.b_Pay_XianXiaAttr.PayMan].ToString(),
        //                dr[CP.Entiry.b_Pay_XianXiaAttr.PayType].ToString(),
        //                dr[CP.Entiry.b_Pay_XianXiaAttr.SellerID].ToString(),
        //                Convert.ToDateTime(dr[CP.Entiry.b_Pay_XianXiaAttr.PayDate]), out errMsg);
        //        }

        //        #endregion

        //        tran.Commit();
        //        errMsg = "";
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        if (tran != null)
        //            tran.Rollback();
        //        throw;
        //    }
        //    finally
        //    {
        //        if (tran != null)
        //        {
        //            tran.Dispose();
        //        }
        //    }
        //}

        #endregion

    }
}
