using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.Core
{
    public class MenuCore
    {
        /// <summary>
        /// 
        /// </summary> 
        /// <returns></returns>
        public bool AddChildMenu(CP.Entiry.sys_Menu menu, int menuID, bool isParent, out string errMsg)
        {
            DbTransaction tran = null;
            try
            {
                #region 数据校验

                #region 非空校验
                // todo
                #endregion

                #region  检查数据是否还存在
                if (menuID != 0)
                {
                    CP.Entiry.sys_Menu query = new CP.Entiry.sys_Menu();
                    query.Query(menuID.ToString());
                    if (query.ID == null)
                        throw new CP.Core.Web.UIException("数据不存在或已经被删除。");
                }
                #endregion
                #endregion

                if (isParent)
                {
                    menu.ParentId = menuID;
                    menu.CreateDate = DateTime.Now;
                    menu.Insert();
                }
                else
                {
                    menu.ID = menuID;
                    menu.Update();
                }

                errMsg = "执行成功";
                return true;
            }
            catch (Exception ex)
            {
                CP.DA.Log.WriteInfo(CP.Common.Common.CurrentHttpContext.Request.Params["menuid"], CP.Common.LogEnum.Operation.Edit, CP.Common.LogEnum.LogRunState.Failure, ex.Message);
                throw;
            }
            finally
            {
                CP.Core.SystemCore.SetCurrentMenu();

                if (tran != null)
                    tran.Dispose();
            }
        }


        /// <summary>
        /// 删除菜单
        /// </summary>
        public bool RemoveMenu(int menuid, out string errMsg)
        {

            DbTransaction tran = null;


            try
            {
                #region 检查是否有下级菜单
                CP.Entiry.sys_Menu query = new CP.Entiry.sys_Menu();
                CP.EntityBase.QueryParams param = new CP.EntityBase.QueryParams();
                param.addWhere(CP.Entiry.sys_MenuAttr.ParentId, "=", "@id", menuid);

                DataTable dt = query.QueryToDataTable(param);

                if (dt.Rows.Count > 0)
                {
                    errMsg = "您选择的数据有下级菜单,不允许删除。";
                    return false;
                }
                #endregion

                tran = CP.Common.DBHelper.GetTransaction(CP.Common.sysConst.WriteConnStrName);

                #region 删除权限

                CP.Entiry.sys_Permissions permission = new CP.Entiry.sys_Permissions();
                param = new CP.EntityBase.QueryParams();
                param.addWhere("FK_Menu_ID", "=", "@menuid", menuid);
                permission.Remove(tran, param);

                #endregion

                #region 删除按钮

                CP.Entiry.sys_MenuButton menubtn = new CP.Entiry.sys_MenuButton();
                param = new CP.EntityBase.QueryParams();
                param.addWhere("FK_Menu_ID", "=", "@menuid", menuid);
                menubtn.Remove(tran, param);

                #endregion

                #region 删除菜单

                CP.Entiry.sys_Menu menu = new CP.Entiry.sys_Menu();
                menu.ID = menuid;
                menu.Remove(tran);
                #endregion


                tran.Commit();

                errMsg = "执行成功";
                return true;
            }
            catch (Exception ex)
            {
                if (tran != null)
                    tran.Rollback();

                CP.DA.Log.WriteInfo(CP.Common.Common.CurrentHttpContext.Request.Params["menuid"], CP.Common.LogEnum.Operation.Edit, CP.Common.LogEnum.LogRunState.Failure, ex.Message);
                throw;
            }
            finally
            {
                CP.Core.SystemCore.SetCurrentMenu();
                CP.Core.SystemCore.SetCurrentPermission();


                if (tran != null)
                    tran.Dispose();
            }
        }

    }
}
